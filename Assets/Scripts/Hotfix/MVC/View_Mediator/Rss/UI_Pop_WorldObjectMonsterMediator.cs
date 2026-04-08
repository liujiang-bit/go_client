// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月21日
// Update Time         :    2020年1月21日
// Class Description   :    UI_Pop_WorldObjectMonsterMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Hotfix;
using PureMVC.Interfaces;
using SprotoType;
using System;
using System.Text.RegularExpressions;
using Data;

namespace Game
{
    public class UI_Pop_WorldObjectMonsterMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Pop_WorldObjectMonsterMediator";

        private MonsterProxy m_MonsterProxy;
        private WorldMapObjectProxy m_worldProxy;        
        private PlayerProxy m_playerProxy;
        private RssProxy m_RssProxy;
        private TroopProxy m_TroopProxy;

        private int monsterId;
        private MapObjectInfoEntity data;
        private UI_Item_ItemSize65_SubView[] itemArray;

        private Timer m_timer;

        #endregion

        //IMediatorPlug needs
        public UI_Pop_WorldObjectMonsterMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Pop_WorldObjectMonsterView view;

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
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        public override void OnRemove()
        {
            base.OnRemove();

            itemArray = null;

            CancelTimer();
        }

        protected override void InitData()
        {
            m_MonsterProxy = AppFacade.GetInstance().RetrieveProxy(MonsterProxy.ProxyNAME) as MonsterProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;            
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;

            itemArray = new UI_Item_ItemSize65_SubView[6];
            itemArray[0] = view.m_UI_Item_WorldObjInfoTDrop.m_UI_Item_item1;
            itemArray[1] = view.m_UI_Item_WorldObjInfoTDrop.m_UI_Item_item2;
            itemArray[2] = view.m_UI_Item_WorldObjInfoTDrop.m_UI_Item_Item3;
            itemArray[3] = view.m_UI_Item_WorldObjInfoTDrop.m_UI_Item_Item4;
            itemArray[4] = view.m_UI_Item_WorldObjInfoTDrop.m_UI_Item_Item5;
            itemArray[5] = view.m_UI_Item_WorldObjInfoTDrop.m_UI_Item_Item6;

            Init();
        }

        protected override void BindUIEvent()
        {
            view.m_btn_descBack_GameButton.onClick.AddListener(() => { OpenDesPanel(false); });
            view.m_btn_descinfo_GameButton.onClick.AddListener(() => { OpenDesPanel(true); });
        }

        protected override void BindUIData()
        {
            view.m_UI_Common_PopFun.InitSubView(data);
        }

        private void Init()
        {
            monsterId = (int)view.data;
            data = m_worldProxy.GetWorldMapObjectByobjectId(monsterId);
            if (data != null)
            {
                ChangeWinPos();

                view.m_lbl_name_LanguageText.text = string.Format("{0}", LanguageUtils.getText(data.monsterDefine.l_nameId));
                string x = (data.gameobject.transform.position.x / 6).ToString("F0");
                string y = (data.gameobject.transform.position.z / 6).ToString("F0");
                view.m_lbl_position_LanguageText.text = LanguageUtils.getTextFormat(300032, x, y);
                view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(data.monsterDefine.l_descId);
                
                ClientUtils.LoadSprite(view.m_img_MonsterHead_PolygonImage, data.monsterDefine.headIcon);

                //城寨
                if (data.objectType == (long)RssType.BarbarianCitadel)
                {
                    OnMonsterCityInit();
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.BarbarianCity);
                }
                //圣地守护者
                else if (data.objectType == (long)RssType.Guardian)
                {
                    OnGuardianInit();
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.Guardian);
                }
                //召唤类型怪物
                else if (data.objectType == (long)RssType.SummonAttackMonster || data.objectType == (long)RssType.SummonConcentrateMonster)
                {
                    OnSummonMonsterInit();
                }
                else
                {
                    OnBarbarianDataInit();
                }

                StartTimer(); 

                MonsterDataUIData monsterDataUiData = m_MonsterProxy.GetMonsterDataUIData(monsterId);
                if (monsterDataUiData != null)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        itemArray[i].gameObject.SetActive(false);

