using System.Collections.Generic;
using Game;
using UnityEngine;
using Timer = Skyunion.Timer;

namespace Hotfix
{
    public enum TroopSaveType
    {
        None = 0,
        Read,
        Save
    }

    public enum TroopSaveNumType
    {
        None = 0,
        Blue,
        Yellow,
        Red,
    }

    public class SaveSoldiersData
    {
        public int id;
        public long num;
    }

    public class SaveData
    {
        public int id;
        public int heroId;
        public int viceId;
        public TroopSaveNumType type;
        public string iconPath;
        public string iconVicePath;
        public bool isDelete = false;
        public bool isShow = false;
        public bool isSelect = false;
        public bool isOnToggle = false;
        public List<SaveSoldiersData> solds = new List<SaveSoldiersData>();
        public long soldierNum;
    }

    public sealed class TroopSave : ITroopSave, IData
    {
        private readonly Dictionary<int, SaveData> dicSave = new Dictionary<int, SaveData>(15);

        private readonly Dictionary<TroopSaveNumType, List<SaveData>> dicSaveNum =
            new Dictionary<TroopSaveNumType, List<SaveData>>();

        private readonly Dictionary<int,int> lsSerializationSoldiersDatas = new Dictionary<int, int>();

        private TroopProxy m_TroopProxy;
        private HeroProxy m_HeroProxy;
        private TroopSaveType m_TroopSaveType;
        private PlayerProxy m_PlayerProxy;
        private bool m_CurIsDelete = false;
        public static int SaveNum = 15;
        private bool isInit = false;
        private bool isInitSendEvent = false;

        public TroopSave()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        public void Init()
        {
            if (isInit)
            {
                return;
            }

            for (int i = 0; i < SaveNum; i++)
            {
                int id = i + 1;
                int heroId = GetSerializationHeroId(id);
                int viceId = GetSerializationViceId(id);
                if (heroId > 0)
                {
                    lsSerializationSoldiersDatas.Clear();
                    TroopSaveNumType type = GetTroopSaveNumType(id);
                    string[] soldier = GetSerializationSoldierId(id);
                    string[] soldierNum = GetSerializationSoldierNum(id);
                    if (soldier.Length > 0 && soldierNum.Length > 0)
                    {
                        for (int j = 0; j < soldier.Length; j++)
                        {
                            int soldierId = int.Parse(soldier[j]);
                            int num = int.Parse(soldierNum[j]);
                            lsSerializationSoldiersDatas[soldierId] = num;
                        }
                    }

                    InsertSave(id, (int) type, heroId, viceId, lsSerializationSoldiersDatas, false);
                }
            }

            isInit = true;
        }

        public void Update(int id, object data)
        {
        }

        public void Clear()
        {
            dicSave.Clear();
            dicSaveNum.Clear();
            isInit = false;
            isInitSendEvent = false;
            if (_timer != null)
            {
                _timer.Cancel();
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshTroopSave);
        }

        public object GetData(int id)
        {
            SaveData d = null;
            if (dicSave.TryGetValue(id, out d))
            {
                return d;
            }

            return null;
        }

        public object GetDataByIndex(int index)
        {
            return null;
        }

        public int GetDataCount()
        {
            return dicSave.Count;
        }

        #region 保存接口实现

        private List<string> lsSoldierId = new List<string>();
        private List<string> lsSoldierNum = new List<string>();

        private SaveData InitSaveData(int id, int type, int heroId, int viceId, Dictionary<int, int> solds)
        {
            SaveData saveData = new SaveData();
            saveData.id = id;
            saveData.heroId = heroId;
            saveData.viceId = viceId;
            HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(saveData.heroId);
            if (hero != null)
            {
                saveData.iconPath = hero.config.heroIcon;
            }

            HeroProxy.Hero heroVice = m_HeroProxy.GetHeroByID(saveData.viceId);
            if (heroVice != null)
            {
                saveData.iconVicePath = heroVice.config.heroIcon;
            }

            saveData.solds.Clear();
            lsSoldierId.Clear();
            lsSoldierNum.Clear();
            long num = 0;
            if (solds != null)
            {
                foreach (var info in solds)
                {
                    num += info.Value;
                    SaveSoldiersData data = new SaveSoldiersData();
                    data.id = info.Key;
                    data.num = info.Value;
                    lsSoldierId.Add(data.id.ToString());
                    lsSoldierNum.Add(data.num.ToString());
                    saveData.solds.Add(data);
                }
            }

            saveData.soldierNum = num;
            saveData.type = (TroopSaveNumType) type;
            saveData.isDelete = m_CurIsDelete;
            saveData.isShow = true;
            saveData.isSelect = true;
            return saveData;
        }


