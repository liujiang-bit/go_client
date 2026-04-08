// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月24日
// Update Time         :    2020年2月24日
// Class Description   :    UI_Item_WorldArmyCmdAp_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game
{
    public partial class UI_Item_WorldArmyCmdAp_SubView : UI_SubView
    {

        private void InitActive()
        {
            if (!this.m_img_ap1_PolygonImage.gameObject.activeSelf)
            {
                this.m_img_ap1_PolygonImage.gameObject.SetActive(true);
            }

            if (!this.m_img_ap2_PolygonImage.gameObject.activeSelf)
            {
                this.m_img_ap2_PolygonImage.gameObject.SetActive(true);
            }
            
            if (!this.m_img_ap3_PolygonImage.gameObject.activeSelf)
            {
                this.m_img_ap3_PolygonImage.gameObject.SetActive(true);
            }
            
            if (!this.m_img_ap4_PolygonImage.gameObject.activeSelf)
            {
                this.m_img_ap4_PolygonImage.gameObject.SetActive(true);
            }
        }


        public void SetImgActive(long count, long num)
        {
            InitActive();
            long sum = count / 4;
            float count1 = count * 0.25f;
            float count2 = count * 0.5f;
            float count3 = count * 0.75f;
            long addnum = 0;
            bool isShow = num > 0;
            bool isShow1 = num >= count1;
            bool isShow2 = num >= count2;
            bool isShow3 = num >= count3;
            if (this.m_img_ap1_PolygonImage != null)
            {
                if (isShow)
                {                
                    this.m_img_ap1_PolygonImage.fillAmount = num <= count1 / 2 ? 0.5f : 1;
                }
                else
                {
                    this.m_img_ap1_PolygonImage.fillAmount = 0;
                }

            }

            if (this.m_img_ap2_PolygonImage != null)
            {
                if (isShow1)
                {                   
                    addnum = num - (int) count1;
                    this.m_img_ap2_PolygonImage.fillAmount = addnum < sum/2 ? 0.5f : 1;
                }
                else
                {
                    this.m_img_ap2_PolygonImage.fillAmount = 0;
                }
            }

            if (this.m_img_ap3_PolygonImage != null)
            {
                if (isShow2)
                {                  
                    addnum = num - (int) count2;
                    this.m_img_ap3_PolygonImage.fillAmount = addnum < sum/2 ? 0.5f : 1;
                }
                else
                {
                    this.m_img_ap3_PolygonImage.fillAmount = 0;
                }
            }

            if (this.m_img_ap4_PolygonImage != null)
            {
                if (isShow3)
                {                   
                    addnum = num - (int) count3;
                    this.m_img_ap4_PolygonImage.fillAmount =  addnum < sum/2 ? 0.5f : 1;
                }
                else
                {
                    this.m_img_ap4_PolygonImage.fillAmount = 0;
                }
            }
        }
    }
}