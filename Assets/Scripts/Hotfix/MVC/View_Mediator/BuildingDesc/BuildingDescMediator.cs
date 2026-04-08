// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    BuildingDescMediator 建筑属性
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
    public class BuildingDescAttr
    {
        public string Name;
        public string Content;
    }
    public class BuildingDescMediator : GameMediator {
        #region Member
        public static string NameMediator = "BuildingDescMediator";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_buildingProxy;
        private SoldierProxy m_soldierProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;

        private int m_showType;
        private int m_buildingLevel;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private Int64 m_buildingIndex;
        private BuildingInfoEntity m_buildingInfo;

        private BuildingTypeConfigDefine m_buildingTypeDefine;

        private List<BuildingDescAttr> m_buildingDescAttrList = new List<BuildingDescAttr>();

        private int m_armyType = 0;
        private List<ArmsDefine> m_armyList = new List<ArmsDefine>();
        private Dictionary<Int64, Int64> m_armyNumData = new Dictionary<Int64, Int64>();

        private int m_unlockMaxIndex = 0;   //已解锁最大index

        //建筑等级列表
        private List<string> m_titleList = new List<string>();
        private List<List<string>> m_contentList = new List<List<string>>();
        private int m_levelItemLoadStatus = 0; //1正在加载 2已加载 
        private bool m_isCreateLevelItem;
        private string m_titleItemName;
        private string m_contentPrefabName;
        private bool m_isHideLevelDetail = false;

        //军队信息
        private GameObject m_itemBuildingDescSoiler;
        private GameObject m_itemBuildingDataGameObjet;

        private bool m_isDispose;

        private string m_imgPrefabName;

        #endregion

        //IMediatorPlug needs
        public BuildingDescMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public BuildingDescView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.InSoldierInfoChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.InSoldierInfoChange:
                    RefreshSoldierData();
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

        public override void OnRemove()
        {
            m_isDispose = true;
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }

        protected override void InitData()
        {
            if (view.data == null)
            {
                Debug.LogError("buildingIndex is null");
                return;
            }
            m_buildingIndex = (Int64)view.data;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;

            m_buildingInfo = m_buildingProxy.GetBuildingInfoByindex(m_buildingIndex);

            m_buildingLevel = (int)m_buildingInfo.level;
            m_buildingLevel = m_buildingLevel < 1 ? 1 : m_buildingLevel;

            //测试数据
            //m_buildingInfo = new BuildingInfoEntity();
            //m_buildingInfo.type = (int)EnumCityBuildingType.ScoutCamp;
            //m_buildingInfo.level = 1;

            m_buildingTypeDefine = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>((int)m_buildingInfo.type);
            if (m_buildingTypeDefine == null)
            {
                Debug.LogError(string.Format("BuildingTypeConfig not find id: {0}", m_buildingInfo.type));
                return;
            }
            if (m_buildingInfo.type == (int)EnumCityBuildingType.BuilderHut || m_buildingInfo.type == (int)EnumCityBuildingType.Market ||
                m_buildingInfo.type == (int)EnumCityBuildingType.CourierStation || m_buildingInfo.type == (int)EnumCityBuildingType.BulletinBoard ||
                m_buildingInfo.type == (int)EnumCityBuildingType.Monument || m_buildingInfo.type == (int)EnumCityBuildingType.Smithy ||
                m_buildingInfo.type == (int)EnumCityBuildingType.Road || m_buildingInfo.type == (int)EnumCityBuildingType.tree ||
                m_buildingInfo.type == (int)EnumCityBuildingType.tree2 || m_buildingInfo.type == (int)EnumCityBuildingType.tree3 || m_buildingInfo.type == (int)EnumCityBuildingType.tree4) 
            {
                view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_buildingTypeDefine.l_nameId);
            }
            else
            {
                view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(300015, m_buildingLevel, LanguageUtils.getText(m_buildingTypeDefine.l_nameId));
            }

            string strId = string.Format("{0}{1}{2}", m_buildingInfo.type, m_playerProxy.GetCivilization(), (int)m_buildingProxy.GetAgeType());
            int id = int.Parse(strId);
            BuildingModelDataDefine modelDefine = CoreUtils.dataService.QueryRecord<BuildingModelDataDefine>(id);
            if (modelDefine == null)
            {
                Debug.LogError(string.Format("BuildingModelData not find: {0}", id));
                return;
            }
            m_imgPrefabName = modelDefine.imgRes;
            view.m_img_buildImg_PolygonImage.gameObject.SetActive(false);
            //ClientUtils.LoadSprite(view.m_img_buildImg_PolygonImage, modelDefine.imgRes, false, ()=> {
            //    view.m_img_buildImg_PolygonImage.gameObject.SetActive(true);
            //});

            //设置显示类型
            SetShowType();

            //属性数据获取
            AttrDataProcess();

            view.gameObject.SetActive(false);

            //预加载预设
            List<string> prefabNames = new List<string>();
            if (m_showType == 2)
            {
                prefabNames.Add("UI_Item_BuildingData");
            }
            else if (m_showType == 3)
            {
                prefabNames.Add("UI_Item_BuildingData");
                prefabNames.Add("UI_Item_BuildingDescSoiler");
            }
            //prefabNames.Add(modelDefine.imgRes);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(() => {
                CoreUtils.uiManager.CloseUI(UI.s_buildingDesc);
            });
            view.m_btn_BuildLevelInfo_GameButton.onClick.AddListener(OnBuildLevelInfo);

            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.onClick.AddListener(OnBack);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var element in dic)
            {
                m_assetDic[element.Key] = element.Value.asset() as GameObject;
            }

            view.gameObject.SetActive(true);

            Refresh();
        }

        private void Refresh()
        {
            //GameObject obj = GameObject.Instantiate(m_assetDic[m_imgPrefabName], view.m_img_buildImg_PolygonImage.transform);
            CoreUtils.assetService.Instantiate(m_imgPrefabName, (obj)=> {
                if (obj == null)
                {
                    return;
                }
                if (m_isDispose)
                {
                    return;
                }
                obj.transform.SetParent(view.m_img_buildImg_PolygonImage.transform);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
                view.m_img_buildImg_PolygonImage.gameObject.SetActive(true);
            });

            view.m_btn_BuildLevelInfo_GameButton.gameObject.SetActive(!m_isHideLevelDetail);

            if (m_showType == 1)
            {
                Refresh1();
            }
            else if (m_showType == 2)
            {
                Refresh2();
            }
            else if (m_showType == 3)
            {
                Refresh3();
            }
        }

        private void Refresh1()
        {
            view.m_sv_center_text_ListView.gameObject.SetActive(true);
            view.m_lbl_center_buildingDesc_LanguageText.text = LanguageUtils.getText(m_buildingTypeDefine.l_descId);
        }

        private void Refresh2()
        {
            view.m_sv_bottom_text2_ListView.SetContainerLayout();

            view.m_pl_attr_PolygonImage.gameObject.SetActive(true);
            int count = m_buildingDescAttrList.Count;
            RectTransform rect = view.m_pl_attr_PolygonImage.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y / 3 * count);
            for (int i = 0; i < count; i++)
            {
                GameObject obj = GameObject.Instantiate(m_assetDic["UI_Item_BuildingData"], view.m_pl_attr_PolygonImage.transform);
                obj.transform.localScale = Vector3.one;
                BuildingDataView itemView = MonoHelper.AddHotFixViewComponent<BuildingDataView>(obj);
                itemView.m_lbl_name_LanguageText.text = m_buildingDescAttrList[i].Name;
                itemView.m_lbl_val_LanguageText.text = m_buildingDescAttrList[i].Content;
            }

            view.m_sv_bottom_text2_ListView.gameObject.SetActive(true);
            view.m_lbl_bottom_buildingDesc2_LanguageText.text = LanguageUtils.getText(m_buildingTypeDefine.l_descId);
            view.m_c_bottom_text2.sizeDelta = new Vector2(view.m_c_bottom_text2.sizeDelta.x, view.m_lbl_bottom_buildingDesc2_LanguageText.preferredHeight);

            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_pl_Right_VerticalLayoutGroup.GetComponent<RectTransform>());
        }

        private void Refresh3()
        {
            view.m_sv_bottom_text_ListView.SetContainerLayout();

            view.m_pl_solider_PolygonImage.gameObject.SetActive(true);
            GameObject obj1 = GameObject.Instantiate(m_assetDic["UI_Item_BuildingDescSoiler"], view.m_pl_solider_PolygonImage.transform);
            m_itemBuildingDescSoiler = obj1;
            obj1.transform.localScale = Vector3.one;
            ItemBuildingDescSoilerView itemView1 = MonoHelper.AddHotFixViewComponent<ItemBuildingDescSoilerView>(obj1);
            List<UI_Item_ArmyInBuildingDesc_SubView> headViewList = new List<UI_Item_ArmyInBuildingDesc_SubView>();
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc1);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc2);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc3);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc4);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc5);

            for (int i = 0; i < headViewList.Count; i++)
            {
                headViewList[i].SetIndex(i);
                headViewList[i].SetHead(SoldierProxy.GetArmyHeadIcon(m_armyList[i].ID));
                int key = i + 1;
                if (m_armyNumData.ContainsKey(key))
                {
                    headViewList[i].SetNum(ClientUtils.FormatComma(m_armyNumData[key]));
                    headViewList[i].SetDissMissBtnShow(!(m_armyNumData[key] < 1));
                }
                else
                {
                    headViewList[i].SetNum("0");
                    headViewList[i].SetDissMissBtnShow(false);
                }
                //headViewList[i].SetDissMissBtnShow(false);
                headViewList[i].SetGray(i>m_unlockMaxIndex);
                headViewList[i].AddDissMissListener(ClickDissMiss);
            }

            view.m_pl_attr_PolygonImage.gameObject.SetActive(true);
            int count = m_buildingDescAttrList.Count;
            RectTransform rect = view.m_pl_attr_PolygonImage.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y / 3 * count);
            for (int i = 0; i < count; i++)
            {
                GameObject obj = GameObject.Instantiate(m_assetDic["UI_Item_BuildingData"], view.m_pl_attr_PolygonImage.transform);
                obj.transform.localScale = Vector3.one;
                BuildingDataView itemView = MonoHelper.AddHotFixViewComponent<BuildingDataView>(obj);
                itemView.m_lbl_name_LanguageText.text = m_buildingDescAttrList[i].Name;
                itemView.m_lbl_val_LanguageText.text = m_buildingDescAttrList[i].Content;
                if (i == (count - 1))
                {
                    m_itemBuildingDataGameObjet = obj;
                }
            }
            //view.m_pl_attr_PolygonImage.transform.localPosition = view.m_pl_attr_pos2.localPosition;

            view.m_sv_bottom_text_ListView.gameObject.SetActive(true);
            view.m_lbl_bottom_buildingDesc_LanguageText.text = LanguageUtils.getText(m_buildingTypeDefine.l_descId);
            view.m_c_bottom_text.sizeDelta = new Vector2(view.m_c_bottom_text.sizeDelta.x, view.m_lbl_bottom_buildingDesc_LanguageText.preferredHeight);

            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_pl_Right_VerticalLayoutGroup.GetComponent<RectTransform>());
        }

        //刷新士兵信息
        private void RefreshSoldierData()
        {
            m_armyNumData = m_soldierProxy.GetInSoldierByType(m_armyType);

            //Int64 totalPower = 0;
            //foreach (var data in m_armyNumData)
            //{
            //    totalPower = totalPower + m_armyList[(int)data.Key - 1].militaryCapability * data.Value;
            //}
            //BuildingDescAttr attr4 = new BuildingDescAttr();
            //attr4.Name = LanguageUtils.getText(180314);
            //attr4.Content = ClientUtils.FormatComma(totalPower);
            //m_buildingDescAttrList[m_buildingDescAttrList.Count-1]=attr4;

            RefreshSoldierHead();
        }

        //刷新士兵头像显示
        private void RefreshSoldierHead()
        {
            if (m_itemBuildingDescSoiler == null)
            {
                return;
            }
            ItemBuildingDescSoilerView itemView1 = MonoHelper.GetOrAddHotFixViewComponent<ItemBuildingDescSoilerView>(m_itemBuildingDescSoiler);
            List<UI_Item_ArmyInBuildingDesc_SubView> headViewList = new List<UI_Item_ArmyInBuildingDesc_SubView>();
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc1);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc2);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc3);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc4);
            headViewList.Add(itemView1.m_UI_Item_ArmyInBuildingDesc5);

            for (int i = 0; i < headViewList.Count; i++)
            {
                int key = i + 1;
                if (m_armyNumData.ContainsKey(key))
                {
                    headViewList[i].SetNum(ClientUtils.FormatComma(m_armyNumData[key]));
                    headViewList[i].SetDissMissBtnShow(!(m_armyNumData[key] < 1));
                }
                else
                {
                    headViewList[i].SetNum("0");
                    headViewList[i].SetDissMissBtnShow(false);
                }
            }
            int index = m_buildingDescAttrList.Count - 1;
            BuildingDataView itemView = MonoHelper.AddHotFixViewComponent<BuildingDataView>(m_itemBuildingDataGameObjet);
            itemView.m_lbl_name_LanguageText.text = m_buildingDescAttrList[index].Name;
            itemView.m_lbl_val_LanguageText.text = m_buildingDescAttrList[index].Content;
        }

        //获取战力
        private Int64 GetPower(int buildType, int buildLevel)
        {
            string str = (buildLevel < 10) ? string.Format("0{0}", buildLevel) : buildLevel.ToString();
            str = string.Format("{0}{1}", buildType, str);
            int id = int.Parse(str);
            BuildingLevelDataDefine define = CoreUtils.dataService.QueryRecord<BuildingLevelDataDefine>(id);
            if (define != null)
            {
                return define.power;
            }
            return 0;
        }

        private void SetShowType()
        {
            Debug.LogFormat("type:{0}", m_buildingInfo.type);
            m_showType = 2;
            switch (m_buildingInfo.type)
            {
                case (int)EnumCityBuildingType.TownCenter:
                    break;
                case (int)EnumCityBuildingType.Farm:
                    break;
                case (int)EnumCityBuildingType.Sawmill:
                    break;
                case (int)EnumCityBuildingType.Quarry:
                    break;
                case (int)EnumCityBuildingType.SilverMine:
                    break;
                case (int)EnumCityBuildingType.CityWall:                
                    break;
                case (int)EnumCityBuildingType.BuilderHut:
                    m_isHideLevelDetail = true;
                    break;
                case (int)EnumCityBuildingType.GuardTower:
                    break;
                case (int)EnumCityBuildingType.Barracks:
                    m_showType = 3;
                    break;
                case (int)EnumCityBuildingType.Stable:
                    m_showType = 3;
                    break;
                case (int)EnumCityBuildingType.ArcheryRange:
                    m_showType = 3;
                    break;
                case (int)EnumCityBuildingType.SiegeWorkshop:
                    m_showType = 3;
                    break;
                case (int)EnumCityBuildingType.Academy:
                    break;
                case (int)EnumCityBuildingType.Hospital:
                    break;
                case (int)EnumCityBuildingType.Storage:
                    break;
                case (int)EnumCityBuildingType.AllianceCenter:
                    break;
                case (int)EnumCityBuildingType.Castel:
                    break;
                case (int)EnumCityBuildingType.Tavern:
                    break;
                case (int)EnumCityBuildingType.TradingPost:
                    break;
                case (int)EnumCityBuildingType.Market:
                    m_showType = 1;
                    m_isHideLevelDetail = true;
                    break;
                case (int)EnumCityBuildingType.CourierStation:
                    m_showType = 1;
                    m_isHideLevelDetail = true;
                    break;
                case (int)EnumCityBuildingType.ScoutCamp:
                    break;
                case (int)EnumCityBuildingType.BulletinBoard:
                    m_showType = 1;
                    m_isHideLevelDetail = true;
                    break;
                case (int)EnumCityBuildingType.Monument:
                    m_showType = 1;
                    m_isHideLevelDetail = true;
                    break;
                case (int)EnumCityBuildingType.Smithy:
                    m_showType = 1;
                    m_isHideLevelDetail = true;
                    break;
                case (int)EnumCityBuildingType.Road:
                    m_showType = 1;
                    m_isHideLevelDetail = true;
                    break;
                case (int)EnumCityBuildingType.tree:
                case (int)EnumCityBuildingType.tree2:
                case (int)EnumCityBuildingType.tree3:
                case (int)EnumCityBuildingType.tree4:
                    m_showType = 1;
                    m_isHideLevelDetail = true;
                    break;
                default:
                    Debug.LogErrorFormat("@策划 未知类型建筑：{0}", m_buildingInfo.type);
                    break;
            }
        }

        private int GetResId(int buildType, int buildLevel)
        {
            string idStr = (buildLevel < 10) ? string.Format("{0}0{1}", buildType, buildLevel) : string.Format("{0}{1}", buildType, buildLevel);
            return int.Parse(idStr);
        }

        //属性数据
        private void AttrDataProcess()
        {
            int buildType = (int)m_buildingInfo.type;
            int buildLevel = m_buildingLevel;
            string powerStr = LanguageUtils.getText(180314);
            if (m_showType == 2)
            {
                if (buildType == (int)EnumCityBuildingType.TownCenter) //市政大厅
                {
                    BuildingTownCenterDefine define1 = CoreUtils.dataService.QueryRecord<BuildingTownCenterDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180312);
                        attr1.Content = ClientUtils.FormatComma(define1.troopsCapacity);
                        m_buildingDescAttrList.Add(attr1);
                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180313);
                        attr2.Content = define1.troopsDispatchNumber.ToString();
                        m_buildingDescAttrList.Add(attr2);
                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.CityWall)//城墙
                {
                    BuildingCityWallDefine define2 = CoreUtils.dataService.QueryRecord<BuildingCityWallDefine>(buildLevel);
                    if (define2 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180324);
                        attr1.Content = ClientUtils.FormatComma(define2.wallDurableMax);
                        m_buildingDescAttrList.Add(attr1);
                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = powerStr;
                        attr2.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr2);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Farm)//农场
                {
                    int id = GetResId(buildType, buildLevel);
                    BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                    if (define1 != null)
                    {
                        float foodCapacityMulti = m_playerAttributeProxy.GetCityAttribute(attrType.foodCapacityMulti).value;

                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180316);
                        if (foodCapacityMulti > 0)
                        {
                            attr1.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.produceSpeed), ClientUtils.FormatComma((int)Mathf.Floor(define1.produceSpeed * foodCapacityMulti)));
                        }
                        else
                        {
                            attr1.Content = ClientUtils.FormatComma(define1.produceSpeed);
                        }
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180317);
                        if (foodCapacityMulti > 0)
                        {
                            attr2.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.gatherMax), ClientUtils.FormatComma((int)Mathf.Floor(define1.gatherMax * foodCapacityMulti)));
                        }
                        else
                        {
                            attr2.Content = ClientUtils.FormatComma(define1.gatherMax);
                        }                       
                        m_buildingDescAttrList.Add(attr2);

                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Sawmill)//木材厂
                {
                    int id = GetResId(buildType, buildLevel);
                    BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                    if (define1 != null)
                    {
                        float woodCapacityMulti = m_playerAttributeProxy.GetCityAttribute(attrType.woodCapacityMulti).value;
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180318);
                        if (woodCapacityMulti > 0)
                        {
                            attr1.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.produceSpeed), ClientUtils.FormatComma((int)Mathf.Floor(define1.produceSpeed * woodCapacityMulti)));
                        }
                        else
                        {
                            attr1.Content = ClientUtils.FormatComma(define1.produceSpeed);
                        }
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180319);
                        if (woodCapacityMulti > 0)
                        {
                            attr2.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.gatherMax), ClientUtils.FormatComma((int)Mathf.Floor(define1.gatherMax * woodCapacityMulti)));
                        }
                        else
                        {
                            attr2.Content = ClientUtils.FormatComma(define1.gatherMax);
                        }                  
                        m_buildingDescAttrList.Add(attr2);

                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Quarry)//采石场
                {
                    int id = GetResId(buildType, buildLevel);
                    BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                    if (define1 != null)
                    {
                        float stoneCapacityMulti = m_playerAttributeProxy.GetCityAttribute(attrType.stoneCapacityMulti).value;
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180320);
                        if (stoneCapacityMulti > 0)
                        {
                            attr1.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.produceSpeed), ClientUtils.FormatComma((int)Mathf.Floor(define1.produceSpeed * stoneCapacityMulti)));
                        }
                        else
                        {
                            attr1.Content = ClientUtils.FormatComma(define1.produceSpeed);
                        }
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180321);
                        if (stoneCapacityMulti > 0)
                        {
                            attr2.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.gatherMax), ClientUtils.FormatComma((int)Mathf.Floor(define1.gatherMax * stoneCapacityMulti)));
                        }
                        else
                        {
                            attr2.Content = ClientUtils.FormatComma(define1.gatherMax);
                        }
                        m_buildingDescAttrList.Add(attr2);

                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.SilverMine)//金矿
                {
                    int id = GetResId(buildType, buildLevel);
                    BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                    if (define1 != null)
                    {
                        float goldCapacityMulti = m_playerAttributeProxy.GetCityAttribute(attrType.glodCapacityMulti).value;
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180322);
                        if (goldCapacityMulti > 0)
                        {
                            attr1.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.produceSpeed), ClientUtils.FormatComma((int)Mathf.Floor(define1.produceSpeed * goldCapacityMulti)));
                        }
                        else
                        {
                            attr1.Content = ClientUtils.FormatComma(define1.produceSpeed);
                        }                        
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180323);
                        if (goldCapacityMulti > 0)
                        {
                            attr2.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.gatherMax), ClientUtils.FormatComma((int)Mathf.Floor(define1.gatherMax * goldCapacityMulti)));
                        }
                        else
                        {
                            attr2.Content = ClientUtils.FormatComma(define1.gatherMax);
                        }
                        m_buildingDescAttrList.Add(attr2);

                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.BuilderHut) //工人小屋
                {
                    WorkerProxy workerProxy = AppFacade.GetInstance().RetrieveProxy(WorkerProxy.ProxyNAME) as WorkerProxy;
                    BuildingDescAttr attr1 = new BuildingDescAttr();
                    attr1.Name = LanguageUtils.getText(180325);
                    attr1.Content = workerProxy.GetBuildQueueCount().ToString();
                    m_buildingDescAttrList.Add(attr1);
                    BuildingDescAttr attr2 = new BuildingDescAttr();
                    attr2.Name = powerStr;
                    attr2.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                    m_buildingDescAttrList.Add(attr2);
                    BuildingDescAttr attr3 = new BuildingDescAttr();
                    attr3.Name = LanguageUtils.getText(180326);
                    attr3.Content = LanguageUtils.getTextFormat(180358, Mathf.Floor(m_playerAttributeProxy.GetCityAttribute(attrType.buildSpeedMulti).value*100));
                    m_buildingDescAttrList.Add(attr3);
                }
                else if (buildType == (int)EnumCityBuildingType.GuardTower) //警戒塔
                {
                    BuildingGuardTowerDefine define1 = CoreUtils.dataService.QueryRecord<BuildingGuardTowerDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180327);
                        attr1.Content = ClientUtils.FormatComma(define1.warningTowerAttack);
                        m_buildingDescAttrList.Add(attr1);
                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180328);
                        attr2.Content = ClientUtils.FormatComma(define1.warningTowerHpMax);
                        m_buildingDescAttrList.Add(attr2);
                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Academy) //学院
                {
                    BuildingCampusDefine define1 = CoreUtils.dataService.QueryRecord<BuildingCampusDefine>(buildLevel);
                    if (define1 != null)
                    {
                        float researchSpeedMulti = m_playerAttributeProxy.GetCityAttribute(attrType.buildSpeedMulti).value*100;
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180333);
                        if (researchSpeedMulti > 0)
                        {
                            attr1.Content = LanguageUtils.getTextFormat(180356, LanguageUtils.getTextFormat(180357, (float)define1.researchSpeedMulti / 10),
                                                                        LanguageUtils.getTextFormat(180357, researchSpeedMulti));
                        }
                        else
                        {
                            attr1.Content = LanguageUtils.getTextFormat(180357, (float)define1.researchSpeedMulti / 10);
                        }
                        m_buildingDescAttrList.Add(attr1);
                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Hospital) //医院
                {
                    BuildingHospitalDefine define1 = CoreUtils.dataService.QueryRecord<BuildingHospitalDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180334);
                        attr1.Content = ClientUtils.FormatComma(define1.armyCnt);
                        m_buildingDescAttrList.Add(attr1);
                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Storage) //仓库
                {
                    BuildingStorageDefine define1 = CoreUtils.dataService.QueryRecord<BuildingStorageDefine>(buildLevel);
                    if (define1 != null)
                    {
                        float spaceMulti = m_playerAttributeProxy.GetCityAttribute(attrType.resourcesProtectSpaceMulti).value;

                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180336);
                        if (spaceMulti > 0)
                        {
                            attr1.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.foodCnt), ClientUtils.FormatComma((int)Mathf.Floor(define1.foodCnt * spaceMulti)));
                        }
                        else
                        {
                            attr1.Content = ClientUtils.FormatComma(define1.foodCnt);
                        }
                        m_buildingDescAttrList.Add(attr1);
                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180337);
                        if (spaceMulti > 0)
                        {
                            attr2.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.woodCnt), ClientUtils.FormatComma((int)Mathf.Floor(define1.woodCnt * spaceMulti)));
                        }
                        else
                        {
                            attr2.Content = ClientUtils.FormatComma(define1.woodCnt);
                        }                        
                        m_buildingDescAttrList.Add(attr2);
                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = LanguageUtils.getText(180338);
                        if (spaceMulti > 0)
                        {
                            attr3.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.stoneCnt), ClientUtils.FormatComma((int)Mathf.Floor(define1.stoneCnt * spaceMulti)));
                        }
                        else
                        {
                            attr3.Content = ClientUtils.FormatComma(define1.stoneCnt);
                        } 
                        m_buildingDescAttrList.Add(attr3);
                        BuildingDescAttr attr4 = new BuildingDescAttr();
                        attr4.Name = LanguageUtils.getText(180339);
                        if (spaceMulti > 0)
                        {
                            attr4.Content = LanguageUtils.getTextFormat(180356, ClientUtils.FormatComma(define1.goldCnt), ClientUtils.FormatComma((int)Mathf.Floor(define1.goldCnt * spaceMulti)));
                        }
                        else
                        {
                            attr4.Content = ClientUtils.FormatComma(define1.goldCnt);
                        }
                        m_buildingDescAttrList.Add(attr4);
                        BuildingDescAttr attr5 = new BuildingDescAttr();
                        attr5.Name = powerStr;
                        attr5.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr5);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.AllianceCenter) //联盟中心
                {
                    BuildingAllianceCenterDefine define1 = CoreUtils.dataService.QueryRecord<BuildingAllianceCenterDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180340);
                        attr1.Content = ClientUtils.FormatComma(define1.defCapacity);
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180355);
                        attr2.Content = ClientUtils.FormatComma(define1.helpCnt);
                        m_buildingDescAttrList.Add(attr2);

                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Castel) //城堡
                {
                    BuildingCastleDefine define1 = CoreUtils.dataService.QueryRecord<BuildingCastleDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180341);
                        attr1.Content = ClientUtils.FormatComma(define1.massTroopsCapacity);
                        m_buildingDescAttrList.Add(attr1);
                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.Tavern) //酒馆
                {
                    BuildingTavernDefine define1 = CoreUtils.dataService.QueryRecord<BuildingTavernDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180342);
                        attr1.Content = ClientUtils.FormatTime(define1.goldBoxCD);
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180343);
                        attr2.Content = ClientUtils.FormatComma(define1.silverBoxCnt);
                        m_buildingDescAttrList.Add(attr2);

                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.TradingPost) //商栈
                {
                    BuildingFreightDefine define1 = CoreUtils.dataService.QueryRecord<BuildingFreightDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180344);
                        attr1.Content = ClientUtils.FormatComma(define1.capacity);
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180345);
                        attr2.Content = LanguageUtils.getTextFormat(180357, ClientUtils.FormatComma(define1.tax/10));
                        m_buildingDescAttrList.Add(attr2);

                        if (define1.transportSpeedMulti != 0)
                        {
                            BuildingDescAttr attr4 = new BuildingDescAttr();
                            attr4.Name = LanguageUtils.getText(180346);
                            attr4.Content = LanguageUtils.getTextFormat(180357, ClientUtils.FormatComma(define1.transportSpeedMulti / 10));
                            m_buildingDescAttrList.Add(attr4);
                        }
                       
                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = powerStr;
                        attr3.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr3);
                    }
                }
                else if (buildType == (int)EnumCityBuildingType.ScoutCamp) //斥候营地
                {
                    BuildingScoutcampDefine define1 = CoreUtils.dataService.QueryRecord<BuildingScoutcampDefine>(buildLevel);
                    if (define1 != null)
                    {
                        BuildingDescAttr attr1 = new BuildingDescAttr();
                        attr1.Name = LanguageUtils.getText(180347);
                        attr1.Content = LanguageUtils.getTextFormat(180357, define1.scoutSpeedMulti / 10);
                        m_buildingDescAttrList.Add(attr1);

                        BuildingDescAttr attr2 = new BuildingDescAttr();
                        attr2.Name = LanguageUtils.getText(180348);
                        attr2.Content = GetArea(define1.scoutView);
                        m_buildingDescAttrList.Add(attr2);

                        BuildingDescAttr attr3 = new BuildingDescAttr();
                        attr3.Name = LanguageUtils.getText(180349);
                        attr3.Content = define1.scoutNumber.ToString();
                        m_buildingDescAttrList.Add(attr3);

                        BuildingDescAttr attr4 = new BuildingDescAttr();
                        attr4.Name = powerStr;
                        attr4.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                        m_buildingDescAttrList.Add(attr4);
                    }
                }
            }
            else if (m_showType == 3)
            {
                ArmyBuildingDataProcess();
            }
        }

        //军队建筑处理
        private void ArmyBuildingDataProcess()
        {
            int buildType = (int)m_buildingInfo.type;
            int buildLevel = m_buildingLevel;
            string powerStr = LanguageUtils.getText(180314);

            BuildingDescAttr attr1 = new BuildingDescAttr();
            attr1.Name = LanguageUtils.getText(180350);
            string content = "";
            int armyType = 0;
            if (buildType == (int)EnumCityBuildingType.Barracks) // 兵营
            {
                BuildingBarracksDefine define1 = CoreUtils.dataService.QueryRecord<BuildingBarracksDefine>(buildLevel);
                if (define1 != null)
                {
                    content = ClientUtils.FormatComma(m_playerAttributeProxy.GetCityAttribute(attrType.infantryTrainNumber).origvalue);
                }
                armyType = (int)EnumSoldierType.Infantry;
            }
            else if (buildType == (int)EnumCityBuildingType.Stable) // 马厩
            {
                BuildingArcheryrangeDefine define1 = CoreUtils.dataService.QueryRecord<BuildingArcheryrangeDefine>(buildLevel);
                if (define1 != null)
                {
                    content = ClientUtils.FormatComma(m_playerAttributeProxy.GetCityAttribute(attrType.cavalryTrainNumber).origvalue);
                }
                armyType = (int)EnumSoldierType.Cavalry;
            }
            else if (buildType == (int)EnumCityBuildingType.ArcheryRange) // 靶场
            {
                BuildingStableDefine define1 = CoreUtils.dataService.QueryRecord<BuildingStableDefine>(buildLevel);
                if (define1 != null)
                {
                    content = ClientUtils.FormatComma(m_playerAttributeProxy.GetCityAttribute(attrType.bowmenTrainNumber).origvalue);
                }
                armyType = (int)EnumSoldierType.Bowmen;
            }
            else if (buildType == (int)EnumCityBuildingType.SiegeWorkshop) // 攻城武器厂
            {
                BuildingSiegeWorkshopDefine define1 = CoreUtils.dataService.QueryRecord<BuildingSiegeWorkshopDefine>(buildLevel);
                if (define1 != null)
                {
                    content = ClientUtils.FormatComma(m_playerAttributeProxy.GetCityAttribute(attrType.siegeCarTrainNumber).origvalue);
                }
                armyType = (int)EnumSoldierType.SiegeEngines;
            }
            attr1.Content = content;
            m_buildingDescAttrList.Add(attr1);
            m_armyType = armyType;

            BuildingDescAttr attr2 = new BuildingDescAttr();
            attr2.Name = LanguageUtils.getText(180351);
            attr2.Content = LanguageUtils.getTextFormat(180358, (int)Mathf.Floor(m_playerAttributeProxy.GetCityAttribute(attrType.trainSpeedMulti).value *100));
            m_buildingDescAttrList.Add(attr2);

            for (int i = 0; i < 5; i++)
            {
                int id = m_soldierProxy.GetTemplateId(m_armyType, i + 1);
                ArmsDefine define1 = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                m_armyList.Add(define1);
                if(m_soldierProxy.IsUnlock(define1))
                {
                    m_unlockMaxIndex = i;
                }
            }

            m_armyNumData = m_soldierProxy.GetInSoldierByType(armyType);

            //Int64 totalPower = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
            //foreach (var data in m_armyNumData)
            //{
            //    totalPower = totalPower + m_armyList[(int)data.Key - 1].militaryCapability * data.Value;
            //}
            BuildingDescAttr attr4 = new BuildingDescAttr();
            attr4.Name = powerStr;
            attr4.Content = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
            m_buildingDescAttrList.Add(attr4);
        }

        //解雇
        private void ClickDissMiss(int index)
        {
            if (index >= m_armyList.Count)
            {
                return;
            }
            TrainDissolveParam param = new TrainDissolveParam();
            param.Id = m_armyList[index].ID;
            int key = index + 1;
            if (m_armyNumData.ContainsKey(key))
            {
                param.ArmyNum = (int)m_armyNumData[key];
            } else
            {
                param.ArmyNum = 0;
            }
            CoreUtils.uiManager.ShowUI(UI.s_trainDissolve, null, param);
        }

        private void OnBack()
        {
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(false);
            view.m_pl_sv_level_Animator.gameObject.SetActive(false);
            view.m_pl_content_Animator.gameObject.SetActive(true);
            view.m_pl_content_Animator.Play("Show");
        }

        //获取千分比
        private string GetPermillage(int num)
        {
            if (num > 0)
            {
                return string.Format("{0}%", (float)num / 10);
            }
            else
            {
                return "";
            }
        }

        //获取区域
        private string GetArea(int num)
        {
            return LanguageUtils.getTextFormat(200030, num, num);
        }

        #region 建筑各等级详情
        //建筑各等级信息
        private void OnBuildLevelInfo()
        {
            if (m_titleList.Count < 1)
            {
                ReadBuildLevelInfo();
            }

            if (m_levelItemLoadStatus == 1)//正加载
            {
                return;
            }
            if (m_levelItemLoadStatus == 2)//已加载
            {
                ShowLevelInfo(false);
                return;
            }

            m_levelItemLoadStatus = 1;
            int count = m_titleList.Count;
            string titlePrefabName = "";
            string contentPrefabName = "";
            if (count == 3)
            {
                titlePrefabName = "UI_Item_BuildingLevelData_Title_Three";
                contentPrefabName = "UI_Item_BuildingLevelData_Info_Three";
            }
            else if (count == 4)
            {
                titlePrefabName = "UI_Item_BuildingLevelData_Title_Four";
                contentPrefabName = "UI_Item_BuildingLevelData_Info_Four";
            }
            else if (count == 5)
            {
                titlePrefabName = "UI_Item_BuildingLevelData_Title_Five";
                contentPrefabName = "UI_Item_BuildingLevelData_Info_Five";
            }
            else if (count == 6)
            {
                titlePrefabName = "UI_Item_BuildingLevelData_Title_Six";
                contentPrefabName = "UI_Item_BuildingLevelData_Info_Six";
            }
            else
            {
                Debug.LogErrorFormat("异常 count:{0}", count);
            }
            m_titleItemName = titlePrefabName;
            m_contentPrefabName = contentPrefabName;
            //预加载预设
            List<string> prefabNames = new List<string>();
            prefabNames.Add(titlePrefabName);
            prefabNames.AddRange(view.m_sv_levelData_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadLevelItemFinish);
        }

        private void LoadLevelItemFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var element in dic)
            {
                m_assetDic[element.Key] = element.Value.asset() as GameObject;
            }
            m_levelItemLoadStatus = 2;

            ShowLevelInfo(true);
        }

        private void ShowLevelInfo(bool isFirst)
        {
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_pl_content_Animator.gameObject.SetActive(false);
            view.m_pl_sv_level_Animator.gameObject.SetActive(true);
            view.m_pl_sv_level_Animator.Play("Show");
            if (m_isCreateLevelItem)
            {
                return;
            }
            m_isCreateLevelItem = true;
            GameObject obj = GameObject.Instantiate(m_assetDic[m_titleItemName], view.m_pl_tilte.transform);
            obj.transform.localScale = Vector3.one;
            ItemBuildingLevelDataTitleSixView titleView = MonoHelper.AddHotFixViewComponent<ItemBuildingLevelDataTitleSixView>(obj);
            List<LanguageText> titleTextList = new List<LanguageText>();
            titleTextList.Add(titleView.m_lbl_name1_LanguageText);
            titleTextList.Add(titleView.m_lbl_name2_LanguageText);
            titleTextList.Add(titleView.m_lbl_name3_LanguageText);
            titleTextList.Add(titleView.m_lbl_name4_LanguageText);
            titleTextList.Add(titleView.m_lbl_name5_LanguageText);
            titleTextList.Add(titleView.m_lbl_name6_LanguageText);
            for (int i = 0; i < m_titleList.Count; i++)
            {
                titleTextList[i].text = m_titleList[i];
            }

            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            functab.GetItemPrefabName = OnGetItemPrefabName;
            view.m_sv_levelData_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_levelData_ListView.FillContent(m_contentList.Count);
        }

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            return m_contentPrefabName;
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            ItemBuildingLevelDataSixView nodeView = null;
            if (!listItem.isInit)
            {
                nodeView = MonoHelper.AddHotFixViewComponent<ItemBuildingLevelDataSixView>(listItem.go);
            }
            else
            {
                nodeView = MonoHelper.GetHotFixViewComponent<ItemBuildingLevelDataSixView>(listItem.go);
            }
            int index = listItem.index;
            for (int i = 0; i < m_contentList[index].Count; i++)
            {
                if (i == 0)
                {
                    nodeView.m_lbl_name1_LanguageText.text = m_contentList[index][i];
                }
                else if (i == 1)
                {
                    nodeView.m_lbl_name2_LanguageText.text = m_contentList[index][i];
                }
                else if (i == 2)
                {
                    nodeView.m_lbl_name3_LanguageText.text = m_contentList[index][i];
                }
                else if (i == 3)
                {
                    nodeView.m_lbl_name4_LanguageText.text = m_contentList[index][i];
                }
                else if (i == 4)
                {
                    nodeView.m_lbl_name5_LanguageText.text = m_contentList[index][i];
                }
                else if (i == 5)
                {
                    nodeView.m_lbl_name6_LanguageText.text = m_contentList[index][i];
                }
            }
            nodeView.m_img_select_PolygonImage.gameObject.SetActive((index + 1) == m_buildingLevel);
        }

        private void ResBuildLevelInfo(int type)
        {
            int index = 0;
            List<BuildingResourcesProduceDefine> list = CoreUtils.dataService.QueryRecords<BuildingResourcesProduceDefine>();
            for (int i = 0; i < list.Count; i++)
            {
                if (type == list[i].type)
                {
                    m_contentList.Add(new List<string>());
                    index = m_contentList.Count - 1;
                    m_contentList[index].Add(list[i].level.ToString());
                    m_contentList[index].Add(ClientUtils.FormatComma(list[i].produceSpeed));
                    m_contentList[index].Add(ClientUtils.FormatComma(list[i].gatherMax));
                    m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                }
            }
        }

        private void ReadBuildLevelInfo()
        {
            int buildType = (int)m_buildingInfo.type;
            switch (buildType)
            {
                case (int)EnumCityBuildingType.TownCenter:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180311));
                        m_titleList.Add(LanguageUtils.getText(180312));
                        m_titleList.Add(LanguageUtils.getText(180313));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int lastAge = -1;
                        int index = 0;
                        List<BuildingTownCenterDefine> list = CoreUtils.dataService.QueryRecords<BuildingTownCenterDefine>();
                        CityAgeSizeDefine ageDefine = null;
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());

                            if (lastAge != list[i].age)
                            {
                                lastAge = list[i].age;
                                ageDefine = CoreUtils.dataService.QueryRecord<CityAgeSizeDefine>(lastAge);
                                if (ageDefine != null)
                                {
                                    m_contentList[index].Add(LanguageUtils.getText(ageDefine.l_nameId));
                                }
                                else
                                {
                                    m_contentList[index].Add("");
                                }
                            }
                            else
                            {
                                m_contentList[index].Add("");
                            }

                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].troopsCapacity));
                            m_contentList[index].Add(list[i].troopsDispatchNumber.ToString());

                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower(buildType, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Farm:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180316));
                        m_titleList.Add(LanguageUtils.getText(180317));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        ResBuildLevelInfo((int)EnumCityBuildingType.Farm);
                    }
                    break;
                case (int)EnumCityBuildingType.Sawmill:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180318));
                        m_titleList.Add(LanguageUtils.getText(180319));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        ResBuildLevelInfo((int)EnumCityBuildingType.Sawmill);
                    }
                    break;
                case (int)EnumCityBuildingType.Quarry:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180320));
                        m_titleList.Add(LanguageUtils.getText(180321));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        ResBuildLevelInfo((int)EnumCityBuildingType.Quarry);
                    }
                    break;
                case (int)EnumCityBuildingType.SilverMine:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180322));
                        m_titleList.Add(LanguageUtils.getText(180323));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        ResBuildLevelInfo((int)EnumCityBuildingType.SilverMine);
                    }
                    break;
                case (int)EnumCityBuildingType.CityWall:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180324));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingCityWallDefine> list = CoreUtils.dataService.QueryRecords<BuildingCityWallDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].wallDurableMax));
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.BuilderHut: //工人小屋
                    {

                    }
                    break;
                case (int)EnumCityBuildingType.GuardTower:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180327));
                        m_titleList.Add(LanguageUtils.getText(180328));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingGuardTowerDefine> list = CoreUtils.dataService.QueryRecords<BuildingGuardTowerDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].warningTowerAttack));
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].warningTowerHpMax));
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Barracks:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180329));
                        m_titleList.Add(LanguageUtils.getText(180352));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingBarracksDefine> list = CoreUtils.dataService.QueryRecords<BuildingBarracksDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].infantryTrainNumber));
                            if (list[i].infantryAttackMulti > 0)
                            {
                                m_contentList[index].Add(LanguageUtils.getTextFormat(180357, (float)list[i].infantryAttackMulti / 1000 * 100));
                            }
                            else
                            {
                                m_contentList[index].Add("");
                            }
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Stable:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180330));
                        m_titleList.Add(LanguageUtils.getText(180353));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingStableDefine> list = CoreUtils.dataService.QueryRecords<BuildingStableDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].cavalryTrainNumber));
                            if (list[i].cavalryDefenseMulti > 0)
                            {
                                m_contentList[index].Add(LanguageUtils.getTextFormat(180357, (float)list[i].cavalryDefenseMulti / 1000 * 100));
                            }
                            else
                            {
                                m_contentList[index].Add("");
                            }
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.ArcheryRange:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180331));
                        m_titleList.Add(LanguageUtils.getText(180335));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingArcheryrangeDefine> list = CoreUtils.dataService.QueryRecords<BuildingArcheryrangeDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].bowmenTrainNumber));
                            if (list[i].bowmenHpMaxMulti > 0)
                            {
                                m_contentList[index].Add(LanguageUtils.getTextFormat(180357, (float)list[i].bowmenHpMaxMulti / 1000 * 100));
                            }
                            else
                            {
                                m_contentList[index].Add("");
                            }
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.SiegeWorkshop:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180332));
                        m_titleList.Add(LanguageUtils.getText(180354));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingSiegeWorkshopDefine> list = CoreUtils.dataService.QueryRecords<BuildingSiegeWorkshopDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].siegeCarTrainNumber));
                            if (list[i].troopsSpaceMulti > 0)
                            {
                                m_contentList[index].Add(LanguageUtils.getTextFormat(180357, (float)list[i].troopsSpaceMulti / 1000 * 100));
                            }
                            else
                            {
                                m_contentList[index].Add("");
                            }
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Academy:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180333));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingCampusDefine> list = CoreUtils.dataService.QueryRecords<BuildingCampusDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(GetPermillage(list[i].researchSpeedMulti));
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }

                    break;
                case (int)EnumCityBuildingType.Hospital:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180334));
                        m_titleList.Add(LanguageUtils.getText(180335));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingHospitalDefine> list = CoreUtils.dataService.QueryRecords<BuildingHospitalDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].armyCnt));
                            if (list[i].allHpBuff > 0)
                            {
                                m_contentList[index].Add(LanguageUtils.getTextFormat(180357, (float)list[i].allHpBuff / 1000 * 100));
                            }
                            else
                            {
                                m_contentList[index].Add("");
                            }
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Storage:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180336));
                        m_titleList.Add(LanguageUtils.getText(180337));
                        m_titleList.Add(LanguageUtils.getText(180338));
                        m_titleList.Add(LanguageUtils.getText(180339));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingStorageDefine> list = CoreUtils.dataService.QueryRecords<BuildingStorageDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].foodCnt));
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].woodCnt));
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].stoneCnt));
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].goldCnt));
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.AllianceCenter:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180340));
                        m_titleList.Add(LanguageUtils.getText(180355));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingAllianceCenterDefine> list = CoreUtils.dataService.QueryRecords<BuildingAllianceCenterDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].defCapacity));
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].helpCnt));
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Castel:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180341));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingCastleDefine> list = CoreUtils.dataService.QueryRecords<BuildingCastleDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].massTroopsCapacity));
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.Tavern:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180342));
                        m_titleList.Add(LanguageUtils.getText(180343));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingTavernDefine> list = CoreUtils.dataService.QueryRecords<BuildingTavernDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatTime(list[i].goldBoxCD));
                            m_contentList[index].Add(list[i].silverBoxCnt.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.TradingPost:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180344));
                        m_titleList.Add(LanguageUtils.getText(180345));
                        m_titleList.Add(LanguageUtils.getText(180346));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingFreightDefine> list = CoreUtils.dataService.QueryRecords<BuildingFreightDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(list[i].capacity));
                            m_contentList[index].Add(GetPermillage(list[i].tax));
                            m_contentList[index].Add(GetPermillage(list[i].transportSpeedMulti));
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                case (int)EnumCityBuildingType.CourierStation:
                    break;
                case (int)EnumCityBuildingType.ScoutCamp:
                    {
                        m_titleList.Add(LanguageUtils.getText(180310));
                        m_titleList.Add(LanguageUtils.getText(180347));
                        m_titleList.Add(LanguageUtils.getText(180348));
                        m_titleList.Add(LanguageUtils.getText(180349));
                        m_titleList.Add(LanguageUtils.getText(180314));
                        int index = 0;
                        List<BuildingScoutcampDefine> list = CoreUtils.dataService.QueryRecords<BuildingScoutcampDefine>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_contentList.Add(new List<string>());
                            index = m_contentList.Count - 1;
                            m_contentList[index].Add(list[i].level.ToString());
                            m_contentList[index].Add(GetPermillage(list[i].scoutSpeedMulti));
                            m_contentList[index].Add(GetArea(list[i].scoutView));
                            m_contentList[index].Add(list[i].scoutNumber.ToString());
                            m_contentList[index].Add(ClientUtils.FormatComma(GetPower((int)m_buildingInfo.type, list[i].level)));
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion       
    }
}