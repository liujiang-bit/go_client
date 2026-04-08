// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_Win_EquipMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game {

    public enum EquipPage
    {
        /// <summary>
        /// 锻造
        /// </summary>
        Forge,
        /// <summary>
        /// 分解
        /// </summary>
        Decompose,
    }

    public enum EquipType
    {
        Forgeable = 997,
        Weapon =107,
        Helmet = 108,
        Breastplate = 109,
        Gloves =110,
        Pants = 111,
        Shoes = 112,
        Accessories = 113,
        Compose = 999,
        Decomposable = 998,
    }

    public enum ForgeEquipSortType
    {
        Forgeable =0,
        HighQuality,
        LowQuality,
        DrawingOrder,
    }

    public enum ResolveEquipSortType
    {
        LowQuality,
        HighQuality,
        Worn,
        Exclusive,
    }

    public class ForgeEquipItemInfo
    {
        public int EquipID;
        public int GroupID;
        public bool IsForgeable;
        public bool IsQuickForgeable;
        public long DrawingNum;
        public int Order;
        public int Compose;

        public void SetItemInfo(int itemID)
        {
            var equipDefine = CoreUtils.dataService.QueryRecord<EquipDefine>(itemID);
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;

            EquipID = itemCfg.ID;
            GroupID = equipDefine.@group;
            Compose = equipDefine.compose;
            DrawingNum = bagProxy.GetItemNum(equipDefine.makeMaterial[0]);
            Order = equipDefine.order;
            IsForgeable = true;
            IsQuickForgeable = true;

            for (int i = 0; i < equipDefine.makeMaterial.Count && i < equipDefine.makeMaterialNum.Count; i++)
            {
                if (IsForgeable && bagProxy.GetItemNum(equipDefine.makeMaterial[i]) <
                    equipDefine.makeMaterialNum[i])
                {
                    IsForgeable = false;
                }

                if (!bagProxy.MaterialCanBeMake(equipDefine.makeMaterial[i], equipDefine.makeMaterialNum[i]))
                {
                    IsQuickForgeable = false;
                    break;
                }
            }
        }
    }
    
    public class UI_Win_EquipMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_EquipMediator";

        private const int SVITEMNUMPERCOW = 2;

        private BagProxy m_bagProxy;
        private CurrencyProxy m_currencyProxy;
        private TroopProxy m_troopProxy;

        private EquipPage m_curPage;
        private ForgeEquipSortType m_curForgeSortType;
        private ResolveEquipSortType m_curResolveSortType;
        private EquipType m_curEquipType;
        
        private Dictionary<string,GameObject> m_equipAssetDic = new Dictionary<string, GameObject>();
        private Dictionary<string,GameObject> m_attrAssetDic = new Dictionary<string, GameObject>();
        private List<UI_Item_EquipMaterialList_SubView> m_materialList = new List<UI_Item_EquipMaterialList_SubView>();

        private List<ForgeEquipItemInfo> m_allForgeItemInfos = new List<ForgeEquipItemInfo>();
        private List<EquipItemInfo> m_allEquipItemInfos;
        private ForgeEquipItemInfo m_curSelectForgeItem;
        private EquipItemInfo m_curSelectResolveItem;
        private int m_curSelectItemIndex = -1;
        private long m_lackGold = -1;
        private Timer m_refreshTimer=null;
        private bool m_isSendingMsg = false;
        
        private List<ForgeEquipItemInfo> m_forgeItemInfos = new List<ForgeEquipItemInfo>();
        private List<EquipItemInfo> m_resolveItemInfos = new List<EquipItemInfo>();
        private List<string> m_showTypeItemPrefabName = new List<string>
        {
            "UI_Item_EquipLiistItem",
            "UI_Item_EquipLiistTitle",
        };
        private List<string> m_forgeToggleName = new List<string>()
        {
            LanguageUtils.getText(182049),
            LanguageUtils.getText(182050),
            LanguageUtils.getText(182051),
            LanguageUtils.getText(182052),
        };
        
        private List<string> m_resolveToggleName = new List<string>()
        {
            LanguageUtils.getText(182053),
            LanguageUtils.getText(182054),
            LanguageUtils.getText(182055),
            LanguageUtils.getText(182056),
        };

        private bool m_isJumpHandle;
        private EquipType m_jumpEquipType;
        private int m_jumpEquipId;

        private Dictionary<int, UI_Item_EquipType_SubView> m_equipTypeSubViewDic = new Dictionary<int, UI_Item_EquipType_SubView>(); 

        #endregion

        //IMediatorPlug needs
        public UI_Win_EquipMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
            IsOpenUpdate = true;
        }


        public UI_Win_EquipView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Build_CheckMakeEquip.TagName,
                Build_MakeEquipment.TagName,
                Build_DecompositionEquipment.TagName,
                CmdConstant.ItemInfoChange,
                CmdConstant.UpdateCurrency,
                CmdConstant.EquipQuickForge,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Build_CheckMakeEquip.TagName:
                    SetSendingMsgStatus(false);
                    Build_CheckMakeEquip.response res = notification.Body as Build_CheckMakeEquip.response;
                    if (res == null)
                    {
                        Tip.CreateTip(182045,Tip.TipStyle.Middle).Show();
                        return;
                    }
                    ForgeEquip(res.exclusive);
                    break;
                case Build_MakeEquipment.TagName:
                    SetSendingMsgStatus(false);
                    break;
                case Build_DecompositionEquipment.TagName:
                    SetSendingMsgStatus(false);
                    break;
                case CmdConstant.ItemInfoChange:
                    OnItemChange();
                    break;
                case CmdConstant.UpdateCurrency:
                    OnUpdateCurrency();
                    
                    break;
                case CmdConstant.EquipQuickForge:
                    CheckForgeEquip();
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
            if (Input.GetMouseButtonDown(0))
            {
                if (view.m_pl_dec_ArabLayoutCompment.gameObject.activeSelf)
                {
                    CheckClickItem(view.m_sv_des_PolygonImage.transform, OnClickDescriptionBack);
                }

                if (view.m_UI_Common_Toggle.gameObject.activeSelf)
                {
                    CheckClickItem(view.m_UI_Common_Toggle.m_root_RectTransform,OnClickArrLst);
                }
            }
        }

        private void CheckClickItem(Transform item, UnityAction callback)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, result);
            if (result.Count > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].gameObject.transform.IsChildOf(item))
                    {
                        return;
                    }
                }
                callback?.Invoke();
            }
        }

        protected override void InitData()
        {
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_view_ListView.ItemPrefabDataList, LoadEquipItemFinish);

            if (view.data != null)
            {
                if (view.data is OpenUiCommonParam)
                {
                    OpenUiCommonParam param = view.data as OpenUiCommonParam;
                    if (param.OpenUiId == 1018) // 锻造界面
                    {
                        m_curPage = EquipPage.Forge;
                        m_isJumpHandle = true;
                        m_jumpEquipId = param.IntData;
                        EquipDefine equipDefine = CoreUtils.dataService.QueryRecord<EquipDefine>(m_jumpEquipId);
                        m_jumpEquipType = (EquipType)equipDefine.group;
                    }
                    else if (param.OpenUiId == 1019) // 分解界面
                    {
                        m_curPage = EquipPage.Decompose;
                    }
                    else
                    {
                        Debug.LogErrorFormat("异常 openUiId:{0}", param.OpenUiId);
                    }

                } else
                {
                    if ((int)view.data == 2)
                    {
                        m_curPage = EquipPage.Decompose;
                    }
                }
            }
            else
            {
                m_curPage = EquipPage.Forge;
            }

            m_curForgeSortType = ForgeEquipSortType.Forgeable;
            m_curResolveSortType = ResolveEquipSortType.LowQuality;
            
            view.m_UI_Item_EquipType0.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeZero);
            view.m_UI_Item_EquipType1.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeWeapon);
            view.m_UI_Item_EquipType2.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeHelmet);
            view.m_UI_Item_EquipType3.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeBreastplate);
            view.m_UI_Item_EquipType4.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeGloves);
            view.m_UI_Item_EquipType5.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypePants);
            view.m_UI_Item_EquipType6.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeShoes);
            view.m_UI_Item_EquipType7.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeAccessories);
            view.m_UI_Item_EquipType8.m_UI_Item_EquipType_GameToggle.onValueChanged.AddListener(OnClickEquipTypeCompose);
            view.m_ck_make_GameToggle.onValueChanged.AddListener(OnClickForgePage);
            view.m_ck_resolve_GameToggle.onValueChanged.AddListener(OnClickDecomposePage);
            view.m_ck_make_GameToggle.SetIsOnWithoutNotify(true);

            view.m_btn_close.AddClickEvent(OnClickClose);

            view.m_btn_dec_GameButton.onClick.AddListener(OnClickDescription);
            view.m_btn_back_GameButton.onClick.AddListener(OnClickDescriptionBack);
            view.m_btn_quick.m_btn_languageButton_GameButton.onClick.AddListener(OnClickQuickForge);
            view.m_btn_make.m_btn_languageButton_GameButton.onClick.AddListener(OnClickForge);
            view.m_btn_resolve.m_btn_languageButton_GameButton.onClick.AddListener(OnClickResolve);
            view.m_btn_arr_GameButton.onClick.AddListener(OnClickArrLst);

            m_equipTypeSubViewDic[(int)EquipType.Forgeable] = view.m_UI_Item_EquipType0;
            m_equipTypeSubViewDic[(int)EquipType.Weapon] = view.m_UI_Item_EquipType1;
            m_equipTypeSubViewDic[(int)EquipType.Helmet] = view.m_UI_Item_EquipType2;
            m_equipTypeSubViewDic[(int)EquipType.Breastplate] = view.m_UI_Item_EquipType3;
            m_equipTypeSubViewDic[(int)EquipType.Gloves] = view.m_UI_Item_EquipType4;
            m_equipTypeSubViewDic[(int)EquipType.Pants] = view.m_UI_Item_EquipType5;
            m_equipTypeSubViewDic[(int)EquipType.Shoes] = view.m_UI_Item_EquipType6;
            m_equipTypeSubViewDic[(int)EquipType.Accessories] = view.m_UI_Item_EquipType7;
            m_equipTypeSubViewDic[(int)EquipType.Compose] = view.m_UI_Item_EquipType8;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            view.m_UI_Item_EquipMaterialList.gameObject.SetActive(false);
            RefreshGold();
        }
       
        #endregion

        private void InitEquipInfo()
        {
            var equipCfgs = CoreUtils.dataService.QueryRecords<EquipDefine>();
            m_allForgeItemInfos.Clear();
            foreach (var equipCfg in equipCfgs)
            {
                var item = new ForgeEquipItemInfo();
                item.SetItemInfo(equipCfg.itemID);
                m_allForgeItemInfos.Add(item);
            }
            SortForgeItemInfos(m_allForgeItemInfos, m_curForgeSortType,false);
        }

        private void RefreshForgeItemInfo()
        {
            if (m_refreshTimer != null)
            {
                m_refreshTimer = null;
            }
            foreach (var forgeItemInfo in m_allForgeItemInfos)
            {
                forgeItemInfo.SetItemInfo(forgeItemInfo.EquipID);
            }
            
            if (m_curPage == EquipPage.Forge)
            {
                SetForgeItemInfos(m_curEquipType);
                if (m_forgeItemInfos.Contains(m_curSelectForgeItem))
                {
                    RefreshSelectForgeItemInfo(m_curSelectForgeItem);
                }
                else
                {
                    SetForgeFirstSelectItem();
                }
                RefreshForgeEquipItemList();
            }
        }
        
        private void LoadEquipItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_equipAssetDic[data.Key] = data.Value.asset() as GameObject;
            }
            view.gameObject.SetActive(true);
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = InitEquipItem;
            functab.GetItemPrefabName = GetPrefabName;
            functab.GetItemSize = GetItemSize;
            view.m_sv_list_view_ListView.SetInitData(m_equipAssetDic, functab);
            
            InitEquipInfo();
            ChangePage(m_curPage);
        }

        private void InitEquipItem(ListView.ListItem item)
        {
            if (m_curPage == EquipPage.Forge)
            {
                InitForgeItemInfo(item);
            }
            else
            {
                InitResolveItemInfo(item);
            }
        }
        
        private float GetItemSize(ListView.ListItem item)
        {
            var index = m_showTypeItemPrefabName.FindIndex(x=>x.Equals(item.prefabName));

            if (index == 0)
            {
                return 143;
            }else if (index == 1)
            {
                return 42.40002f;
            }

            return 143;
        }


        private string GetPrefabName(ListView.ListItem item)
        {
            if (m_curEquipType != EquipType.Compose)
            {
                return m_showTypeItemPrefabName[0];
            }
            else
            {
                List<ForgeEquipItemInfo> itemInfos = null;
                int composeID = 0;
                bool isSuccess = GetForgeComposeItemInfoByIndex(item.index,out itemInfos,out composeID);
                if (isSuccess == false)
                {
                    CoreUtils.logService.Warn($"铁匠铺装备锻造====数组越界  index:{item.index}");
                    return "";
                }
                if (itemInfos == null)
                {
                    return m_showTypeItemPrefabName[1];
                }
                else
                {
                    return m_showTypeItemPrefabName[0];
                }
            }
        }

        private void RefreshEquipItemList() 
        {
            if (m_curPage == EquipPage.Forge)
            {
                RefreshForgeEquipItemList();
            }
            else
            {
                RefreshResolveEquipItemList();
            }
        }
        
        #region 锻造页面
        private void InitForgeToggle()
        {
            view.m_UI_Common_Toggle.SetToggleInfo( (int)m_curForgeSortType,m_forgeToggleName, (isOn,index) =>
            {
                if (isOn == false)
                {
                    return;
                }
                SetForgeSortType((ForgeEquipSortType)index);
                OnClickArrLst();
                view.m_lbl_arrType_LanguageText.text = m_forgeToggleName[index];
            });
            view.m_UI_Common_Toggle.gameObject.SetActive(false);
            view.m_lbl_arrType_LanguageText.text = m_forgeToggleName[(int)m_curForgeSortType];
        }

        private void RefreshForgeEquipItemList()
        {
            if (m_equipAssetDic.Count <= 0)
            {
                return;
            }

            if (m_curEquipType == EquipType.Compose)
            {
                view.m_btn_arr_GameButton.gameObject.SetActive(false);    
            }
            else
            {
                view.m_btn_arr_GameButton.gameObject.SetActive(true);  
            }

            int itemNum = GetForgeListItemNumByEquipType(m_curEquipType);

            view.m_sv_list_view_ListView.FillContent(itemNum);
            view.m_sv_list_view_ListView.ScrollList2IdxCenter(m_curSelectItemIndex);
        }

        private void InitForgeItemInfo(ListView.ListItem item)
        {
            List<ForgeEquipItemInfo> itemInfos = null;

            if (m_curEquipType == EquipType.Compose)
            {
                int composeID = 0;
                if (!GetForgeComposeItemInfoByIndex(item.index, out itemInfos, out composeID))
                {
                    CoreUtils.logService.Warn("铁匠铺装备锻造====套装数据获取错误");
                    return;
                }
                if (itemInfos == null && composeID!=0)
                {
                    UI_Item_EquipLiistTitle_SubView itemView = null;
                    if (item.data != null)
                    {
                        itemView = item.data as UI_Item_EquipLiistTitle_SubView;
                    }
                    else
                    {
                        itemView = new UI_Item_EquipLiistTitle_SubView(item.go.GetComponent<RectTransform>());
                        item.data = itemView;
                    }

                    var composeCfg = CoreUtils.dataService.QueryRecord<EquipComposeDefine>(composeID);
                    itemView.SetText(LanguageUtils.getText(composeCfg.l_nameID));
                    return;
                }
            }
            
            UI_Item_EquipLiistItem_SubView equipItemView = null;
            if (item.data != null)
            {
                equipItemView = item.data as UI_Item_EquipLiistItem_SubView;
            }
            else
            {
                equipItemView = new UI_Item_EquipLiistItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = equipItemView;
            }

            if (itemInfos == null)
            {
                int index = item.index * 2;
                if (m_forgeItemInfos.Count <= index)
                {
                    CoreUtils.logService.Warn("铁匠铺装备锻造====数组溢出");
                    return;
                }
                itemInfos = new List<ForgeEquipItemInfo>();
                for (int i = index; i < m_forgeItemInfos.Count &&i<SVITEMNUMPERCOW+index; i++)
                {
                    itemInfos.Add(m_forgeItemInfos[i]);
                }
            }

            int curSelectID=0;
            if (m_curSelectForgeItem != null)
            {
                curSelectID = m_curSelectForgeItem.EquipID;
                foreach (var itemInfo in itemInfos)
                {
                    if (itemInfo == m_curSelectForgeItem)
                    {
                        m_curSelectItemIndex = item.index;
                        break;
                    }
                }
            }
            equipItemView.SetForgeItemInfo(item.index,itemInfos,curSelectID,OnSelectForgeItem);
        }

        private void RefreshSelectForgeItemInfo(ForgeEquipItemInfo itemInfo)
        {
            m_curSelectForgeItem = itemInfo;
            if (m_curSelectForgeItem == null)
            {
                view.m_UI_Item_EquipAtt.gameObject.SetActive(false);
                view.m_pl_ues.gameObject.SetActive(false);
                view.m_btn_resolve.gameObject.SetActive(false);
                view.m_btn_make.gameObject.SetActive(false);
                view.m_btn_quick.gameObject.SetActive(false);
                view.m_lbl_materialTitle_LanguageText.gameObject.SetActive(false);
                DisableMaterialItems();
                view.m_img_euqip_PolygonImage.gameObject.SetActive(false);
                return;
            }
            var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(itemInfo.EquipID);
            
            view.m_lbl_materialTitle_LanguageText.gameObject.SetActive(false);
            //属性
            view.m_UI_Item_EquipAtt.gameObject.SetActive(true);
            view.m_UI_Item_EquipAtt.SetForgeItemInfo(itemInfo);
            view.m_pl_ues.gameObject.SetActive(false);
            
            //按钮
            m_lackGold = equipCfg.costGold - m_currencyProxy.Gold>0? equipCfg.costGold:0;
            view.m_btn_resolve.gameObject.SetActive(false);
            bool makeBtnIsOn = (!itemInfo.IsForgeable && itemInfo.IsQuickForgeable) == false;
            view.m_btn_make.gameObject.SetActive(makeBtnIsOn);
            view.m_btn_make.SetGray(!itemInfo.IsForgeable);
            view.m_btn_quick.gameObject.SetActive(!makeBtnIsOn);
            if (makeBtnIsOn)
            {
                view.m_btn_make.SetNum(ClientUtils.FormatComma(equipCfg.costGold));
            }
            
            //材料
            DisableMaterialItems();
            for (int i = 0; i < equipCfg.makeMaterial.Count; i++)
            {
                if (m_materialList.Count <= i)
                {
                    var materialItemObj = GameObject.Instantiate(view.m_UI_Item_EquipMaterialList.gameObject,view.m_pl_materialQueue_GridLayoutGroup.transform);
                    var materialItem = new UI_Item_EquipMaterialList_SubView(materialItemObj.GetComponent<RectTransform>());
                    m_materialList.Add(materialItem);
                }
                var makeMaterial = equipCfg.makeMaterial[i];
                var makeMaterialNum = equipCfg.makeMaterialNum[i];
                m_materialList[i].SetInfo(makeMaterial, makeMaterialNum, true, () => {
                    CaptainItemSourceViewData data = new CaptainItemSourceViewData
                    {
                        ResourceType = EnumCaptainLevelResourceType.EquipForge,
                        RequireItemId = makeMaterial,
                        RequireItemNum = makeMaterialNum,
                    };
                    CoreUtils.uiManager.ShowUI(UI.s_captainItemSource, null, data);
                });
                m_materialList[i].gameObject.SetActive(true);
            }
            //图标
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemInfo.EquipID);
            view.m_img_euqip_PolygonImage.gameObject.SetActive(true);
            ClientUtils.LoadSprite(view.m_img_euqip_PolygonImage, itemCfg.itemIcon);
            view.m_img_euqip_Animation.Play("UA_Equip_Scale");
        }

        private void OnSelectForgeItem(ForgeEquipItemInfo itemInfo,int index)
        {
            RefreshSelectForgeItemInfo(itemInfo);
            if (m_curSelectItemIndex != -1)
            {
                view.m_sv_list_view_ListView.RefreshItem(m_curSelectItemIndex);
            }
            view.m_sv_list_view_ListView.RefreshItem(index);
        }

        private void DisableMaterialItems()
        {
            foreach (var materialListSubView in m_materialList)
            {
                materialListSubView.gameObject.SetActive(false);
            }
        }

        private void SetForgeSortType(ForgeEquipSortType type)
        {
            m_curForgeSortType = type;
            SortForgeItemInfos(m_forgeItemInfos, m_curForgeSortType, m_curEquipType == EquipType.Compose);
            RefreshForgeEquipItemList();
        }

        private void SetForgeFirstSelectItem()
        {
            m_curSelectForgeItem = null;
            m_curSelectItemIndex = -1;
            if (m_forgeItemInfos.Count > 0)
            {
                m_curSelectForgeItem = m_forgeItemInfos[0];
            }
            RefreshSelectForgeItemInfo(m_curSelectForgeItem);
        }

        private void SetForgeJumpItem()
        {
            m_curSelectForgeItem = null;
            m_curSelectItemIndex = -1;
            if (m_forgeItemInfos.Count > 0)
            {
                if (m_jumpEquipId > 0)
                {
                    for (int i = 0; i < m_forgeItemInfos.Count; i++)
                    {
                        if (m_forgeItemInfos[i].EquipID == m_jumpEquipId)
                        {
                            m_curSelectForgeItem = m_forgeItemInfos[i];
                            break;
                        }
                    }
                }
                if (m_curSelectForgeItem == null)
                {
                    m_curSelectForgeItem = m_forgeItemInfos[0];
                }
            }
            RefreshSelectForgeItemInfo(m_curSelectForgeItem);
        }

        private void CheckForgeEquip()
        {
            if (m_curSelectForgeItem == null)
            {
                return;
            }
            m_bagProxy.CheckMakeEquip(m_curSelectForgeItem.EquipID);
            SetSendingMsgStatus(true);
        }

        private void ForgeEquip(bool isExclusive)
        {
            if (isExclusive)
            {
                CoreUtils.uiManager.ShowUI(UI.s_EquipTalentChoose,null,m_curSelectForgeItem);
            }
            else
            {
                m_bagProxy.MakeEquipment(m_curSelectForgeItem.EquipID,0);
                SetSendingMsgStatus(true);

            }
        }

        private void QuickForge()
        {
            CoreUtils.uiManager.ShowUI(UI.s_EquipQuick,null,m_curSelectForgeItem);
        }
        
        
        #endregion

        #region 分解页面

        private void InitResolveToggle()
        {
            view.m_UI_Common_Toggle.SetToggleInfo( (int)m_curResolveSortType,m_resolveToggleName, (isOn,index) =>
            {
                if (isOn == false)
                {
                    return;
                }
                SetResolveSortType((ResolveEquipSortType)index);
                OnClickArrLst();
                view.m_lbl_arrType_LanguageText.text = m_resolveToggleName[index];
            });
            view.m_UI_Common_Toggle.gameObject.SetActive(false);
            view.m_lbl_arrType_LanguageText.text = m_resolveToggleName[(int)m_curResolveSortType];
        }

        private void RefreshResolveEquipItemList()
        {
            if (m_equipAssetDic.Count <= 0)
            {
                return;
            }
            view.m_btn_arr_GameButton.gameObject.SetActive(true);   
            int itemNum = GetEquipLitItemNum();
            //空列表
            view.m_lbl_empty_LanguageText.gameObject.SetActive(itemNum == 0);
            
            view.m_sv_list_view_ListView.FillContent(itemNum);
            view.m_sv_list_view_ListView.ScrollList2IdxCenter(m_curSelectItemIndex);
        }

        private void InitResolveItemInfo(ListView.ListItem item)
        {
            UI_Item_EquipLiistItem_SubView equipItemView = null;
            if (item.data != null)
            {
                equipItemView = item.data as UI_Item_EquipLiistItem_SubView;
            }
            else
            {
                equipItemView = new UI_Item_EquipLiistItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = equipItemView;
            }

            int index = item.index * 2;
            if (m_resolveItemInfos.Count <= index)
            {
                CoreUtils.logService.Warn("铁匠铺装备分解====数组溢出");
                return;
            }

            List<EquipItemInfo> itemInfos = new List<EquipItemInfo>();
            for (int i = index; i < m_resolveItemInfos.Count && i < SVITEMNUMPERCOW + index; i++)
            {
                itemInfos.Add(m_resolveItemInfos[i]);
                if (m_resolveItemInfos[i] == m_curSelectResolveItem)
                {
                    m_curSelectItemIndex = item.index;
                }
            }

            long curSelectID = 0;
            if (m_curSelectResolveItem != null)
            {
                curSelectID = m_curSelectResolveItem.ItemIndex;
            }

            equipItemView.SetEquipItemInfo(item.index, itemInfos, curSelectID, OnSelectResolveItem);
        }

        private void RefreshSelectResolveItemInfo(EquipItemInfo itemInfo)
        {
            m_curSelectResolveItem = itemInfo;
            if (m_curSelectResolveItem == null)
            {
                view.m_UI_Item_EquipAtt.gameObject.SetActive(false);
                view.m_pl_ues.gameObject.SetActive(false);
                view.m_btn_resolve.gameObject.SetActive(false);
                view.m_btn_make.gameObject.SetActive(false);
                view.m_btn_quick.gameObject.SetActive(false);
                view.m_lbl_materialTitle_LanguageText.gameObject.SetActive(false);
                DisableMaterialItems();
                view.m_img_euqip_PolygonImage.gameObject.SetActive(false);
                return;
            }
            view.m_lbl_materialTitle_LanguageText.gameObject.SetActive(true);
            var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(itemInfo.ItemID);

            //属性
            view.m_UI_Item_EquipAtt.gameObject.SetActive(true);
            view.m_UI_Item_EquipAtt.SetEquipItemInfo(itemInfo);
            if (itemInfo.HeroID != 0)
            {
                view.m_pl_ues.gameObject.SetActive(true);
                view.m_UI_Model_CaptainHead.SetHero(itemInfo.HeroID,0);
            }
            else
            {
                view.m_pl_ues.gameObject.SetActive(false);
            }
            
            //按钮
            view.m_btn_resolve.gameObject.SetActive(true);
            view.m_btn_resolve.SetGrayAndDisable(false);
            view.m_btn_make.gameObject.SetActive(false);
            view.m_btn_quick.gameObject.SetActive(false);
            //材料
            DisableMaterialItems();
            for (int i = 0; i < equipCfg.decomposeMaterial.Count; i++)
            {
                if (m_materialList.Count <= i)
                {
                    var materialItemObj = GameObject.Instantiate(view.m_UI_Item_EquipMaterialList.gameObject,view.m_pl_materialQueue_GridLayoutGroup.transform);
                    var materialItem = new UI_Item_EquipMaterialList_SubView(materialItemObj.GetComponent<RectTransform>());
                    m_materialList.Add(materialItem);
                }
                m_materialList[i].SetInfo(equipCfg.decomposeMaterial[i],equipCfg.decomposeMaterialNum[i],false);
                m_materialList[i].gameObject.SetActive(true);
            }
            //图标
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemInfo.ItemID);
            view.m_img_euqip_PolygonImage.gameObject.SetActive(true);
            ClientUtils.LoadSprite(view.m_img_euqip_PolygonImage, itemCfg.itemIcon);
            view.m_img_euqip_Animation.Play("UA_Equip_Scale");
        }

        private void OnSelectResolveItem(EquipItemInfo itemInfo, int index)
        {
            RefreshSelectResolveItemInfo(itemInfo);
            if (m_curSelectItemIndex != -1)
            {
                view.m_sv_list_view_ListView.RefreshItem(m_curSelectItemIndex);
            }
            view.m_sv_list_view_ListView.RefreshItem(index);
        }

        private void SetResolveSortType(ResolveEquipSortType type)
        {
            m_curResolveSortType = type;
            SortResolveItemInfos(m_resolveItemInfos, m_curResolveSortType);
            RefreshResolveEquipItemList();
        }
        
        private void SetResolveFirstSelectItem()
        {
            m_curSelectResolveItem = null;
            m_curSelectItemIndex = -1;
            if (m_resolveItemInfos.Count > 0)
            {
                m_curSelectResolveItem = m_resolveItemInfos[0];
            }
            RefreshSelectResolveItemInfo(m_curSelectResolveItem);
        }

        //展示分解所获道具
        private void ShowResolveReward(int itemID)
        {
            RewardGetData rewardGetData = new RewardGetData();
            RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(itemID);

            rewardGetData.rewardGroupDataList =new List<RewardGroupData>();
            for (int i = 0; i < equipCfg.decomposeMaterial.Count && i<equipCfg.decomposeMaterialNum.Count; i++)
            {
                
                rewardGetData.rewardGroupDataList.Add(m_rewardGroupProxy.ConvertRewardGroupData((int)EnumRewardType.Item,equipCfg.decomposeMaterial[i],equipCfg.decomposeMaterialNum[i]));
            }
            rewardGetData.nameType = 2;
            if (rewardGetData.rewardGroupDataList.Count != 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
            }
        }

        //判断装备是否可被分解
        private void ResolveEquip()
        {
            m_bagProxy.ResolveEquipment(m_curSelectResolveItem.ItemIndex);
            SetSendingMsgStatus(true);
        }

        private void CheckResolveEquip()
        {
            if (m_curSelectResolveItem.HeroID != 0)
            {
                if (m_troopProxy.IsWarByHero((int) m_curSelectResolveItem.HeroID))
                {
                    Tip.CreateTip(182047).Show();
                    return;
                }
                else
                {
                    var heroCfg = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_curSelectResolveItem.HeroID);
                    string txt = LanguageUtils.getTextFormat(182046, LanguageUtils.getText(heroCfg.l_nameID));
                    Alert.CreateAlert(txt).SetLeftButton().SetRightButton(() =>
                    {
                        ResolveEquip();
                    }).Show();
                }
                return;
            }

            ResolveEquip();
        }
        
        #endregion
        
        #region 通用模块

        private void RefreshGold()
        {
            long num = m_currencyProxy.Gold;
            view.m_UI_Gold.SetRes( (int)EnumCurrencyType.gold,num,1);
        }
        
        private void SetSendingMsgStatus(bool bValue)
        {
            m_isSendingMsg = bValue;
        }

        private void OnItemChange()
        {
            var itemInfos = m_bagProxy.ItemChangeList;
            foreach (var itemIndex in itemInfos)
            {
                var equipItem = m_bagProxy.GetEquipItemInfo(itemIndex);
                if (m_curPage == EquipPage.Forge)
                {
                    //锻造成功
                    if (equipItem!=null && equipItem.ItemID == m_curSelectForgeItem.EquipID)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_EquipSuccess, null, equipItem);
                        UpdateResolveItemInfos();
                        m_isSendingMsg = false;
                    }
                    break;
                }

                if (m_curPage == EquipPage.Decompose)
                {
                    //分解成功
                    if ( equipItem == null && m_curSelectResolveItem!=null && m_curSelectResolveItem.ItemIndex == itemIndex )
                    {
                        ShowResolveReward(m_curSelectResolveItem.ItemID);
                        UpdateResolveItemInfos();
                        ChangeEquipType(m_curEquipType);
                        m_isSendingMsg = false;
                    }
                }
            }

            //刷新锻造信息
            if (m_refreshTimer == null)
            {
                m_refreshTimer = Timer.Register(0.5f, RefreshForgeItemInfo, null, false);
            }
        }

        private void OnUpdateCurrency()
        {
            RefreshSelectForgeItemInfo(m_curSelectForgeItem);
            RefreshGold();
        }
        
        private void ChangePage(EquipPage page)
        {
            m_curPage = page;
            if (page == EquipPage.Forge)
            {
                InitForgeToggle();
                view.m_UI_Item_EquipType8.gameObject.SetActive(true);
                view.m_lbl_materialTitle_LanguageText.text = LanguageUtils.getText(182065);
                
                view.m_lbl_empty_LanguageText.gameObject.SetActive(false);
                view.m_lbl_make_LanguageText.color = new Color(82/255.0f,68/255.0f,32/255.0f);
                view.m_lbl_make_Shadow.useGraphicAlpha = true;
                view.m_lbl_resolve_LanguageText.color = new Color(183/255.0f,167/255.0f,118/255.0f);
                view.m_lbl_resolve_Shadow.useGraphicAlpha = false;
            }
            else
            {
                InitResolveToggle();
                view.m_UI_Item_EquipType8.gameObject.SetActive(false);
                view.m_lbl_materialTitle_LanguageText.text = LanguageUtils.getText(182066);
                view.m_lbl_resolve_LanguageText.color = new Color(82/255.0f,68/255.0f,32/255.0f);
                view.m_lbl_resolve_Shadow.useGraphicAlpha = true;
                view.m_lbl_make_LanguageText.color = new Color(183/255.0f,167/255.0f,118/255.0f);
                view.m_lbl_make_Shadow.useGraphicAlpha = false;
            }
            OnClickDescriptionBack();
            if (m_isJumpHandle)
            {
                m_isJumpHandle = false;
                m_equipTypeSubViewDic[(int)m_jumpEquipType].m_UI_Item_EquipType_GameToggle.SetIsOnWithoutNotify(true);
                JumpEquipType();
            }
            else
            {
                view.m_UI_Item_EquipType0.m_UI_Item_EquipType_GameToggle.SetIsOnWithoutNotify(true);
                OnClickEquipTypeZero(true);
            }
        }

        private void JumpEquipType()
        {
            if (m_curPage == EquipPage.Forge)
            {
                ChangeEquipType(m_jumpEquipType, true);
            }
            else
            {
                ChangeEquipType(EquipType.Decomposable, true);
            }
        }

        private void ChangeEquipType(EquipType type, bool findItem = false)
        {
            m_curEquipType = type;
            if (m_curPage == EquipPage.Forge)
            {
                SetForgeItemInfos(m_curEquipType);
                if (findItem)
                {
                    SetForgeJumpItem();
                }
                else
                {
                    SetForgeFirstSelectItem();
                }
                RefreshForgeEquipItemList();
            }
            else
            {
                if (m_allEquipItemInfos == null)
                {
                    UpdateResolveItemInfos();
                }
                SetResolveItemInfos(m_curEquipType);
                SetResolveFirstSelectItem();
                RefreshResolveEquipItemList();
            }
            
            var equipGroupCfg = CoreUtils.dataService.QueryRecord<EquipMaterialGroupDefine>((int) type);
            view.m_lbl_listType_LanguageText.text = LanguageUtils.getText(equipGroupCfg.l_nameID);
        }

        private void OnClickForgePage(bool isOn)
        {
            if (isOn == false && m_curPage == EquipPage.Forge)
            {
                return;
            }
            ChangePage(EquipPage.Forge);
        }

        private void OnClickDecomposePage(bool isOn)
        {
            if (isOn == false && m_curPage == EquipPage.Decompose)
            {
                return;
            }
            ChangePage(EquipPage.Decompose);
        }

        private void OnClickEquipTypeZero(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            if (m_curPage == EquipPage.Forge)
            {
                ChangeEquipType(EquipType.Forgeable);
            }
            else
            {
                ChangeEquipType(EquipType.Decomposable);
            }
        }

        private void OnClickEquipTypeWeapon(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Weapon);
        }
        private void OnClickEquipTypeHelmet(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Helmet);
        }
        private void OnClickEquipTypeBreastplate(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Breastplate);
        }
        private void OnClickEquipTypeGloves(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Gloves);
        }
        private void OnClickEquipTypePants(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Pants);
        }
        private void OnClickEquipTypeShoes(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Shoes);
        }
        private void OnClickEquipTypeAccessories(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Accessories);
        }
        private void OnClickEquipTypeCompose(bool isOn)
        {
            if (isOn == false)
            {
                return;
            }
            ChangeEquipType(EquipType.Compose);
        }

        private void OnClickDescription()
        {
            view.m_pl_dec_ArabLayoutCompment.gameObject.SetActive(true);
            view.m_pl_att_ArabLayoutCompment.gameObject.SetActive(false);
            if (m_curPage == EquipPage.Forge)
            {
                view.m_lbl_describe_LanguageText.text = LanguageUtils.getText(182077);
            }
            else
            {
                view.m_lbl_describe_LanguageText.text = LanguageUtils.getText(182078);
            }
            view.m_sv_des_ListView.SetContainerPos(0);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_sv_des_ListView.listContainer);
        }

        private void OnClickDescriptionBack()
        {
            view.m_pl_dec_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_pl_att_ArabLayoutCompment.gameObject.SetActive(true);
        }

        private void OnClickForge()
        {
            if (m_isSendingMsg)
            {
                return;
            }
            
            if (!m_curSelectForgeItem.IsForgeable)
            {
                Tip.CreateTip(182045,Tip.TipStyle.Middle).Show();
                return;
            }
            
            if (m_lackGold > 0)
            {
                m_currencyProxy.LackOfResources(0, 0, 0, m_lackGold); //普通资源
                return;
            }
            
            CheckForgeEquip();
        }

        private void OnClickQuickForge()
        {
            if (m_isSendingMsg == true)
            {
                return;
            }
            QuickForge();
        }

        private void OnClickResolve()
        {
            if (m_isSendingMsg == true)
            {
                return;
            }
            CheckResolveEquip();
        }

        private void OnClickArrLst()
        {
            view.m_UI_Common_Toggle.gameObject.SetActive(!view.m_UI_Common_Toggle.gameObject.activeSelf);
        }

        private void OnClickClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_Equip);
        }

        #endregion
        
        #region 数据处理

        #region 锻造
        
        private void SetForgeItemInfos(EquipType type)
        {
            m_forgeItemInfos.Clear();
            GetForgeItemInfosByEquipType(ref m_forgeItemInfos, type);
        }

        //获取锻造页面装备并分类排序
        private void GetForgeItemInfosByEquipType(ref List<ForgeEquipItemInfo> itemInfos, EquipType type)
        {
            switch (type)
            {
                case EquipType.Forgeable:
                    itemInfos.AddRange(m_allForgeItemInfos.FindAll(x=>x.IsForgeable||x.IsQuickForgeable));
                    break;
                case EquipType.Decomposable:
                    break;
                case EquipType.Compose:
                    itemInfos.AddRange(m_allForgeItemInfos.FindAll(x=>x.Compose!=0));
                    break;
                default:
                    itemInfos.AddRange(m_allForgeItemInfos.FindAll(x=>x.GroupID == (int)type));
                    break;
            }
            SortForgeItemInfos(itemInfos,m_curForgeSortType,type == EquipType.Compose);
        }

        private void SortForgeItemInfos(List<ForgeEquipItemInfo> itemInfos,ForgeEquipSortType type,bool isCompose)
        {
            itemInfos.Sort((x,y)=> CompareForgeItemInfo(x,y,type,isCompose));
        }

        private int CompareForgeItemInfo(ForgeEquipItemInfo x, ForgeEquipItemInfo y, ForgeEquipSortType type,bool isCompose)
        {
            int comp = 0;
            if (isCompose)
            {
                comp = x.Compose.CompareTo(y.Compose);
            }
            if (comp != 0)
            {
                if (type == ForgeEquipSortType.LowQuality)
                {
                    comp *= -1;
                }
                return comp;
            }
            switch (type)
            {
                case ForgeEquipSortType.Forgeable:
                    comp = -x.IsForgeable.CompareTo(y.IsForgeable);
                    if (comp == 0)
                    {
                        comp = -x.IsQuickForgeable.CompareTo(y.IsQuickForgeable);
                    }
                    break;
                case ForgeEquipSortType.DrawingOrder:
                    if (x.DrawingNum > 0 && y.DrawingNum > 0)
                    {
                        comp = 0;
                    }
                    else
                    {
                        comp = -x.DrawingNum.CompareTo(y.DrawingNum);
                    }
                    break;
                case ForgeEquipSortType.HighQuality:
                    comp = -x.Order.CompareTo(y.Order);
                    break;
                case ForgeEquipSortType.LowQuality:
                    comp = x.Order.CompareTo(y.Order);
                    break;
            }
            if (comp == 0 && type!= ForgeEquipSortType.HighQuality && type!= ForgeEquipSortType.LowQuality)
            {
                comp = -x.Order.CompareTo(y.Order);
            }
            if (comp == 0)
            {
                comp = x.EquipID.CompareTo(y.EquipID);
            }
            return comp;
        }

        private int GetForgeListItemNumByEquipType(EquipType type)
        {
            int num = 0;
            if (type == EquipType.Compose)
            {
                int preComposeID = 0;
                int equipNum = 0;
                for (int i=0; i<m_forgeItemInfos.Count;i++)
                {
                    var itemInfo = m_forgeItemInfos[i];
                    equipNum++;
                    if (i == m_forgeItemInfos.Count-1 || m_forgeItemInfos[i+1].Compose!= itemInfo.Compose)
                    {
                        var curComposeEquipNum = (equipNum + SVITEMNUMPERCOW - 1) / SVITEMNUMPERCOW;
                        if (curComposeEquipNum > 0)
                        {
                            num+= curComposeEquipNum+1;
                        }
                        equipNum = 0;
                    }
                }
            }
            else
            {
                num = (m_forgeItemInfos.Count + SVITEMNUMPERCOW - 1) / SVITEMNUMPERCOW;
            }

            return num;
        }

        private bool GetForgeComposeItemInfoByIndex(int index, out List<ForgeEquipItemInfo> forgeEquipItemInfos,
            out int composeID)
        {
            int num = 0;
            int equipNum = 0;
            for (int i=0; i< m_forgeItemInfos.Count;i++)
            {
                var itemInfo = m_forgeItemInfos[i];
                equipNum++;
                if (i == m_forgeItemInfos.Count-1 || m_forgeItemInfos[i+1].Compose!= itemInfo.Compose)
                {
                    var curComposeEquipNum = (equipNum + SVITEMNUMPERCOW - 1) / SVITEMNUMPERCOW;
                    if (curComposeEquipNum > 0)
                    {
                        num++;
                        if (num > index)
                        {
                            forgeEquipItemInfos = null;
                            composeID = itemInfo.Compose;
                            return true;
                        }
                        num+= curComposeEquipNum;
                        if (num > index)
                        {
                            num-=curComposeEquipNum;
                            composeID = itemInfo.Compose;
                            forgeEquipItemInfos=new List<ForgeEquipItemInfo>();
                            for (int j = i-equipNum+1; j <= i ; j += SVITEMNUMPERCOW)
                            {
                                num++;
                                if (num > index)
                                {
                                    for (int k = j; k <= i; k++)
                                    {
                                        forgeEquipItemInfos.Add(m_forgeItemInfos[k]);
                                    }
                                }
                            }
                            return true;
                        }
                    }
                    equipNum = 0;
                }
            }
            forgeEquipItemInfos = null;
            composeID = 0;
            return false;
        }

        #endregion

        #region 分解

        private void UpdateResolveItemInfos()
        {
            m_allEquipItemInfos = m_bagProxy.GetEquipItems();
            SortResolveItemInfos(m_allEquipItemInfos,m_curResolveSortType);
        }

        private void SetResolveItemInfos(EquipType type)
        {
            m_resolveItemInfos.Clear();
            GetResolveItemInfosByEquipType(ref m_resolveItemInfos, type);
        }
        private void GetResolveItemInfosByEquipType(ref List<EquipItemInfo> itemInfos, EquipType type)
        {
            switch (type)
            {
                case EquipType.Decomposable:
                    itemInfos.AddRange(m_allEquipItemInfos);
                    break;
                default:
                    itemInfos.AddRange(m_allEquipItemInfos.FindAll(x=>x.Group == (int)type));
                    break;
            }
            SortResolveItemInfos(itemInfos,m_curResolveSortType);
        }

        private void SortResolveItemInfos(List<EquipItemInfo> itemInfos, ResolveEquipSortType type)
        {
            itemInfos.Sort((x, y) => CompareEquipItemInfo(x, y, type));
        }

        private int CompareEquipItemInfo(EquipItemInfo x, EquipItemInfo y, ResolveEquipSortType type)
        {
            int comp = 0;
            switch (type)
            {
                case ResolveEquipSortType.LowQuality:
                    comp = x.Order.CompareTo(y.Order);
                    break;
                case ResolveEquipSortType.HighQuality:
                    comp = -x.Order.CompareTo(y.Order);
                    break;
                case ResolveEquipSortType.Worn:
                    comp = -(x.HeroID !=0).CompareTo(y.HeroID !=0);
                    break;
                case ResolveEquipSortType.Exclusive:
                    comp = -x.Exclusive.CompareTo(y.Exclusive);
                    break;
            }
            if (comp == 0&& type!= ResolveEquipSortType.HighQuality && type != ResolveEquipSortType.LowQuality )
            {
                comp = x.Order.CompareTo(y.Order);
            }
            if (comp == 0)
            {
                comp = x.ItemID.CompareTo(y.ItemID);
            }
            return comp;
        }

        private int GetEquipLitItemNum()
        {
            return (m_resolveItemInfos.Count + SVITEMNUMPERCOW - 1) / SVITEMNUMPERCOW;

        }
        #endregion
        
        
        #endregion
    }
}