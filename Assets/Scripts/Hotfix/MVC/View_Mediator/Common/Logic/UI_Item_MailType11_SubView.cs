// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月15日
// Update Time         :    2020年6月15日
// Class Description   :    UI_Item_MailType11_SubView 邮件 联盟援助
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_MailType11_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private bool m_isInitList;
        public Dictionary<string, GameObject> AssetDic;
        private List<EmailInfoEntity> m_assistList;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo, List<EmailInfoEntity> assistList)
        {
            m_assistList = assistList;
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
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

            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ListViewItemByIndex;
                m_sv_list_view_ListView.SetInitData(AssetDic, funcTab);
                m_isInitList = true;
            }
            int showLimit = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).emailResourceHelpSave;
            int num = (m_assistList.Count > showLimit) ? showLimit : m_assistList.Count;
            m_sv_list_view_ListView.FillContent(num);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            EmailInfoEntity emailInfo = m_assistList[listItem.index];

            UI_Item_Assistance_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_Assistance_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
            }
            else
            {
                subView = listItem.data as UI_Item_Assistance_SubView;
            }
            subView.Refresh(emailInfo);
        }
    }
}