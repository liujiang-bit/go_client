// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    UI_Win_MaterialMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Core;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

namespace Game {
    public enum MaterialPageType
    {
        Production=0,//生产
        Mix,         //合成
        Resolve,     //分解
    }

    public enum MaterialType
    {
        Material=100000, //材料
        Drawing=200000,  //图纸
    }
    
    public class UI_Win_MaterialMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_MaterialMediator";

        private PlayerProxy m_playerProxy;
        private BagProxy m_bagProxy;
        private CurrencyProxy m_currencyProxy;
        private MaterialPageType m_curPageType;
        private MaterialType m_curMixPage;
        private int m_curQuickOperationItemID = 0;
        private bool m_isSendingMsg = false;
        private UI_Item_BlacksmithQueue_SubView[] m_productionGo = new UI_Item_BlacksmithQueue_SubView[5];
        
        private Dictionary<string, GameObject> m_productionAssetDic = new Dictionary<string, GameObject>();
        private Dictionary<string,GameObject> m_mixAssetDic = new Dictionary<string, GameObject>();
        private Dictionary<string,GameObject> m_resolveDic = new Dictionary<string, GameObject>();
        private List<int> m_productionItemInfos = new List<int>();
        private Dictionary<int,List<MaterialItem>> m_materialItemInfos = new Dictionary<int,List<MaterialItem>>();
        private Dictionary<int,List<MaterialItem>> m_drawingItemInfos = new Dictionary<int, List<MaterialItem>>();
        private List<UI_Model_Item_SubView> m_resolveItems = new List<UI_Model_Item_SubView>();
        private List<UI_Model_Item_SubView> m_mixItems = new List<UI_Model_Item_SubView>();
        
        
        private Timer m_produceTimer;

        private int MINQUICKOPERATIONNUM = 10;
        private int LISTITEMNUMPERROW = 4;

        private List<string> m_showTypeItemPrefabName = new List<string>()
        {
            "UI_Item_MaterialTitle",
            "UI_Item_MaterialElement",
        };

        private MaterialItem m_curSelectMaterialItem;
        private int m_curSelectIndex = -1;
        private int m_mixCurItemInfoIndex = 0;
        private Timer m_itemChangeTimer = null;

        private Dictionary<string, int> m_effectLoadStatus = new Dictionary<string, int>(); // 1加载中 2已加载
        private bool m_isJumpHandle;
        private MaterialType m_jumpMixType;
        private int m_jumpMixItemId;
        private int m_jumpMixIndex = -1;

        private string m_lastEffectName;
        private Transform m_lastParentTrans;
        private int m_lastQuality;

        private Dictionary<string, GameObject> m_loadedItemEffect = new Dictionary<string, GameObject>();

        #endregion

        //IMediatorPlug needs
        public UI_Win_MaterialMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_MaterialView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.EquipMaterialProductionInfoChange,
                CmdConstant.ItemInfoChange,
                CmdConstant.UpdateCurrency,
                Build_MaterialDecomposition.TagName,
                Build_ProduceMaterial.TagName,
                Build_MaterialSynthesis.TagName,
                Build_CancelProduceMaterial.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.EquipMaterialProductionInfoChange:
                    RefreshProducingItemInfo();
                    SetSendingMsgStatus(false);
                    break;
                case CmdConstant.ItemInfoChange:
                    if (m_itemChangeTimer != null)
                    {
                        return;
                    }
                    m_itemChangeTimer = Timer.Register(0.1f, () =>
                    {
                        UpdateMaterialInfo();
                        UpDateRedDot();
                        if (m_curPageType == MaterialPageType.Mix)
                        {
                            RefreshMix();
                        }else if (m_curPageType == MaterialPageType.Production)
                        {
                            RefreshProductionList();
                        }else if (m_curPageType == MaterialPageType.Resolve)
                        {
                            RefreshResolve();
                        }
                        SetSendingMsgStatus(false);
                        m_itemChangeTimer = null;
                    });
                    break;
                case  Build_MaterialDecomposition.TagName:
                case  Build_ProduceMaterial.TagName:
                case  Build_MaterialSynthesis.TagName:
                case  Build_CancelProduceMaterial.TagName:
                    SetQuickOperationItemID(0);
                    SetSendingMsgStatus(false);
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
            ClearProduceTimer();
            if (m_itemChangeTimer != null)
            {
                m_itemChangeTimer.Cancel();
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;

            if (view.data!=null)
            {
                if (view.data is OpenUiCommonParam)
                {
                    OpenUiCommonParam param = view.data as OpenUiCommonParam;
                    if (param.OpenUiId == 1015) //生产
                    {
                        m_curPageType = MaterialPageType.Production;
                    }
                    else if (param.OpenUiId == 1016)//合成 材料
                    {
                        m_curPageType = MaterialPageType.Mix;
                        m_isJumpHandle = true;
                        m_jumpMixType = MaterialType.Material;
                        m_jumpMixItemId = param.IntData;
                    }
                    else if (param.OpenUiId == 1039)//合成 图纸
                    {
                        m_curPageType = MaterialPageType.Mix;
                        m_isJumpHandle = true;
                        m_jumpMixType = MaterialType.Drawing;
                        m_jumpMixItemId = param.IntData;
                    }
                    else if (param.OpenUiId == 1017) //分解
                    {
                        m_curPageType = MaterialPageType.Resolve;
                    }
                }
                else
                {
                    m_curPageType = (MaterialPageType)view.data;
                }
            }
            else
            {
                m_curPageType = MaterialPageType.Production;
            }

            UpdateMaterialInfo();
            InitProduction();
            InitMix();
            InitResolve();
            
            view.m_img_resolveItem.gameObject.SetActive(false);
            view.m_img_mixItem.gameObject.SetActive(false);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.AddPageListener((int)MaterialPageType.Production,OnShowProductionPage);
            view.m_UI_Model_Window_Type1.AddPageListener((int)MaterialPageType.Mix,OnShowMixPage);
            view.m_UI_Model_Window_Type1.AddPageListener((int)MaterialPageType.Resolve,OnShowResolvePage);
            view.m_UI_Model_Window_Type1.setCloseHandle(OnClickClose);
            
            view.m_ck_material_GameToggle.onValueChanged.AddListener(OnClickMaterialPage);
            view.m_ck_picture_GameToggle.onValueChanged.AddListener(OnClickDrawingPage);
            view.m_btn_mix.m_btn_languageButton_GameButton.onClick.AddListener(OnClickMix);
            view.m_btn_resolve.AddClickEvent(OnClickResolve);
            
            view.m_btn_back_GameButton.onClick.AddListener(OnCloseTips);
            view.m_btn_info_GameButton.onClick.AddListener(OnShowTips);
            
            UpDateRedDot();
            view.m_UI_Model_Window_Type1.ActivePage((int)m_curPageType);
        }

