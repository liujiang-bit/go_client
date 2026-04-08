// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月29日
// Update Time         :    2020年4月29日
// Class Description   :    MinimapProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SprotoType;
using System.Linq;

namespace Game {
    public class MinimapProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "MinimapProxy";

        #endregion

        // Use this for initialization
        public MinimapProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" MinimapProxy register");   
        }


        public override void OnRemove()
        {
            Debug.Log(" MinimapProxy remove");
        }

        public Dictionary<long, MemberPosInfo> MemberPos = new Dictionary<long, MemberPosInfo>();

        public void UpdateAllianceMap(object body)
        {
            Guild_GuildMemberPos.request req = body as Guild_GuildMemberPos.request;
            if (req.HasMemberPos)
            {
                var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                req.memberPos.Values.ToList().ForEach((item) =>
                {
                    if (item.rid != playerProxy.Rid)
                    {
                        if (MemberPos.ContainsKey(item.rid))
                        {
                            MemberPos[item.rid] = item;
                        }
                        else
                        {
                            MemberPos.Add(item.rid, item);
                        }
                    }
                });
                AppFacade.GetInstance().SendNotification(CmdConstant.RefreshGuildMemberPosView);
            }
            if (req.HasDeleteRid)
            {
                MemberPos.Remove(req.deleteRid);
                AppFacade.GetInstance().SendNotification(CmdConstant.DeleteMiniMapGuildMemberPos, req.deleteRid);
            }
        }

        public bool AcceptMap = true;
    }
}