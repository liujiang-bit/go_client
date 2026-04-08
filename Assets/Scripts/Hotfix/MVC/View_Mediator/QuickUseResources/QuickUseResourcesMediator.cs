// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月26日
// Update Time         :    2019年12月26日
// Class Description   :    QuickUseResourcesMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using Data;
using UnityEngine.Events;
using System;

namespace Game {
    public class QuickUseResourcesMediator : GameMediator {
        #region Member
        public static string NameMediator = "QuickUseResourcesMediator";

        private CurrencyProxy m_currencyProxy;
        private PlayerProxy m_playerProxy;
        private BagProxy m_bagProxy;

        private ListView.FuncTab m_itemFunc;

        private int currentSelectCurrencyID;
        private int currentSelectCurrencyIndex = 0;

        private UI_Model_PageButton_Side_SubView[] m_btnGo = new UI_Model_PageButton_Side_SubView[4];

        private List<ItemDefine> m_itemList = new List<ItemDefine>();
        private List<KeyValuePair<int, int>> m_itemIndexs = new List<KeyValuePair<int, int>>();
        private Dictionary<string, GameObject> assetDic;
        private bool isInitedListView;
        private int showItemid = -1;
        private bool TipLoading = false;
        private int batchUseNum;
        #endregion

        //IMediatorPlug needs
        public QuickUseResourcesMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public QuickUseResourcesView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.ItemInfoChange,
                CmdConstant.UpdateCurrency,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                case CmdConstant.ItemInfoChange:
                    InitListView();
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
            AppFacade.GetInstance().SendNotification(CmdConstant.ShowCurrencyMaskInfo, false);
            CloseTip();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.ShowCurrencyMaskInfo,true);
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

            m_btnGo[0] = view.m_UI_Model_PageButton_food;
            m_btnGo[1] = view.m_UI_Model_PageButton_wood;
            m_btnGo[2] = view.m_UI_Model_PageButton_stone;
            m_btnGo[3] = view.m_UI_Model_PageButton_gold;


