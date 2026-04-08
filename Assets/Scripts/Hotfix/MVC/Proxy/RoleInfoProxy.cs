// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月17日
// Update Time         :    2020年9月17日
// Class Description   :    RoleInfoProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game {

    public class ServerListTypeDefineTag
    {
       public List<ServerListTypeDefine> LsServerListTypeDefines= new List<ServerListTypeDefine>();
       public int PrefabIndex;
       public bool isSelected = false;
       public ServerListTypeDefine M_ServerListTypeDefineMin;
        public ServerListTypeDefine M_ServerListTypeDefingMax;
        public List<ServerListTypeDefine> subItemsData =new List<ServerListTypeDefine>();
        public int itemType = 0;
    }


    public class RoleInfoProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "RoleInfoProxy";

        private readonly Dictionary<long, RoleInfoEntity> dicRoleInfos= new Dictionary<long, RoleInfoEntity>();
        private readonly Dictionary<long, ServerListTypeDefine> dicServerLists= new Dictionary<long, ServerListTypeDefine>();
        private readonly Dictionary<int,List<ServerListTypeDefine>> dicServerListTypeDefinesGroup= new Dictionary<int, List<ServerListTypeDefine>>();

        public Dictionary<int, List<ServerListTypeDefine>> GetDicServerListTypeDefinesGroup
        {
            get { return dicServerListTypeDefinesGroup; }
        }

        private int newseverTimeLimit;
        public int NewseverTimeLimit
        {
            get { return newseverTimeLimit; }
        }

        #endregion

        // Use this for initialization
        public RoleInfoProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" RoleInfoProxy register");  
        }


        public override void OnRemove()
        {
            Debug.Log(" RoleInfoProxy remove");
            dicRoleInfos.Clear();
            dicServerLists.Clear();
            dicServerListTypeDefinesGroup.Clear();
        }

        public void InitServerList()
        {
            List<ServerListTypeDefine> lsServerListTypeDefines = CoreUtils.dataService.QueryRecords<ServerListTypeDefine>();
            if (lsServerListTypeDefines == null)
            {
                return;
            }

            dicServerLists.Clear();
            foreach (var info in lsServerListTypeDefines)
            {
                if (info.IsDisplay == 0)
                {
                    dicServerLists.Add(info.ID,info);
                    int group = info.group;
                    if (!dicServerListTypeDefinesGroup.ContainsKey(group))
                    {
                        dicServerListTypeDefinesGroup[group]= new List<ServerListTypeDefine>();
                    }
                    dicServerListTypeDefinesGroup[group].Add(info);
                }
            }

            newseverTimeLimit = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).newseverTimeLimit;
        }


        public void UpdateRoleInfoData(INotification notification)
        {
            dicRoleInfos.Clear();
            Role_GetRoleList.response info = notification.Body as Role_GetRoleList.response;
            if (info != null)
            {
                if (info.HasRoleInfos)
                {
                    foreach (var roleInfo in info.roleInfos.Values)
                    {
                        var  m_roleInfo = new RoleInfoEntity();
                        RoleInfoEntity.updateEntity(m_roleInfo, roleInfo);
                        dicRoleInfos[m_roleInfo.rid] = m_roleInfo;
                    }
                }
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.RoleInfo_Refresh);
        }

        public RoleInfoEntity GetRoleInfoEntity(int rid)
        {
            RoleInfoEntity roleInfoEntity;
            if (dicRoleInfos.TryGetValue(rid, out roleInfoEntity))
            {
                return roleInfoEntity;
            }

            return null;
        }


        public List<RoleInfoEntity> GetRoleInfoEntityS()
        {
            List<RoleInfoEntity> ls= new List<RoleInfoEntity>();
            foreach (var info in dicRoleInfos.Values)
            {
                ls.Add(info);
            }   
            return ls;
        }

        public List<ServerListTypeDefine> GetServerLists()
        {
            List<ServerListTypeDefine> ls= new List<ServerListTypeDefine>();
            foreach (var info in dicServerLists.Values)
            {
                ls.Add(info);
            }  
            
            ls.Sort(OnSort);
            return ls;
        }
        
        private int OnSort(ServerListTypeDefine a, ServerListTypeDefine b)
        {
            if (a.ID > b.ID)
            {
                return -1;
            }
            
            return 0;
        }

        public ServerListTypeDefine GetServerListTypeDefine(int id)
        {
            ServerListTypeDefine serverListTypeDefine;
            if (dicServerLists.TryGetValue(id, out serverListTypeDefine))
            {
                return serverListTypeDefine;
            }
            return null;
        }
        


        public int GetServerGroupCount()
        {
            return dicServerListTypeDefinesGroup.Count;
        }

        public bool GetIsCreateRole(int gameNode)
        {
            int createRoleMax = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).createRoleMax;
            int count = 0;
            foreach (var info in dicRoleInfos.Values)
            {
               int gameId= RoleInfoHelp.GetServerId(info.gameNode);
                if (gameNode == gameId)
                {
                    count += 1;
                }
            }
            return count<createRoleMax;
        }


        public List<ServerListTypeDefine> GetServerListTypeDefinesGroups(int group)
        {
            List<ServerListTypeDefine> ls = null;
            dicServerListTypeDefinesGroup.TryGetValue(group, out ls);
            return ls;
        }

        public void SortGroup(int group)
        {
            List<ServerListTypeDefine> ls = null;
            if (dicServerListTypeDefinesGroup.TryGetValue(group, out ls))
            {          
                ls.Sort(SortGroupList);
            }
        }


        private int SortGroupList(ServerListTypeDefine a, ServerListTypeDefine b)
        {
            if (a.order > b.order)
            {
                return 1;
            }

            if (a.order < b.order)
            {
                return -1;
            }

            return 0;
        }


        public int IsHaveRole(int gameNode)
        {
            int num = 0;
            foreach (var info in dicRoleInfos.Values)
            {
                int id = RoleInfoHelp.GetRoleInfoServerId(info.gameNode);
                if (gameNode == id)
                {
                    num += 1;
                }
            }                      
            return num;
        }

        #region 协议

        /// <summary>
        /// 请求角色列表
        /// </summary>
        public void SendRoleInfo()
        {
            Role_GetRoleList.request req=  new Role_GetRoleList.request();            
            AppFacade.GetInstance().SendSproto(req);
        }

        public void SendRoleSetLastServerAndRole(string gameNode, long rid)
        {
            Role_SetLastServerAndRole.request req = new Role_SetLastServerAndRole.request();
            req.selectGameNode = gameNode;
            if (rid > 0)
            {
                req.selectRid = rid;
            }
            AppFacade.GetInstance().SendSproto(req);
        }
        

        #endregion
    }
}