using System;
using System.Collections.Generic;
using System.Linq;
using Client;
using DG.Tweening;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public enum BattleSoundType
    {
        None=0,
        BattleHit,
        Stop,
        Fail,
        Move
    }

    public class BattleHitSoundData
    {
        public GameObject go;
        public int objectId;
    }

    public class BattleSoundHandler :  IBattleSoundHandler
    {
        private const string sound_Battle_hit = "Sound_Battle_hit";
        private const string sound_ArmyStop = "Sound_ArmyStop";
        private const string sound_ArmyMove = "Sound_ArmyMove";
        private const string sound_ArmyFail = "Sound_ArmyFail";
        private readonly  Dictionary<int,BattleHitSoundData> dicBattleHit= new Dictionary<int, BattleHitSoundData>();
        private readonly  Dictionary<int, BattleHitSoundData> dicBattleHitView= new Dictionary<int, BattleHitSoundData>();
        private Timer m_Timer;
        
        private GameObject m_SoundGo;
        private AudioHandler m_AudioHandler;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private bool isUpdate = true;
        public void Init()
        {
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }

        public void Clear()
        {
            dicBattleHit.Clear();
            dicBattleHitView.Clear();
            if (m_SoundGo != null)
            {
                GameObject.Destroy(m_SoundGo);               
                m_SoundGo = null;
                m_AudioHandler = null;
            }

            if (m_Timer != null)
            {
                m_Timer.Cancel();
            }
        }
        
        
        public void AddBattleSoundByBattleHit(ArmyData armyData,MapObjectInfoEntity infoEntity)
        {
            if (armyData != null)
            {
                if (!dicBattleHit.ContainsKey(armyData.objectId))
                {
                    BattleHitSoundData soundData= new BattleHitSoundData();
                    soundData.go = armyData.go;
                    soundData.objectId = armyData.objectId;
                    dicBattleHit[armyData.objectId] = soundData;
                    ChangeArmySoundPos(armyData);
                }
            }

            if (infoEntity != null)
            {
                int objectId = (int)infoEntity.objectId;
                if (!dicBattleHit.ContainsKey(objectId))
                {
                    BattleHitSoundData soundData= new BattleHitSoundData();
                    soundData.go = infoEntity.gameobject;
                    soundData.objectId = objectId;
                    dicBattleHit[objectId] = soundData;
                    ChangeMonsterSoundPos(infoEntity);
                }
            }
        }

        public void RemoveBattleSoundByBattleHit(ArmyData armyData, MapObjectInfoEntity mapObjectInfoEntity)
        {
            int objectId = 0;
            if (armyData != null)
            {
                objectId = armyData.objectId;
            }else if (mapObjectInfoEntity != null)
            {
                objectId = (int)mapObjectInfoEntity.objectId;
            }

            if (dicBattleHit.ContainsKey(objectId))
            {
                dicBattleHit.Remove(objectId);
                if (dicBattleHitView.ContainsKey(objectId))
                {
                    dicBattleHitView.Remove(objectId);
                }

                BattleHitSoundData data = GetBattleHitData();
                if (data == null) //当前视野上没有战斗的部队
                {
                    dicBattleHitView.Clear();
                }
                if (dicBattleHitView.Count == 0)
                {
                    if (m_AudioHandler != null)
                    {             
                        isUpdate = false;
                        CoreUtils.audioService.FadeHandlerVolume(m_AudioHandler, 0f, 1f, true); 
                        m_Timer = Timer.Register(1f, () =>
                        {
                            GameObject.Destroy(m_SoundGo);
                            m_SoundGo = null;
                            m_AudioHandler = null;
                            if (m_Timer != null)
                            {
                                m_Timer.Cancel();
                            }

                            isUpdate = true;
                        });
                    }
                }
                else
                {
                    int id = dicBattleHitView.First().Value.objectId;
                    ArmyData armyNextData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                    if (armyNextData != null)
                    {
                        ChangeArmySoundPos(armyNextData);
                    }
                    else
                    {
                        MapObjectInfoEntity infoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
                        if (infoEntity != null)
                        {
                            ChangeMonsterSoundPos(infoEntity);
                        }
                    }
                }      
            }       
        }

     

        public void RefreshBattleSoundHitPos()
        {
            BattleHitSoundData data = GetBattleHitData();
            if (data == null)
            {
                if (this.m_SoundGo != null)
                {
                    GameObject.Destroy(this.m_SoundGo);
                    this.m_SoundGo = null;                    
                }
                return;
            }

            AddBattleAttackSounds(data.objectId,data.go);           
        }
        

        public void AddBattleStopSound(GameObject go, Action<AudioHandler> callback)
        {
            if (go != null)
            {
                CoreUtils.audioService.PlayOneShot3D(sound_ArmyStop, go, (ah) =>
                {
                    if (ah != null)
                    {
                        callback?.Invoke(ah);
                    }

                });
            }
        }
        
        public void AddBattleMoveSound(GameObject go, Action<AudioHandler> callback)
        {
            if (go != null)
            {
                CoreUtils.audioService.PlayLoop3D(sound_ArmyMove, go, (ah) =>
                {
                    if (ah != null)
                    {
                        CoreUtils.audioService.SetHandlerVolume(ah,0f);
                        callback?.Invoke(ah);
                    }

                });
            }
        }

        public void AddBattleFailSound(GameObject go, Action<AudioHandler> callback)
        {
            if (go != null)
            {
                CoreUtils.audioService.PlayOneShot3D(sound_ArmyFail, go, (ah) =>
                {
                    if (ah != null)
                    {
                        callback?.Invoke(ah);
                    }

                });
            }
        }

        public void RemoveSound(AudioHandler ah,bool onlyAudioComp = false)
        {
            if (ah != null)
            {              
                if (!onlyAudioComp)
                    CoreUtils.audioService.StopByHandler(ah);
                else
                {
                    ah.OnDestroy();
                }
            }
        }

        public void DestroySound(AudioHandler ah,Action callback)
        {
            if (ah != null)
            {              
                ah.OnDestroy();
                if (ah.NeedDestroyGameObject && ah.gameObject != null)
                {
                    UnityEngine.Object.DestroyImmediate(ah.gameObject);
                }
                callback?.Invoke();
            }
        }

        public void PlaySound(AudioHandler ah)
        {
            if (ah != null && ah.gameObject != null)
            {
                CoreUtils.audioService.SetHandlerVolume(ah, 1f);
            }
        }

        public void StopSound(AudioHandler ah)
        {
            if (ah != null && ah.gameObject != null)
            {
                CoreUtils.audioService.SetHandlerVolume(ah, 0f);
            }
        }

        public void Update()
        {
            if (!isUpdate)
            {
                return;
            }

            BattleHitSoundData data= GetBattleHitData();
            if (data == null)
            {
                if (dicBattleHitView.Count > 0)
                {                 
                    dicBattleHitView.Clear();
                }

                if (m_SoundGo != null)
                {                   
                    GameObject.Destroy(m_SoundGo);
                    m_SoundGo = null; 
                }                
            }
        }
        
        
        private void AddBattleAttackSound(GameObject go, Action<AudioHandler> callback)
        {
            if (go != null)
            {
                CoreUtils.audioService.PlayLoop3D(sound_Battle_hit, go, (ah) =>
                {
                    if (ah != null)
                    {
                        callback?.Invoke(ah);
                    }

                });
            }
        }

        private void ChangeArmySoundPos(ArmyData armyData)
        {
            if (armyData.go != null)
            {
                float x = armyData.go.transform.position.x;
                float y = armyData.go.transform.position.z;
                if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y))
                {
                    FogSystemMediator fogSystemMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                    if (!fogSystemMediator.HasFogAtWorldPos(x, y))
                    {                                    
                        AddBattleAttackSounds(armyData.objectId,armyData.go);
                    }
                }
            }
        }

        private void ChangeMonsterSoundPos(MapObjectInfoEntity infoEntity)
        {
            if (infoEntity == null)
            {
                return;
            }

            if (infoEntity.gameobject == null)
            {
                return;
            }
            
            float x = infoEntity.gameobject.transform.position.x;
            float y = infoEntity.gameobject.transform.position.z;
            if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y))
            {
                FogSystemMediator fogSystemMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                if (!fogSystemMediator.HasFogAtWorldPos(x, y))
                {                                    
                    AddBattleAttackSounds((int)infoEntity.objectId,infoEntity.gameobject);
                }
            }
        }

        private void AddBattleAttackSounds(int objectId, GameObject go)
        {
            if (m_SoundGo == null)
            {
                m_SoundGo = new GameObject();
                m_SoundGo.gameObject.name = "m_SoundGo";
                AddBattleAttackSound(m_SoundGo,
                    (ah) =>
                    {
                        if (ah.IsDestroyed)
                        {
                            if (m_AudioHandler != null)
                            {
                                CoreUtils.audioService.OnHandlerDestroy(m_AudioHandler);
                            }            
                            GameObject.Destroy(m_SoundGo);
                            m_SoundGo = null;
                            m_AudioHandler = null;
                        }
                        else
                        {
                            m_AudioHandler = ah;
                            CoreUtils.audioService.FadeHandlerVolume(m_AudioHandler, 1f, 0.5f, false);
                        }
                    });
            }

            m_SoundGo.transform.SetParent(go.transform);
            m_SoundGo.transform.localPosition = Vector3.zero;
            if (!dicBattleHitView.ContainsKey(objectId))
            {
                BattleHitSoundData soundData = new BattleHitSoundData();
                soundData.go = go;
                soundData.objectId = objectId;
                dicBattleHitView[objectId] = soundData;
            }
        }
        
        private  BattleHitSoundData GetBattleHitData()
        {
            foreach (var data in dicBattleHit.Values)
            {
                if (data.go == null)
                {
                    continue;
                }

                float x = data.go.transform.position.x;
                float y = data.go.transform.position.z;
                if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y))
                {
                    FogSystemMediator fogSystemMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                    
                    if (!fogSystemMediator.HasFogAtWorldPos(x, y))
                    {
                        MapObjectInfoEntity mapobjectinfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(data.objectId);
                        if (mapobjectinfo != null)
                        {
                            Troops.ENMU_SQUARE_STAT state =  TroopHelp.GetTroopState(mapobjectinfo.status);
                            if (mapobjectinfo.targetObjectIndex != 0 &&  state == Troops.ENMU_SQUARE_STAT.FIGHT)
                            {
                                return data;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}