            if(view.data is int)
            {
                currentSelectCurrencyIndex = (int)view.data;
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_sv_list_view_ListView.onDragBegin = CloseTip;
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_quickUseRss);
        }

        protected override void BindUIData()
        {
            ClientUtils.PreLoadRes(view.gameObject,view.m_sv_list_view_ListView.ItemPrefabDataList, (tmpDic)=>
            {
                assetDic = tmpDic;
                HideLights(currentSelectCurrencyIndex);
                InitListView();
            }
            );
            for(int i = 0;i< m_btnGo.Length;i++)
            {
                int k = i;
                m_btnGo[i].m_btn_btn_GameButton.onClick.AddListener(()=>
                {
                    currentSelectCurrencyIndex = k;
                    showItemid = -1;
                    HideLights(k);
                    InitListView();
                });
            }
        }
       
        #endregion

        private void HideLights(int index)
        {
            for(int i = 0;i<m_btnGo.Length;i++)
            {
                m_btnGo[i].m_img_highLight_PolygonImage.gameObject.SetActive(index==i);
                m_btnGo[i].m_img_dark_PolygonImage.gameObject.SetActive(index!=i);
            }
        }

        private void InitListView()
        {
            switch (currentSelectCurrencyIndex)
            {
                case 0: currentSelectCurrencyID = 100; break;
                case 1: currentSelectCurrencyID = 101; break;
                case 2: currentSelectCurrencyID = 102; break;
                case 3: currentSelectCurrencyID = 103; break;
            }
            if (m_itemFunc==null)
            {
                m_itemFunc = new ListView.FuncTab();
                m_itemFunc.ItemEnter = OnItemEnter;
            }
            if(assetDic==null)
            {
                return;
            }
            int subType = CoreUtils.dataService.QueryRecord<CurrencyDefine>(currentSelectCurrencyID).itemSubType;
            m_itemList = CoreUtils.dataService.QueryRecords<ItemDefine>().FindAll((i) => { return i.subType == subType; });
            m_itemIndexs.Clear();
            for (int i = 0; i < m_itemList.Count; i++)
            {
                int num = (int)m_bagProxy.GetItemNum(m_itemList[i].ID);
                if (m_itemList[i].shortcutPrice > 0)
                {
                    m_itemIndexs.Add(new KeyValuePair<int, int>(num, i));
                }
                if(num>0)
                {
                    m_itemIndexs.Add(new KeyValuePair<int, int>(0, i));
                }
            }
            m_itemIndexs.Sort((r1, r2) =>
            {
                long num1 = (r1.Key==0? 100000 :0 ) + r1.Value;
                long num2 = (r2.Key==0? 100000 :0 ) + r2.Value;
                return num1.CompareTo(num2);
            });
            if (!isInitedListView)
            {
                isInitedListView = true;
                view.m_sv_list_view_ListView.SetInitData(assetDic, m_itemFunc);
            }
            view.m_sv_list_view_ListView.FillContent(m_itemIndexs.Count);
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
        public void CloseTip()
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
            TipLoading = false;
        }

        private void OnItemEnter(ListView.ListItem item)
        {
            if(!item.HasGameObject())
            {
                return;
            }
            int index = m_itemIndexs[item.index].Value;
            int ItemNum = m_itemIndexs[item.index].Key;
            long itemIndex = m_bagProxy.GetItemIndex(m_itemList[index].ID);
            UI_Item_StandardUseItemView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
            itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(m_itemList[index].l_nameID);
            itemView.m_lbl_itemDesc_LanguageText.text = string.Format( LanguageUtils.getText(m_itemList[index].l_desID), m_itemList[index].topData.ToString("N0"));
            itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_count_LanguageText.text =string.Empty;
            itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_desc_LanguageText.text = m_itemList[index].topData.ToString("N0");
            ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_icon_PolygonImage,m_itemList[index].itemIcon);
            //设置品质图片
            ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_quality_PolygonImage, RS.ItemQualityBg[m_itemList[index].quality-1]);
            if (showItemid == m_itemList[index].ID && ItemNum > 1)//显示批量使用
            {
                 batchUseNum = GetBatchUseNumber(ItemNum);
                ShowPopMemu(item.index, $"X{batchUseNum}", (itemIndex1) => {
                    int totalNum = m_itemList[index].topData * batchUseNum;
                    m_currencyProxy.UseCurrencyResources(itemIndex, batchUseNum);
                    Tip.CreateTip(LanguageUtils.getTextFormat(128005, totalNum.ToString("N0"), LanguageUtils.getText(m_currencyProxy.CurrencyDefine[currentSelectCurrencyID].l_desID))).Show();
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;

                    ListView.ListItem tempItem = view.m_sv_list_view_ListView.GetItemByIndex(itemIndex1);
                    UI_Item_StandardUseItemView tempView = MonoHelper.GetHotFixViewComponent<UI_Item_StandardUseItemView>(tempItem.go);

                    mt.FlyUICurrency(currentSelectCurrencyID, m_itemList[index].topData * batchUseNum, tempView.m_UI_Model_Blue_big.m_root_RectTransform.position);
                }, LanguageUtils.getText(200024),(itemIndex2) => {
                    ItemNum = m_itemIndexs[item.index].Key;
                    int totalNum = m_itemList[index].topData * ItemNum;
                    Tip.CreateTip(LanguageUtils.getTextFormat(128005, totalNum.ToString("N0"), LanguageUtils.getText(m_currencyProxy.CurrencyDefine[currentSelectCurrencyID].l_desID))).Show();
                    m_currencyProxy.UseCurrencyResources(itemIndex, ItemNum);
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;

                    ListView.ListItem tempItem = view.m_sv_list_view_ListView.GetItemByIndex(itemIndex2);
                    UI_Item_StandardUseItemView tempView = MonoHelper.GetHotFixViewComponent<UI_Item_StandardUseItemView>(tempItem.go);

                    mt.FlyUICurrency(currentSelectCurrencyID, m_itemList[index].topData * batchUseNum, tempView.m_UI_Model_Blue_big.m_root_RectTransform.position);
                });

            }
            if(ItemNum<=0)//未拥有
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
                itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(() =>//购买
                {
                    if(!m_currencyProxy.ShortOfDenar(m_itemList[index].shortcutPrice))
                    {
                        UIHelper.DenarCostRemain(m_itemList[index].shortcutPrice, () =>
                        {
                            Tip.CreateTip(LanguageUtils.getTextFormat(128005, m_itemList[index].topData.ToString("N0"), LanguageUtils.getText(m_currencyProxy.CurrencyDefine[currentSelectCurrencyID].l_desID))).Show();
                            m_currencyProxy.BuyAndUseCurrencyResources(m_itemList[index].ID);
                            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                            mt.FlyUICurrency(currentSelectCurrencyID, m_itemList[index].topData, itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.transform.position);
                            showItemid = -1;
                        });
                    }
                });
            }
            else//已拥有
            {
                itemView.m_UI_Model_Yellow.gameObject.SetActive(false);
                itemView.m_UI_Model_Blue_big.gameObject.SetActive(true);
                itemView.m_lbl_itemCount_LanguageText.text = string.Format(LanguageUtils.getText(110001), ItemNum.ToString("N0"));
                itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    Tip.CreateTip(LanguageUtils.getTextFormat(128005, m_itemList[index].topData.ToString("N0"), LanguageUtils.getText(m_currencyProxy.CurrencyDefine[currentSelectCurrencyID].l_desID))).Show();
                    m_currencyProxy.UseCurrencyResources(itemIndex);
                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                    mt.FlyUICurrency(currentSelectCurrencyID, m_itemList[index].topData, itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.transform.position);
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
        /// <summary>
        /// 全部使用，批量使用
        /// </summary>
        /// <param name="index"></param>
        /// <param name="btnName"></param>
        /// <param name="btnAction"></param>
        /// <param name="btnNane1"></param>
        /// <param name="btnAction1"></param>
        private void ShowPopMemu(int index, string btnName, Action<int> btnAction,string btnNane1 = "", Action<int> btnAction1 =null )
        {
            int tIndex = index;
            if (powTipView != null)
            {
                powTipView.m_UI_right.SetText(btnName);
                powTipView.m_UI_left.SetText(btnNane1);
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
                if (showItemid == -1 || batchUseNum < 1)
                {
                    CoreUtils.assetService.Destroy(obj);
                    return;
                }
                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_UseAddItemView>(obj);

                powTipView.m_UI_right.gameObject.SetActive(true);
                powTipView.m_UI_left.gameObject.SetActive(true);
                powTipView.m_UI_right.SetText(btnName);
                powTipView.m_UI_left.SetText(btnNane1);
                ClientUtils.UIReLayout(powTipView.m_pl_bg_ContentSizeFitter.gameObject);

                powTipView.m_UI_right.m_btn_languageButton_GameButton.onClick.AddListener(()=> {
                    btnAction(tIndex);
                });
                powTipView.m_UI_left.m_btn_languageButton_GameButton.onClick.AddListener(()=> {
                    btnAction1(tIndex);
                });

                var item = view.m_sv_list_view_ListView.GetItemByIndex(index);
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

        private int GetBatchUseNumber(int leftNum)
        {
            if(leftNum>1&&leftNum<10)
            {
                return leftNum;
            }
            if(leftNum>=10&&leftNum<50)
            {
                return 10;
            }
            if(leftNum>=50)
            {
                return 50;
            }
            return 0;
        }

    }
}