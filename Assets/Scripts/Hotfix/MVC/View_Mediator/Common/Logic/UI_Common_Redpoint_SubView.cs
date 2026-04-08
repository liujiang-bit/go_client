// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, June 5, 2020
// Update Time         :    Friday, June 5, 2020
// Class Description   :    UI_Common_Redpoint_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Common_Redpoint_SubView : UI_SubView
    {

        public void ShowRedPoint(int count)
        {
            gameObject.SetActive(count>=0);
            m_lbl_num_LanguageText.text = count.ToString();
            m_img_redpoint_PolygonImage.gameObject.SetActive(false);
            m_img_redpointNum_PolygonImage.gameObject.SetActive(true);
            m_img_redpointNum_PolygonImage.gameObject.SetActive(count > 0);
            if (count > 99)
            {
                m_lbl_num_LanguageText.text = "99+";
            }
        }
        
        public void ShowSmallRedPoint(int count)
        {
            gameObject.SetActive(count>0);
            m_img_redpoint_PolygonImage.gameObject.SetActive(count >0);
            m_img_redpointNum_PolygonImage.gameObject.SetActive(false);
        }
        public void ShowStringRedPoint(string str)
        {
            gameObject.SetActive(true);
            m_lbl_num_LanguageText.text = str;
            m_img_redpoint_PolygonImage.gameObject.SetActive(false);
            m_img_redpointNum_PolygonImage.gameObject.SetActive(true);
        }


        public void HideRedPoint()
        {
            gameObject.SetActive(false);
        }
    }
}