                        if (i < monsterDataUiData.type.Count)
                        {
                            itemArray[i].gameObject.SetActive(true);

                            float offset = itemArray[i].m_UI_Model_Item.m_btn_animButton_GameButton.GetComponent<RectTransform>().sizeDelta.y / 4;

                            int tempi = i;

                            if (monsterDataUiData.type[i] == ItemType.Item && monsterDataUiData.itemDefine[i] != null)
                            {
                                itemArray[i].m_UI_Model_Item.Refresh(monsterDataUiData.itemDefine[i], monsterDataUiData.num[i],true);                                
                                HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(5000);
                                string des=LanguageUtils.getTextFormat(monsterDataUiData.itemDefine[tempi].l_desID, ClientUtils.FormatComma(monsterDataUiData.itemDefine[tempi].desData1), ClientUtils.FormatComma(monsterDataUiData.itemDefine[tempi].desData2));
                                string str = LanguageUtils.getTextFormat(tipDefine.l_typeID,
                                    LanguageUtils.getText(monsterDataUiData.itemDefine[tempi].l_nameID),
                                    des);                            
                                itemArray[i].m_UI_Model_Item.AddBtnListener(() =>
                                {
                                    HelpTip.CreateTip(str, itemArray[tempi].m_UI_Model_Item.m_btn_animButton_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                                });

                            }
                            else if (monsterDataUiData.type[i] == ItemType.Currency && monsterDataUiData.currencyDefine[i] != null)
                            {
                                itemArray[i].m_UI_Model_Item.Refresh(monsterDataUiData.currencyDefine[i],monsterDataUiData.num[i]);
                                itemArray[i].m_UI_Model_Item.AddBtnListener(() => {
                                    HelpTip.CreateTip(LanguageUtils.getText(monsterDataUiData.currencyDefine[tempi].l_desID), itemArray[tempi].m_UI_Model_Item.m_btn_animButton_GameButton.transform)
                                    .SetStyle(HelpTipData.Style.arrowDown)
                                    .SetOffset(offset)
                                    .Show();
                                });
                            }
                        }
                    }
                }

                view.m_ck_Situ_GameToggle.isOn = m_playerProxy.CurrentRoleInfo.situStation;
            }
        }

        private void SetBtnActive()
        {
            view.m_UI_Item_WorldObjectPopBtns.m_pl_1g.gameObject.SetActive(false);
            view.m_UI_Item_WorldObjectPopBtns.m_pl_2g_GridLayoutGroup.gameObject.SetActive(true);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn1.gameObject.SetActive(false);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.gameObject.SetActive(true);
        }

        private void OnMonsterCityInit()
        {
            SetBtnActive();
            view.m_pl_inSitu.gameObject.SetActive(false);
            view.m_lbl_recommend_LanguageText.text = LanguageUtils.getTextFormat(500209, data.monsterDefine.armCnt.ToString("N0"), data.monsterDefine.armLevel);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500014);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                FightHelper.Instance.Concentrate((int) data.objectId);
            });
        }        

        private void OnGuardianInit()
        {
            SetBtnActive();
            view.m_lbl_recommend_LanguageText.text = LanguageUtils.getTextFormat(500209, data.monsterDefine.armCnt.ToString("N0"), data.monsterDefine.armLevel);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500012);
            view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                m_TroopProxy.SituStation = view.m_ck_Situ_GameToggle.isOn;
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                FightHelper.Instance.AttackMonster((int)data.objectId);
            });
        }

        private void OnSummonMonsterInit()
        {
            //只能集结挑战
            if (data.monsterDefine.battleType == 2)
            {
                SetBtnActive();

                view.m_pl_inSitu.gameObject.SetActive(false);

                view.m_lbl_recommend_LanguageText.text = LanguageUtils.getTextFormat(500209, data.monsterDefine.armCnt.ToString("N0"), data.monsterDefine.armLevel);

                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500014);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                    FightHelper.Instance.Concentrate((int)data.objectId);
                });
            }
            //可单人挑战也可集结挑战
            else if (data.monsterDefine.battleType == 3)
            {
                view.m_UI_Item_WorldObjectPopBtns.m_pl_1g.gameObject.SetActive(false);
                view.m_UI_Item_WorldObjectPopBtns.m_pl_2g_GridLayoutGroup.gameObject.SetActive(false);
                view.m_UI_Item_WorldObjectPopBtns.m_pl_3g_GridLayoutGroup.gameObject.SetActive(true);

                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_3gbtn1.gameObject.SetActive(false);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_3gbtn2.gameObject.SetActive(true);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_3gbtn3.gameObject.SetActive(true);

                view.m_lbl_recommend_LanguageText.text = string.Empty;

                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_3gbtn3.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500014);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_3gbtn3.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                    FightHelper.Instance.Concentrate((int)data.objectId);
                });

                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_3gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500012);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_3gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                    FightHelper.Instance.AttackMonster((int)data.objectId);
                });
            }
            //只能单人挑战（或者配置异常）
            else
            {
                SetBtnActive();

                view.m_lbl_recommend_LanguageText.text = string.Empty;

                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500012);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    m_TroopProxy.SituStation = view.m_ck_Situ_GameToggle.isOn;
                    CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                    FightHelper.Instance.AttackMonster((int)data.objectId);
                });
            }

            if (data.monsterDefine.battleType != 1 && data.monsterDefine.battleType != 2 && data.monsterDefine.battleType != 3)
                CoreUtils.logService.Error($"monsterDefine battleType error. ID:{data.monsterDefine.ID}");
        }

        private void OnBarbarianDataInit()
        {
            if (data.monsterDefine.level > m_playerProxy.CurrentRoleInfo.barbarianLevel + 1)
            {
                view.m_pl_inSitu.gameObject.SetActive(false);
                view.m_lbl_recommend_LanguageText.text = LanguageUtils.getTextFormat(500201, m_playerProxy.CurrentRoleInfo.barbarianLevel + 1);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_1gbtn1.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500204);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_1gbtn1.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                    CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                    CoreUtils.uiManager.ShowUI(UI.s_iF_SearchRes, null, new SearchJump(SearchType.Barbarian, (int)m_playerProxy.CurrentRoleInfo.barbarianLevel + 1));
                });
            }
            else
            {
                SetBtnActive();
                view.m_lbl_recommend_LanguageText.text = string.Empty;
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_lbl_Text_LanguageText.text = LanguageUtils.getText(500012);
                view.m_UI_Item_WorldObjectPopBtns.m_UI_Model_2gbtn2.m_btn_languageButton_GameButton.onClick.AddListener(() => {
                    m_TroopProxy.SituStation = view.m_ck_Situ_GameToggle.isOn;
                    CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
                    FightHelper.Instance.AttackMonster(monsterId);
                });

                if (!GuideProxy.IsGuideing)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.Barbarian);
                }
            }
        }

        private void OpenDesPanel(bool isShow)
        {
            view.m_pl_description_Animator.gameObject.SetActive(isShow);
            view.m_pl_normalInfo_Animator.gameObject.SetActive(!isShow);
            if (isShow)
            {
                view.m_pl_description_Animator.Play("Show");
            }
            else
            {
                view.m_pl_normalInfo_Animator.Play("Show");
            }         
        }

        private void StartTimer()
        {
            if ((data.refreshTime + data.monsterDefine.showTime) > ServerTimeModule.Instance.GetServerTime())
            {
                m_timer = Timer.Register(1.0f, UpdateCountDown, null, true, true);
            }

            UpdateCountDown();
        }

        private void UpdateCountDown()
        {
            Int64 diffTime = (data.refreshTime + data.monsterDefine.showTime) - ServerTimeModule.Instance.GetServerTime();
            if (diffTime <= 0)
            {
                view.m_UI_Item_WorldObjInfoTDrop.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(0);
                CancelTimer();
                CoreUtils.uiManager.CloseUI(UI.s_pop_WorldMonster);
            }
            else
            {
                view.m_UI_Item_WorldObjInfoTDrop.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int) diffTime);
            }
        }

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
            var rect = view.m_pl_content_Animator.gameObject.GetComponent<RectTransform>().rect;

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
            view.m_pl_content_Animator.gameObject.GetComponent<RectTransform>().transform.localPosition = localPos;
        }

        private void CancelTimer()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        #endregion
    }
}