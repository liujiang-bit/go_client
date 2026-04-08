// =============================================================================== 
// Author              :    xzl
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    BagMediator 背包界面
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using System;
using Data;
using SprotoType;
using UnityEngine.UI;

namespace Game {

    public class BagParam
    {
        public int PageType; //菜单/分页类型 必传
        public int SubType;  //对应item表SubType属性
        public int Type;     //对应item表type属性
        public int TypeGroup;//对应item表TypeGroup属性
        public bool IsFindItem; //如果要查找具体物品 这个要传true
    }

    public class BagMediator : GameMediator {
        #region Member
        public static string NameMediator = "BagMediator";

        private PlayerProxy m_playerProxy;
        private BagProxy m_bagProxy;
        private CityBuffProxy m_cityBuffProxy;
        private Dictionary<Int64, ItemInfoEntity> m_items = new Dictionary<Int64, ItemInfoEntity>();
        private int m_currType = 0;
        private Dictionary<int, List<Int64>> m_itemDataDic = new Dictionary<int, List<Int64>>();
        private Dictionary<int, bool> m_sortDic = new Dictionary<int, bool>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private int m_itemCol = 4;
        private int m_itemLineCount = 0;

        private Vector3 m_zeroVec = Vector3.zero;
        private Vector3 m_oneVec = Vector3.one;

        private Dictionary<int, string> m_qualityDic = new Dictionary<int, string>();
        private int m_selectItemIndex;
        private ItemBagView m_selectItem;
        private ItemInfoEntity m_selectItemInfo;

        private float m_detailTextHeight;
        private float m_detailTextOffsetY;
        private float m_listDefaultHeight;

        private bool m_isAllowSwitch; //是否预先切换

        private bool m_isRequestUseing;

        private int m_lastBatchUseNum = 1;
        private bool m_isBatchUse;

        private int m_inputLimit = 9999;

        private int[] m_inputArr = new int[] { 0,0,0};

        private List<LanguageText> m_pageTextList;

        private List<LanguageText> m_pageReddotTextList;
        private List<GameObject> m_pageReddotImgList;

        private BagParam m_bagParam;
        private int m_jumpIndex = -1;

        #endregion

