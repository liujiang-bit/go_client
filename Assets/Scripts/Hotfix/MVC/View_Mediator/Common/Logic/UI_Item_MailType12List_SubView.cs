// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_Item_MailType12List_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Item_MailType12List_SubView : UI_SubView
    {
        private bool m_isInit;
        private EmailProxy m_emailProxy;
        private EmailInfoEntity m_emailInfo;

        public void Refresh(EmailInfoEntity emailInfo)
        {
            m_emailInfo = emailInfo;
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_btn_look_GameButton.onClick.AddListener(OnLook);
                m_isInit = true;
            }
            m_img_reddot_PolygonImage.gameObject.SetActive(emailInfo.status == 0);
            m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(emailInfo.sendTime);

            if (emailInfo.guildEmail != null)
            {
                m_UI_Model_PlayerHead.LoadPlayerIcon(emailInfo.guildEmail.roleHeadId, emailInfo.guildEmail.roleHeadFrameId);
            }

            MailDefine mailDefine = CoreUtils.dataService.QueryRecord<MailDefine>((int)emailInfo.emailId);
            if (mailDefine != null)
            {
                m_lbl_message_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), emailInfo.emailContents);
            }  
        }

        //查看
        private void OnLook()
        {
            if (m_emailInfo.guildEmail == null)
            {
                return;
            }
            Guild_CheckBoardMessage.request request = new Guild_CheckBoardMessage.request();
            request.guildId = m_emailInfo.guildEmail.guildId;
            request.messageIndex = m_emailInfo.guildEmail.boardMessageIndex;
            AppFacade.GetInstance().SendSproto(request);

        }
    }
}