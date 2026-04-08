// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_Win_EquipQuickMediator
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
using UnityEngine.UI;

namespace Game {
    public class EquipQuickItemInfo
    {
        public int ItemID { get; set; }
        public long ItemNum { get; set; }
        public Dictionary<int, long> MaterialList;

        public EquipQuickItemInfo(int _itemId, long _itemNum)
        {
            ItemID = _itemId;
            ItemNum = _itemNum;
            SetMaterialList();
        }

        private void SetMaterialList()
        {
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            MaterialList =new Dictionary<int, long>();
            bagProxy.MakeMaterial(ref MaterialList,ItemID, ItemNum);
        }
    }
    public class UI_Win_EquipQuickMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_EquipQuickMediator";

        private BagProxy m_bagProxy;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;
        
        private Dictionary<string,GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<EquipQuickItemInfo> m_itemInfos = new List<EquipQuickItemInfo>();
        private ForgeEquipItemInfo m_quickForgeItem = null;
        private long m_lackGold = 0;
        #endregion

        //IMediatorPlug needs
        public UI_Win_EquipQuickMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_EquipQuickView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdateCurrency,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                    RefreshMaterialInfo();
                    RefreshMaterialItemList();
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
            
        }

        public override void PrewarmComplete(){
            
        }

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_quickForgeItem = view.data as ForgeEquipItemInfo;

            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, LoadItemFinish);


        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.AddCloseEvent(OnClickClose);
            view.m_btn_make.m_btn_languageButton_GameButton.onClick.AddListener(OnQuickForge);
        }

        protected override void BindUIData()
        {
            RefreshInfo();
            
            var currency = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.gold);
            ClientUtils.LoadSprite(view.m_btn_make.m_img_icon2_PolygonImage,currency.iconID);
        }
       
        #endregion

        private void LoadItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitMaterialQuickMakeItem;
            view.m_sv_list_ListView.SetInitData(m_assetDic, functab);
            RefreshMaterialItemList();
        }

        private void InitMaterialQuickMakeItem(ListView.ListItem item)
        {

            UI_Item_EquipQuick_SubView itemView = null;
            if (item.data != null)
            {
                itemView = item.data as UI_Item_EquipQuick_SubView;
            }
            else
            {
                itemView = new UI_Item_EquipQuick_SubView(item.go.GetComponent<RectTransform>());
                item.data = itemView;
            }
            
            itemView.SetMaterialInfo(m_itemInfos[item.index]);
        }

        private void RefreshMaterialItemList()
        {
            if (m_assetDic.Count <= 0)
            {
                return;
            }
            view.m_sv_list_ListView.FillContent(m_itemInfos.Count);
        }
        
        private void RefreshInfo()
        {
            RefreshMaterialInfo();
            RefreshItemInfo();
        }
        
        private void RefreshMaterialInfo()
        {
            var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(m_quickForgeItem.EquipID);

            m_itemInfos.Clear();
            for (int i = 0; i < equipCfg.makeMaterial.Count && i< equipCfg.makeMaterialNum.Count; i++)
            { 
                m_itemInfos.Add(new EquipQuickItemInfo(equipCfg.makeMaterial[i],equipCfg.makeMaterialNum[i]));
            }

            m_lackGold = equipCfg.costGold - m_currencyProxy.Gold>0? equipCfg.costGold:0;
            view.m_btn_make.SetLine2TxtAndRebuildLayout(ClientUtils.FormatComma(equipCfg.costGold));
        }
        
        private void RefreshItemInfo()
        {
            
            view.m_UI_Item_EquipAtt.SetForgeItemInfo(m_quickForgeItem);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_sv_att_ListView.listContainer);


            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(m_quickForgeItem.EquipID);
            view.m_img_equip_PolygonImage.gameObject.SetActive(true);
            ClientUtils.LoadSprite(view.m_img_equip_PolygonImage, itemCfg.itemIcon);
            
            view.m_UI_Item_ItemEffect.SetQuality(itemCfg.quality);
        }


        private void OnQuickForge()
        {
            if (m_lackGold>0)
            {
                m_currencyProxy.LackOfResources(0,0,0,m_lackGold); //普通资源
                return;
            }
            AppFacade.GetInstance().SendNotification(CmdConstant.EquipQuickForge);
            OnClickClose();
        }

        private void OnClickClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_EquipQuick);
        }
    }
}