        //IMediatorPlug needs
        public BagMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public BagView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                CmdConstant.ItemInfoChange,
                CmdConstant.ItemUse,
                CmdConstant.ItemReddotChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ItemInfoChange:
                    //背包信息发生改变
                    ItemInfoChangeRefresh();
                    break;
                case CmdConstant.ItemUse:
                    //m_isRequestUseing = false;
                    break;
                case CmdConstant.ItemReddotChange:
                    UpdatePageMenuReddot();
                    break;
                default:
                    break;
            }
        }

        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {

        }

        public override void OnRemove()
        {
            base.OnRemove();
            if (m_bagProxy != null)
            {
                m_bagProxy.ClearAllReddotRecord();
            }
        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            view.m_ipt_num_GameInput.placeholder.GetComponent<LanguageText>().text = "";
            m_pageTextList = new List<LanguageText>();
            m_pageTextList.Add(view.m_lbl_res_LanguageText);
            m_pageTextList.Add(view.m_lbl_speed_LanguageText);
            m_pageTextList.Add(view.m_lbl_gain_LanguageText);
            m_pageTextList.Add(view.m_lbl_equip_LanguageText);
            m_pageTextList.Add(view.m_lbl_other_LanguageText);

            m_pageReddotImgList = new List<GameObject>();
            m_pageReddotImgList.Add(view.m_img_redpoint1_PolygonImage.gameObject);
            m_pageReddotImgList.Add(view.m_img_redpoint2_PolygonImage.gameObject);
            m_pageReddotImgList.Add(view.m_img_redpoint3_PolygonImage.gameObject);
            m_pageReddotImgList.Add(view.m_img_redpoint4_PolygonImage.gameObject);
            m_pageReddotImgList.Add(view.m_img_redpoint5_PolygonImage.gameObject);

            m_pageReddotTextList = new List<LanguageText>();
            m_pageReddotTextList.Add(view.m_lbl_num1_LanguageText);
            m_pageReddotTextList.Add(view.m_lbl_num2_LanguageText);
            m_pageReddotTextList.Add(view.m_lbl_num3_LanguageText);
            m_pageReddotTextList.Add(view.m_lbl_num4_LanguageText);
            m_pageReddotTextList.Add(view.m_lbl_num5_LanguageText);

            m_qualityDic[1] = RS.ItemQualityBg[0];
            m_qualityDic[2] = RS.ItemQualityBg[1];
            m_qualityDic[3] = RS.ItemQualityBg[2];
            m_qualityDic[4] = RS.ItemQualityBg[3];
            m_qualityDic[5] = RS.ItemQualityBg[4];

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;

            RectTransform detailRect = view.m_lbl_detail_LanguageText.GetComponent<RectTransform>();
            m_detailTextHeight = detailRect.sizeDelta.y;
            m_detailTextOffsetY = detailRect.anchoredPosition.y;
            m_listDefaultHeight = view.m_sv_list_detail_ListView.GetComponent<RectTransform>().rect.height;

            List<string> prefabNames = new List<string>();
            prefabNames.Add("UI_Item_Bag");
            prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);

            if (view.data != null)
            {
                BagParam param = view.data as BagParam;
                m_bagParam = param;
                m_currType = (int)param.PageType;
            }
            else
            {
                m_currType = 1;
            }

            DataProcess();

            m_selectItemIndex = -1;
            FindItem();

            view.m_pl_group_ToggleGroup.allowSwitchOff = false;

            view.m_pl_mes_Image.gameObject.SetActive(false);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_bg.setCloseHandle(Close);
            view.m_btn_operate_GameButton.onClick.AddListener(BtnOperate);
            view.m_btn_substract_GameButton.onClick.AddListener(OnSubstract);
            view.m_btn_max_GameButton.onClick.AddListener(OnMax);
            view.m_btn_add_GameButton.onClick.AddListener(OnAdd);
            view.m_ipt_num_GameInput.onValueChanged.AddListener(OnInputChange);
            view.m_ipt_num_GameInput.onEndEdit.AddListener(OnInputValueSubmit);

            view.m_ck_res_GameToggle.onValueChanged.AddListener(OnMenuRes);
            view.m_ck_speed_GameToggle.onValueChanged.AddListener(OnMenuSpeed);
            view.m_ck_equip_GameToggle.onValueChanged.AddListener(OnMenuEquip);
            view.m_ck_gain_GameToggle.onValueChanged.AddListener(OnMenuGain);
            view.m_ck_other_GameToggle.onValueChanged.AddListener(OnMenuOther);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            view.m_pl_mes_Image.gameObject.SetActive(true);

            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            UpdatePageMenuReddot();

            InitBagList();

            m_isAllowSwitch = false;
            SetDefaultSelectMenu();
            m_isAllowSwitch = true;

            SetPageTextColor(m_currType, true);
            if (m_selectItemIndex > 0)
            {
                SwitchMenuRefresh();
                view.m_sv_list_view_ListView.MovePanelToItemIndex(m_jumpIndex);
            }
            else
            {
                m_selectItemIndex = 0;
                SwitchMenuRefresh();
            }

            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.Bag);
        }

        //初始化背包list
        private void InitBagList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
        }

        private void RefreshBagList(bool isForceRefreshPos = true)
        {
            if (isForceRefreshPos)
            {
                view.m_sv_list_view_ListView.RefreshAndRestPos();
            }
            m_itemLineCount = (int)Math.Ceiling((float)m_itemDataDic[m_currType].Count / m_itemCol);
            //Debug.Log("m_itemLineCount:"+ m_itemLineCount);
            view.m_sv_list_view_ListView.FillContent(m_itemLineCount);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            BagColElement itemColScript = null;
            if (listItem.isInit == false)
            {
                itemColScript = MonoHelper.AddHotFixViewComponent<BagColElement>(listItem.go);
                listItem.isInit = true;
                for (int i = 0; i < m_itemCol; i++)
                {
                    GameObject itemObj = CoreUtils.assetService.Instantiate(m_assetDic["UI_Item_Bag"]);

                    itemObj.transform.SetParent(listItem.go.transform);
                    itemObj.transform.localPosition = Vector3.zero;
                    itemObj.transform.localScale = Vector3.one;
                    ItemBagView itemView = MonoHelper.AddHotFixViewComponent<ItemBagView>(itemObj.gameObject);
                    itemView.m_UI_Model_Item.SetSelectImgActive(false);
                    itemColScript.ElemItemList.Add(itemView);
                    itemColScript.ElemIndexList.Add(-1);

                    int num = i;
                    itemView.m_UI_Model_Item.AddBtnListener(()=>{
                        ClickItem(itemColScript.ElemItemList[num], itemColScript.ElemIndexList[num]);
                    });
                }
            }
            else
            {
                itemColScript = MonoHelper.GetOrAddHotFixViewComponent<BagColElement>(listItem.go);
            }

            ItemBagView nodeView;
            int min = listItem.index* m_itemCol;
            int max = min + (m_itemCol - 1);
            int tnum = -1;
            for (int i = min; i <= max; i++)
            {
                tnum = i - min;
                nodeView = itemColScript.ElemItemList[tnum];
                itemColScript.ElemIndexList[tnum] = i;
                if (i >=m_itemDataDic[m_currType].Count)
                {
                    nodeView.gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    nodeView.gameObject.SetActive(true);
                }
                RefreshItem(nodeView, i);
            }
        }

        //刷新item
        private void RefreshItem(ItemBagView itemView, int index)
        {
            Int64 itemIndex = m_itemDataDic[m_currType][index];
            ItemInfoEntity itemInfo = m_items[itemIndex];
            if (itemInfo == null)
            {
                return;
            }
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemInfo.itemId);
            if (itemDefine == null)
            {
                Debug.LogWarningFormat("not find itemId:{0}", itemInfo.itemId);
                return;
            }

            if (m_selectItemIndex == index)
            {
                itemView.m_UI_Model_Item.Refresh(itemDefine, itemInfo.overlay, true);
                m_selectItem = itemView;
            }
            else
            {
                itemView.m_UI_Model_Item.Refresh(itemDefine, itemInfo.overlay, false);
            }

            //红点显示
            bool isNew = m_bagProxy.IsShowReddot(itemInfo.itemIndex, itemDefine.ID);
            itemView.m_pl_new.gameObject.SetActive(isNew);
        }

        //刷新详情
        private void RefreshItemDetail()
        {
            if (m_selectItemIndex >= m_itemDataDic[m_currType].Count)
            {
                return;
            }
            Int64 itemIndex = m_itemDataDic[m_currType][m_selectItemIndex];
            ItemInfoEntity itemInfo = m_items[itemIndex];
            m_selectItemInfo = itemInfo;
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemInfo.itemId);

            //清除红点
            m_bagProxy.ClearReddotRecordByIndex(itemIndex, itemInfo.itemId);
            if (m_selectItem != null)
            {
                m_selectItem.m_pl_new.gameObject.SetActive(false);
            }

            //设置品质图片
            ClientUtils.LoadSprite(view.m_img_item_quality_PolygonImage, GetQualityImg(itemDefine.quality));
            //设置icon
            ClientUtils.LoadSprite(view.m_img_item_PolygonImage, itemDefine.itemIcon);

            if (itemDefine.l_topID < 1)
            {
                view.m_pl_desc_bg_PolygonImage.transform.localScale = m_zeroVec;
            }
            else
            {
                view.m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
                view.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), ClientUtils.FormatComma(itemDefine.topData));
            }
            //名称
            view.m_lbl_item_name_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);

            //详情

            if (itemDefine.typeGroup == 2)//装备
            {
                view.m_lbl_own_num_LanguageText.gameObject.SetActive(false);
                view.m_sv_equip_ListView.gameObject.SetActive(true);
                view.m_sv_list_detail_ListView.gameObject.SetActive(false);

                EquipItemInfo equipInfo = m_bagProxy.GetEquipItemInfo(itemIndex);
                if (equipInfo != null)
                {
                    view.m_UI_Item_EquipAtt.SetEquipItemInfo(equipInfo);
                    float itemHeight = view.m_UI_Item_EquipAtt.GetHeight();
                    view.m_c_equip.sizeDelta = new Vector2(view.m_c_equip.sizeDelta.x, itemHeight);
                }
            }
            else
            {
                view.m_lbl_own_num_LanguageText.gameObject.SetActive(true);
                view.m_sv_equip_ListView.gameObject.SetActive(false);
                view.m_sv_list_detail_ListView.gameObject.SetActive(true);

                view.m_lbl_detail_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_desID), ClientUtils.FormatComma(itemDefine.desData1), ClientUtils.FormatComma(itemDefine.desData2));
                RectTransform listRect = view.m_sv_list_detail_ScrollRect.GetComponent<RectTransform>();
                RectTransform textRect = view.m_lbl_detail_LanguageText.GetComponent<RectTransform>();
                float textHeight = view.m_lbl_detail_LanguageText.preferredHeight - m_detailTextOffsetY;

                if (textHeight > m_listDefaultHeight)
                {
                    view.m_c_list_detail.sizeDelta = new Vector2(view.m_c_list_detail.sizeDelta.x, textHeight);
                    textRect.anchoredPosition = new Vector2(textRect.anchoredPosition.x, textRect.anchoredPosition.y - (view.m_lbl_detail_LanguageText.preferredHeight - m_detailTextHeight) / 2);
                }
                else
                {
                    view.m_c_list_detail.sizeDelta = new Vector2(view.m_c_list_detail.sizeDelta.x, m_listDefaultHeight);
                    textRect.anchoredPosition = new Vector2(textRect.anchoredPosition.x, m_detailTextOffsetY);
                }
            }

            if (itemDefine.batchUse == 1)
            {
                m_isBatchUse = true;
                view.m_pl_input_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_lbl_own_num_LanguageText.text = string.Format(LanguageUtils.getText(110001), ClientUtils.FormatComma(itemInfo.overlay));
                Int64 batch = Math.Min(m_lastBatchUseNum, itemInfo.overlay);
                batch = Math.Max(1, batch);
                RefreshAddOrSubstract((int)batch, (int)itemInfo.overlay);
                SetIptText((int)batch);
                view.m_btn_operate_GameButton.gameObject.SetActive(true);
                view.m_lbl_operate_LanguageText.text = LanguageUtils.getText(itemDefine.l_buttonDes);
            }
            else
            {
                m_lastBatchUseNum = 1;
                m_isBatchUse = false;
                view.m_lbl_own_num_LanguageText.text = string.Format(LanguageUtils.getText(110001), ClientUtils.FormatComma(itemInfo.overlay));
                view.m_pl_input_ArabLayoutCompment.gameObject.SetActive(false);
                if (itemDefine.l_buttonDes < 1)
                {
                    view.m_btn_operate_GameButton.gameObject.SetActive(false);
                }
                else
                {
                    view.m_btn_operate_GameButton.gameObject.SetActive(true);
                    view.m_lbl_operate_LanguageText.text = LanguageUtils.getText(itemDefine.l_buttonDes);
                }
            }
        }

        //获取品质图标
        private string GetQualityImg(int quality)
        {
            if(m_qualityDic.ContainsKey(quality))
            {
                return m_qualityDic[quality];
            }
            return "";
        }

        private void ClickItem(ItemBagView itemView, int index)
        {
            if (m_selectItem != null)
            {
                m_selectItem.m_UI_Model_Item.SetSelectImgActive(false);
            }
            m_selectItemIndex = index;
            m_selectItem = itemView;
            m_selectItem.m_UI_Model_Item.SetSelectImgActive(true);
            RefreshItemDetail();
        }

        private int GetInputNum()
        {
            string inputStr = view.m_ipt_num_GameInput.text;
            int num = int.Parse(inputStr);
            return num;
        }

        private void SetIptText(int num)
        {
            SetIptFormatText(num);
            view.m_ipt_num_GameInput.text = num.ToString();
        }

        private void SetIptFormatText(int num)
        {
            m_lastBatchUseNum = num;
            view.m_lbl_ipt_format_LanguageText.text = ClientUtils.FormatComma(num);
        }

        //减少
        private void OnSubstract()
        {
            int num = GetInputNum();
            if (num <= 1)
            {
                return;
            }
            view.m_img_add_gray_PolygonImage.gameObject.SetActive(false);
            view.m_img_add_normal_PolygonImage.gameObject.SetActive(true);
            num = num - 1;
            if (num == 1)
            {
                view.m_img_substract_gray_PolygonImage.gameObject.SetActive(true);
                view.m_img_substract_normal_PolygonImage.gameObject.SetActive(false);
            }
            SetIptText(num);
            NumChange(num.ToString());
        }

        //增加
        private void OnAdd()
        {
            int num = GetInputNum();
            Int64 itemId = m_itemDataDic[m_currType][m_selectItemIndex];
            int max = Mathf.Min((int)m_items[itemId].overlay, m_inputLimit);
            if (num >= max)
            {
                return;
            }
            num = num + 1;
            SetIptText(num);
            NumChange(num.ToString());
        }

        //最大
        private void OnMax()
        {
            Int64 itemId = m_itemDataDic[m_currType][m_selectItemIndex];
            if (!m_items.ContainsKey(itemId))
            {
                return;
            }
            int num = (int)m_items[itemId].overlay;
            SetIptText(num);
            NumChange(num.ToString());
        }

        //输入框变更
        private void OnInputChange(string val)
        {
            view.m_lbl_ipt_format_LanguageText.text = val;
        }

        private void NumChange(string val)
        {
            if (val.Length < 1)
            {
                SetIptText(1);
                return;
            }
            int num = 0;
            bool isChange = false;
            if (val.Length < 1)
            {
                num = 1;
                isChange = true;
            }
            else
            {
                try
                {
                    num = int.Parse(val);
                }
                catch
                {
                    if (m_selectItemInfo != null)
                    {
                        num = (int)m_selectItemInfo.overlay;
                    }
                    else
                    {
                        num = 1;
                    }
                    isChange = true;
                }
                if (val.Length != num.ToString().Length)
                {
                    SetIptText(num);
                    return;
                }
            }
            if (num < 1)
            {
                num = 1;
                isChange = true;
            }
            else if (m_selectItemInfo != null)
            {
                m_inputArr[0] = num;
                m_inputArr[1] = (int)m_selectItemInfo.overlay;
                m_inputArr[2] = m_inputLimit;
                //ClientUtils.Print(m_inputArr);
                num = Mathf.Min(m_inputArr);
                isChange = true;
            }
            if (isChange)
            {
                SetIptText(num);
            }
            SetIptFormatText(num);
            if (m_selectItemInfo != null)
            {
                RefreshAddOrSubstract(num, (int)m_selectItemInfo.overlay);
            }
        }

        private void OnInputValueSubmit(string val)
        {
            NumChange(val);
        }

        private void RefreshAddOrSubstract(int num, int overlay)
        {
            int max = Mathf.Min(overlay, m_inputLimit);
            view.m_img_add_gray_PolygonImage.gameObject.SetActive(num == max);
            view.m_img_add_normal_PolygonImage.gameObject.SetActive(!(num == max));
            view.m_img_substract_gray_PolygonImage.gameObject.SetActive(num == 1);
            view.m_img_substract_normal_PolygonImage.gameObject.SetActive(!(num == 1));
        }

        //使用 锻造 合成
        private void BtnOperate()
        {
            //测试断线重连
            //int count = m_itemDataDic[m_currType].Count - 1;
            //for (int i = count; i > (count - 1); i--)
            //{
            //    m_items.Remove(m_itemDataDic[m_currType][i]);
            //}
            //DisReconnect();

            ////测试物品更新
            //AppFacade.GetInstance().SendNotification(CmdConstant.ItemInfoChange);

            int inputNum = 0;
            if (view.m_ipt_num_GameInput.IsActive())
            {
                try
                {
                    inputNum = Convert.ToInt32(view.m_ipt_num_GameInput.text);
                }
                catch
                {
                    inputNum = 0;
                }
            }
            else
            {
                inputNum = 1;
            }
            if (inputNum == 0)
            {
                return;
            }
            if (m_selectItemInfo == null)
            {
                return;
            }

            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_selectItemInfo.itemId);

            int itemFunc = itemDefine.itemFunction;
            if (itemFunc == 1) //礼包类道具
            {
                Send(m_selectItemInfo.itemIndex, inputNum, 0);
            }
            else if (itemFunc == 2) //礼包类道具 选择奖励
            {
                BagGiftOpenParam param = new BagGiftOpenParam();
                param.ItemIndex = m_selectItemInfo.itemIndex;
                param.ItemId = m_selectItemInfo.itemId;
                param.Num = inputNum;
                param.ShowType = 1;
                CoreUtils.uiManager.ShowUI(UI.s_bagGiftOpen, null, param);
            }
            else if (itemFunc == 3) //礼包类道具 兑换奖励
            {
                BagGiftOpenParam param = new BagGiftOpenParam();
                param.ItemIndex = m_selectItemInfo.itemIndex;
                param.ItemId = m_selectItemInfo.itemId;
                param.Num = inputNum;
                param.ShowType = 2;
                CoreUtils.uiManager.ShowUI(UI.s_bagGiftOpen, null, param);
            }
            else if (itemFunc == 4)//使用城市buff道具
            {

                m_cityBuffProxy.SendUseItem(itemDefine.data2, 1, () =>
                {
                    Send(m_selectItemInfo.itemIndex, 1, 0);
                });
            }
            else if (itemFunc == 5)//VIP经验道具使用
            {
                if (m_playerProxy.GetVipLevel() >= m_playerProxy.GetMaxVipLevel())
                {
                    Tip.CreateTip(LanguageUtils.getText(800122)).Show();
                    return;
                }
                Send(m_selectItemInfo.itemIndex, inputNum, 0);
            }
            else if (itemFunc == 6) //行动力道具使用
            {
                Send(m_selectItemInfo.itemIndex, inputNum, 0);
            }
            else if (itemFunc == 7)//跳转界面
            {
                if (SystemOpen.IsCanOpenByUiId(itemDefine.data1, true))
                {
                    OpenUiCommonParam param = new OpenUiCommonParam();
                    param.OpenUiId = itemDefine.data1;
                    param.IntData = itemDefine.data2;
                    object[] dataArr = new object[2];
                    dataArr[0] = itemDefine.data1;
                    dataArr[1] = param;

                    Close();
                    AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI2, dataArr);
                }
            }
            else if (itemFunc == 10) //使用王国地图
            {
                if (WarFogMgr.IsAllFogOpen())
                {
                    //无迷雾 直接使用物品
                    Send(m_selectItemInfo.itemIndex, inputNum, 0);
                }
                else
                {
                    Close();
                    CoreUtils.uiManager.ShowUI(UI.s_openFogShow, null, m_selectItemInfo.itemIndex);
                }
            }
            else if (itemFunc == 12) // 召唤怪物
            {
                Send(m_selectItemInfo.itemIndex, 1, 0);
                Close();
            }
            else if (itemFunc == 13)//工人招募
            {
                Send(m_selectItemInfo.itemIndex, inputNum, 0);
            }
            else if (itemFunc == 14) //增益道具
            {
                if (m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacityCount > 0 && m_playerProxy.CurrentRoleInfo.itemAddTroopsCapacity != itemDefine.data1)
                {
                    Alert.CreateAlert(192038).SetRightButton(null, LanguageUtils.getText(192010))
                                             .SetLeftButton(() =>
                                             {
                                                 if (view.gameObject == null)
                                                 {
                                                     return;
                                                 }
                                                 Send(m_selectItemInfo.itemIndex, inputNum, 0);
                                             }, 
                                             LanguageUtils.getText(192009)).Show();
                    return;
                }
                Send(m_selectItemInfo.itemIndex, inputNum, 0);
            }
            else if (itemFunc == 17)//界面跳转
            {
                Tip.CreateTip(LanguageUtils.getText(100045)).Show();
            }
            else if (itemFunc == 30)//通用迁城
            {
                if (m_cityBuffProxy.CheckGuildBuildCreatePre())
                {
                    CoreUtils.uiManager.ShowUI(UI.s_moveCity, null, itemDefine.data1);
                }
                CoreUtils.uiManager.CloseUI(UI.s_bagInfo);
            }
            else if (itemFunc == 31) //随机迁城
            {
                if (m_cityBuffProxy.CheckGuildBuildCreatePre())
                {
                    if (m_cityBuffProxy.CheckMoveCity(true))
                    {
                        Alert.CreateAlert(LanguageUtils.getText(770101))
                   .SetRightButton(null, LanguageUtils.getText(192010))
                       .SetLeftButton(() =>
                       {
                           Map_MoveCity.request request = new Map_MoveCity.request();
                           request.type = 4;
                           AppFacade.GetInstance().SendSproto(request);
                           CoreUtils.uiManager.CloseUI(UI.s_bagInfo);
                           float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                           WorldCamera.Instance().SetCameraDxf(Firstdxf, 0, () => { });
                       }, LanguageUtils.getText(192009))
                   .Show();
                    }
                    else
                    {

                    }
                }
            }
            else if (itemFunc == 35)
            {
                Send(m_selectItemInfo.itemIndex, inputNum, 0);
            }
            else
            {
                Tip.CreateTip(LanguageUtils.getText(100045)).Show();
                //if (itemDefine.lv > m_playerProxy.GetTownHall())
                //{
                //    Debug.LogError(LanguageUtils.getText(128000));
                //    return;
                //}
            }
        }

        //设置默认选中菜单
        private void SetDefaultSelectMenu()
        {
            if (m_currType == 1)
            {
                view.m_ck_res_GameToggle.isOn = true;
            }
            else if (m_currType == 2)
            {
                view.m_ck_speed_GameToggle.isOn = true;
            }
            else if (m_currType == 3)
            {
                view.m_ck_gain_GameToggle.isOn = true;
            }
            else if (m_currType == 4)
            {
                view.m_ck_equip_GameToggle.isOn = true;
            }
            else if (m_currType == 5)
            {
                view.m_ck_other_GameToggle.isOn = true;
            }
            //if (m_isForceSwitch) //如果没有刷新 则再强制刷新一下
            //{
            //    SwitchMenu(m_currType);
            //}
        }

        private void SwitchMenuRefresh()
        {
            //排序处理
            if (!m_sortDic.ContainsKey(m_currType))
            {
                DataSort();
            }

            RefreshContent(true);
        }

        private void RefreshContent(bool isRefreshListPos)
        {
            if (m_itemDataDic[m_currType].Count <= 0)
            {
                view.m_pl_list_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_pl_detail_PolygonImage.gameObject.SetActive(false);
                view.m_lbl_no_item_LanguageText.gameObject.SetActive(true);
            }
            else
            {
                view.m_pl_list_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_pl_detail_PolygonImage.gameObject.SetActive(true);
                view.m_lbl_no_item_LanguageText.gameObject.SetActive(false);

                RefreshBagList(isRefreshListPos);
            }
            RefreshItemDetail();
        }

        //资源
        private void OnMenuRes(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(1);
            }
            else
            {
                SetPageTextColor(1, false);
            }
        }

        //加速
        private void OnMenuSpeed(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(2);
            }
            else
            {
                SetPageTextColor(2, false);
            }
        }

        //增益
        private void OnMenuGain(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(3);
            }
            else
            {
                SetPageTextColor(3, false);
            }
        }

        //装备
        private void OnMenuEquip(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(4);
            }
            else
            {
                SetPageTextColor(4, false);
            }
        }

        //其它
        private void OnMenuOther(bool isBool)
        {
            if (isBool)
            {
                SwitchMenu(5);
            }
            else
            {
                SetPageTextColor(5, false);
            }
        }

        private void SwitchMenu(int type)
        {
            if (!m_isAllowSwitch)
            {
                return;
            }
            if (type == m_currType)
            {
                return;
            }
            m_bagProxy.ClearReddotRecordByType(m_currType);
            SetPageTextColor(type, true);
            m_currType = type;
            m_selectItemIndex = 0;
            SwitchMenuRefresh();
        }

        private void SetPageTextColor(int index, bool isShow)
        {
            int page = index - 1;
            if (isShow)
            {
                m_pageTextList[page].color = ClientUtils.StringToHtmlColor("#FFFFFF");
            } else
            {
                m_pageTextList[page].color = ClientUtils.StringToHtmlColor("#A49D92");
            }
        }

        private void DataProcess()
        {
            //获取item数据
            if (m_itemDataDic.Count > 0)
            {
                for (int i = 1; i < 6; i++)
                {
                    m_itemDataDic[i].Clear();
                }
            }
            else
            {
                m_items = m_bagProxy.Items;
                for (int i = 1; i < 6; i++)
                {
                    m_itemDataDic[i] = new List<Int64>();
                }
            }

            int type = 0;
            foreach (var data in m_items)
            {
                if (data.Value.overlay > 0)
                {
                    type = m_bagProxy.GetItemTypeById(data.Value.itemId);
                    if (type > 0 && type < 6)
                    {
                        m_itemDataDic[type].Add(data.Value.itemIndex);
                    }
                }
            }
        }

        //数据排序
        private void DataSort()
        {
            m_itemDataDic[m_currType].Sort((x, y) => {
                int re = GetRank(m_items[x].itemId).CompareTo(GetRank(m_items[y].itemId));
                if (re == 0)
                {
                    re = m_items[x].itemId.CompareTo(m_items[y].itemId);
                }
                return re;
            });
            m_sortDic[m_currType] = true;
        }

        private int GetRank(Int64 itemId)
        {
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemId);
            return itemDefine.rank;
        }

        //断线重连处理
        private void DisReconnect()
        {
            //重新获取数据
            DataProcess();

            //排序下
            DataSort();

            //更新下被选中的item
            UpdateSelectItemIndex();

            //刷新内容显示
            RefreshContent(false);
        }

        private void UpdateSelectItemIndex()
        {
            int lastIndex = m_selectItemIndex;
            if (m_selectItemInfo != null)
            {
                m_selectItemIndex = m_itemDataDic[m_currType].FindIndex(item => item.Equals(m_selectItemInfo.itemIndex));
            }
            else
            {
                m_selectItemIndex = -1;
            }
            if (m_selectItemIndex < 0)
            {
                int newIndex = lastIndex;
                int count = m_itemDataDic[m_currType].Count;

                if (newIndex >= 0 && newIndex < count)
                {
                    m_selectItemIndex = newIndex;
                }
                else
                {
                    if (count > 0)
                    {
                        m_selectItemIndex = count - 1;
                    }
                    else
                    {
                        m_selectItemIndex = 0;
                    }
                }
            }
        }

        //item信息变更刷新
        private void ItemInfoChangeRefresh()
        {
            m_items = m_bagProxy.Items;
            int type = 0;
            Int64 itemIndex = 0;
            for (int i = 0; i < m_bagProxy.ItemChangeList.Count; i++)
            {
                itemIndex = m_bagProxy.ItemChangeList[i];

                type = m_bagProxy.GetItemTypeById(m_items[itemIndex].itemId);
                if (m_items[itemIndex].overlay > 0)
                {
                    m_sortDic[type] = false;
                    if (!m_itemDataDic[type].Contains(itemIndex))
                    {
                        //新增物品
                        m_itemDataDic[type].Add(itemIndex);
                    }
                    else
                    {
                        //数量更新
                        view.m_sv_list_view_ListView.ForceRefresh();
                    }
                }
                else
                {
                    //物品移除了
                    m_itemDataDic[type].Remove(itemIndex);
                    m_sortDic[type] = false;
                }
            }
            if (m_sortDic[m_currType] == false)
            {
                //排序下
                DataSort();

                //更新下被选中的item
                UpdateSelectItemIndex();

                //刷新内容显示
                RefreshContent(false);
            }
        }

        private void FindItem()
        {
            if (m_bagParam!=null && m_bagParam.IsFindItem)
            {
                if (m_itemDataDic[m_currType].Count < 1)
                {
                    return;
                }

                //先排序下
                DataSort();
                List<Int64> dataList = m_itemDataDic[m_currType];

                List<ItemInfoEntity> tempList = new List<ItemInfoEntity>();
                if (m_bagParam.SubType > 0)
                {
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        ItemInfoEntity itemInfo = m_items[dataList[i]];
                        ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemInfo.itemId);
                        if (itemDefine != null && itemDefine.subType == m_bagParam.SubType)
                        {
                            tempList.Add(itemInfo);
                        }
                    }
                }
                else if (m_bagParam.Type > 0)
                {
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        ItemInfoEntity itemInfo = m_items[dataList[i]];
                        ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemInfo.itemId);
                        if (itemDefine != null && itemDefine.type == m_bagParam.Type)
                        {
                            if (m_bagParam.TypeGroup > 0)
                            {
                                if (itemDefine.typeGroup == m_bagParam.TypeGroup)
                                {
                                    tempList.Add(itemInfo);
                                }
                            }
                            else
                            {
                                tempList.Add(itemInfo);
                            }
                        }
                    }
                }
                if (tempList.Count > 0)
                {
                    //升序排序
                    tempList.Sort((x, y) => {
                        return x.itemId.CompareTo(y.itemId);
                    });

                    int findIndex = -1;
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        if (dataList[i] == tempList[0].itemIndex)
                        {
                            findIndex = i;
                            break;
                        }
                    }
                    if (findIndex != -1)
                    {
                        m_selectItemIndex = findIndex;
                        findIndex = findIndex + 1;
                        int itemNum = (int)Math.Ceiling((float)findIndex / m_itemCol);
                        m_jumpIndex = itemNum - 1;
                        m_jumpIndex = (m_jumpIndex < 0) ? 0 : m_jumpIndex;
                    }
                }
            }
        }

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_bagInfo);
        }

        private void Send(Int64 itemIndex, Int64 itemNum, Int64 id)
        {
            //if (m_isRequestUseing)
            //{
            //    return;
            //}
            //m_isRequestUseing = true;
            var sp = new Item_ItemUse.request();
            sp.itemIndex = itemIndex;
            sp.itemNum = itemNum;
            if (id > 0)
            {
                sp.id = id;
            }
            AppFacade.GetInstance().SendSproto(sp);
        }

        //更新菜单红点
        private void UpdatePageMenuReddot()
        {
            long total = 0;
            int index = 0;
            int count = m_pageReddotImgList.Count;
            for (int i = 1; i <= count; i++)
            {
                index = i - 1;
                total = m_bagProxy.GetBagReddotNumByType(i);
                if (total > 0)
                {
                    m_pageReddotImgList[index].SetActive(true);
                    m_pageReddotTextList[index].text = UIHelper.NumerBeyondFormat((int)total);
                }
                else
                {
                    m_pageReddotImgList[index].SetActive(false);
                }
            }
        }
    }
}