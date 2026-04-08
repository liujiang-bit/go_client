// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月26日
// Update Time         :    2019年12月26日
// Class Description   :    BuildCityMediator
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
using SprotoType;
using UnityEngine.UI;

namespace Game
{
    public enum BuildCityType
    {
        /// <summary>
        /// 可建造状态
        /// </summary>
        Buildable = 0,
        /// <summary>
        /// 待解锁状态
        /// </summary>
        Notunlock = 1,
        /// <summary>
        /// 建筑已满状态
        /// </summary>
        BuildMax = 2,
    }
    public class BuildCityMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "BuildCityMediator";
        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private Material gray_material, gray_mask_material , defaultMaterial;
        List<BuildingTypeConfigDefine> buildingTypeConfigList = new List<BuildingTypeConfigDefine>();
        private List<BuildingTypeConfigDefine> m_jingjiBuildList = new List<BuildingTypeConfigDefine>();
        private List<BuildingTypeConfigDefine> m_junshiBuildList = new List<BuildingTypeConfigDefine>();
        private List<BuildingTypeConfigDefine> m_zhuangshiBuildList = new List<BuildingTypeConfigDefine>();

        private List<BuildingTypeConfigDefine> m_curBuildList = new List<BuildingTypeConfigDefine>();

        private bool m_assetsReady = false;

        private DataProxy m_dataProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private CurrencyProxy m_currencyProxy;
        private BagProxy m_bagProxy;

        private EnumBuildingGroupType m_buildingGroupType;//快捷定位到类型
        private int m_buildingtype = 0;//快捷定位到建筑
        private int m_buildingIndex = 0;//快捷定位下标
        private Timer m_timer;// Item动画调用

        private const float duration = 0.05f;//动画播放间隔
        #endregion

        //IMediatorPlug needs
        public BuildCityMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public BuildCityView view;

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

        protected override void InitData()
        {
            m_dataProxy = AppFacade.GetInstance().RetrieveProxy(DataProxy.ProxyNAME) as DataProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (view.data is int)
            {
                m_buildingtype = (int)view.data;
            }
            else if (view.data is GOScrptGuide)
            {
                GOScrptGuide goScrptGuide = view.data as GOScrptGuide;
                if (goScrptGuide.buildingGroupkType != EnumBuildingGroupType.NON)
                {
                    m_buildingGroupType = goScrptGuide.buildingGroupkType;
                }
                m_buildingtype = goScrptGuide.param1;
            }
           buildingTypeConfigList = CoreUtils.dataService.QueryRecords<BuildingTypeConfigDefine>();
            for (int i = 0; i < buildingTypeConfigList.Count; i++)
            {
                BuildingTypeConfigDefine buildingTypeConfig = buildingTypeConfigList[i];
                EnumBuildingGroupType enumBuildingGroupType = (EnumBuildingGroupType)buildingTypeConfig.group;
                switch (enumBuildingGroupType)
                {
                    case EnumBuildingGroupType.Economic:
                        m_jingjiBuildList.Add(buildingTypeConfig);
                        break;
                    case EnumBuildingGroupType.Military:
                        m_junshiBuildList.Add(buildingTypeConfig);
                        break;
                    case EnumBuildingGroupType.Decorative:
                        m_zhuangshiBuildList.Add(buildingTypeConfig);
                        break;
                    case EnumBuildingGroupType.NON:
                        break;
                    default:
                        break;
                }
            }
            m_jingjiBuildList.Sort(delegate (BuildingTypeConfigDefine x, BuildingTypeConfigDefine y)
            {
                int re = ((int)(m_cityBuildingProxy.GetBuildCityType(x.type))).CompareTo((int)(m_cityBuildingProxy.GetBuildCityType(y.type)));
                if (re == 0)
                {
                    x.type.CompareTo(y.type);
                }
                return re;
            });

            m_junshiBuildList.Sort(delegate (BuildingTypeConfigDefine x, BuildingTypeConfigDefine y)
            {
                int re = ((int)(m_cityBuildingProxy.GetBuildCityType(x.type))).CompareTo((int)((m_cityBuildingProxy.GetBuildCityType(y.type))));
                if (re == 0)
                {
                    x.type.CompareTo(y.type);
                }
                return re;
            });
            m_zhuangshiBuildList.Sort(delegate (BuildingTypeConfigDefine x, BuildingTypeConfigDefine y)
            {
                int re = ((int)(m_cityBuildingProxy.GetBuildCityType(x.type))).CompareTo((int)((m_cityBuildingProxy.GetBuildCityType(y.type))));
                if (re == 0)
                {
                    x.type.CompareTo(y.type);
                }
                return re;
            });
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Item_BuildCityBtn1.m_btn_btn_GameButton.onClick.AddListener(OnEconomicBtnClick);
            view.m_UI_Item_BuildCityBtn2.m_btn_btn_GameButton.onClick.AddListener(OnMilitaryBtnClick);
            view.m_UI_Item_BuildCityBtn3.m_btn_btn_GameButton.onClick.AddListener(OnDecorativeBtnClick);
            CheckRedPoint();
        }

