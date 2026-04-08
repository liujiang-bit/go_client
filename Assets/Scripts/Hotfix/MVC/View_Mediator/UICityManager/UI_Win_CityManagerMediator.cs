// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月21日
// Update Time         :    2020年4月21日
// Class Description   :    UI_Win_CityManagerMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using System;
using Data;
using UnityEngine.UI;

namespace Game {

    public class CityBuffItemData
    {
        public int type;//1,title,2item
        public string title;
        public CityBuffGroupData cityBuffGroupData;


        public CityBuffItemData(int type, string title)
        {
            this.type = type;
            this.title = title;
        }

        public CityBuffItemData(int type, CityBuffGroupData cityBuffGroupData)
        {
            this.type = type;
            this.cityBuffGroupData = cityBuffGroupData;
        }
    }
    public class UI_Win_CityManagerMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_CityManagerMediator";

        private PlayerProxy m_playerProxy;
        private CityBuffProxy m_cityBuffProxy;
        private BagProxy m_bagProxy;
        private CurrencyProxy m_currencyProxy;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<string> m_preLoadRes = new List<string>();
        private List<CityBuffItemData> m_scrollItemList = new List<CityBuffItemData>();
        private List<CityBuffDefine> m_scrollItem1List = new List<CityBuffDefine>();
        private List<BattleBuffDefine> m_scrollItem2List = new List<BattleBuffDefine>();
        private GameObject obj_UI_Item_BuffWarLevel;
        private Dictionary<int, Timer> m_timers = new Dictionary<int, Timer>();

        private CityBuffGroupData m_curCityBuffGroupData;
        private int m_list = 0;//当前页

        private const int itemHeight = 60;
        private int buffid = 0;
        private bool useitem = false;
        #endregion

