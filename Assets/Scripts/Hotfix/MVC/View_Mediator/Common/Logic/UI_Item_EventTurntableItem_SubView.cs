// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Saturday, 17 October 2020
// Update Time         :    Saturday, 17 October 2020
// Class Description   :    UI_Item_EventTurntableItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_EventTurntableItem_SubView : UI_SubView
    {
        private ItemInfoBody m_itemInfo;
        private int m_getNum;
        private bool m_isInit;
        private ItemDefine m_itemDefine;

        public void Refresh(ItemInfoBody itemInfo)
        {
            if (!m_isInit)
            {
                m_btn_node_GameButton.onClick.AddListener(OnClick);
                m_isInit = true;
            }
            m_getNum = 0;
            m_itemInfo = itemInfo;
            var define = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemInfo.ItemId);
            m_itemDefine = define;
            if (define != null)
            {
                ClientUtils.LoadSprite(m_img_item_PolygonImage, define.itemIcon);

                if (define.l_topID < 1)
                {
                    m_pl_desc_bg_PolygonImage.transform.gameObject.SetActive(false);
                }
                else
                {
                    m_pl_desc_bg_PolygonImage.transform.gameObject.SetActive(true);
                    m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(define.l_topID), ClientUtils.FormatComma(define.topData));
                }

                if ((define.quality - 1) < RS.TurntableActivityItemIcon.Length)
                {
                    ClientUtils.LoadSprite(m_img_rare_PolygonImage, RS.TurntableActivityItemIcon[define.quality - 1]);
                }
            }
            m_lbl_num_LanguageText.text = ClientUtils.FormatComma(itemInfo.ItemNum);
            m_img_get_PolygonImage.gameObject.SetActive(false);
        }

        public void AddGetNum()
        {
            m_getNum = m_getNum + 1;
            m_lbl_get_LanguageText.text = ClientUtils.FormatComma(m_getNum);
            m_img_get_PolygonImage.gameObject.SetActive(true);
        }

        public void Reset()
        {
            m_getNum = 0;
            m_img_get_PolygonImage.gameObject.SetActive(false);
        }

        public long GetPackageId()
        {
            return m_itemInfo.PackageId;
        }

        private void OnClick()
        {
            if (m_itemDefine == null)
            {
                return;
            }
            var define = m_itemDefine;
            string descFormat = string.Format(LanguageUtils.getText(define.l_desID), ClientUtils.FormatComma(define.desData1), ClientUtils.FormatComma(define.desData2));
            HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(5000);
            string str = LanguageUtils.getTextFormat(tipDefine.l_typeID,
                                                     LanguageUtils.getText(define.l_nameID),
                                                     descFormat);
            HelpTip.CreateTip(str, m_btn_node_GameButton.GetComponent<RectTransform>()).SetStyle(HelpTipData.Style.arrowDown).SetOffset(20).Show();
        }
    }
}