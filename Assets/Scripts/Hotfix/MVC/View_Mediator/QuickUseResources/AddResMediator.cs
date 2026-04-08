// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月15日
// Update Time         :    2020年1月15日
// Class Description   :    AddResMediator
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
using Data;
using UnityEngine.Events;
using System;

namespace Game {

    public class AddResMediator : GameMediator {
        #region Member
        public static string NameMediator = "AddResMediator";

        private CurrencyProxy m_currencyProxy;
        private BagProxy m_bagProxy;

        private UI_Model_PageButton_Side_SubView[] m_btnGo = new UI_Model_PageButton_Side_SubView[4];
        private long[] m_needRss = new long[4];
        private long[] m_rss = new long[4];

        private Dictionary<string, GameObject> m_assetDic;
        private bool isInitedListView;

        private List<ItemDefine> m_itemList = new List<ItemDefine>();
        private List<KeyValuePair<int,int>> m_itemIndexs = new List<KeyValuePair<int, int>>();
        private int m_currentCurrencyType;
        private int currentSelectCurrencyIndex = 0;
        private int showItemid = -1;
        private long m_currencyTotalNum;
        private bool TipLoading = false;
        private ItemDefine m_ItemDefine;
        private int m_Itemid = 0;
        private int m_ItemNum = 0;//需要的物品数
        private int type = 0;//1,资源补足，2物品补足

        private int m_needItemNum = 0;//需要补足的物品数
        private int m_bagItemNum = 0;//背包物品数

        private int batchUseNum;

        #endregion

        //IMediatorPlug needs
        public AddResMediator(object viewComponent ):base(NameMediator, viewComponent ) {  }


        public AddResView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdateCurrency,
                CmdConstant.ItemInfoChange,
                Role_BuyResource.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                    if (type == 1)
                    {
                        InitView();
                    }
                    break;
                case CmdConstant.ItemInfoChange:
                    if (type == 1)
                    {
                        InitView();
                    }
                    break;
                case Role_BuyResource.TagName:
                    if (type == 2)
                    {
                        RefreshViewType2();
                    }
                    break;
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
            AppFacade.GetInstance().SendNotification(CmdConstant.AddResWinClose);
            CloseTip();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
          
        }        

