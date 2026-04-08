// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月11日
// Update Time         :    2020年3月11日
// Class Description   :    UI_Pop_WorldObjectVillageCaveFinishMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using ICSharpCode.SharpZipLib.Core;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Pop_WorldObjectVillageCaveFinishMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_WorldObjectVillageCaveFinishMediator";
        private int id;
        private WorldMapObjectProxy m_RssProxy;
        MapObjectInfoEntity data;
        #endregion

        //IMediatorPlug needs
        public UI_Pop_WorldObjectVillageCaveFinishMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_WorldObjectVillageCaveFinishView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            id = (int)((long)view.data);
             data = m_RssProxy.GetWorldMapObjectByobjectId(id);
            if (data != null)
            {
                ChangeWinPos();
                view.m_lbl_name_LanguageText.text =
                    LanguageUtils.getText(data.resourceGatherTypeDefine.l_nameId);
                view.m_lbl_normalInfo_desc_LanguageText.text = LanguageUtils.getText(500502);
                view.m_lbl_position_LanguageText.text = LanguageUtils.getTextFormat(300032, data.mapFixPointDefine.posX * 100 / 600, data.mapFixPointDefine.posY * 100 / 600);
            }
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_UI_Common_PopFun.InitSubView(data);
        }

        #endregion
        //改变窗口位置
        private void ChangeWinPos()
        {
            if (data.gameobject == null)
            {
                return;
            }

            //屏幕坐标转界面局部坐标
            Vector2 localPos;
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), data.gameobject.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(view.m_pl_pos.gameObject.GetComponent<RectTransform>(),
                                                                    pos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            RectTransform viewRect = view.gameObject.GetComponent<RectTransform>();
            var rect = view.m_pl_content.rect;

            float diffNum = 50f;

            // 左
            if (localPos.x < viewRect.rect.width / 2)
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    localPos.y = localPos.y - (rect.height / 2) - diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideTop_PolygonImage.transform.localPosition = new Vector2(offset,
                                                                           view.m_img_arrowSideTop_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                                                   view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x + (rect.width / 2) + diffNum;
                    view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                }
            }
            // 右
            else
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.y - (viewRect.rect.height - rect.height / 2);
                        view.m_img_arrowSideR_PolygonImage.transform.localPosition = new Vector2(view.m_img_arrowSideR_PolygonImage.transform.localPosition.x,
                                                                                                 offset);
                        view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);

                        localPos.x = localPos.x - (rect.width / 2) - diffNum;
                        localPos.y = viewRect.rect.height - rect.height / 2;
                    }
                    else
                    {
                        localPos.y = localPos.y - (rect.height / 2) - diffNum;
                        view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                    }
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.x - (viewRect.rect.width - rect.width / 2);
                        view.m_img_arrowSideButtom_PolygonImage.transform.localPosition = new Vector2(offset,
                                                                                                       view.m_img_arrowSideButtom_PolygonImage.transform.localPosition.y);
                        localPos.x = (viewRect.rect.width - rect.width / 2);
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x - (rect.width / 2) - diffNum;
                    view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                }
            }
            view.m_pl_content.transform.localPosition = localPos;
        }
    }
}