        //IMediatorPlug needs
        public UI_Win_CityManagerMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_CityManagerView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                  CmdConstant.CityBuffChange,
                  Role_AddBuff.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.CityBuffChange:
                    {
                        m_scrollItemList = m_cityBuffProxy.GetScrollItemList();
                        view.m_sv_list_ListView.FillContent(m_scrollItemList.Count);
                        view.m_sv_list_ListView.ForceRefresh();

                        if (m_list == 1)
                        {
                            m_scrollItem1List = m_cityBuffProxy.GetScrollItem1List(m_curCityBuffGroupData.groupDefine.ID);
                            view.m_sv_list1_ListView.FillContent(m_scrollItem1List.Count);
                            view.m_sv_list1_ListView.ForceRefresh();
                        }
                    }
                    break;
                case Role_AddBuff.TagName:
                    {
                        Role_AddBuff.response response = notification.Body as Role_AddBuff.response;
                        if (response != null)
                        {
                            MainInterfaceMediator mainInterfaceMediator = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
                            Transform transformpar = mainInterfaceMediator.view.m_UI_Item_PlayerPowerInfo.m_Pl_Buffs_ArabLayoutCompment.transform;
                            Transform endTransform = transformpar.Find(buffid.ToString());
                            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)response.itemId);
                            if (itemDefine != null)
                            {
                                Tip.CreateTip(LanguageUtils.getTextFormat(128009, LanguageUtils.getText(itemDefine.l_nameID))).Show();
                                int itemid = (int)itemDefine.ID;
                                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                                if (useitem)
                                {
                                    if (endTransform != null)
                                    {
                                        mt.FlyItemEffect(itemid, 1, Vector3.zero, endTransform, () =>
                                        {
                                            AddEffect(RS.CityBuff, endTransform, null);
                                        });
                                    }
                                }
                                else
                                {
                                    AddEffect(RS.CityBuff, endTransform, null);
                                }
                            }
                            else
                            {
                              
                            }
                        }
                       
                    }
                    CoreUtils.uiManager.CloseUI(UI.s_cityManager);
                  
                    break;
            default:
                    break;
            }
        }



        #region UI template method
        public void AddEffect(string effectName, Transform targetPart, Action<GameObject> callBack)
        {
            CoreUtils.assetService.Instantiate(effectName, (effectGO) =>
            {

                effectGO.name = effectName;
                effectGO.SetActive(true);
                effectGO.transform.SetParent(targetPart);
                effectGO.transform.localPosition = Vector3.zero;
                effectGO.transform.localScale = Vector3.one;
                if (targetPart != null)
                {

                }
                else
                {
                    CoreUtils.assetService.Destroy(effectGO);
                }
                callBack?.Invoke(effectGO);
            });
        }
        public GameObject GetEffect(string effectName, Transform targetPart)
        {
            if (targetPart == null)
            {
                return null;
            }
            Transform effectGO = targetPart.Find(effectName);
            if (effectGO == null)
            {
                return null;
            }
            return effectGO.gameObject;
        }
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

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;

            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_list1_ListView.ItemPrefabDataList);
            m_scrollItemList = m_cityBuffProxy.GetScrollItemList();
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                InitListView();
            });

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.onClick.AddListener(() =>
            {
                ShowList(0);
            });
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_cityManager);
            });
        }

        protected override void BindUIData()
        {
            view.m_UI_Item_BuffWarLevel.gameObject.gameObject.SetActive(false);
            obj_UI_Item_BuffWarLevel = view.m_UI_Item_BuffWarLevel.gameObject;
            m_scrollItem2List = m_cityBuffProxy.GetScrollItem2List();
            view.m_pl_c.sizeDelta = new Vector2(view.m_pl_c.sizeDelta.x, 117.85f+itemHeight* m_scrollItem2List.Count);
            m_scrollItem2List.ForEach((buffWarLevel) =>
                {
                    string lbl1 = string.Empty;
                    lbl1 = buffWarLevel.minLevel == buffWarLevel.maxLevel ? buffWarLevel .minLevel.ToString():string.Format("{0}-{1}",buffWarLevel.minLevel ,buffWarLevel.maxLevel);
                   GameObject obj =  CoreUtils.assetService.Instantiate(obj_UI_Item_BuffWarLevel);
                    obj.transform.SetParent(view.m_pl_view_GridLayoutGroup.transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
                    obj.gameObject.SetActive(true);
                    obj.transform.Find("lbl_lbl1").GetComponent<LanguageText>().text = lbl1;
                    CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>(buffWarLevel.buff);
                    if (cityBuffDefine != null)
                    {
                        obj.transform.Find("lbl_lbl2").GetComponent<LanguageText>().text = ClientUtils.FormatTimeCityBuff (cityBuffDefine.duration);
                    }
               
            });
        }


        #endregion


        private void InitListView()
        {
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ItemListEnter;
                funcTab.GetItemPrefabName = GetItemPrefabName;
                funcTab.GetItemSize = GetItemSize;
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_scrollItemList.Count);
            }
        }

        private float GetItemSize(ListView.ListItem item)
        {
            int index = item.index;
            CityBuffItemData scrollItemData = m_scrollItemList[index];
            if (scrollItemData.type == 1)
            {
                return m_assetDic["UI_Item_CityMgrTitle"].GetComponent<RectTransform>().sizeDelta.y;
            }
            else
            {
                return m_assetDic["UI_Item_CityMgrItem"].GetComponent<RectTransform>().sizeDelta.y;
            }
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            int index = item.index;
            CityBuffItemData scrollItemData = m_scrollItemList[index];
            if (scrollItemData.type == 1)
            {
                return "UI_Item_CityMgrTitle";
            }
            else
            {
                return "UI_Item_CityMgrItem";
            }
        }

        private void ItemListEnter(ListView.ListItem scrollItem)
        {
            int index = scrollItem.index;
            CityBuffItemData scrollItemData = m_scrollItemList[index];
            if (scrollItemData.type == 1)
            {
                UI_Item_CityMgrTitleView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_CityMgrTitleView>(scrollItem.go);
                itemView.m_lbl_title_LanguageText.text = scrollItemData.title;
            }
            else
            {
                UI_Item_CityMgrItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_CityMgrItemView>(scrollItem.go);

                ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, scrollItemData.cityBuffGroupData.groupDefine.icon);
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(scrollItemData.cityBuffGroupData.groupDefine.nameID);
                itemView.m_lbl_desc_LanguageText .text = LanguageUtils.getText(scrollItemData.cityBuffGroupData.groupDefine.desID);
                if (scrollItemData.cityBuffGroupData.groupDefine.func == 1)
                {
                    itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_use.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_use.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_cityManager);
                        m_cityBuffProxy.SendMoveCity();
                    });
                    itemView.m_btn_use.gameObject.SetActive(true);
                    itemView.m_img_arrow_PolygonImage.gameObject.SetActive(false);
                }
                else if (scrollItemData.cityBuffGroupData.groupDefine.func == 2)
                {
                    itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_use.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                    {
                        m_curCityBuffGroupData = scrollItemData.cityBuffGroupData;
                        ShowLIst2(scrollItemData.cityBuffGroupData.groupDefine.ID);
                    });
                    itemView.m_btn_use.gameObject.SetActive(false);
                    itemView.m_img_arrow_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_use.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                    {
                        m_curCityBuffGroupData = scrollItemData.cityBuffGroupData;
                        ShowLIst1(scrollItemData.cityBuffGroupData.groupDefine.ID);
                    });
                    itemView.m_btn_use.gameObject.SetActive(false);
                    itemView.m_img_arrow_PolygonImage.gameObject.SetActive(true);
                }
                if (scrollItemData.cityBuffGroupData.cityBuff != null && scrollItemData.cityBuffGroupData.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
                {

                    itemView.m_pb_rogressBar_ArabLayoutCompment.gameObject.SetActive(true);
                    itemView.m_pb_rogressBar_GameSlider.minValue = 0;
                    CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)scrollItemData.cityBuffGroupData.cityBuff.id);
                    itemView.m_pb_rogressBar_GameSlider.maxValue = cityBuffDefine.duration;
                    if (m_timers.ContainsKey(index))
                    {
                        if (m_timers[index] != null)
                        { 
                            m_timers[index].Cancel();
                        m_timers[index] = null;
                    }
                    }
                    if (itemView.data is int)
                    {
                        int lastindex = (int)itemView.data;
                        if (m_timers.ContainsKey(lastindex))
                        {
                            if (m_timers[lastindex] != null)
                            {
                                m_timers[lastindex].Cancel();
                                m_timers[lastindex] = null;
                            }
                        }
                    }
                    itemView.data = index;
                    RefreshItemView(itemView, scrollItemData, cityBuffDefine);
                    m_timers[index] = Timer.Register(1, () =>
                    {
                        if (scrollItemData.cityBuffGroupData.cityBuff!=null&& scrollItemData.cityBuffGroupData.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
                        {
                            RefreshItemView(itemView, scrollItemData, cityBuffDefine);
                        }
                        else
                        {
                            itemView.m_pb_rogressBar_ArabLayoutCompment.gameObject.SetActive(false);
                        }

                    }, null, true, false, view.vb);
                }
                else
                {
                    itemView.m_pb_rogressBar_ArabLayoutCompment.gameObject.SetActive(false);
                }
            }
        }
        private void RefreshItemView(UI_Item_CityMgrItemView itemView, CityBuffItemData scrollItemData, CityBuffDefine cityBuffDefine)
        {
            if (scrollItemData.cityBuffGroupData.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
            {
                itemView.m_pb_rogressBar_GameSlider.value = scrollItemData.cityBuffGroupData.cityBuff.expiredTime - ServerTimeModule.Instance.GetServerTime();
                if (cityBuffDefine.tagData.Count > 0)
                {
                    itemView.m_lbl_dec_LanguageText.text = string.Format(LanguageUtils.getText(cityBuffDefine.tag), cityBuffDefine.tagData[0]);
                }
                else
                {
                    itemView.m_lbl_dec_LanguageText.text = LanguageUtils.getText(cityBuffDefine.tag);
                }
                itemView.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int)(scrollItemData.cityBuffGroupData.cityBuff.expiredTime - ServerTimeModule.Instance.GetServerTime()));
            }
        }

        private void ItemList2Enter(ListView.ListItem scrollItem)
        {
            int index = scrollItem.index;
            BattleBuffDefine battleBuffDefine = m_scrollItem2List[index];
        }
        private void ItemList1Enter(ListView.ListItem scrollItem)
        {
            int index = scrollItem.index;
            CityBuffDefine CityBuff = m_scrollItem1List[index];
            UI_Item_StandardUseItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_StandardUseItemView>(scrollItem.go);
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(CityBuff.item);
            if (itemDefine != null)
            {
                itemView.m_UI_Item_Bag.m_UI_Model_Item.Refresh(itemDefine,0,false);
                itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
                itemView.m_lbl_itemDesc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_desID), ClientUtils.FormatComma(itemDefine.desData1), ClientUtils.FormatComma(itemDefine.desData2));
                long num = m_bagProxy.GetItemNum(itemDefine.ID);
                itemView.m_lbl_itemCount_LanguageText.text = LanguageUtils.getTextFormat(110001, num);
                if (num == 0)
                {
                    itemView.m_UI_Model_Yellow.gameObject.SetActive(true);
                    itemView.m_UI_Model_Blue_big.gameObject.SetActive(false);
                    itemView.m_lbl_itemCount_LanguageText.text = string.Empty;
                    itemView.m_UI_Model_Yellow.m_lbl_line2_LanguageText.text = itemDefine.shortcutPrice.ToString("N0");
                    itemView.m_UI_Model_Yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(300097);
                    itemView.m_UI_Model_Yellow.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                    itemView.m_UI_Model_Yellow.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                    itemView.m_UI_Model_Yellow.m_btn_languageButton_VerticalLayoutGroup.SetLayoutVertical();
                    ClientUtils.LoadSprite(itemView.m_UI_Model_Yellow.m_img_icon2_PolygonImage, m_currencyProxy.GeticonIdByType(EnumCurrencyType.denar));
                    itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {
                        OnYellowBtnClick(CityBuff, itemDefine.ID, itemDefine.shortcutPrice);
                    });
                }
                else
                {
                    itemView.m_UI_Model_Blue_big.gameObject.SetActive(true);
                    itemView.m_UI_Model_Yellow.gameObject.SetActive(false);
                    itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                    itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                    {
                      //  OnBlueBtnClick(CityBuff, itemDefine.ID, itemDefine.shortcutPrice);
                        m_cityBuffProxy.SendUseItem(CityBuff.ID, itemDefine.ID);
                        buffid = CityBuff.ID;
                        useitem = true;
                    });
                }

            }
        }
        private void OnYellowBtnClick(CityBuffDefine citybuffid, int itemid, int shortcutPrice)
        {
            string tip = string.Empty;

            if (m_curCityBuffGroupData.cityBuff != null && m_curCityBuffGroupData.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
            {
                Alert.CreateAlert(128010)
                    .SetRightButton(null , LanguageUtils.getText(192010))
                    .SetLeftButton(() =>
                       {
                           OkYellowCallBack(citybuffid, itemid, shortcutPrice);

                       }, LanguageUtils.getText(192009))
                    .Show();

            }
            else
            {
                if (m_cityBuffProxy.HasseriesBuff(m_curCityBuffGroupData, out tip))
                {
                    Alert.CreateAlert(LanguageUtils.getTextFormat(128011, tip))
                        .SetRightButton(null, LanguageUtils.getText(192010))
                        .SetLeftButton(() =>
                         {
                             OkYellowCallBack(citybuffid, itemid, shortcutPrice);

                         }, LanguageUtils.getText(192009))
                        .Show();

                }
                else
                {
                    OkYellowCallBack(citybuffid, itemid, shortcutPrice);
                }

            }

        }
        public void OkYellowCallBack(CityBuffDefine citybuffid, int itemid, int shortcutPrice)
        {
            Alert.CreateAlert(LanguageUtils.getTextFormat(300072, shortcutPrice.ToString("N0"))).SetRightButton(()=> {
                if (!m_currencyProxy.ShortOfDenar(shortcutPrice))
                {
                    if (citybuffid.type == 2 && m_cityBuffProxy.HasType1Buff())
                    {
                        Tip.CreateTip(128008).Show();
                    }
                    else
                    {
                        Role_AddBuff.request req = new Role_AddBuff.request();
                        req.buffId = citybuffid.ID;
                        AppFacade.GetInstance().SendSproto(req);
                        buffid = citybuffid.ID;
                        useitem = false;
                    }
                }
            }).SetLeftButton().Show();
        }

            private void ShowLIst1(int group)
        {
            ShowList(1);
            m_scrollItem1List = m_cityBuffProxy.GetScrollItem1List(group);
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemList1Enter;
            view.m_sv_list1_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list1_ListView.FillContent(m_scrollItem1List.Count);
        }

        private void ShowLIst2(int group)
        {
                view.m_lbl_att_LanguageText.text = m_cityBuffProxy.GetWarFrenzyDesc();
            if (m_curCityBuffGroupData.cityBuff != null && m_curCityBuffGroupData.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
            {
                view.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(700024, LanguageUtils.getText(m_curCityBuffGroupData.groupDefine.nameID), LanguageUtils.getText(181185));
            }
            else
            {
           view.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(700024,LanguageUtils.getText(m_curCityBuffGroupData.groupDefine.nameID), LanguageUtils.getText(181184));

            }
            ShowList(2);
        }
        


        /// <summary>
        /// 0,1,2
        /// </summary>
        /// <param name="listid"></param>
        public void ShowList(int listid)
        {
            m_list = listid;
            if (listid == 0)
            {
                view.m_pl_rect0.gameObject.SetActive(true);
                view.m_pl_rect1.gameObject.SetActive(false);
                view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(false);
            }
            else if (listid == 1)
            {
                view.m_pl_rect0.gameObject.SetActive(false);
                view.m_pl_rect1.gameObject.SetActive(true);
                view.m_pl_view1.gameObject.SetActive(true);
                view.m_pl_view2.gameObject.SetActive(false);
                view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(true);
            }
            else if (listid == 2)
            {
                view.m_pl_rect0.gameObject.SetActive(false);
                view.m_pl_rect1.gameObject.SetActive(true);
                view.m_pl_view1.gameObject.SetActive(false);
                view.m_pl_view2.gameObject.SetActive(true);
                view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(true);

            }
        }

    }
}