        protected override void InitData()
        {
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            var rss = view.data as long[];
            if (rss.Length == 2)
            {
                type = 2;
                m_Itemid = (int)rss[0];
                m_ItemNum = (int)rss[1];
                m_ItemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(m_Itemid);
                OnItemRefresh();
            }
            else if (rss.Length == 4)
            {
                type = 1;
                m_rss = rss;
                OnCurrencyRefresh();

                m_btnGo[0] = view.m_UI_Model_PageButton_food;
                m_btnGo[1] = view.m_UI_Model_PageButton_wood;
                m_btnGo[2] = view.m_UI_Model_PageButton_stone;
                m_btnGo[3] = view.m_UI_Model_PageButton_gold;

                bool firstPage = false;
                for (int i = 0; i < m_btnGo.Length; i++)
                {
                    m_btnGo[i].m_img_redpot_PolygonImage.gameObject.SetActive(false);
                    m_btnGo[i].m_UI_Common_Redpoint.gameObject.SetActive(true);
                    m_btnGo[i].m_UI_Common_Redpoint.m_img_new_PolygonImage.gameObject.SetActive(false);
                    m_btnGo[i].m_UI_Common_Redpoint.m_img_redpointNum_PolygonImage.gameObject.SetActive(false);
                    m_btnGo[i].m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(false);
                    int k = i;
                    m_btnGo[i].gameObject.SetActive(m_rss[i] > 0);
                    if (m_rss[i] > 0 && !firstPage)
                    {
                        currentSelectCurrencyIndex = i;
                        firstPage = true;

                    }
                    m_btnGo[i].m_btn_btn_GameButton.onClick.AddListener(() =>
                    {
                        showItemid = -1;
                        currentSelectCurrencyIndex = k;
                        HideLights(k);
                        InitView();
                    });
                }
            }
            m_rss = view.data as long[];
 
        }


        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_sv_list_ListView.onDragBegin = CloseTip;  
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AddRes);
            CloseTip();
        }

        protected override void BindUIData()
        {
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, (tmpDic) =>
            {
                m_assetDic = tmpDic;
                if (type == 1)
                {
              
                    view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(300050);
                    HideLights(currentSelectCurrencyIndex);
                    InitView();
                }
                else
                {
                    view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(300144);
                    HidePagToggle();
                    InitViewType2();
                }
            });


        }

        #endregion
        private void HideLights(int index)
        {
            for (int i = 0; i < m_btnGo.Length; i++)
            {
                m_btnGo[i].m_img_highLight_PolygonImage.gameObject.SetActive(index == i);
                m_btnGo[i].m_img_dark_PolygonImage.gameObject.SetActive(index != i);
            }
        }
        private void HidePagToggle()
        {
            view.m_pl_pageBtn_VerticalLayoutGroup.gameObject.SetActive(false);
        }

        private void OnCurrencyRefresh()
        {
            m_needRss[0] = m_currencyProxy.Food - m_rss[0] >= 0 ? 0 : -(m_currencyProxy.Food - m_rss[0]);
            m_needRss[1] = m_currencyProxy.Wood - m_rss[1] >= 0 ? 0 : -(m_currencyProxy.Wood - m_rss[1]);
            m_needRss[2] = m_currencyProxy.Stone - m_rss[2] >= 0 ? 0 : -(m_currencyProxy.Stone - m_rss[2]);
            m_needRss[3] = m_currencyProxy.Gold - m_rss[3] >= 0 ? 0 : -(m_currencyProxy.Gold - m_rss[3]);
        }
        private void OnItemRefresh()
        {
            m_bagItemNum = (int)m_bagProxy.GetItemNum(m_Itemid);
            m_needItemNum = (int)((m_bagItemNum - m_ItemNum) > 0 ? 0 : -(m_bagItemNum - m_ItemNum));
        }
        private void InitViewType2()
        {
            OnProgress();
            m_itemList.Add(m_ItemDefine);
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = OnItemEnterType2;
            view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_ListView.FillContent(m_itemList.Count);
        }
        private void RefreshViewType2()
        {
            OnItemRefresh();
            OnProgress();
            view.m_sv_list_ListView.ForceRefresh();
        }
        private void InitView()
        {
            switch (currentSelectCurrencyIndex)
            {
                case 0: m_currentCurrencyType = 100; m_currencyTotalNum = m_currencyProxy.Food; break;
                case 1: m_currentCurrencyType = 101; m_currencyTotalNum = m_currencyProxy.Wood; break;
                case 2: m_currentCurrencyType = 102; m_currencyTotalNum = m_currencyProxy.Stone; break;
                case 3: m_currentCurrencyType = 103; m_currencyTotalNum = m_currencyProxy.Gold; break;
            }
            OnProgress();
            OnPageRedDot();
            if (!isInitedListView)
            {
                isInitedListView = true;
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = OnItemEnter;
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
            }
            if (m_assetDic == null)
            {
                return;
            }
            int subType = CoreUtils.dataService.QueryRecord<CurrencyDefine>(m_currentCurrencyType).itemSubType;
            m_itemList = CoreUtils.dataService.QueryRecords<ItemDefine>().FindAll((i) => { return i.subType == subType; });
            m_itemIndexs.Clear();
            for(int i = 0;i<m_itemList.Count;i++)
            {
                int num = (int)m_bagProxy.GetItemNum(m_itemList[i].ID);
                m_itemIndexs.Add(new KeyValuePair<int, int>(num, i));
                if (num > 0 && m_itemList[i].shortcutPrice > 0)
                {
                    m_itemIndexs.Add(new KeyValuePair<int, int>(0, i));
                }
            }
            m_itemIndexs.Sort((r1,r2)=>
            {
                long num1 = (r1.Key == 0 ? 100000 : 0) + r1.Value;
                long num2 = (r2.Key == 0 ? 100000 : 0) + r2.Value;
                return num1.CompareTo(num2);
            });
            view.m_sv_list_ListView.FillContent(m_itemIndexs.Count);
            long itemnum = m_bagProxy.GetItemNum(showItemid);
            if (itemnum <= 1)
            {
                CloseTip();
            }

        }
        public void CloseTip(UnityEngine.EventSystems.PointerEventData data)
        {
            CloseTip();
        }
        public  void CloseTip()
        {
            if (powTipView != null && powTipView.gameObject != null)
            {
                CoreUtils.assetService.Destroy(powTipView.gameObject);
                powTipView = null;
            }
            if (m_tipView != null)
            {
                m_tipView.Close();
                m_tipView = null;
            }
            showItemid = -1;
            batchUseNum = 0;
            TipLoading = false;
        }
        private void OnItemEnterType2(ListView.ListItem item)
        {
            if (!item.HasGameObject())
            {
                return;
            }
           int index = item.index;
            int bagItemNum = this.m_bagItemNum;
            UI_Item_StandardUseItemView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
            itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(m_itemList[index].l_nameID);
            itemView.m_lbl_itemDesc_LanguageText.text = string.Format(LanguageUtils.getText(m_itemList[index].l_desID), m_itemList[index].topData.ToString("N0"));
            //设置品质图片
            itemView.m_UI_Item_Bag.m_UI_Model_Item.Refresh(m_ItemDefine,0,false,false,false);
                itemView.m_UI_Model_Yellow.gameObject.SetActive(true);
                itemView.m_UI_Model_Blue_big.gameObject.SetActive(false);
            itemView.m_lbl_itemCount_LanguageText.text = LanguageUtils.getTextFormat( 300074 ,bagItemNum.ToString("N0"));
                itemView.m_UI_Model_Yellow.m_lbl_line2_LanguageText.text = m_itemList[index].shortcutPrice.ToString("N0");
                itemView.m_UI_Model_Yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(300097);
                itemView.m_UI_Model_Yellow.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                itemView.m_UI_Model_Yellow.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                itemView.m_UI_Model_Yellow.m_btn_languageButton_VerticalLayoutGroup.SetLayoutVertical();
                ClientUtils.LoadSprite(itemView.m_UI_Model_Yellow.m_img_icon2_PolygonImage, m_currencyProxy.GeticonIdByType(EnumCurrencyType.denar));
                itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    if (!m_currencyProxy.ShortOfDenar(m_itemList[index].shortcutPrice))
                    {
                        UIHelper.DenarCostRemain(m_itemList[index].shortcutPrice,()=> {
                            m_currencyProxy.BuyAndUseCurrencyResources(m_itemList[index].ID);
                        });

                    }
                });
            
        }
        private void OnItemEnter(ListView.ListItem item)
        {
            if (!item.HasGameObject())
            {
                return;
            }
            int index = m_itemIndexs[item.index].Value;
            int ItemNum = m_itemIndexs[item.index].Key;
            long itemIndex = m_bagProxy.GetItemIndex(m_itemList[index].ID);
            UI_Item_StandardUseItemView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
            itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(m_itemList[index].l_nameID);
            itemView.m_lbl_itemDesc_LanguageText.text = string.Format(LanguageUtils.getText(m_itemList[index].l_desID), m_itemList[index].topData.ToString("N0"));
            itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_count_LanguageText.text = string.Empty;
            itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_desc_LanguageText.text = m_itemList[index].topData.ToString("N0");
            ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_icon_PolygonImage, m_itemList[index].itemIcon);
            //设置品质图片
            ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_quality_PolygonImage, RS.ItemQualityBg[m_itemList[index].quality-1]);
            if (showItemid == m_itemList[index].ID && ItemNum > 1 && m_currencyTotalNum < m_rss[currentSelectCurrencyIndex])//显示批量使用
            {

                batchUseNum = GetBatchUseNumber(m_itemList[index].topData, ItemNum);
                batchUseNum = batchUseNum > ItemNum ? ItemNum : batchUseNum;
                if (batchUseNum < 1)
                {
                    CloseTip();
                }
                else
                {
                    ShowPopMemu(item.index, "", (tIndex) =>
                    {
                        int totalNum = m_itemList[index].topData * batchUseNum;
                        m_currencyProxy.UseCurrencyResources(itemIndex, batchUseNum);
                        Tip.CreateTip(LanguageUtils.getTextFormat(128005, totalNum.ToString("N0"), LanguageUtils.getText(m_currencyProxy.CurrencyDefine[m_currentCurrencyType].l_desID))).Show();

                        ListView.ListItem tempItem = view.m_sv_list_ListView.GetItemByIndex(tIndex);
                        UI_Item_StandardUseItemView tempView = MonoHelper.GetHotFixViewComponent<UI_Item_StandardUseItemView>(tempItem.go);

                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyUICurrency(m_currentCurrencyType, m_itemList[index].topData * batchUseNum, tempView.m_UI_Model_Blue_big.m_root_RectTransform.position);
                    });
                }
            }
            else if (showItemid == m_itemList[index].ID && ItemNum > 1 && m_currencyTotalNum >= m_rss[currentSelectCurrencyIndex])//显示批量使用
            {
                CloseTip();
            }
            if (ItemNum <= 0)//未拥有
            {
                itemView.m_UI_Model_Yellow.gameObject.SetActive(true);
                itemView.m_UI_Model_Blue_big.gameObject.SetActive(false);
                itemView.m_lbl_itemCount_LanguageText.text = string.Empty;
                itemView.m_UI_Model_Yellow.m_lbl_line2_LanguageText.text = m_itemList[index].shortcutPrice.ToString("N0");
                itemView.m_UI_Model_Yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(300097);
                itemView.m_UI_Model_Yellow.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                itemView.m_UI_Model_Yellow.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                itemView.m_UI_Model_Yellow.m_btn_languageButton_VerticalLayoutGroup.SetLayoutVertical();
                ClientUtils.LoadSprite(itemView.m_UI_Model_Yellow.m_img_icon2_PolygonImage, m_currencyProxy.GeticonIdByType(EnumCurrencyType.denar));
                itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(()=>
                {
                    if (!m_currencyProxy.ShortOfDenar(m_itemList[index].shortcutPrice))
                    {
                        UIHelper.DenarCostRemain(m_itemList[index].shortcutPrice, () => {
                            Tip.CreateTip(LanguageUtils.getTextFormat(128005, m_itemList[index].topData.ToString("N0"), LanguageUtils.getText(m_currencyProxy.CurrencyDefine[m_currentCurrencyType].l_desID))).Show();
                            m_currencyProxy.BuyAndUseCurrencyResources(m_itemList[index].ID);
                            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                            mt.FlyUICurrency(m_currentCurrencyType, m_itemList[index].topData, itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.transform.position);
                            showItemid = -1;
                        }); 
                    }
                });
            }
            else//已拥有
            {
                itemView.m_UI_Model_Yellow.gameObject.SetActive(false);
                itemView.m_UI_Model_Blue_big.gameObject.SetActive(true);
                itemView.m_lbl_itemCount_LanguageText.text = string.Format(LanguageUtils.getText(110001), ItemNum);
                itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.AddListener(()=>
                {
                    Tip.CreateTip(LanguageUtils.getTextFormat(128005, m_itemList[index].topData.ToString("N0"), LanguageUtils.getText(m_currencyProxy.CurrencyDefine[m_currentCurrencyType].l_desID))).Show();
                    m_currencyProxy.UseCurrencyResources(itemIndex);
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    mt.FlyUICurrency(m_currentCurrencyType, m_itemList[index].topData, itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.transform.position);
                    if (showItemid != m_itemList[index].ID)
                    {
                        CloseTip();
                    }
                    showItemid = m_itemList[index].ID;
                });
            }
        }
        private UI_Pop_UseAddItemView powTipView;
        private HelpTip m_tipView;
        private void ShowPopMemu( int index,string btnName, Action<int> btnAction)
        {
            int tIndex = index;
            if (powTipView != null)
            {
                powTipView.m_UI_left.SetText($"X{batchUseNum}");
                return;
            }
            if (TipLoading)
            {
                return;
            }
            TipLoading = true;
            CoreUtils.assetService.Instantiate("UI_Pop_UseAddItem", (obj) =>
            {
                TipLoading = false;
                if (showItemid == -1|| batchUseNum<1)
                {
                    CoreUtils.assetService.Destroy(obj);
                    return;
                }

                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_UseAddItemView>(obj);

                powTipView.m_UI_left.gameObject.SetActive(true);
                powTipView.m_UI_right.gameObject.SetActive(false);
                powTipView.m_UI_left.SetText($"X{batchUseNum}");
                ClientUtils.UIReLayout(powTipView.m_pl_bg_ContentSizeFitter.gameObject);

                powTipView.m_UI_left.m_btn_languageButton_GameButton.onClick.AddListener(()=> {
                    btnAction(tIndex);
                });

                var item = view.m_sv_list_ListView.GetItemByIndex(index);
                if (item != null)
                {
                    UI_Item_StandardUseItemView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
                    if (LanguageUtils.IsArabic())
                    {
                        m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.gameObject.GetComponent<RectTransform>().sizeDelta, itemView.m_UI_Model_Blue_big.m_root_RectTransform.position)
                     .SetStyle(HelpTipData.Style.arrowLeft).SetOffset(100).UnEnableTouchClose().Show();
                    }
                    else
                    {
                        m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.gameObject.GetComponent<RectTransform>().sizeDelta, itemView.m_UI_Model_Blue_big.m_root_RectTransform.position)
                   .SetStyle(HelpTipData.Style.arrowRight).SetOffset(100).UnEnableTouchClose().Show();
                    }
                 
                }
            });
        }

        private int GetBatchUseNumber(int perNum,int itemNum)
        {
            if(m_currencyTotalNum < m_rss[currentSelectCurrencyIndex])
            {
                int i = 0;
                for (;i<=itemNum; i++)
                {
                    if((m_rss[currentSelectCurrencyIndex]-m_currencyTotalNum) <= perNum*i)
                    {
                        break;
                    }
                }
                return i;
            }
            return 0;
        }

        private void OnProgress()
        {
            if (type == 1)
            {
                view.m_UI_Item_PBinTech.m_lbl_time_LanguageText.text = $"{m_currencyTotalNum.ToString("N0")}/{m_rss[currentSelectCurrencyIndex].ToString("N0")}";
                float percent = m_currencyTotalNum / (float)m_rss[currentSelectCurrencyIndex];
                
                ClientUtils.SetPro(percent,view.m_UI_Item_PBinTech.m_pb_rogressBar_GameSlider);
                ClientUtils.LoadSprite(view.m_UI_Item_PBinTech.m_img_icon_PolygonImage, m_currencyProxy.GeticonIdByType(m_currentCurrencyType));
            }
            else if (type == 2)
            {
                
                view.m_UI_Item_PBinTech.m_lbl_time_LanguageText.text = $"{m_bagItemNum.ToString("N0")}/{m_ItemNum.ToString("N0")}";
                float percent = m_bagItemNum / (float)m_ItemNum;
                ClientUtils.SetPro(percent,view.m_UI_Item_PBinTech.m_pb_rogressBar_GameSlider);
                ClientUtils.LoadSprite(view.m_UI_Item_PBinTech.m_img_icon_PolygonImage, m_ItemDefine.itemIcon);
            }
        }


        private void OnPageRedDot()
        {
            view.m_UI_Model_PageButton_food.m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(m_currencyProxy.Food<m_rss[0]);
            view.m_UI_Model_PageButton_wood.m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(m_currencyProxy.Wood<m_rss[1]);
            view.m_UI_Model_PageButton_stone.m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(m_currencyProxy.Stone<m_rss[2]);
            view.m_UI_Model_PageButton_gold.m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(m_currencyProxy.Gold<m_rss[3]);
        }
    }
}