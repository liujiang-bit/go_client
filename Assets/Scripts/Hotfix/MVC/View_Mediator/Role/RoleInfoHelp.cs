using System;
using Data;
using Skyunion;
using UnityEngine;

namespace Game
{
    public static class RoleInfoHelp
    {
        public static void SetIsNewCreateRole()
        {
            NetProxy netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            string key = "NewCreate_" + netProxy.MUserName;
            PlayerPrefs.SetInt(key,1);
            PlayerPrefs.Save();         
        }

        public static bool GetIsNewCreateRole()
        {
            NetProxy netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            string key = "NewCreate_" + netProxy.MUserName;
            int value = PlayerPrefs.GetInt(key);      
            return value==1;
        }

        public static void DeleteNewCreateRole()
        {
            NetProxy netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            string key = "NewCreate_" + netProxy.MUserName;
            PlayerPrefs.DeleteKey(key);
        }

        public static  int GetRoleInfoServerId(string str)
        {
            string[] s= str.Split('e');
            int gameId = 0;
            if (s.Length >= 2)
            {
                gameId = int.Parse(s[1]);
            }
            return gameId;
        }

        public static string GetServerIdDes(string gameNode)
        {             
            return  LanguageUtils.getTextFormat(100525, GetServerId(gameNode));
        }

        public static string GetServerNameId(string gameNode)
        {
            RoleInfoProxy mRoleInfoProxy= AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            ServerListTypeDefine serverListTypeDefine = mRoleInfoProxy?.GetServerListTypeDefine(GetServerId(gameNode));
            if (serverListTypeDefine != null&& serverListTypeDefine.serverNameId>0)
            {
                return LanguageUtils.getText(serverListTypeDefine.serverNameId);
            }

            return string.Empty;
        }

        public static int  GetServerId(string gameNode)
        {
            int serverId = 0;
            if (!string.IsNullOrEmpty(gameNode))
            {
                serverId = GetRoleInfoServerId(gameNode);
            } 

            return serverId;
        }
        
        public static string FormatCountDown(TimeSpan timeSpan)
        {
            string str = "";
            if (timeSpan.Days > 0)
            {
                str = string.Format(LanguageUtils.getText(128004), timeSpan.Days);
                str = string.Format("{0} {1:D2}:{2:D2}:{3:D2}", str, timeSpan.Hours, timeSpan.Minutes,
                    timeSpan.Seconds);
            }
            else
            {
                str = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes,
                    timeSpan.Seconds);
            }

            return str;
        }
        
        
        public static long ToUnixTimestamp(this DateTime target)
        {
            return ((target.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

    }
}