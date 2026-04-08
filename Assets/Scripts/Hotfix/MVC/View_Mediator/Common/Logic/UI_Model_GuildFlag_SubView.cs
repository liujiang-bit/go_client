// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 7, 2020
// Update Time         :    Tuesday, April 7, 2020
// Class Description   :    UI_Model_GuildFlag_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Model_GuildFlag_SubView : UI_SubView
    {
        public int m_flagID;
        public int m_flagIndex;
        
        public int m_flagColorID;
        public int m_flagColorIndex;

        public int m_flagLogoID;
        public int m_flagLogoIndex;

        public int m_flagLogoColorID;
        public int m_flagLogoColorIndex; 
        
        
        private AllianceProxy m_allianceProxy;
        
        private List<AllianceSignDefine> m_signFlag;

        private List<AllianceSignDefine> m_signFlagColor;

        private List<AllianceSignDefine> m_signFlagLogo;

        private List<AllianceSignDefine> m_singFlagLogoColor;


      

        public void Clone(UI_Model_GuildFlag_SubView view)
        {
            m_flagID = view.m_flagID;
            m_flagIndex = view.m_flagIndex;
            m_flagColorID = view.m_flagColorID;
            m_flagColorIndex = view.m_flagColorIndex; 
            m_flagLogoID = view.m_flagLogoID;
            m_flagLogoIndex = view.m_flagLogoIndex;
            m_flagLogoColorID = view.m_flagLogoColorID;
            m_flagLogoColorIndex = view.m_flagLogoColorIndex;
            
           
        }

        

        private void Init()
        {
            if (m_allianceProxy == null)
            {
                m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

                m_signFlag = m_allianceProxy.SiginFlagSimples();
                m_signFlagColor = m_allianceProxy.SiginFlagSimpleColors();

                m_signFlagLogo = m_allianceProxy.SiginFlags();
                m_singFlagLogoColor = m_allianceProxy.SiginFlagColors();
            }
        }

       
        public void RandomFlag()
        {
            
            Init();
            
            m_flagIndex = Random.Range(0, m_signFlag.Count-1);

            m_flagID = m_signFlag[m_flagIndex].ID;

            m_flagColorIndex = Random.Range(0, m_signFlagColor.Count);
            m_flagColorID = m_signFlagColor[m_flagColorIndex].ID;

            

            m_flagLogoIndex = Random.Range(0, m_signFlagLogo.Count-1);
            m_flagLogoID = m_signFlagLogo[m_flagLogoIndex].ID;
            
//            Debug.Log(m_flagLogoIndex+"  "+m_flagLogoID);

            m_flagLogoColorIndex = Random.Range(0, m_singFlagLogoColor.Count-1);
            m_flagLogoColorID = m_singFlagLogoColor[m_flagLogoColorIndex].ID;
            
            setFlag();
        }
        
        public List<long> GetSigns()
        {
            return new List<long>(new []{(long)m_flagID,(long)m_flagColorID,(long)m_flagLogoID,(long)m_flagLogoColorID});
        }
        public void setData(GuildInfo info)
        {
            
            if (info!=null)
            {
                m_flagID = (int) info.signs[0];
                m_flagColorID = (int) info.signs[1];
                m_flagLogoID = (int) info.signs[2];
                m_flagLogoColorID = (int)info.signs[3];
                
                setFlag();
            }
        }
        
        public void setData(GuildInfoEntity info)
        {
            
            if (info!=null)
            {
                m_flagID = (int) info.signs[0];
                m_flagColorID = (int) info.signs[1];
                m_flagLogoID = (int) info.signs[2];
                m_flagLogoColorID = (int)info.signs[3];
                
                setFlag();
            }
        }

        public void setData(List<System.Int64> info)
        {
            if (info != null)
            {
                m_flagID = (int) info[0];
                m_flagColorID = (int) info[1];
                m_flagLogoID = (int) info[2];
                m_flagLogoColorID = (int) info[3];
                setFlag();
            }
        }

        public void setData(List<int> info)
        {
            if (info != null && info.Count >= 4)
            {
                m_flagID = info[0];
                m_flagColorID = info[1];
                m_flagLogoID = info[2];
                m_flagLogoColorID = info[3];
                setFlag();
            }
        }



        public void setFlag()
        {

            Init();
//            var data = m_allianceProxy.GetAlliance();
//            m_img_flag_noali_PolygonImage.gameObject.SetActive(data == null);
            
            ClientUtils.ImageSetColor(m_img_flag_PolygonImage,
                m_allianceProxy.GetSignByID(m_flagColorID).colour);
            
            ClientUtils.LoadSprite(m_img_flag_PolygonImage,m_allianceProxy.GetSignByID(m_flagID).realityIcon);
            
            
            ClientUtils.ImageSetColor(m_img_flagBigIcon_PolygonImage,
                m_allianceProxy.GetSignByID(m_flagLogoColorID).colour);
            
            ClientUtils.LoadSprite(m_img_flagBigIcon_PolygonImage,m_allianceProxy.GetSignByID(m_flagLogoID).realityIcon,false);
            
        }

        public void setDefaultFlag(string flagStr)
        {
            ClientUtils.LoadSprite(m_img_flag_PolygonImage, flagStr);
        }
    }
}