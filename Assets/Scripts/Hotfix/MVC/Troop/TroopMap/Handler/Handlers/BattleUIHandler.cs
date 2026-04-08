using System.Collections.Generic;
using Client;
using Data;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public enum BattleUIType
    {
        None = 0,

        //普攻
        BattleUI_GeneralAttack,

        //夹击
        BattleUI_Attack,

        //技能
        BattleUI_Skills,

        //击溃
        BattleUI_Rout,

        //战败
        BattleUI_Fail,

        //技能伤害值
        BattleUI_HP,
        //技能伤害值
        BattleUI_DOTHP,

        //治疗
        BattleUI_AddBlood,

        //buff技能
        BattleUI_BuffSkill,

        //buff icon
        BattleUI_BuffIconRed,
        BattleUI_BuffIconGreen,

        //主将播放技能
        BattleUI_MainPlaySkill,
        //副将播放技能
        BattleUI_ViceSkill,

        //头像变大
        BattleUI_HeadChangeScale,

        //显示副将头像
        BattleUI_ShowViceHead,

        //显示头像特效
        BattleUI_ShowHeadEffect,
        //显示副将头像特效
        BattleUI_ShowViceHeadEffect,

        //受击表现
        BattleUI_ShowBeAttack,

        // 受击表现里的头像框表现
        BattleUI_ShowBeAttackIcon,
        
        //主英雄升级
        UpdateHeroLevel,
        //hot恢复
        BattleUI_HOT,
    }

    public class BattleUIData
    {
        public int id;
        public BattleUIType type;
        public object parm1;
        public object parm2;
    }

    public class BattleUIBuffData
    {
        public int id;
        public BattleUIType buffType;
        public int parm;
    }

    public sealed class BattleUIHandler :IBattleUIHandler
    {
        private TroopProxy m_TroopProxy;
        private WorldMapObjectProxy m_WorldMapObjectProxy;

        private readonly Dictionary<int, Queue<BattleUIBuffData>> dicBuffIcon = new Dictionary<int, Queue<BattleUIBuffData>>();
        private Timer m_TimerUpdate;
        private Dictionary<int, Timer> m_TimerUpdateByAttackDic = new Dictionary<int, Timer>();
        private Dictionary<int, Timer> m_TimerUpdateBySkillkDic = new Dictionary<int, Timer>();

        public void Init()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_WorldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_TimerUpdate = Timer.Register(2.5f, Update, null, true);
        }

        public void SetBattleUIData(int id, BattleUIType type, object parm1, object parm2 = null)
        {
            // UI 上面的效果也是改成，同一帧只能触发一个
            HotfixUtil.InvokOncePerfOneFrame($"SetBattleUIData{id}_{(int)type}", () =>
            {

                BattleUIData info = new BattleUIData();
                info.id = id;
                info.type = type;
                info.parm1 = parm1;
                info.parm2 = parm2;

                switch (type)
                {
                    case BattleUIType.BattleUI_GeneralAttack:
                    case BattleUIType.BattleUI_Fail:
                    case BattleUIType.BattleUI_Rout:
                    case BattleUIType.BattleUI_Attack:
                    case BattleUIType.BattleUI_DOTHP:
                    case BattleUIType.BattleUI_AddBlood:
                    case BattleUIType.BattleUI_BuffIconRed:
                    case BattleUIType.BattleUI_BuffIconGreen:
                    case BattleUIType.UpdateHeroLevel:
                    case BattleUIType.BattleUI_HOT:
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapPlayShottTextHud, info);
                        break;
                    case BattleUIType.BattleUI_HP:
                        PlayBattleUISkillDamage(ref info);
                        break;
                    case BattleUIType.BattleUI_Skills:
                        PlayTroopTriggerSkillS(id, (int)parm1, (int)parm2);
                        PlayMonsterTriggerSkills(id, (int)parm1, (int)parm2);
                        break;
                    case BattleUIType.BattleUI_BuffSkill:
                        PlayTroopBuffSkill(id, (int)parm1);
                        PlayMonsterBuffSkill(id, (int)parm1);
                        break;
                    case BattleUIType.BattleUI_MainPlaySkill:
                    case BattleUIType.BattleUI_ViceSkill:
                        PlaySkillUUIEffect(id, (int)parm1);
                        break;
                    case BattleUIType.BattleUI_HeadChangeScale:
                    case BattleUIType.BattleUI_ShowHeadEffect:
                    case BattleUIType.BattleUI_ShowViceHead:
                    case BattleUIType.BattleUI_ShowViceHeadEffect:
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapSetFightBattleUIData, info);
                        break;
                    case BattleUIType.BattleUI_ShowBeAttack:
                        PlayBattleUIShowBeAttack(ref info);
                        AppFacade.GetInstance().SendNotification(CmdConstant.MapSetFightBattleUIData, info);
                        break;
                }
            });
        }

        public void Clear()
        {
            dicBuffIcon.Clear();
            if (m_TimerUpdate != null)
            {
                m_TimerUpdate.Cancel();
            }

            foreach (var Value in m_TimerUpdateByAttackDic.Values)
            {
                Value.Cancel();
            }
            m_TimerUpdateByAttackDic.Clear();

            foreach (var Value in m_TimerUpdateBySkillkDic.Values)
            {
                Value.Cancel();
            }
            m_TimerUpdateBySkillkDic.Clear();
        }

        public void Remove(int id)
        {
            dicBuffIcon.Remove(id);
        }

        public void PushBattleBuff(int id, int buffId)
        {
            if (!dicBuffIcon.ContainsKey(id))
            {
                dicBuffIcon[id] = new Queue<BattleUIBuffData>();
            }

            SkillStatusDefine statusDefine = CoreUtils.dataService.QueryRecord<SkillStatusDefine>(buffId);
            if (statusDefine != null)
            {
                if (statusDefine.showIcon == 0)
                {
                    return;
                }

                BattleUIBuffData buffData = new BattleUIBuffData();
                buffData.id = id;
                if (statusDefine.buffType == 1)
                {
                    buffData.buffType = BattleUIType.BattleUI_BuffIconGreen;
                }
                else
                {
                    buffData.buffType = BattleUIType.BattleUI_BuffIconRed;
                }

                buffData.parm = statusDefine.showIcon;
                dicBuffIcon[id].Enqueue(buffData);
            }
        }

        private void PlayTroopTriggerSkillS(int id, object parm1, object parm2)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                string heroId = armyData.heroId.ToString();
                bool isViceSkill = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().isVicePlaySkill(id, (int)parm1);
                if (isViceSkill)
                {
                    heroId = armyData.viceId.ToString();
                }

                Vector3 skillEffectPos = Vector3.zero;
                EffectInfoDefine effectInfoDefine = CoreUtils.dataService.QueryRecord<EffectInfoDefine>((int)parm1);
                if (effectInfoDefine != null)
                {
                    if (effectInfoDefine.beAttacked == 1)
                    {
                        TroopHelp.GetRssPos((int)parm2, out skillEffectPos);
                    }
                }

                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().TriggerSkillS(id, heroId, skillEffectPos);
                PlayCombatEffect(id, (int)parm1);
            }
        }

        private void PlayMonsterTriggerSkills(int id, object parm1, object parm2)
        {
            MapObjectInfoEntity monsterData = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null)
            {
                if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().IsContainTroop(id))
                {
                    return;
                }

                Vector3 skillEffectPos = Vector3.zero;
                EffectInfoDefine effectInfoDefine = CoreUtils.dataService.QueryRecord<EffectInfoDefine>((int)parm1);
                if (effectInfoDefine != null)
                {
                    if (effectInfoDefine.beAttacked == 1)
                    {
                        TroopHelp.GetRssPos((int)parm2, out skillEffectPos);
                    }
                }

                if (monsterData.rssType == RssType.Monster ||
                    monsterData.rssType == RssType.SummonAttackMonster ||
                    monsterData.rssType == RssType.SummonConcentrateMonster)
                {                 
                    string heroId = monsterData.mainHeroId.ToString();
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().TriggerSkillS(id, heroId, skillEffectPos); 
                }
                else if (monsterData.rssType == RssType.Guardian)
                {
                    if (monsterData.gameobject != null)
                    {                       
                        Guardian formationGuardian = monsterData.gameobject.GetComponent<Guardian>();
                        if (formationGuardian != null)
                        {                           
                            WorldMapLogicMgr.Instance.GuardianHandler.TriggerSkillS(formationGuardian, string.Empty);
                        }
                    }
                }
            }
        }

        private void PlayTroopBuffSkill(int id, int buffid)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                WorldMapLogicMgr.Instance.BattleBuffHandler.CreateBuffGo(id, buffid, armyData.go.transform);
            }
        }

        private void PlayCombatEffect(int id,int skillId)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                EffectInfoDefine effectInfoDefine = CoreUtils.dataService.QueryRecord<EffectInfoDefine>(skillId);
                if (effectInfoDefine != null)
                {
                    if (armyData.go == null)
                    {
                        Debug.LogError("部队实体不存在，请检查"+armyData.objectId);
                        return;
                    }
                    var pos = armyData.go.transform.position;
                    var forward = armyData.go.transform.forward;
                    if (Application.isEditor)
                    {
                        Color color;
                        ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                        CoreUtils.logService.Debug($"{id}\tBattleData: skillId:{skillId}\t{forward}", color);
                    }
                    if (!string.IsNullOrEmpty(effectInfoDefine.effectArea))
                    {
                        CoreUtils.assetService.Instantiate(effectInfoDefine.effectArea, (go) =>
                            {                           
                                go.transform.position = pos;
                                go.transform.forward = forward;
                                go.transform.localScale= Vector3.one;;
                                
                            }); 
                    }                  
                }
            }
        }

        private void PlayMonsterBuffSkill(int id, int buffid)
        {
            MapObjectInfoEntity monsterData = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (monsterData != null)
            {
                if (WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .IsContainTroop(id))
                {
                    return;
                }

                WorldMapLogicMgr.Instance.BattleBuffHandler.CreateBuffGo(id, buffid, monsterData.gameobject.transform);
            }
        }

        private void PlaySkillUUIEffect(int id, int skillId)
        {
            SkillBattleDefine skillBattleDefine = CoreUtils.dataService.QueryRecord<SkillBattleDefine>(skillId);
            if (skillBattleDefine != null)
            {
                if (skillBattleDefine.autoActive == 0)
                {
                    SetBattleUIData(id, BattleUIType.BattleUI_HeadChangeScale, null);

                    bool isViceSkill = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().isVicePlaySkill(id, skillId/100);
                    if(isViceSkill)
                    {
                        SetBattleUIData(id, BattleUIType.BattleUI_ShowViceHeadEffect, null);
                    }
                    else
                    {
                        SetBattleUIData(id, BattleUIType.BattleUI_ShowHeadEffect, null);
                    }
                }
            }
        }

        private void PlayBattleUISkillDamage(ref BattleUIData battleUiData)
        {
            var data = battleUiData;
            EffectInfoDefine effectInfoDefine = CoreUtils.dataService.QueryRecord<EffectInfoDefine>((int)data.parm2);
            if (effectInfoDefine != null)
            {
                if (m_TimerUpdateBySkillkDic.ContainsKey(data.id))
                {
                    m_TimerUpdateBySkillkDic[data.id].Cancel();
                    m_TimerUpdateBySkillkDic.Remove(data.id);
                }

                var TimerUpdateByAttack = Timer.Register(effectInfoDefine.key / 100.0f, () =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapPlayShottTextHud, data);

                    if (m_TimerUpdateBySkillkDic.ContainsKey(data.id))
                    {
                        m_TimerUpdateBySkillkDic[data.id].Cancel();
                        m_TimerUpdateBySkillkDic.Remove(data.id);
                    }
                });

                m_TimerUpdateBySkillkDic.Add(data.id, TimerUpdateByAttack);
            }
        }

        private void PlayBattleUIShowBeAttack(ref BattleUIData battleUiData)
        {
            var data = battleUiData;
            EffectInfoDefine effectInfoDefine = CoreUtils.dataService.QueryRecord<EffectInfoDefine>((int)data.parm1);
            if (effectInfoDefine != null)
            {
                if (m_TimerUpdateByAttackDic.ContainsKey(data.id))
                {
                    m_TimerUpdateByAttackDic[data.id].Cancel();
                    m_TimerUpdateByAttackDic.Remove(data.id);
                }

                var TimerUpdateByAttack = Timer.Register(effectInfoDefine.key / 100.0f, () =>
                {
                    data.type = BattleUIType.BattleUI_ShowBeAttackIcon;
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapSetFightBattleUIData, data);

                    if (m_TimerUpdateByAttackDic.ContainsKey(data.id))
                    {
                        m_TimerUpdateByAttackDic[data.id].Cancel();
                        m_TimerUpdateByAttackDic.Remove(data.id);
                    }
                });

                m_TimerUpdateByAttackDic.Add(data.id, TimerUpdateByAttack);
            }
        }

        private void Update()
        {
            if (dicBuffIcon.Count <= 0)
            {
                return;
            }

            foreach (var info in dicBuffIcon.Values)
            {
                if (info.Count > 0)
                {
                    BattleUIBuffData buffData = info.Dequeue();
                    if (buffData != null)
                    {
                        SetBattleUIData(buffData.id, buffData.buffType, buffData.parm);
                    }
                }
            }
        }
    }
}