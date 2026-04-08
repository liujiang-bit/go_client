// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    DataProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using SprotoTemp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SprotoType;
using Client;
using Client.Utils;
using Data;
using Skyunion;

namespace Game {
    public class DataProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "DataProxy";

        #endregion

        // Use this for initialization
        public DataProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" DataProxy register");

            if (!BannedWord.HasInited())
            {
                var badconfig = CoreUtils.dataService.QueryRecords<BlockDefine>();
                var chatBadConfigs = CoreUtils.dataService.QueryRecords<ChatBlockDefine>();
                List<string> badwords = new List<string>(Mathf.Max(badconfig.Count,chatBadConfigs.Count));
                for (int i = 0; i < badconfig.Count; i++)
                {
                    badwords.Add(badconfig[i].ID);
                }
                BannedWord.InitBadWord(badwords);

                badwords.Clear();
                for (int i = 0; i < chatBadConfigs.Count; i++)
                {
                    badwords.Add(chatBadConfigs[i].ID);
                }
                BannedWord.InitChatWord(badwords);
                
            }
        }


        public override void OnRemove()
        {
            Debug.Log(" DataProxy remove");
        }



        /// <summary>
        /// 传入中心点和尺寸
        /// </summary>
        /// <param name="PosInfo"></param>
        /// <param name="size"></param>
        public void OpenFogFromContent(PosInfo PosInfo, int size = 5)
        {
            int radius = size / 2;
            int x = (int)(PosInfo.x/18 - radius);
            int y = (int)(PosInfo.y/18 - radius);
            x = Mathf.Clamp(x, 0, 399);
            y = Mathf.Clamp(y, 0, 399);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    WarFogMgr.OpenFog(i + x, j + y);
                }
            }
        }
        /// <summary>
        /// 传入左下角和尺寸
        /// </summary>
        /// <param name="PosInfo"></param>
        /// <param name="size"></param>
        public void OpenFogFromLeftBottom(PosInfo PosInfo, int size = 5)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    WarFogMgr.OpenFog(i + (int)PosInfo.x, j + (int)PosInfo.y);
                }
            }
        }



    }
}