        protected override void BindUIData()
        {
            
        }
        #endregion

        private void UpdateMaterialInfo()
        {
            var curMaterialItems = m_bagProxy.GetMaterialItems();
            ClearItemInfo();
            var groupID = 0;
            ItemInfoEntity item = null;
            Dictionary<int, List<MaterialItem>> itemInfos = null;
            foreach (var equipMaterialItem in curMaterialItems)
            {
                if (equipMaterialItem.Value.MaterialDefine == null)
                {
                    continue;
                }
                groupID = equipMaterialItem.Value.MaterialDefine.@group;
                if (equipMaterialItem.Value.MaterialDefine.order < (int) MaterialType.Drawing)
                {
                    itemInfos = m_materialItemInfos;
                }
                else
                {
                    itemInfos = m_drawingItemInfos;
                }

                if (!itemInfos.ContainsKey(groupID))
                {
                    itemInfos.Add(groupID,
                        new List<MaterialItem>());
                }

                
                //排序
                var index = itemInfos[groupID]
                    .FindIndex(x => x.MaterialDefine.order > equipMaterialItem.Value.MaterialDefine.order);
                if (index >= 0)
                {
                    itemInfos[groupID].Insert(index, equipMaterialItem.Value);
                }
                else
                {
                    itemInfos[groupID].Add(equipMaterialItem.Value);
                }
            }
            OrderItemInfos(ref m_materialItemInfos);
            OrderItemInfos(ref m_drawingItemInfos);
        }

        private void OrderItemInfos(ref Dictionary<int, List<MaterialItem>> itemInfos)
        {
            List<KeyValuePair<int, List<MaterialItem>>> itemInfoList = new List<KeyValuePair<int,List<MaterialItem>>>(itemInfos);
            itemInfoList.Sort((i1, i2) => { return i1.Key.CompareTo(i2.Key); });
            itemInfos.Clear();
            foreach (var info in itemInfoList)
            {
                itemInfos.Add(info.Key,info.Value);
            }
        }

        private void ClearItemInfo()
        {
            foreach (var materialItemInfo in m_materialItemInfos)
            {
                materialItemInfo.Value.Clear();
            }
            m_materialItemInfos.Clear();
            foreach (var drawingItemInfo in m_drawingItemInfos)
            {
                drawingItemInfo.Value.Clear();
            }
            m_drawingItemInfos.Clear();

        }
        
