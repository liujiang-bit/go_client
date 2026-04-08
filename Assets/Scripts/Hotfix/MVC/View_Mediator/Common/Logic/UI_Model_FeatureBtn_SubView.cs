// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Model_FeatureBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Model_FeatureBtn_SubView : UI_SubView
    {
        private bool m_bagIsDelay;

        public void AddListener(UnityAction clickCallback)
        {
            m_btn_btn_GameButton.onClick.AddListener(clickCallback);
        }
        
        
        public void SetRedCount(long count, bool isBag = false)
        {
            this.m_img_redpoint_PolygonImage.gameObject.SetActive(count>0);
            this.m_lbl_count_LanguageText.gameObject.SetActive(count>0);
            if (count>0)
            {
                if (isBag)
                {
                    this.m_lbl_count_LanguageText.text = UIHelper.NumerBeyondFormat((int)count);
                }
                else
                {
                    this.m_lbl_count_LanguageText.text = count.ToString();
                }

                if (count > 99)
                {
                    this.m_lbl_count_LanguageText.text = "99+";
                }
            }
        }

        //延迟更新
        public void DelayUpdateBagReddot()
        {
            if (m_bagIsDelay)
            {
                return;
            }
            m_bagIsDelay = true;
            Timer.Register(0.5f, UpdateBagReddot);
        }

        //立即更新
        public void UpdateBagReddot()
        {
            if (gameObject == null)
            {
                return;
            }
            m_bagIsDelay = false;
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            long count = bagProxy.GetBagReddotTotal();
            SetRedCount(count, true);
        }
    }
}