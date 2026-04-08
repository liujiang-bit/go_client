// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月10日
// Update Time         :    2020年3月10日
// Class Description   :    UI_Pop_WorldObjectVillageCaveMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game
{
    public class VillageRewardData
    {
        public const int ItemType = 2;
        public const int ArmType = 1;
    }
    public class UI_Pop_WorldObjectVillageCaveMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Pop_WorldObjectVillageCaveMediator";
        private RssProxy m_RssProxy;
        private WorldMapObjectProxy m_worldProxy;
        private int rssId;
        private MapObjectInfoEntity data;

        private bool m_first = true;
        #endregion

        //IMediatorPlug needs
        public UI_Pop_WorldObjectVillageCaveMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Pop_WorldObjectVillageCaveView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
               
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

        public override void OpenAniEnd()
        {
        }

        public override void WinFocus()
        {
        }

        public override void WinClose()
        {
            if (data != null)
            {
                if (data.rssType == RssType.Village)
                {
                    if (data.villageRewardDataDefine != null)
                    {
                        if (data.villageRewardDataDefine.type == VillageRewardData.ArmType)
                        {
                            ArmsDefine armsDefine =
                              CoreUtils.dataService.QueryRecord<ArmsDefine>(data.villageRewardDataDefine.typeData);
                            if (armsDefine != null)
                            {
                                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                                mt.FlyPowerUpEffect(view.m_UI_Model_RewardGet.gameObject, view.m_UI_Model_RewardGet.m_root_RectTransform, Vector3.one);
                                if (data.villageRewardDataDefine.multiple > 1)
                                {
                                    Tip.CreateTip(LanguageUtils.getTextFormat(data.villageRewardDataDefine.l_desc2, LanguageUtils.getText(armsDefine.l_armsID), data.villageRewardDataDefine.typeNum), Tip.TipStyle.Top).Show();
                                }
                                else
                                {
                                    Tip.CreateTip(LanguageUtils.getTextFormat(500515, LanguageUtils.getText(armsDefine.l_armsID), data.villageRewardDataDefine.typeNum), Tip.TipStyle.Top).Show();
                                }

                            }
                        }
                        else if (data.villageRewardDataDefine.type == VillageRewardData.ItemType)
                        {
                            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                            mt.FlyItemEffect(data.villageRewardDataDefine.typeData, data.villageRewardDataDefine.typeNum, view.m_UI_Model_RewardGet.m_root_RectTransform);
                        }
                    }
                    else
                    {
                        Debug.Log("data.villageRewardDataDefine == null");
                    }
                }
                else
                {
                    Debug.Log("data == null");
                }
        
  
            }
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            rssId = (int)((long) view.data);
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            InitViewData();
        }

        protected override void BindUIEvent()
        {
            view.m_btn_descinfo_GameButton.onClick.AddListener(OnDesInfoClick);
            view.m_btn_descBack_GameButton.onClick.AddListener(OnCloseDesInfoClick);
            view.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(OnBtnClick);
        }

        protected override void BindUIData()
        {
            view.m_UI_Common_PopFun.InitSubView(data);
        }

        private void InitViewData()
        {
            data = m_worldProxy.GetWorldMapObjectByobjectId(rssId);
            if (data != null)
            {
                ChangeWinPos();
                if (data.mapFixPointDefine != null)
                {              
                    view.m_lbl_position_LanguageText.text = LanguageUtils.getTextFormat(300032, data.mapFixPointDefine.posX * 100 / 600, data.mapFixPointDefine.posY * 100 / 600);
                }

                if (data.resourceGatherTypeDefine != null)
                {
                    view.m_lbl_name_LanguageText.text = LanguageUtils.getText(data.resourceGatherTypeDefine.l_nameId);
                }

                if (data.rssType == RssType.Village)
                {
                    SetVillageViewData();
                }
                else if (data.rssType == RssType.Cave)
                {
                    view.m_pl_villageReward.gameObject.SetActive(false);
                    view.m_pl_caveReward.gameObject.SetActive(true);
                    view.m_UI_Model_StandardButton_Yellow.m_lbl_Text_LanguageText.text = LanguageUtils.getText(181146);
                    if (data.resourceGatherTypeDefine != null)
                    {
                        view.m_lbl_cave_rewardName_LanguageText.text =
                            LanguageUtils.getText(data.resourceGatherTypeDefine.l_nameId);
                        string levelName = string.Empty;
                        switch (data.resourceGatherTypeDefine.level)
                        {
                            case 1:
                                levelName = LanguageUtils.getText(500505);
                                break;
                            case 2:
                                levelName = LanguageUtils.getText(500504);
                                break;
                            case 3:
                                levelName = LanguageUtils.getText(500503);
                                break;
                        }

                        view.m_lbl_cave_rewardCount_LanguageText.text = levelName;
                    }

                    OnCloseDesInfoClick();
                }
            }
        }

        private void SetVillageViewData()
        {
            if (data.villageRewardDataDefine != null)
            {
                view.m_pl_villageReward.gameObject.SetActive(true);
                view.m_pl_caveReward.gameObject.SetActive(false);
                if (data.villageRewardDataDefine.multiple > 1)
                {
                    ClientUtils.UIAddEffect(RS.UI_Common_Crit1,view.m_pl_content_Animator.transform, (go) => {
                        UI_Common_Crit1_SubView ui_Common_Crit1_SubView = new UI_Common_Crit1_SubView(go.transform as RectTransform);
                        switch (data.villageRewardDataDefine.multiple)
                        {
                            case 2:
                                ClientUtils.LoadSprite(ui_Common_Crit1_SubView.m_img_num_PolygonImage, RS.ui_num[0]);
                                break;
                            case 5:
                                ClientUtils.LoadSprite(ui_Common_Crit1_SubView.m_img_num_PolygonImage, RS.ui_num[1]);
                                break;
                            case 10:
                                ClientUtils.LoadSprite(ui_Common_Crit1_SubView.m_img_num_PolygonImage, RS.ui_num[2]);
                                break;
                            default:
                                Debug.LogError(" not find multiple ");
                                break;
                        }
        
                    }, true);
                }
                if (data.villageRewardDataDefine.type == VillageRewardData.ArmType)
                {
                    ArmsDefine armsDefine =
                        CoreUtils.dataService.QueryRecord<ArmsDefine>(data.villageRewardDataDefine.typeData);
                    if (armsDefine != null)
                    {
                        view.m_lbl_rewardName_LanguageText.text = LanguageUtils.getText(armsDefine.l_armsID); ;
                        view.m_UI_Model_RewardGet.RefreshSoldier(armsDefine, 0, false);
                        view.m_UI_Model_StandardButton_Yellow.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500507);
                    }
                }
                else if (data.villageRewardDataDefine.type == VillageRewardData.ItemType)
                {
                    ItemDefine itemDefine =  CoreUtils.dataService.QueryRecord<ItemDefine>(data.villageRewardDataDefine.typeData);
                    if (itemDefine != null)
                    {
                        view.m_lbl_rewardName_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
                        view.m_UI_Model_RewardGet.RefreshItem(itemDefine, 0, false);
                        view.m_UI_Model_StandardButton_Yellow.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500506);
                    }    
                }
                view.m_lbl_rewardCount_LanguageText.text = data.villageRewardDataDefine.typeNum.ToString();
                SetVillageDes();
            }
        }


        private void OnDesInfoClick()
        {
            view.m_pl_description_Animator.gameObject.SetActive(true);
            view.m_pl_normalInfo_Animator.gameObject.SetActive(false);
            view.m_UI_Model_StandardButton_Yellow.gameObject.SetActive(false);
            view.m_pl_description_Animator.Play("Show");
            if (data.rssType == RssType.Cave)
            {
                view.m_lbl_desc_LanguageText.text =
                    LanguageUtils.getText(data.resourceGatherTypeDefine.l_descId);
                view.m_btn_descinfo_GameButton.gameObject.SetActive(false);
            }
            else if (data.rssType == RssType.Village)
            {
                SetVillageDes();
            }
        }

        private void OnCloseDesInfoClick()
        {
            view.m_pl_description_Animator.gameObject.SetActive(false);
            view.m_pl_normalInfo_Animator.gameObject.SetActive(true);
            view.m_UI_Model_StandardButton_Yellow.gameObject.SetActive(true);
            if (m_first)
            {
                m_first = false;
            }
            else
            {
                view.m_pl_normalInfo_Animator.Play("Show");
            }
            if (data.rssType == RssType.Cave)
            {
                view.m_lbl_normalInfo_desc_LanguageText.text =
                    LanguageUtils.getText(data.resourceGatherTypeDefine.l_descId2);
                view.m_btn_descinfo_GameButton.gameObject.SetActive(true);
            }
            else if (data.rssType == RssType.Village)
            {
                SetVillageDes();
            }
        }

        private void SetVillageDes()
        {
            view.m_btn_descinfo_GameButton.gameObject.SetActive(false);
            if (data.villageRewardDataDefine != null)
            {
                view.m_lbl_normalInfo_desc_LanguageText.text =
                    LanguageUtils.getText(data.villageRewardDataDefine.l_desc);
            }
        }

        private void OnBtnClick()
        {
            if (data.rssType == RssType.Village)
            {
                CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldObjectVillageCave);
                
            }
            else if (data.rssType == RssType.Cave)
            {
                var scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
                if (!scoutProxy.isScoutBuildingExit())
                {
                    CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldObjectVillageCave);
                    Tip.CreateTip(LanguageUtils.getText(610024)).Show();
                    return;
                }

                Vector2 v2 = new Vector2(data.mapFixPointDefine.posX, data.mapFixPointDefine.posY);
                UI_Pop_ScoutSelectMediator.Param param = new UI_Pop_ScoutSelectMediator.Param();
                param.pos = v2;
                param.targetIndex = data.objectId;
                CoreUtils.uiManager.ShowUI(UI.s_scoutSelectMenu, null, param);
                CoreUtils.uiManager.CloseUI(UI.s_Pop_WorldObjectVillageCave);
            }
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
            var rect = view.m_pl_content_Animator.GetComponent<RectTransform>().rect;

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
            view.m_pl_content_Animator.transform.localPosition = localPos;
        }
    }
}