        private Timer _timer;
        public void InsertSave(int id, int type, int heroId, int viceId, Dictionary<int, int> solds,
            bool isSerialization = true)
        {
            TroopSaveNumType troopSaveNumType = (TroopSaveNumType) type;
            if (!dicSaveNum.ContainsKey(troopSaveNumType))
            {
                dicSaveNum[troopSaveNumType] = new List<SaveData>();
            }

            SaveData saveData = InitSaveData(id, type, heroId, viceId, solds);
            dicSave[saveData.id] = saveData;
            dicSaveNum[troopSaveNumType].Add(saveData);
            OnSetSelect(saveData.id);
            AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshTroopSave);
            if (isSerialization)
            {
                SendEvent(troopSaveNumType);
                SetSerializationHeroId(id, heroId);
                SetSerializationViceId(id, viceId);
                SetSerializationSoldier(id, lsSoldierId.ToArray(), lsSoldierNum.ToArray());
            }
            else
            {
                if (_timer == null)
                {
                    _timer= Timer.Register(0.2f, () =>
                    {
                        if (!isInitSendEvent)
                        {
                            SaveData data = GetNextTroopSaveNumType(troopSaveNumType) as SaveData;
                            if (data != null)
                            {
                                SendEvent(data.type);
                                isInitSendEvent = true;
                            }
                        }
                    });
                }
            }
        }