        protected override void BindUIData()
        {
 
            m_preLoadRes.Add("UI_Model_ResourcesConsumeInBCB");
            for (int i = 0; i < m_zhuangshiBuildList.Count; i++)
            {
                m_preLoadRes.Add( m_cityBuildingProxy.GetImgIdByType(m_zhuangshiBuildList[i].type));
            }
            for (int i = 0; i < m_junshiBuildList.Count; i++)
            {
                m_preLoadRes.Add( m_cityBuildingProxy.GetImgIdByType(m_junshiBuildList[i].type));
            }
            for (int i = 0; i < m_jingjiBuildList.Count; i++)
            {
                m_preLoadRes.Add(m_cityBuildingProxy.GetImgIdByType(m_jingjiBuildList[i].type));
            }
            m_preLoadRes.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            CoreUtils.assetService.LoadAssetAsync<Material>("Assets/Shader/UIMaskGray", (IAsset asset) =>
            {
                gray_mask_material = asset.asset() as Material;
            });
            CoreUtils.assetService.LoadAssetAsync<Material>("UI_GrayWithMask", (IAsset asset) =>
            {
                gray_material = asset.asset() as Material;
                ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
                {
                    m_assetDic = assetDic;
                    m_assetsReady = true;
                    if (m_buildingtype != 0&& m_buildingGroupType == EnumBuildingGroupType.NON)
                    {
                        BuildingTypeConfigDefine buildingTypeConfig = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>(m_buildingtype);
                        m_buildingGroupType = (EnumBuildingGroupType)buildingTypeConfig.group;
                        switch (m_buildingGroupType)
                        {
                            case EnumBuildingGroupType.Economic:
                                ShowEconomic();

                                break;
                            case EnumBuildingGroupType.Military:
                                ShowMilitary();
                                break;
                            case EnumBuildingGroupType.Decorative:
                                ShowDecorative();
                                break;
                            case EnumBuildingGroupType.NON:
                                ShowEconomic();
                                break;
                            default:
                                break;
                        }
                        m_buildingIndex = m_curBuildList.IndexOf(buildingTypeConfig);
                        view.m_sv_list_view_ListView.ScrollList2IdxCenter(m_buildingIndex);
                        ListView.ListItem ListItem = view.m_sv_list_view_ListView.GetItemByIndex(m_buildingIndex);
                        if (ListItem != null)
                        {
                            FingerTargetParam param = new FingerTargetParam();
                            param.AreaTarget = ListItem.go;
                            param.ArrowDirection = (int)EnumArrorDirection.Up;
                            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                        }
                    }
                    else if (m_buildingtype == 0&& m_buildingGroupType != EnumBuildingGroupType.NON)
                    {
                        switch (m_buildingGroupType)
                        {
                            case EnumBuildingGroupType.Economic:
                                ShowEconomic();

                                break;
                            case EnumBuildingGroupType.Military:
                                ShowMilitary();
                                break;
                            case EnumBuildingGroupType.Decorative:
                                ShowDecorative();
                                break;
                            case EnumBuildingGroupType.NON:
                                ShowEconomic();
                                break;
                            default:
                                break;
                        }
                        view.m_sv_list_view_ListView.ScrollList2IdxCenter(m_buildingIndex);
                        ListView.ListItem ListItem = view.m_sv_list_view_ListView.GetItemByIndex(m_buildingIndex);
                        if (ListItem != null)
                        {
                            FingerTargetParam param = new FingerTargetParam();
                            param.AreaTarget = ListItem.go;
                            param.ArrowDirection = (int)EnumArrorDirection.Up;
                            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                        }
                    }
                    else
                    {
                        ShowEconomic(); 
                    }
                    if (m_zhuangshiBuildList.Count == 0)
                    {
                        view.m_UI_Item_BuildCityBtn3.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.m_UI_Item_BuildCityBtn3.gameObject.SetActive(true);
                    }
                    ShowItem();
                });
            }, view.gameObject);      

        }

        #endregion
        private void CheckRedPoint()
        {
            int countEconomic = 0;//经济
            int  countMilitary = 0;//军事
            int countDecorative = 0;
            countEconomic = m_cityBuildingProxy.CountBuildableBuild(out countMilitary);
            view.m_UI_Item_BuildCityBtn1.m_UI_Common_Redpoint.ShowRedPoint(countEconomic);
            view.m_UI_Item_BuildCityBtn2.m_UI_Common_Redpoint.ShowRedPoint(countMilitary);
        }


        private void ShowItem()
        {

            int count = 0;
            int i = 0;
            m_timer = null;
            Transform parent = view.m_sv_list_view_ListView.transform.Find("n/v/c");
            if (parent != null)
            {
                count = parent.childCount;
                Debug.Log(count);
                if (m_buildingIndex == 0)
                {
                    m_timer = Timer.Register(duration, () =>
                    {
                        ItemBuildView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBuildView>(parent.GetChild(i).gameObject);
                        itemView.m_pl_rect_Animator.Play("Show");
                        i++;
                        if (i == parent.childCount)
                        {
                            if (m_timer != null)
                            {
                                m_timer.Cancel();
                                m_timer = null;
                            }
                        }


                    }, null, true, true, view.m_sv_list_view_ListView);
                }
                else
                {
                    m_timer = Timer.Register(duration, () =>
                    {
                        ListView.ListItem ListItem = view.m_sv_list_view_ListView.GetItemByIndex(m_buildingIndex);
                        if (ListItem != null)
                        {
                            ItemBuildView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBuildView>(ListItem.go);
                            itemView.m_pl_rect_Animator.Play("Show");

                        }
                        i++;
                        if (i == 6)
                        {
                            if (m_timer != null)
                            {
                                m_timer.Cancel();
                                m_timer = null;
                            }
                        }


                    }, null, true, true, view.m_sv_list_view_ListView);
            }
             
                //for (int i = 0; i < 6; i++)
                //{
                //    ListView.ListItem ListItem = view.m_sv_list_view_ListView.GetItemByIndex(m_buildingIndex);
                //}
            }
        }
        #region 点击事件
        private void OnEconomicBtnClick()
        {
            if (m_assetsReady)
            {
                if (m_buildingGroupType != EnumBuildingGroupType.Economic)
                {
                    ShowEconomic();
                }
            }
        }
        private void ShowEconomic()
        {
            ShowBuildingGroupTypeList(EnumBuildingGroupType.Economic);
            m_curBuildList = m_jingjiBuildList;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_view_ListView.FillContent(m_curBuildList.Count);
        }
        private void OnMilitaryBtnClick()
        {
            if (m_assetsReady)
            {
                if (m_buildingGroupType != EnumBuildingGroupType.Military)
                {
                    ShowMilitary();
                }
            }
        }
        private void ShowMilitary()
        {
            ShowBuildingGroupTypeList(EnumBuildingGroupType.Military);
            m_curBuildList = m_junshiBuildList;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_view_ListView.FillContent(m_curBuildList.Count);
        }
        private void OnDecorativeBtnClick()
        {
            if (m_assetsReady)
            {
                if (m_buildingGroupType != EnumBuildingGroupType.Decorative)
                {
                    ShowDecorative();
                }

            }
        }
        private void ShowDecorative()
        {
            ShowBuildingGroupTypeList(EnumBuildingGroupType.Decorative);
            m_curBuildList = m_zhuangshiBuildList;
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_list_view_ListView.FillContent(m_curBuildList.Count);
        }
        #endregion

        void ItemEnter(ListView.ListItem scrollItem)
        {
            ItemBuildView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBuildView>(scrollItem.go);
            scrollItem.data = scrollItem.index;
            var buildingType = m_curBuildList[scrollItem.index];
            if (!scrollItem.isInit)
            {
                scrollItem.isInit = true;

            }
            else
            {
                if (m_timer != null)
                {
                    m_timer.Cancel();
                    m_timer = null;
                }
                itemView.m_pl_rect_Animator.enabled = false;
                itemView.m_pl_rect_Animator.transform.localScale = new Vector3(1, 1, 1);
            }
            if (buildingType != null)
            {
                int ChildCount = itemView.m_pl_cost_GridLayoutGroup.transform.childCount;
                itemView.m_pl_icon.DestroyAllChild();

                BuildingLevelDataDefine buildingLevelDataDefine = m_cityBuildingProxy.BuildingLevelDataBylevel(buildingType.type, 1);
                GameObject buildObj = null;
                if (m_assetDic.ContainsKey(m_cityBuildingProxy.GetImgIdByType(buildingType.type)))
                {
                     buildObj = CoreUtils.assetService.Instantiate(m_assetDic[m_cityBuildingProxy.GetImgIdByType(buildingType.type)]);
               
                }
                else
                {
                    buildObj = new GameObject();
                }
                buildObj.transform.SetParent(itemView.m_pl_icon);
                buildObj.transform.localScale = new Vector3(1, 1, 1);
                buildObj.transform.localPosition = new Vector3(0, 0, 0);
                int objCount = itemView.m_pl_cost_GridLayoutGroup.transform.childCount;
                for (int i = objCount - 1; i >= 0; i--)
                {
                    itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(false);
                }
                GameObject temp = itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(0).gameObject;
                int count = 0;

                {
                    if (buildingLevelDataDefine.food > 0)
                    {
                        GameObject gameObject = null;
                        if (count < objCount)
                        {
                            gameObject = itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(count).gameObject;
                        }
                        else
                        {
                            gameObject = CoreUtils.assetService.Instantiate(temp);
                        }
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.SetActive(true);
                        gameObject.name = EnumCurrencyType.food.ToString();
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        UI_Model_ResourcesConsumeInBCB_SubView ResourcesConsumeView = new UI_Model_ResourcesConsumeInBCB_SubView(gameObject.GetComponent<RectTransform>());
                        ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.food), buildingLevelDataDefine.food, (buildingLevelDataDefine.food <= m_playerProxy.CurrentRoleInfo.food));
                        count++;
                    }
                }
                {
                    if (buildingLevelDataDefine.wood > 0)
                    {
                        GameObject gameObject = null;
                        if (count < objCount)
                        {
                            gameObject = itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(count).gameObject;
                        }
                        else
                        {
                            gameObject = CoreUtils.assetService.Instantiate(temp);
                        }
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.SetActive(true);
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.name = EnumCurrencyType.wood.ToString();
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        UI_Model_ResourcesConsumeInBCB_SubView ResourcesConsumeView = new UI_Model_ResourcesConsumeInBCB_SubView(gameObject.GetComponent<RectTransform>());
                        ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.wood), buildingLevelDataDefine.wood, (buildingLevelDataDefine.wood <= m_playerProxy.CurrentRoleInfo.wood));
                        count++;
                    }
                }
                {
                    if (buildingLevelDataDefine.stone > 0)
                    {
                        GameObject gameObject = null;
                        if (count < objCount)
                        {
                            gameObject = itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(count).gameObject;
                        }
                        else
                        {
                            gameObject = CoreUtils.assetService.Instantiate(temp);
                        }
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.SetActive(true);
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.name = EnumCurrencyType.stone.ToString();
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        UI_Model_ResourcesConsumeInBCB_SubView ResourcesConsumeView = new UI_Model_ResourcesConsumeInBCB_SubView(gameObject.GetComponent<RectTransform>());
                        ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.stone), buildingLevelDataDefine.stone, (buildingLevelDataDefine.stone <= m_playerProxy.CurrentRoleInfo.stone));
                        count++;
                    }
                }
                {
                    if (buildingLevelDataDefine.coin > 0)
                    {
                        GameObject gameObject = null;
                        if (count < objCount)
                        {
                            gameObject = itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(count).gameObject;
                        }
                        else
                        {
                            gameObject = CoreUtils.assetService.Instantiate(temp);
                        }
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.SetActive(true);
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.name = EnumCurrencyType.gold.ToString();
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        UI_Model_ResourcesConsumeInBCB_SubView ResourcesConsumeView = new UI_Model_ResourcesConsumeInBCB_SubView(gameObject.GetComponent<RectTransform>());
                        ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.gold), buildingLevelDataDefine.coin, (buildingLevelDataDefine.coin <= m_playerProxy.CurrentRoleInfo.gold));
                        count++;
                    }
                }

                {
                    if (buildingLevelDataDefine.denar > 0)
                    {
                        GameObject gameObject = null;
                        if (count < objCount)
                        {
                            gameObject = itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(count).gameObject;
                        }
                        else
                        {
                            gameObject = CoreUtils.assetService.Instantiate(temp);
                        }
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.SetActive(true);
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.name = EnumCurrencyType.denar.ToString();
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        UI_Model_ResourcesConsumeInBCB_SubView ResourcesConsumeView = new UI_Model_ResourcesConsumeInBCB_SubView(gameObject.GetComponent<RectTransform>());
                        ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.denar), buildingLevelDataDefine.denar, (buildingLevelDataDefine.denar <= m_playerProxy.CurrentRoleInfo.denar));
                        count++;

                    }
                }
                {
                    if (buildingLevelDataDefine.itemType1 > 0)
                    {
                        GameObject gameObject = null;
                        if (count < objCount)
                        {
                            gameObject = itemView.m_pl_cost_GridLayoutGroup.transform.GetChild(count).gameObject;
                        }
                        else
                        {
                            gameObject = CoreUtils.assetService.Instantiate(temp);
                        }
                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.SetActive(true);

                        gameObject.transform.SetParent(itemView.m_pl_cost_GridLayoutGroup.transform);
                        gameObject.name = EnumCurrencyType.denar.ToString();
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(buildingLevelDataDefine.itemType1);
                        UI_Model_ResourcesConsumeInBCB_SubView ResourcesConsumeView = new UI_Model_ResourcesConsumeInBCB_SubView(gameObject.GetComponent<RectTransform>());
                        ResourcesConsumeView.SetResourcesConsume(itemDefine.itemIcon, buildingLevelDataDefine.itemCnt, (buildingLevelDataDefine.itemCnt <= m_bagProxy.GetItemNum(buildingLevelDataDefine.itemType1)));
                        count++;
                    }

                }
                BuildCityType buildCityType = m_cityBuildingProxy.GetBuildCityType(buildingType.type);
                {
                    switch (buildCityType)
                    {
                        case BuildCityType.Buildable:
                            {
                                itemView.m_pl_claim.gameObject.SetActive(true);
                                itemView.m_img_bgLight_PolygonImage.color = new Color(0.75f, 1f, 1f);
                                itemView.m_pl_lock_PolygonImage.gameObject.SetActive(false);

                                itemView.m_lbl_name_LanguageText.color = new Color(0.1490196f, 0.3254902f, 0.454902f);
                                itemView.m_lbl_desc_LanguageText.color = new Color(0.1490196f, 0.3254902f, 0.454902f);
                                PolygonImage image = buildObj.GetComponentInChildren<PolygonImage>();
                                if(image!=null)
                                {
                                    image.material = buildObj.GetComponentInChildren<PolygonImage>().defaultMaterial;
                                }
                                itemView.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(181104, m_cityBuildingProxy.GetBuildCount(buildingType.type), m_cityBuildingProxy.GetBuildCountLimit(buildingType.type));
                                itemView.m_lbl_time_LanguageText.text = ClientUtils.FormatTimeUpgrad(buildingLevelDataDefine.buildingTime);

                                itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                                itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                                {
                                    itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                                    CoreUtils.uiManager.CloseUI(UI.s_buildCity);
                                    if (m_cityBuildingProxy.IsCountLimitMax(buildingType.type))
                                    {
                                        Debug.LogWarning("最大创建数量");
                                        // Tip.CreateTip("（缺）最大创建数量").Show();
                                    }
                                    else
                                    {
                                        if (buildingLevelDataDefine.itemCnt <= m_bagProxy.GetItemNum(buildingLevelDataDefine.itemType1))
                                        {

                                            AppFacade.GetInstance().SendNotification(CmdConstant.CreateTempBuild, buildingType);
                                        }
                                        else
                                        {
                                            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(buildingLevelDataDefine.itemType1);
                                            Tip.CreateTip(LanguageUtils.getTextFormat(200010, LanguageUtils.getText(itemDefine.l_nameID))).Show();
                                        }
                                    }
                            
                                });
                            }
                            break;
                        case BuildCityType.BuildMax:
                            {
                                itemView.m_img_bgLight_PolygonImage.color = new Color(0.1f, 0.6f, 0.8f);
                                itemView.m_pl_claim.gameObject.SetActive(false);
                                itemView.m_pl_lock_PolygonImage.gameObject.SetActive(true);

                                itemView.m_lbl_name_LanguageText.color = Color.white;
                                itemView.m_lbl_desc_LanguageText.color = Color.white;
                                PolygonImage[] images = buildObj.GetComponentsInChildren<PolygonImage>();
                                foreach (var image in images)
                                {
                                    image.material = gray_material;
                                }
                                PolygonImageMask imageMask = buildObj.GetComponentInChildren<PolygonImageMask>();
                                if (imageMask != null)
                                {
                                    imageMask.SetMaterial(gray_mask_material);
                                }
                                itemView.m_lbl_lock_LanguageText.text = LanguageUtils.getText(180500);
                                itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();

                            }
                            break;
                        case BuildCityType.Notunlock:
                            {
                                itemView.m_img_bgLight_PolygonImage.color = new Color(0.1f, 0.6f, 0.8f);
                                itemView.m_pl_claim.gameObject.SetActive(false);
                                itemView.m_pl_lock_PolygonImage.gameObject.SetActive(true);

                                itemView.m_lbl_name_LanguageText.color = Color.white;
                                itemView.m_lbl_desc_LanguageText.color = Color.white;
                                PolygonImage[] images = buildObj.GetComponentsInChildren<PolygonImage>();
                                foreach (var image in images)
                                {
                                    image.material = gray_material;
                                }
                                PolygonImageMask imageMask = buildObj.GetComponentInChildren<PolygonImageMask>();
                                if (imageMask != null)
                                {
                                    imageMask.SetMaterial(gray_mask_material);
                                }
                                itemView.m_lbl_lock_LanguageText.text = LanguageUtils.getTextFormat(180501, m_cityBuildingProxy.GetBuildCountNext(buildingType.type));
                                itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                            }
                            break;
                    }
                }
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(buildingType.l_nameId);
                itemView.m_lbl_desc_LanguageText.text = LanguageUtils.getText(buildingType.l_descId2);

            }
        }

        void ShowBuildingGroupTypeList(EnumBuildingGroupType buildingGroupType)
        {
            m_buildingGroupType = buildingGroupType;
            switch (buildingGroupType)
            {
                case EnumBuildingGroupType.Economic:
                    view.m_UI_Item_BuildCityBtn1.m_img_iconOff_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Item_BuildCityBtn1.m_img_iconOn_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn2.m_img_iconOff_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn2.m_img_iconOn_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Item_BuildCityBtn3.m_img_iconOff_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn3.m_img_iconOn_PolygonImage.gameObject.SetActive(false);
                    break;
                case EnumBuildingGroupType.Military:
                    view.m_UI_Item_BuildCityBtn1.m_img_iconOff_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn1.m_img_iconOn_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Item_BuildCityBtn2.m_img_iconOff_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Item_BuildCityBtn2.m_img_iconOn_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn3.m_img_iconOff_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn3.m_img_iconOn_PolygonImage.gameObject.SetActive(false);
                    break;
                case EnumBuildingGroupType.Decorative:
                    view.m_UI_Item_BuildCityBtn1.m_img_iconOff_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn1.m_img_iconOn_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Item_BuildCityBtn2.m_img_iconOff_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Item_BuildCityBtn2.m_img_iconOn_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Item_BuildCityBtn3.m_img_iconOff_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Item_BuildCityBtn3.m_img_iconOn_PolygonImage.gameObject.SetActive(true);
                    break;
                default:
                    Debug.Log("not find type");
                    break;

            }
        }
    }
}