        #region Production 生产
        private void InitProduction()
        {
            m_productionGo[0] = view.m_UI_BlacksmithQueue1;
            m_productionGo[1] = view.m_UI_BlacksmithQueue2;
            m_productionGo[2] = view.m_UI_BlacksmithQueue3;
            m_productionGo[3] = view.m_UI_BlacksmithQueue4;
            m_productionGo[4] = view.m_UI_BlacksmithQueue5;
            
            foreach (var queueSubView in m_productionGo)
            {
                queueSubView.AddDelBtnListener(OnDelProduce);
            }

            m_productionItemInfos =CoreUtils.dataService.QueryRecord<Data.ConfigDefine>(0).equipMaterialMake;
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_produc_ListView.ItemPrefabDataList, LoadProductionItemFinish);
        }
        

        private void UpdateProduceTime(long costTime, long finishTime)
        {
            long leftTime;
            long currentTime = ServerTimeModule.Instance.GetServerTime();
            leftTime = finishTime - currentTime;
            if (leftTime < 0)
            {
                ClearProduceTimer();
                return;
            }
            view.m_pb_bar_GameSlider.value = (float) (costTime - leftTime) / costTime;
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int) leftTime);
        }

        private void ClearProduceTimer()
        {
            if (m_produceTimer != null)
            {
                m_produceTimer.Cancel();
                m_produceTimer = null;
            }
        }

        private void RefreshProductionList()
        {
            if (m_productionAssetDic.Count <= 0)
            {
                return;
            }
            
            var num =(m_productionItemInfos.Count+LISTITEMNUMPERROW-1)/LISTITEMNUMPERROW;
            view.m_sv_produc_ListView.FillContent(num);
        }

        private void LoadProductionItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_productionAssetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitProductionItem;
            view.m_sv_produc_ListView.SetInitData(m_productionAssetDic, functab);
            if (m_curPageType == MaterialPageType.Production)
            {
                RefreshProductionList();
            }
        }

        private void InitProductionItem(ListView.ListItem item)
        {
            UI_Item_MaterialElement_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_MaterialElement_SubView;
            }
            else
            {
                itemView = new UI_Item_MaterialElement_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            var beginIndex = item.index * LISTITEMNUMPERROW;
            var infos = new List<MaterialItem>();
            for (int i = beginIndex;i < m_productionItemInfos.Count && i<beginIndex+LISTITEMNUMPERROW; i++)
            {
                int curNum = (int)m_bagProxy.GetItemNum(m_productionItemInfos[i]);
                infos.Add(new MaterialItem(m_productionItemInfos[i],curNum));
            }
            itemView.SetInfo(item.index,infos,OnProduce,MaterialPageType.Production);
        }
        
        //当前生产队列信息展示
        private void RefreshProducingItemInfo()
        {
            ClearProduceTimer();
            var productionQueueInfo = m_playerProxy.GetCurProduceItems();
            view.m_lbl_product_LanguageText.text = LanguageUtils.getText(182004);
            for (int i = 0; i < m_productionGo.Length; i++)
            {
                if (productionQueueInfo.produceItems==null || i >= productionQueueInfo.produceItems.Count)
                {
                    m_productionGo[i].SetInfo(i, 0, 0);
                }
                else
                {
                    if (i == 0)
                    {
                        var itemCfg =
                            CoreUtils.dataService.QueryRecord<ItemDefine>((int)productionQueueInfo.produceItems[i].itemId);
                        view.m_lbl_product_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);
                    }
                    m_productionGo[i].SetInfo(i, (int) productionQueueInfo.produceItems[i].itemId, (int) productionQueueInfo.produceItems[i].itemNum);
                }
            }
            
            if (productionQueueInfo.produceItems ==null || productionQueueInfo.produceItems.Count<=0)
            {
                view.m_pb_bar_ArabLayoutCompment.gameObject.SetActive(false);
                return;
            }
            view.m_pb_bar_ArabLayoutCompment.gameObject.SetActive(true);
            long costTime = productionQueueInfo.finishTime - productionQueueInfo.beginTime;
            long finishTime = productionQueueInfo.finishTime;
            long curTime = ServerTimeModule.Instance.GetServerTime();
            var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            
            //防止客户端与服务端时间误差导致时间显示过长
            if (finishTime - curTime > config.equipMaterialMakeTime)
            {
                finishTime = curTime +config.equipMaterialMakeTime;
            }

            if (costTime <= 0 || finishTime < curTime)
            {
                view.m_pb_bar_GameSlider.value = 0;
                view.m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown(0) ;
                return;
            }
            UpdateProduceTime(costTime, finishTime);
            m_produceTimer = Timer.Register(1, ()=>{ UpdateProduceTime(costTime,finishTime);},null,true);
        }
        
        private void OnProduce(int index, MaterialItem info)
        {
            if (m_isSendingMsg)
            {
                return;
            }
            if (!m_playerProxy.IsMaterialProduceQueueFull())
            {
                SetSendingMsgStatus(true);
                m_bagProxy.ProduceItem(info.ItemID);
            }
            else
            {
                CoreUtils.logService.Warn("======材料生产队列已满");
            }
        }
        
        private void OnDelProduce(int index)
        {
            if (m_isSendingMsg)
            {
                return;
            }
            if (index == 0)
            {
                Alert.CreateAlert(182088).SetLeftButton().SetRightButton(() => { m_bagProxy.CancelProduceItem(index + 1); })
                    .Show();
                return;
            }
            SetSendingMsgStatus(true);
            m_bagProxy.CancelProduceItem(index+1);

        }
        #endregion

        #region Mix 合成

        private void InitMix()
        {
            m_curMixPage = MaterialType.Material;
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_mix_ListView.ItemPrefabDataList, LoadMixItemFinish);
        }
        
        private void RefreshMix()
        {
            if (m_curSelectMaterialItem==null || m_bagProxy.GetItemNum(m_curSelectMaterialItem.ItemID) <= 0)
            {
                SelectFirstItem(GetItemInfosByMaterialType(m_curMixPage), MaterialPageType.Mix);
            }
            RefreshMixItemInfos();
            SetMixItem(m_curSelectMaterialItem);
        }

        private void LoadMixItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_mixAssetDic[data.Key] = data.Value.asset() as GameObject;
            }
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitMixItemInfo;
            functab.GetItemPrefabName = GetMixItemPrefabName;
            functab.GetItemSize = GetItemSize;
            view.m_sv_mix_ListView.SetInitData(m_mixAssetDic, functab);
            if (m_curPageType == MaterialPageType.Mix)
            {
                RefreshMix();
            }
        }
        private void RefreshMixItemInfos()
        {
            if (m_mixAssetDic.Count <= 0)
            {
                return;
            }
            var itemInfos = GetItemInfosByMaterialType(m_curMixPage);

            var itemNum = GetItemCount(itemInfos, LISTITEMNUMPERROW);
            view.m_sv_mix_ListView.FillContent(itemNum);

            if (m_jumpMixIndex >= 0)
            {
                view.m_sv_mix_ListView.MovePanelToItemIndex(m_jumpMixIndex);
                m_jumpMixIndex = -1;
            }
        }

        private void InitMixItemInfo(ListView.ListItem item)
        {
            var itemInfos = GetItemInfosByMaterialType(m_curMixPage);
            InitItemInfo(item,itemInfos,OnClickMixItem,MaterialPageType.Mix);

        }
        
        private float GetItemSize(ListView.ListItem item)
        {
            var index = m_showTypeItemPrefabName.FindIndex(x=>x.Equals(item.prefabName));

            if (index == 0)
            {
                return 50;
            }else if (index == 1)
            {
                return 150;
            }

            return 150;
        }

        private string GetMixItemPrefabName(ListView.ListItem item)
        {
            var itemInfos = GetItemInfosByMaterialType(m_curMixPage);
            var name = GetItemPrefabName(item.index, itemInfos);
            if (name.Equals(""))
            {
                CoreUtils.logService.Warn($"铁匠铺材料 合成 === 数组越界   index:[{item.index}] ");
            }
            return name;
        }

        private Dictionary<int, List<MaterialItem>> GetItemInfosByMaterialType(MaterialType type)
        {
            var itemInfos = m_materialItemInfos;
            if (type == MaterialType.Drawing)
            {
                itemInfos = m_drawingItemInfos;
            }
            return itemInfos;
        }

        private void OnClickMixItem(int index, MaterialItem info)
        {
            SetMixItem(info);
            if (m_curSelectIndex != -1)
            {
                view.m_sv_mix_ListView.RefreshItem(m_curSelectIndex);
            }
            view.m_sv_mix_ListView.RefreshItem(index);
            m_curSelectIndex = index;
        }
        
        //合成展示页面
        private bool SetMixItem( MaterialItem info)
        {
            ClearMixItems();
            view.m_lbl_product_LanguageText.text = "";
            if (info == null)
            {
                view.m_btn_mix.SetGrayAndDisable(true);
                view.m_pl_mix2_effect1_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_img_mix.gameObject.SetActive(false);
                view.m_img_mixItem.gameObject.SetActive(false);
                view.m_lbl_mix_quality_LanguageText.gameObject.SetActive(false);
                view.m_pl_mix_effect.gameObject.SetActive(false);
                return false;
            }
            var equipMaterial = info.MaterialDefine;
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(info.ItemID);
            m_curSelectMaterialItem = info;
            //最高品质处理
            if (equipMaterial.mix == 0)
            {
                view.m_lbl_product_LanguageText.text =  LanguageUtils.getText(730054);
                view.m_btn_mix.SetGrayAndDisable(true);
                view.m_pl_mix2_effect1_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_img_mix.gameObject.SetActive(false);
                view.m_lbl_mix_quality_LanguageText.gameObject.SetActive(true);
                view.m_pl_mix_effect.gameObject.SetActive(false);
                ShowQuickOperation(false);
                SetRequireMixItems(itemCfg, 1,info.ItemNum.ToString());
                return false;
            }
            view.m_pl_mix_effect.gameObject.SetActive(true);
            view.m_img_mix.gameObject.SetActive(true);
            view.m_lbl_mix_quality_LanguageText.gameObject.SetActive(false);
            RecycleChildItemEffect(view.m_pl_mix_effect);
            
            //合成道具信息
            var mixItemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(equipMaterial.mix);
            view.m_img_mix.Refresh(mixItemCfg,0);
            view.m_lbl_product_LanguageText.text = LanguageUtils.getText(mixItemCfg.l_nameID);
            ShowItemEffect(mixItemCfg,view.m_pl_mix_effect);
            
            //合成所需道具展示
            string numStr = "";
            if (equipMaterial.mixCostNum > info.ItemNum)
            {
                numStr = LanguageUtils.getTextFormat(182025, info.ItemNum, equipMaterial.mixCostNum);
                view.m_btn_mix.SetGrayAndDisable(true);
                view.m_pl_mix2_effect1_ArabLayoutCompment.gameObject.SetActive(false);
            }
            else
            {
                numStr = LanguageUtils.getTextFormat(145048, equipMaterial.mixCostNum);
                view.m_btn_mix.SetGrayAndDisable(false);
                view.m_pl_mix2_effect1_ArabLayoutCompment.gameObject.SetActive(true);
                var count = equipMaterial.mixCostNum > 10 ? 10 : equipMaterial.mixCostNum;
                SetMix2Effect1Size(count, view.m_pl_mix_GridLayoutGroup);
            }

            SetRequireMixItems(itemCfg, equipMaterial.mixCostNum, numStr);
            SetQuickOperationMix(info);
            return true;
        }
        private void SetMix2Effect1Size(int mixCostNumm, GridLayoutGroup gridLayoutGroup,int spacingX = 42,int spacingY = 32)
        {
            float width = 0; float height = 0;
            if (mixCostNumm > 0)
            {
                width = (mixCostNumm * gridLayoutGroup.cellSize.x) + ((mixCostNumm - 1) * gridLayoutGroup.spacing.x) + gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
                height = gridLayoutGroup.cellSize.y;
                view.m_pl_mix2_effect1_ArabLayoutCompment.transform.localScale = new Vector3(1, 1, 1);
                PolygonImage image = view.m_pl_mix2_effect1_ArabLayoutCompment.transform.GetComponentInChildren<PolygonImage>();
                if (image != null)
                {
                    image.transform.localScale = gridLayoutGroup.transform.localScale;
                    image.transform.localPosition = Vector3.zero;
                    image.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width + spacingX * 2, height + spacingY * 2);
                }
            }
        }

        private void ClearMixItems()
        {
            foreach (var mixItem in m_mixItems)
            {
                mixItem.gameObject.SetActive(false);
            }
        }

        private void SetRequireMixItems(ItemDefine itemCfg, int mixCostNum,string numStr)
        {
            var count = mixCostNum > 10 ? 10 : mixCostNum;
            for (int i = 0; i < count; i++)
            {
                if (m_mixItems.Count <= i)
                {
                    var go = GameObject.Instantiate(view.m_img_mixItem.gameObject,
                        view.m_pl_mix_GridLayoutGroup.transform);
                    m_mixItems.Add(new UI_Model_Item_SubView(go.GetComponent<RectTransform>()));
                }

                m_mixItems[i].gameObject.SetActive(true);
                if (i == count - 1)
                {
                    m_mixItems[i].Refresh(itemCfg, numStr);
                }
                else
                {
                    m_mixItems[i].Refresh(itemCfg, 0);
                }
            }
        }
        
        //设置快捷合成按钮
        private void SetQuickOperationMix(MaterialItem info)
        {
            ShowQuickOperation(false);
            if (info == null)
            {
                return;
            }

            if (m_curQuickOperationItemID != info.ItemID)
            {
                SetQuickOperationItemID(0);
                return;
            }
            long max = info.ItemNum / info.MaterialDefine.mixCostNum;
            long min = max;
            if (max > 1 && max > MINQUICKOPERATIONNUM)
            {
                min = MINQUICKOPERATIONNUM;
            }
            SetQuickOperation(min,max,MaterialMix);
        }
        
        private void MaterialMix(long num ,bool isMax)
        {
            if (m_isSendingMsg)
            {
                return;
            }
            if (m_curSelectMaterialItem != null &&
                m_curSelectMaterialItem.MaterialDefine.mixCostNum <= m_curSelectMaterialItem.ItemNum)
            {
                SetQuickOperationItemID( m_curSelectMaterialItem.ItemID);
                m_bagProxy.MixMaterial(m_curSelectMaterialItem.ItemID, num, isMax);
                SetSendingMsgStatus(true);
                GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                ClientUtils.UIAddEffect("UI_10072",view.m_pl_mix1_effect1,null,true);
                mt.FlyItemEffect(m_curSelectMaterialItem.ItemID,0,view.m_img_mix.m_img_icon_PolygonImage.rectTransform);
            }
            else
            {
                CoreUtils.logService.Warn("====材料不足");
            }
        }

        private void OnClickMix()
        {
            MaterialMix(1,false);
        }

        private void ChangeMixPage(MaterialType type)
        {
            if (m_curMixPage != type)
            {
                view.m_sv_mix_ListView.MovePanelToItemIndex(0);
                SetQuickOperationItemID(0);
                m_curMixPage = type;
                m_curSelectMaterialItem = null;
                RefreshMix();
                ClearEffect();
            }
        }

        private void OnClickMaterialPage(bool value)
        {
            if (value)
            {
                ChangeMixPage(MaterialType.Material);
            }
        }

        private void OnClickDrawingPage(bool value)
        {
            if (value)
            {
                ChangeMixPage(MaterialType.Drawing);
            }
        }


        #endregion

        #region Resolve 分解
        private void InitResolve()
        {
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_resolve_ListView.ItemPrefabDataList, LoadResolveItemFinish);

        }

        private void RefreshResolve()
        {
            if (m_curSelectMaterialItem==null || m_bagProxy.GetItemNum(m_curSelectMaterialItem.ItemID) <= 0)
            {
                SelectFirstItem(GetItemInfosByMaterialType(m_curMixPage), MaterialPageType.Resolve);
            }
            RefreshResolveItemInfos();
            SetResolveItem(m_curSelectMaterialItem);
        }
        
        private void RefreshResolveItemInfos()
        {
            if (m_resolveDic.Count <= 0)
            {
                return;
            }
            var itemNum = GetItemCount(m_materialItemInfos, LISTITEMNUMPERROW);
            view.m_sv_resolve_ListView.FillContent(itemNum);
        }

        private void LoadResolveItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_resolveDic[data.Key] = data.Value.asset() as GameObject;
            }
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitResolveItemInfo;
            functab.GetItemPrefabName = GetResolvePrefabName;
            functab.GetItemSize = GetItemSize;
            view.m_sv_resolve_ListView.SetInitData(m_resolveDic, functab);
            if (m_curPageType == MaterialPageType.Resolve)
            {
                RefreshResolve();
            }
        }

        private void InitResolveItemInfo(ListView.ListItem item)
        {
            InitItemInfo(item, m_materialItemInfos, OnClickResolveItem, MaterialPageType.Resolve);
        }
        
        private string GetResolvePrefabName(ListView.ListItem item)
        {
            var name = GetItemPrefabName(item.index, m_materialItemInfos);
            if (name.Equals(""))
            {
                CoreUtils.logService.Warn($"铁匠铺材料 分解 === 数组越界   index:[{item.index}] ");
            }
            return name;
        }

        private void OnClickResolveItem(int index, MaterialItem materialItem)
        {
            if (SetResolveItem(materialItem) == false)
            {
                return;
            }
            
            if (m_curSelectIndex != -1)
            {
                view.m_sv_resolve_ListView.RefreshItem(m_curSelectIndex);
            }
            view.m_sv_resolve_ListView.RefreshItem(index);
            m_curSelectIndex = index;
        }
        
        //分解信息展示
        private bool SetResolveItem(MaterialItem materialItemInfo)
        {
            m_curSelectMaterialItem = materialItemInfo;
            view.m_lbl_resolve_quality_LanguageText.gameObject.SetActive(false);
            view.m_lbl_product_LanguageText.text = "";
            if (materialItemInfo == null)
            {
                view.m_img_resolve.gameObject.SetActive(false);
                view.m_btn_resolve.SetInteractable(false);
                view.m_btn_resolve.ShowLineTwo(false);
                view.m_pl_break_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_btn_resolve.SetNum("0");
                view.m_pl_mix_resolve_GridLayoutGroup.gameObject.SetActive(false);
                return false;
            }
            
            var equipMaterial =materialItemInfo.MaterialDefine;
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(materialItemInfo.ItemID);
            
            view.m_img_resolve.gameObject.SetActive(true);
            view.m_img_resolve.Refresh(itemCfg,0);
            
            //最低品质
            if (equipMaterial.split == 0)
            {
                view.m_lbl_product_LanguageText.text =  LanguageUtils.getText(730054);
                view.m_btn_resolve.SetInteractable(false);
                view.m_btn_resolve.ShowLineTwo(false);
                view.m_lbl_resolve_quality_LanguageText.gameObject.SetActive(true);
                view.m_pl_break_ArabLayoutCompment.gameObject.SetActive(false);
                ShowQuickOperation(false);
                view.m_pl_mix_resolve_GridLayoutGroup.gameObject.SetActive(false);
                return true;
            }
            foreach (var resolveItem in m_resolveItems)
            {
                resolveItem.gameObject.SetActive(false);
            }
            
            view.m_btn_resolve.SetInteractable(true);

            view.m_pl_mix_resolve_GridLayoutGroup.gameObject.SetActive(true);
            view.m_pl_break_ArabLayoutCompment.gameObject.SetActive(true);
            view.m_btn_resolve.ShowLineTwo(true);
            view.m_btn_resolve.SetIcon(m_currencyProxy.GeticonIdByType(equipMaterial.splitCostCur));
            view.m_btn_resolve.SetNum(ClientUtils.FormatComma(equipMaterial.splitCostCurNum));
            RecycleChildItemEffect(view.m_pl_mix_resolve_GridLayoutGroup.transform);
            
            var splitItemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(equipMaterial.split);
            view.m_lbl_product_LanguageText.text = LanguageUtils.getText(splitItemCfg.l_nameID);

            for (int i = 0; i < equipMaterial.splitGetNum; i++)
            {
                if (m_resolveItems.Count <= i)
                {
                    var go = GameObject.Instantiate(view.m_img_resolveItem.gameObject,view.m_pl_break_ArabLayoutCompment.transform);
                    m_resolveItems.Add(new UI_Model_Item_SubView(go.GetComponent<RectTransform>()));
                }
                m_resolveItems[i].gameObject.SetActive(true);
                m_resolveItems[i].Refresh(splitItemCfg,0);
                ShowItemEffect(splitItemCfg,view.m_pl_mix_resolve_GridLayoutGroup.transform);
            }
            SetQuickOperationResolve(materialItemInfo);
            return true;
        }
        
        private void SetQuickOperationResolve(MaterialItem info)
        {
            ShowQuickOperation(false);

            if (info == null)
            {
                return;
            }
            if (m_curQuickOperationItemID != info.ItemID)
            {
                SetQuickOperationItemID(0);
                return;
            }
            long max = GetCanResolveNum(info);
            
            long min = max;
            if (max > 1 && max > MINQUICKOPERATIONNUM)
            {
                min = MINQUICKOPERATIONNUM;
            }
            SetQuickOperation(min,max,MaterialResolve);
        }

        private long GetCanResolveNum(MaterialItem info)
        {
            if (info.MaterialDefine.split == 0)
            {
                return 0;
            }
            long num = m_playerProxy.GetResNumByType(info.MaterialDefine.splitCostCur) / info.MaterialDefine.splitCostCurNum;
            if (num > info.ItemNum)
            {
                num = info.ItemNum;
            }

            return num;
        }

        private void MaterialResolve(long num, bool isMax)
        {
            if (m_isSendingMsg || m_curSelectMaterialItem == null)
            {
                return;
            }
            if (m_playerProxy.GetResNumByType(m_curSelectMaterialItem.MaterialDefine.splitCostCur) <
                m_curSelectMaterialItem.MaterialDefine.splitCostCurNum)
            {
                m_currencyProxy.LackOfResourcesByType((EnumCurrencyType)m_curSelectMaterialItem.MaterialDefine.splitCostCur,
                    m_curSelectMaterialItem.MaterialDefine.splitCostCurNum);
                return;
            }
            SetQuickOperationItemID( m_curSelectMaterialItem.ItemID);
            m_bagProxy.ResolveMaterial(m_curSelectMaterialItem.ItemID,num,isMax);
            GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
            
            int count = m_resolveItems.Count;
            int i = 0;
            Timer timer = null;
            timer =  Timer.Register(0.25f, () =>
            {
                if (i < count)
                {
                    int j = i;
                    mt.FlypResolveEffect(view.m_img_resolve.m_root_RectTransform, m_resolveItems[j].m_root_RectTransform,()=> {
                    });
                    i++;
                }
                else
                {
                    if (timer != null)
                    {
                        timer.Cancel();
                        timer = null;
                    }
                }

            }, null, true, true, view.vb);

            SetSendingMsgStatus(true);
        }

        private void OnClickResolve()
        {
            MaterialResolve(1,false);
        }

        #endregion

        private void InitItemInfo(ListView.ListItem item,Dictionary<int,List<MaterialItem>> itemInfos,UnityAction<int, MaterialItem> callback,MaterialPageType type)
        {
            var index = 0;
            
            foreach (var itemInfo in itemInfos)
            {
                index += 1;
                if (index > item.index)
                {
                    UI_Item_MaterialTitle_SubView itemView = null;
                    if (item.data != null)
                    {
                        itemView = item.data as UI_Item_MaterialTitle_SubView;
                    }
                    else
                    {
                        itemView = new UI_Item_MaterialTitle_SubView(item.go.GetComponent<RectTransform>());
                        item.data = itemView;
                    }

                    var materialGroup = CoreUtils.dataService.QueryRecords<EquipMaterialGroupDefine>();
                    var cfg = materialGroup.Find(x => x.@group == itemInfo.Key);
                    itemView.SetTitle(LanguageUtils.getText(cfg.l_nameID));
                    return;
                }
                for (int i = 0; i < itemInfo.Value.Count; i += LISTITEMNUMPERROW)
                {
                    index += 1;
                    if (index > item.index)
                    {
                        var infos = new List<MaterialItem>();
                        var curSelectItemID = 0;
                        if (m_curSelectMaterialItem != null)
                        {
                            curSelectItemID = m_curSelectMaterialItem.ItemID;
                        }
                        for (int j=i; j < itemInfo.Value.Count && j<i+LISTITEMNUMPERROW; j++)
                        {
                            infos.Add(itemInfo.Value[j]);
                            if (itemInfo.Value[j].ItemID == curSelectItemID)
                            {
                                m_curSelectIndex = item.index;
                            }
                        }
                        UI_Item_MaterialElement_SubView itemView = null;
                        if (item.data != null)
                        {
                            itemView = item.data as UI_Item_MaterialElement_SubView;
                        }
                        else
                        {
                            itemView = new UI_Item_MaterialElement_SubView(item.go.GetComponent<RectTransform>());
                            item.data = itemView;
                        }
                        
                        itemView.SetInfo(item.index,infos,callback,type,curSelectItemID);
                        return;
                    }
                }
            }
        }
        
        
        private string GetItemPrefabName(int itemIndex, Dictionary<int, List<MaterialItem>> itemInfos)
        {
            var index = 0;
            foreach (var itemInfo in itemInfos)
            {
                index += 1;
                if (index > itemIndex)
                {
                    return m_showTypeItemPrefabName[0];
                }

                index += (itemInfo.Value.Count + LISTITEMNUMPERROW-1) / LISTITEMNUMPERROW;
                if (index > itemIndex)
                {
                    return m_showTypeItemPrefabName[1];
                }
            }
            return "";
        }

        //查找第一个合成或分解的材料
        private void SelectFirstItem(Dictionary<int, List<MaterialItem>> itemInfos,MaterialPageType type)
        {
            foreach (var itemInfo in itemInfos)
            {
                foreach (var info in itemInfo.Value)
                {
                    if (CheckItemCanSelect(info,type))
                    {
                        m_curSelectMaterialItem = info;
                        return;
                    }
                }
            }
            m_curSelectMaterialItem = null;
        }
        private void ClearEffect()
        {
            view.m_pl_mix1_effect1.DestroyAllChild();
        }

        //查找指定材料
        private int SelectJumpItem(Dictionary<int, List<MaterialItem>> itemInfos, MaterialPageType type, int itemId)
        {
            m_curSelectMaterialItem = null;
            int num = 0;
            foreach (var itemInfo in itemInfos)
            {
                num = num + LISTITEMNUMPERROW;
                foreach (var info in itemInfo.Value)
                {
                    num = num + 1;
                    if (info.MaterialDefine != null && info.MaterialDefine.itemID == itemId)
                    {
                        m_curSelectMaterialItem = info;
                        break;
                    }
                }
                if (m_curSelectMaterialItem != null)
                {
                    break;
                }
                else
                {
                    int num1 = num % LISTITEMNUMPERROW;
                    num = num + LISTITEMNUMPERROW - num1;
                }
            }
            int index = (int)Math.Ceiling((float)num/LISTITEMNUMPERROW);
            index = index - 1;
            return index;
        }

        //判断当前材料是否可合成或分解
        private bool CheckItemCanSelect(MaterialItem info,MaterialPageType type)
        {
            if (info == null)
            {
                return false;
            }
            var equipMaterial =info.MaterialDefine;
            switch (type)
            {
                case MaterialPageType.Mix:
                    if (equipMaterial.mix == 0)
                    {
                        return false;
                    }
                    break;
                case MaterialPageType.Resolve:
                    if (equipMaterial.split == 0)
                    {
                        return false;
                    }
                    break;
            }
            
            return true;
        }
        
        private void ChangePage(MaterialPageType type)
        {
            bool isMix = type == MaterialPageType.Mix;
            bool isProduction = type ==  MaterialPageType.Production;
            bool isResolve = type == MaterialPageType.Resolve;
            int title = 182001;
            m_curPageType = type;
            view.m_pl_mix1.gameObject.SetActive(isMix);
            view.m_pl_mix_ArabLayoutCompment.gameObject.SetActive(isMix);
            view.m_pl_produc1.gameObject.SetActive(isProduction);
            view.m_pl_produc_ArabLayoutCompment.gameObject.SetActive(isProduction);
            view.m_pl_resolve1.gameObject.SetActive(isResolve);
            view.m_pl_resolve_ArabLayoutCompment.gameObject.SetActive(isResolve);
            OnCloseTips();
            ClearEffect();
            ShowQuickOperation(false);
            SetQuickOperationItemID(0);
            if (isMix)
            {
                title = 182002;
                if (m_isJumpHandle)
                {
                    m_isJumpHandle = false;
                    ChangeMixPage(m_jumpMixType);
                    if (m_jumpMixType == MaterialType.Material)
                    {
                        view.m_ck_material_GameToggle.SetIsOnWithoutNotify(true);
                    }
                    else
                    {
                        view.m_ck_picture_GameToggle.SetIsOnWithoutNotify(true);
                    }
                    m_jumpMixIndex = SelectJumpItem(GetItemInfosByMaterialType(m_curMixPage), type, m_jumpMixItemId);
                    RefreshMix();
                    view.m_sv_mix_ListView.MovePanelToItemIndex(m_jumpMixIndex);
                }
                else
                {
                    ChangeMixPage(MaterialType.Material);
                    view.m_ck_material_GameToggle.SetIsOnWithoutNotify(true);
                    SelectFirstItem(GetItemInfosByMaterialType(m_curMixPage), type);
                    view.m_sv_mix_ListView.MovePanelToItemIndex(0);
                    RefreshMix();
                }

            }else if(isResolve)
            {
                title = 182003;
                SelectFirstItem(m_materialItemInfos,type);
                view.m_sv_resolve_ListView.MovePanelToItemIndex(0);
                RefreshResolve();
            }
            else
            {
                view.m_sv_produc_ListView.MovePanelToItemIndex(0);
                RefreshProducingItemInfo();
                RefreshProductionList();
            }
            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(title));

        }
        
        private int GetItemCount(Dictionary<int, List<MaterialItem>> itemInfos, int itemLength)
        {
            var itemNum = 0;
            foreach (var itemInfo in itemInfos)
            {
                itemNum += (itemInfo.Value.Count + itemLength-1) / itemLength + 1;
            }

            return itemNum;
        }

        private void OnShowProductionPage(bool value)
        {
            if (value)
            {
                ChangePage(MaterialPageType.Production);
            }
        }

        private void OnShowMixPage(bool value)
        {
            if (value)
            {
                ChangePage(MaterialPageType.Mix);
            }
        }

        private void OnShowResolvePage(bool value)
        {
            if (value)
            {
                ChangePage(MaterialPageType.Resolve);
            }
        }

        private void OnClickClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_Material);
        }

        private void OnShowTips()
        {
            view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_pl_tips_ArabLayoutCompment.gameObject.SetActive(true);
            switch (m_curPageType)
            {
                case MaterialPageType.Mix:
                    view.m_lbl_tips_LanguageText.text = LanguageUtils.getText(182006);
                    break;
                case MaterialPageType.Production:
                    view.m_lbl_tips_LanguageText.text = LanguageUtils.getText(182005);
                    break;
                case MaterialPageType.Resolve:
                    view.m_lbl_tips_LanguageText.text = LanguageUtils.getText(182007);
                    break;
            }
        }

        private void OnCloseTips()
        {
            view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(true);
            view.m_pl_tips_ArabLayoutCompment.gameObject.SetActive(false);
        }

        private void UpDateRedDot()
        {
            long mixNum = m_bagProxy.GetCanMixDrawingMaterialNum();
            view.m_UI_Model_Window_Type1.ShowRedDotInPageSide((int) MaterialPageType.Mix, (int)mixNum );
            view.m_UI_Common_Redpoint.ShowRedPoint((int)mixNum);
        }

        private void SetQuickOperation(long min, long max, UnityAction<long, bool> operation)
        {
            if (max <=1)
            {
                ShowQuickOperation(false);
                return;
            }
            else if(max== min)
            {
                view.m_pl_quick1.gameObject.SetActive(true);
                view.m_pl_quick2.gameObject.SetActive(false);
                view.m_btn_quick1.SetText(LanguageUtils.getTextFormat(145048,min));
                view.m_btn_quick1.RemoveAllClickEvent();
                view.m_btn_quick1.AddClickEvent(() =>
                {
                    operation?.Invoke(min,false);
                    ShowQuickOperation(false);
                });
            }
            else
            {
                view.m_pl_quick2.gameObject.SetActive(true);
                view.m_pl_quick1.gameObject.SetActive(false);
                view.m_btn_quick2.SetText(LanguageUtils.getTextFormat(200024));
                view.m_btn_quick2.RemoveAllClickEvent();
                view.m_btn_quick2.AddClickEvent(() =>
                {
                    operation?.Invoke(max,true);
                    ShowQuickOperation(false);
                });
                view.m_btn_quick3.SetText(LanguageUtils.getTextFormat(145048,min));
                view.m_btn_quick3.RemoveAllClickEvent();
                view.m_btn_quick3.AddClickEvent(() =>
                {
                    operation?.Invoke(min,false);
                    ShowQuickOperation(false);
                });
            }
            ShowQuickOperation(true);

        }

        private void ShowQuickOperation(bool isShow)
        {
            view.m_pl_quick_ArabLayoutCompment.gameObject.SetActive(isShow);

        }

        private void SetSendingMsgStatus(bool bValue)
        {
            m_isSendingMsg = bValue;
        }

        private void SetQuickOperationItemID(int itemID)
        {
            m_curQuickOperationItemID = itemID;
        }
        //显示物品品质特效
        private void ShowItemEffect(ItemDefine item, Transform parent)
        {
            var quality = item.quality;
  
            string effectString = RS.ItemEffectName[quality - 1];
            m_lastParentTrans = parent;
            m_lastQuality = quality;

            m_lastEffectName = effectString;

            if (m_loadedItemEffect.ContainsKey(effectString))
            {
                var obj = m_loadedItemEffect[effectString];
                obj.SetActive(true);
                InitItemEffect(obj,parent,quality);
                return;
            }

            //判断是否正在加载中
            if (!m_effectLoadStatus.ContainsKey(effectString))
            {
                m_effectLoadStatus[effectString] = 1;
            }
            else
            {
                if (m_effectLoadStatus[effectString] == 1)
                {
                    return;
                }
            }

            CoreUtils.assetService.Instantiate(effectString, (asset) =>
            {
                if (view.gameObject == null)
                {
                    return;
                }
                m_loadedItemEffect[effectString] = asset;
                m_effectLoadStatus[effectString] = 2;
                if (effectString != m_lastEffectName)
                {
                    asset.SetActive(false);
                    return;
                }
                var obj = asset;
                InitItemEffect(obj,parent,quality);
            });
        }

        private void InitItemEffect(GameObject obj,Transform parent,int quality)
        {
            obj.transform.SetParent(parent);
            obj.transform.localPosition= Vector3.zero;
            obj.transform.localScale = Vector3.one*2;
        }
        
        //回收子物体品质特效
        private void RecycleChildItemEffect(Transform trans)
        {
            for (int i = 0; i < trans.childCount; i++)
            {
                var obj = trans.GetChild(i).gameObject;
                obj.SetActive(false);
            }
        }
    }
}