        public void DeleteSave(int id)
        {
            if (dicSave.ContainsKey(id))
            {
                DeleteKey(id);
                dicSave.Remove(id);
                AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshTroopSave);
            }
        }

        public void UpdateSave(int id, int heroId, int viceId, Dictionary<int,int> solds)
        {
            if (dicSave.ContainsKey(id))
            {
                dicSave[id].heroId = heroId;
                dicSave[id].viceId = viceId;
                HeroProxy.Hero hero = m_HeroProxy.GetHeroByID(heroId);
                if (hero != null)
                {
                    dicSave[id].iconPath = hero.config.heroIcon;
                }

                HeroProxy.Hero heroVice = m_HeroProxy.GetHeroByID(viceId);
                if (heroVice != null)
                {
                    dicSave[id].iconVicePath = heroVice.config.heroIcon;
                }
                else
                {
                    dicSave[id].iconVicePath = string.Empty;
                }

                dicSave[id].solds.Clear();
                lsSoldierId.Clear();
                lsSoldierNum.Clear();
                long num = 0;
                foreach (var info in solds)
                {
                    SaveSoldiersData data = new SaveSoldiersData();
                    data.id = info.Key;
                    data.num = info.Value;
                    num += info.Value;
                    lsSoldierId.Add(data.id.ToString());
                    lsSoldierNum.Add(data.num.ToString());
                    dicSave[id].solds.Add(data);
                }

                dicSave[id].soldierNum = num;
                AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshTroopSave);
                SetSerializationHeroId(id, heroId);
                SetSerializationViceId(id, viceId);
                SetSerializationSoldier(id, lsSoldierId.ToArray(), lsSoldierNum.ToArray());
            }
        }

        public void RestSave()
        {
            foreach (var info in dicSave.Values)
            {
                info.isSelect = false;
            }
        }

        public void UpdateAllSave(object parm)
        {
            bool isDelete = parm is bool ? (bool) parm : false;
            m_CurIsDelete = isDelete;
            foreach (var item in this.dicSave.Values)
            {
                item.isDelete = isDelete;
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshTroopSave);
        }

        public bool GetIsDelete()
        {
            return m_CurIsDelete;
        }

        public void SetSaveType(TroopSaveType type)
        {
            this.m_TroopSaveType = type;
        }

        public TroopSaveType GetSaveType()
        {
            return m_TroopSaveType;
        }

        public void DeletSaveType(TroopSaveNumType type, object parm)
        {
            if (dicSaveNum.ContainsKey(type))
            {
                SaveData saveData = parm as SaveData;
                dicSaveNum[type].Remove(saveData);
                SendEvent(type);
                AppFacade.GetInstance().SendNotification(CmdConstant.OnAutoRefreshSaveIndexView, type);
            }
        }

        public object GetLsSave(TroopSaveNumType type, int id)
        {
            if (dicSaveNum.ContainsKey(type))
            {
                return dicSaveNum[type][id];
            }

            return null;
        }

        public int GetLsSaveCount(TroopSaveNumType type)
        {
            List<SaveData> ls;
            if (dicSaveNum.TryGetValue(type, out ls))
            {
                ls.Sort((a, b) =>
                {
                    if (a.id > b.id)
                    {
                        return 1;
                    }

                    if (a.id < b.id)
                    {
                        return -1;
                    }

                    return 0;
                });
                return ls.Count;
            }

            return 0;
        }

        public bool IsContainsData(TroopSaveNumType type)
        {
            List<SaveData> ls;
            if (dicSaveNum.TryGetValue(type, out ls))
            {
                return ls.Count > 0;
            }

            return false;
        }

        public object GetSaveDataByType(TroopSaveNumType type)
        {
            List<SaveData> ls;
            if (dicSaveNum.TryGetValue(type, out ls))
            {
                return ls;
            }

            return null;
        }


        public object GetNextTroopSaveNumType(TroopSaveNumType troopSaveNumType)
        {
            List<SaveData> ls;
            if (dicSaveNum.TryGetValue(troopSaveNumType, out ls))
            {
                if (ls.Count > 0)
                {
                    ls.Sort((a, b) =>
                    {
                        if (a.id > b.id)
                        {
                            return 1;
                        }

                        if (a.id < b.id)
                        {
                            return -1;
                        }

                        return 0;
                    });

                    return ls[0];
                }
                else
                {
                    foreach (var info in dicSaveNum.Values)
                    {
                        info.Sort((a, b) =>
                        {
                            if (a.id > b.id)
                            {
                                return 1;
                            }

                            if (a.id < b.id)
                            {
                                return -1;
                            }

                            return 0;
                        });
                        foreach (var item in info)
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        public void SetSelect(TroopSaveNumType type, int id)
        {
            OnSetSelect(id);
            SendEvent(type);
        }

        private void OnSetSelect(int id)
        {
            foreach (var item in dicSave.Values)
            {
                if (id == item.id)
                {
                    item.isSelect = true;
                }
                else
                {
                    item.isSelect = false;
                }
            }
        }

        private void SetSerializationHeroId(int id, int heroId)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "hero");
            PlayerPrefs.SetInt(key, heroId);
        }

        private void SetSerializationViceId(int id, int viceId)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "vice");
            PlayerPrefs.SetInt(key, viceId);
        }

        private void SetSerializationSoldier(int id, string[] soldierId, string[] num)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "soldierId");
            string keyNum = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "soldierNum");
            TroopHelp.SetStringArray(key, soldierId);
            TroopHelp.SetStringArray(keyNum, num);
            PlayerPrefs.Save();
        }


        private int GetSerializationHeroId(int id)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "hero");
            return PlayerPrefs.GetInt(key);
        }

        private int GetSerializationViceId(int id)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "vice");
            return PlayerPrefs.GetInt(key);
        }

        private string[] GetSerializationSoldierId(int id)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "soldierId");
            return TroopHelp.GetStringArray(key);
        }

        private string[] GetSerializationSoldierNum(int id)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "soldierNum");
            return TroopHelp.GetStringArray(key);
        }

        private void DeleteKey(int id)
        {
            string key = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "hero");
            string key1 = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "vice");
            string key2 = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "soldierId");
            string key3 = string.Format("{0}_{1}_{2}", m_PlayerProxy.CurrentRoleInfo.rid, id, "soldierNum");
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.DeleteKey(key1);
            PlayerPrefs.DeleteKey(key2);
            PlayerPrefs.DeleteKey(key3);
        }

        private void SendEvent(TroopSaveNumType type)
        {
            switch (type)
            {
                case TroopSaveNumType.Blue:
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshSaveIndexViewBule);
                    break;
                case TroopSaveNumType.Yellow:
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshSaveIndexViewYellow);
                    break;
                case TroopSaveNumType.Red:
                    AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshSaveIndexViewRed);
                    break;
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.OnRefreshSaveNumView);
        }

        private TroopSaveNumType GetTroopSaveNumType(int index)
        {
            TroopSaveNumType type = TroopSaveNumType.Blue;
            if (index >= 1 && index <= 5)
            {
                type = TroopSaveNumType.Blue;
            }
            else if (index > 5 && index <= 10)
            {
                type = TroopSaveNumType.Yellow;
            }
            else if (index > 10 && index <= 15)
            {
                type = TroopSaveNumType.Red;
            }

            return type;
        }

        #endregion
    }
}