// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    UI_Model_PlayerHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System;

namespace Game {
    public partial class UI_Model_PlayerHead_SubView : UI_SubView
    {

        public void LoadHeadImg(string name = null,Action callback = null)
        {
            if(string.IsNullOrEmpty(name))
            {
                ClientUtils.LoadSprite(m_UI_Model_PlayerHead_PolygonImage, RS.RoleCommonHead, false,callback);
            }
            else
            {
                ClientUtils.LoadSprite(m_UI_Model_PlayerHead_PolygonImage, name,false,callback);
            }
        }

        public void LoadHeadCountry(int country)
        {
            CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>(country);

            if (define!=null)
            {
                ClientUtils.LoadSprite(m_UI_Model_PlayerHead_PolygonImage, define.playerImg);
            }
            else
            {
                LoadPlayerIcon(country);
            }
        }

        public void LoadFrameImg(string name = null)
        {
            if(string.IsNullOrEmpty(name))
            {
                ClientUtils.LoadSprite(m_img_circle_PolygonImage, RS.RoleCommonHeadFrame);
            }
            else
            {
                ClientUtils.LoadSprite(m_img_circle_PolygonImage, name);
            }

        }

        public void LoadPlayerIcon(Action callback = null)
        {
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if(playerProxy!=null&&playerProxy.CurrentRoleInfo!=null)
            {
                if(playerProxy.CurrentRoleInfo.headId > 0)
                {
                    PlayerHeadDefine define = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)playerProxy.CurrentRoleInfo.headId);
                    if (define != null)
                    {
                        LoadHeadImg(define.icon, callback);
                    }
                    else
                    {
                        LoadHeadImg(string.Empty,callback);
                    }
                }
                else
                {
                    LoadHeadImg(string.Empty,callback);
                }
                PlayerHeadDefine frameDefine = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)playerProxy.CurrentRoleInfo.headFrameID);
                if(frameDefine!=null)
                {
                    LoadFrameImg(frameDefine.icon);
                }
                else
                {
                    LoadFrameImg();
                }
            }
        }

        public void LoadPlayerIcon(long headID, long frameId,Action callback = null)
        {
            PlayerHeadDefine define = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)headID);
            if (define != null)
            {
                LoadHeadImg(define.icon, callback);
            }
            else
            {
                LoadHeadImg(RS.PlayerDefaultHeadIcon	, callback);
            }
            PlayerHeadDefine frameDefine = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)frameId);
            if (frameDefine != null)
            {
                LoadFrameImg(frameDefine.icon);
            }
            else
            {
                LoadFrameImg();
            }
        }
        public void LoadPlayerIcon(long headID ,Action callback = null)
        {
            PlayerHeadDefine define = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)headID);
            if (define != null)
            {
                LoadHeadImg(define.icon, callback);
            }
            else
            {
                LoadHeadImg(string.Empty, callback);
            }
        }

        public void HideFrameImg()
        {
            m_img_circle_PolygonImage.gameObject.SetActive(false);
        }

        public void ShowFrameImg()
        {
            m_img_circle_PolygonImage.gameObject.SetActive(true);
        }
    }
}