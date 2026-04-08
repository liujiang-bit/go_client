// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月14日
// Update Time         :    2020年9月14日
// Class Description   :    UI_Item_MailType20_SubView 联盟标记 图片邮件
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_MailType20_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_btn_go.AddClickEvent(OnGo);
                m_isInit = true;
            }
            m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), mailInfo.titleContents);
            m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_subheadingID), mailInfo.subTitleContents);
            m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(mailInfo.sendTime);
            if (!string.IsNullOrEmpty(mailDefine.poster))
            {
                ClientUtils.LoadSprite(m_UI_Item_MailTitle.m_img_bg_PolygonImage, mailDefine.poster);
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }

            if (mailInfo.guildEmail != null)
            {
                MapMarkerTypeDefine define = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)mailInfo.guildEmail.markerId);
                if (define == null)
                {
                    return;
                }

                //联盟标记
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, define.iconImg);
                
                //描述
                List<string> list = new List<string>();
                list.Add(string.Format("{0},{1}", "", mailInfo.guildEmail.roleName));
                if (string.IsNullOrEmpty(mailInfo.guildEmail.markerDesc))
                {
                    list.Add("");
                }
                else
                {
                    list.Add(string.Format(":{0}", mailInfo.guildEmail.markerDesc));
                }
                m_lbl_mes_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), list);
            }
        }

        //前往
        private void OnGo()
        {
            if (m_emailInfo == null && m_emailInfo.guildEmail == null)
            {
                return;
            }
            if (m_emailInfo.guildEmail.pos == null)
            {
                Debug.LogError("m_emailInfo.guildEmail.pos is null");
                return;
            }
            Vector2 pos = PosHelper.ServerPosToLocal(m_emailInfo.guildEmail.pos);
            CoreUtils.uiManager.CloseUI(UI.s_Email);
            GameHelper.CoordinateJump((int)pos.x, (int)pos.y);
        }
    }
}