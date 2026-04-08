// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    ScrollMessageProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using PureMVC.Core;
using Skyunion;
using System.Collections;
using System.Collections.Generic;
using Data;
using SprotoType;
using UnityEngine;

namespace Game {
    public class ScrollMessage
    {
        public int lanugageID;
        public object[] args;
        public string msg { get; private set; }
        public ScrollMessage(int _lanugageID, object[] _args = null)
        {
            this.lanugageID = _lanugageID;
            this.args = _args;
            SetMsg();
        }

        public ScrollMessage(string _msg)
        {
            msg = _msg;
        }

        public ScrollMessage(Chat_MarqueeNotify.request info)
        {
            if (info.languageId<=0)
            {
                msg = info.msg;
            }
            else
            {
                lanugageID = (int)info.languageId;
                args = info.args.ToArray();
                SetMsg();
            }
        }

        private void SetMsg()
        {
            if (args != null && args.Length > 0)
            {    //纪念碑结束
                if (lanugageID == 183016)
                {
                    msg = LanguageUtils.getTextFormat(lanugageID, args[0],LanguageUtils.getText(Convert.ToInt32(args[1])));
                }
                //联盟建筑被摧毁
                else if (lanugageID == 732047)
                {
                    if (args.Length < 7)
                    {
                        msg = "";
                        return;
                    }
                    var param4 = "";
                    int x = PosHelper.ServerUnitToClientPos(int.Parse(args[4].ToString()));
                    int y = PosHelper.ServerUnitToClientPos(int.Parse(args[5].ToString()));

                    int id = int.Parse(args[6].ToString());
                    AllianceBuildingTypeDefine allianceBuild = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(id);
                    if (allianceBuild != null)
                    {
                        param4 = LanguageUtils.getText(allianceBuild.l_nameId);
                    }
                    msg = LanguageUtils.getTextFormat(lanugageID, LanguageUtils.getTextFormat(730138,args[0]),LanguageUtils.getTextFormat(730138,args[2]),LanguageUtils.getTextFormat(300032,x,y),param4);
                }else
                {
                    msg = LanguageUtils.getTextFormat(lanugageID, args);
                }
            }
            else
            {
                msg = LanguageUtils.getText(lanugageID);
            }
        }
    }

    public class ScrollMessageProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "ScrollMessageProxy";

        private Queue<ScrollMessage> m_scrollMessage = new Queue<ScrollMessage>();

        #endregion

        // Use this for initialization
        public ScrollMessageProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" ScrollMessageProxy register");   
        }


        public override void OnRemove()
        {
            Debug.Log(" ScrollMessageProxy remove");
        }

        public int GetCount()
        {
            return m_scrollMessage.Count;
        }

        public void Dequeue()
        {
           if(m_scrollMessage.Count>0)
            {
                CoreUtils.uiManager.ShowUI(UI.s_scrollMessage,null,m_scrollMessage.Dequeue());
            }
        }

        public void Enqueue(int languageID,object[] args = null)
        {
            Enqueue(new Game.ScrollMessage(languageID,args));
        }

        public void Enqueue(Chat_MarqueeNotify.request request)
        {
            ScrollMessage msg = new ScrollMessage(request);
            Enqueue(msg);
        }
        
        public void Enqueue(ScrollMessage msg)
        {
            m_scrollMessage.Enqueue(msg);
            if (!CoreUtils.uiManager.ExistUI(UI.s_scrollMessage))
            {
                Dequeue();
            }
        }
    }
}