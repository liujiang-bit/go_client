// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Win_BuildingUpdateMediator
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

namespace Game
{
    public class BuildingUpdateAttr
    {
        public string Name;
        public string curValue;
        public string addValue;
        public Symbol symbol;
        public enum Symbol
        {
            Plus = 1,//正
            minus = 2,//负
            arrows1 = 3,//箭头1
            arrows2 = 4,//箭头2
        }
    }

    public class UpGradeData
    {
        public long buildingIndex;//建筑id
        public int page;//0,升级页，1信息页
        public UpGradeData(long buildingIndex, int page)
        {
            this.buildingIndex = buildingIndex;
            this.page = page;
        }
    }
    public class UI_Win_BuildingUpdateMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_BuildingUpdateMediator";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_buildingProxy;
        private CurrencyProxy m_currencyProxy;
        private BagProxy m_bagProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private EffectinfoProxy m_effectinfoProxy;

        private int m_showType;

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private Int64 m_buildingIndex;
        private int m_page = 0;
        private BuildingInfoEntity m_buildingInfo;

        private BuildingTypeConfigDefine m_buildingTypeDefine;
        private BuildingLevelDataDefine m_curbuildingLevelData;//当前等级建筑数据
        private BuildingLevelDataDefine m_nexbuildingLevelData;//下一等级建筑数据
        private List<BuildingUpdateAttr> m_buildingDescAttrList = new List<BuildingUpdateAttr>();

        //建筑等级列表
        private List<string> m_titleList = new List<string>();
        private List<List<string>> m_contentList = new List<List<string>>();
        private int m_levelItemLoadStatus = 0; //1正在加载 2已加载 
        private bool m_isCreateLevelItem;
        private string m_titleItemName;
        private string m_contentPrefabName;
        private bool m_isHideLevelDetail = false;

        private Color m_originDenarTextColor ; //代币的字体颜色
        private bool readyLeft = false;
        private bool readyAttrItem = false;

        private float attrItemHeight;
        private UpGradeData m_upGradeData;
        #endregion

        //IMediatorPlug needs
        public UI_Win_BuildingUpdateMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_BuildingUpdateView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.UpdateCurrency,
                Build_UpGradeBuilding.TagName,
                CmdConstant.ItemInfoChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdateCurrency:
                    {
                        if (GuideProxy.IsGuideing)
                        {
                            return;
                        }
                        Refresh3();
                        ResourcesConsume();
                    }
                    break;
                case CmdConstant.ItemInfoChange:
                    {
                        if (GuideProxy.IsGuideing)
                        {
                            return;
                        }
                        Refresh3();
                        ResourcesConsume();
                    }
                    break;
                case Build_UpGradeBuilding.TagName:
                    {
                        Build_UpGradeBuilding.response res = notification.Body as Build_UpGradeBuilding.response;
                        if (res != null)
                        {
                            UpGradeBuilding(res);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void UpGradeBuilding(Build_UpGradeBuilding.response res)
        {
            if (m_buildingIndex == res.buildingIndex)
            {
                m_buildingInfo = m_buildingProxy.GetBuildingInfoByindex(m_buildingIndex);
                m_curbuildingLevelData = m_buildingProxy.BuildingLevelDataBylevel(m_buildingInfo.type, m_buildingInfo.level);
                m_nexbuildingLevelData = m_buildingProxy.BuildingLevelDataBylevel(m_buildingInfo.type, m_buildingInfo.level + 1);
                if (m_nexbuildingLevelData == null)
                {
                    CoreUtils.uiManager.CloseUI(UI.s_buildingUpdate);
                    return;
                }

                view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(180519, LanguageUtils.getText(m_buildingTypeDefine.l_nameId), m_nexbuildingLevelData.level);


                Loadleft();

                //属性数据获取
                AttrDataProcess();

                Refresh();
                AddEffect("UI_10015", view.m_pl_buildImg.transform, null);
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
            if (view.data == null)
            {
                Debug.LogError("buildingIndex is null");
                return;
            }


            if (view.data is long)
            {
                m_buildingIndex = (long)view.data;
            }
            else if (view.data is UpGradeData)
            {
                m_upGradeData = view.data as UpGradeData;
                m_buildingIndex = m_upGradeData.buildingIndex;
                m_page = m_upGradeData.page;
            }
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_buildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            m_effectinfoProxy = AppFacade.GetInstance().RetrieveProxy(EffectinfoProxy.ProxyNAME) as EffectinfoProxy;

            m_buildingInfo = m_buildingProxy.GetBuildingInfoByindex(m_buildingIndex);
            if (m_buildingInfo == null)
            {
                Debug.LogError(" not find " + m_buildingIndex);
            }
            m_curbuildingLevelData = m_buildingProxy.BuildingLevelDataBylevel(m_buildingInfo.type, m_buildingInfo.level);
            m_nexbuildingLevelData = m_buildingProxy.BuildingLevelDataBylevel(m_buildingInfo.type, m_buildingInfo.level + 1);
            if (m_nexbuildingLevelData == null)
            {
                Debug.LogError("现在没有最高等级的处理");
            }
            m_buildingTypeDefine = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>((int)m_buildingInfo.type);
            if (m_buildingTypeDefine == null)
            {
                Debug.LogError(string.Format("BuildingTypeConfig not find id:{0}", m_buildingInfo.type));
                return;
            }

            view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(180519, LanguageUtils.getText(m_buildingTypeDefine.l_nameId), m_nexbuildingLevelData.level);
            m_originDenarTextColor = view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.color;

            Loadleft();
            //设置显示类型
            SetShowType();

            //属性数据获取
            AttrDataProcess();

            view.gameObject.SetActive(false);

            //预加载预设
            List<string> prefabNames = new List<string>();


            prefabNames.Add("UI_Model_AttrItem");
            prefabNames.Add("UI_Model_ResourcesConsume");
            prefabNames.Add("UI_Item_BuildingUpLimit");


            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_BuildLevelInfo_GameButton.onClick.AddListener(OnBuildLevelInfo);
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.onClick.AddListener(OnBack);
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_buildingUpdate);
            });
        }

        protected override void BindUIData()
        {
            GameObject.Destroy(view.m_UI_Item_BuildingUpgrade.m_UI_Item_BuildingUpLimit.gameObject);
            if (m_page == 1)
            {
                view.m_pl_content_Animator.gameObject.SetActive(false);
                view.m_pl_sv_level_Animator.gameObject.SetActive(true);
            }
            else
            {
                view.m_pl_content_Animator.gameObject.SetActive(true);
                view.m_pl_sv_level_Animator.gameObject.SetActive(false);
            }
        }

        #endregion

        /// <summary>
        /// 加载左半边
        /// </summary>
        private void Loadleft()
        {
            string str = m_buildingProxy.GetImgIdByType(m_buildingInfo.type);
            view.m_pl_buildImg.DestroyAllChild();
            CoreUtils.assetService.Instantiate(str, (go) =>
            {
                go.transform.SetParent(view.m_pl_buildImg);
                go.transform.localPosition = new Vector3(0, 0, 0);

                go.transform.localScale = new Vector3(1, 1, 1);
                readyLeft = true; BuildingFinish();
            });
            view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getTextFormat(180519, LanguageUtils.getText(m_buildingTypeDefine.l_nameId), m_nexbuildingLevelData.level);

            if (m_buildingProxy.NewAgeByLevel(m_nexbuildingLevelData.level) && m_buildingInfo.type == (long)EnumCityBuildingType.TownCenter)
            {
                int age = (int)m_buildingProxy.GetAgeType(m_nexbuildingLevelData.level);
                CityAgeSizeDefine cityAgeSize = CoreUtils.dataService.QueryRecord<CityAgeSizeDefine>(age);
                view.m_img_titleflag_PolygonImage.gameObject.SetActive(true);
                switch ((EnumAgeType)cityAgeSize.age)
                {
                    case EnumAgeType.StoneAge:
                        view.m_lbl_languageText_left_LanguageText.text = LanguageUtils.getText(180505);
                        break;
                    case EnumAgeType.BronzeAge:
                        view.m_lbl_languageText_left_LanguageText.text = LanguageUtils.getText(180506);
                        break;
                    case EnumAgeType.IronAge:
                        view.m_lbl_languageText_left_LanguageText.text = LanguageUtils.getText(180507);
                        break;
                    case EnumAgeType.DarkAge:
                        view.m_lbl_languageText_left_LanguageText.text = LanguageUtils.getText(180508);
                        break;
                    case EnumAgeType.FeudalAge:
                        view.m_lbl_languageText_left_LanguageText.text = LanguageUtils.getText(180509);
                        break;
                    case EnumAgeType.OTHER:
                        view.m_lbl_languageText_left_LanguageText.text = LanguageUtils.getText(180532);
                        break;
                    default:
                        Debug.LogError("not find type ");
                        break;
                }
            }
            else
            {
                view.m_lbl_languageText_left_LanguageText.text = "";
                view.m_img_titleflag_PolygonImage.gameObject.SetActive(false);
            }
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var element in dic)
            {
                m_assetDic[element.Key] = element.Value.asset() as GameObject;
            }

            Refresh();
            if (m_page == 1)
            {
                OnBuildLevelInfo();
            }            
        }

        private void Refresh()
        {
            view.m_btn_BuildLevelInfo_GameButton.gameObject.SetActive(!m_isHideLevelDetail);
            ResourcesConsume();
            RefreshDescAttr();
            Refresh3();

        }

        private bool m_isCheckGuide;
        private void Refresh3()
        {
            view.m_UI_Item_BuildingUpgrade.m_pl_limit_HorizontalLayoutGroup.transform.DestroyAllChild();
            bool Type1 = false, Type2 = false, Type3 = false;
            if (m_buildingProxy.GetMaxLevelofType(m_nexbuildingLevelData.reqType1) >= m_nexbuildingLevelData.reqLevel1)
            {
                Type1 = true;
            }
            if (m_buildingProxy.GetMaxLevelofType(m_nexbuildingLevelData.reqType2) >= m_nexbuildingLevelData.reqLevel2)
            {
                Type2 = true;
            }
            if (m_buildingProxy.GetMaxLevelofType(m_nexbuildingLevelData.reqType3) >= m_nexbuildingLevelData.reqLevel3)
            {
                Type3 = true;
            }
            if (m_nexbuildingLevelData.reqType1 == 0)
            {
                Type1 = true;
            }
            if (m_nexbuildingLevelData.reqType2 == 0)
            {
                Type2 = true;
            }
            if (m_nexbuildingLevelData.reqType3 == 0)
            {
                Type3 = true;
            }
            if (Type1 && Type2 && Type3)

            {
                if (!m_isCheckGuide)
                {
                    m_isCheckGuide = true;
                    //功能介绍引导
                    if (view.m_img_titleflag_PolygonImage.gameObject.activeSelf)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.AgeChangeIntro);
                    }
                }

                view.m_UI_Item_BuildingUpgrade.m_pl_limit_HorizontalLayoutGroup.gameObject.SetActive(false);
                view.m_UI_Item_BuildingUpgrade.m_pl_btns_HorizontalLayoutGroup.gameObject.SetActive(true);
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.gameObject.SetActive(true);
                ClientUtils.LoadSprite(view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_img_icon2_PolygonImage, m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.denar));
                PlayerAttributeProxy playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
                float buildSpeedMulti = playerAttributeProxy.GetCityAttribute(attrType.buildSpeedMulti).origvalue;
                int needTime = (int)(m_nexbuildingLevelData.buildingTime *(1000- buildSpeedMulti)/1000);
                List<RequireItemParam> requireItemParam = null;
                if(m_nexbuildingLevelData.itemCnt > 0 && m_nexbuildingLevelData.itemType1 > 0)
                {
                    requireItemParam = new List<RequireItemParam>();
                    requireItemParam.Add(new RequireItemParam() { ItemId = m_nexbuildingLevelData.itemType1, Num = m_nexbuildingLevelData.itemCnt });
                }
                long needDenar = m_currencyProxy.CaculateImmediatelyFinishPrice(needTime, m_nexbuildingLevelData.food, m_nexbuildingLevelData.wood, 
                    m_nexbuildingLevelData.stone, m_nexbuildingLevelData.coin, requireItemParam);
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.color = m_currencyProxy.Gem < needDenar ? Color.red : m_originDenarTextColor;
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.text = needDenar.ToString("N0");
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(300046);
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_ContentSizeFitter.enabled = true;
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_LanguageText.text = ClientUtils.FormatTimeUpgrad((int)needTime);
                float width = view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_LanguageText.preferredWidth;
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_LanguageText.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 27.7f);
                float width1 = view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.preferredWidth;
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_lbl_line2_LanguageText.GetComponent<RectTransform>().sizeDelta = new Vector2(width1, 27.7f);
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_lbl_line1_LanguageText.text = LanguageUtils.getText(180359);
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_lbl_line2_ContentSizeFitter.enabled = true;
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    if (!m_currencyProxy.ShortOfDenar(needDenar))
                    {
                        if (m_buildingInfo.level == 7 && m_buildingInfo.type == (long)EnumCityBuildingType.TownCenter)
                        {
                            Alert.CreateAlert(180533).SetLeftButton(null, LanguageUtils.getText(192010))
                           .SetRightButton(() =>
                           {
                               SendUpGradeBuilding((int)needDenar);
                           }, LanguageUtils.getText(192009)).Show();
                        }
                        else
                        {
                            SendUpGradeBuilding((int)needDenar);
                        }
                    }
                });
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                view.m_UI_Item_BuildingUpgrade.m_UI_Model_DoubleLineButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    if (m_nexbuildingLevelData.coin <= m_playerProxy.CurrentRoleInfo.gold &&
                    m_nexbuildingLevelData.stone <= m_playerProxy.CurrentRoleInfo.stone &&
                    m_nexbuildingLevelData.wood <= m_playerProxy.CurrentRoleInfo.wood &&
                    m_nexbuildingLevelData.food <= m_playerProxy.CurrentRoleInfo.food &&
                    m_nexbuildingLevelData.itemCnt <= m_bagProxy.GetItemNum(m_nexbuildingLevelData.itemType1))
                    {
                        if (m_buildingProxy.IsbuildQueueleisur((int)needTime))
                        {
                            if (m_buildingInfo.level == 7 && m_buildingInfo.type == (long)EnumCityBuildingType.TownCenter)
                            {
                                Alert.CreateAlert(180533).SetLeftButton(null, LanguageUtils.getText(192010))
                               .SetRightButton(() =>
                               {
                                   CoreUtils.audioService.PlayOneShot(RS.SoundBuildingStartLevelup);
                                   Build_UpGradeBuilding.request req = new Build_UpGradeBuilding.request();
                                   req.buildingIndex = m_buildingIndex;
                                   req.immediately = false;
                                   AppFacade.GetInstance().SendSproto(req);
                                   CoreUtils.uiManager.CloseUI(UI.s_buildingUpdate);

                               }, LanguageUtils.getText(192009)).Show();
                            }
                            else
                            {
                                CoreUtils.audioService.PlayOneShot(RS.SoundBuildingStartLevelup);
                                Build_UpGradeBuilding.request req = new Build_UpGradeBuilding.request();
                                req.buildingIndex = m_buildingIndex;
                                req.immediately = false;
                                AppFacade.GetInstance().SendSproto(req);
                                CoreUtils.uiManager.CloseUI(UI.s_buildingUpdate);
                            }
                        }
                        else
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_worker, null, needTime);
                        }
                    }
                    else
                    {
                        if (m_nexbuildingLevelData.itemCnt <= m_bagProxy.GetItemNum(m_nexbuildingLevelData.itemType1))
                        {
                            m_currencyProxy.LackOfResources(m_nexbuildingLevelData.food, m_nexbuildingLevelData.wood, m_nexbuildingLevelData.stone, m_nexbuildingLevelData.coin);
                        }
                        else
                        {
                        m_currencyProxy.LackOfResources(m_nexbuildingLevelData.food, m_nexbuildingLevelData.wood, m_nexbuildingLevelData.stone, m_nexbuildingLevelData.coin, m_nexbuildingLevelData.itemType1, m_nexbuildingLevelData.itemCnt);
                        }
                    }

                });
            }
            else
            {
                view.m_UI_Item_BuildingUpgrade.m_pl_limit_HorizontalLayoutGroup.gameObject.SetActive(true);
                view.m_UI_Item_BuildingUpgrade.m_pl_btns_HorizontalLayoutGroup.gameObject.SetActive(false);
                if (!Type1)
                {
                    InstantiateBuildingUpLimit(m_nexbuildingLevelData.reqType1, m_nexbuildingLevelData.reqLevel1);
                }
                if (!Type2)
                {
                    InstantiateBuildingUpLimit(m_nexbuildingLevelData.reqType2, m_nexbuildingLevelData.reqLevel2);
                }
                if (!Type3)
                {
                    InstantiateBuildingUpLimit(m_nexbuildingLevelData.reqType3, m_nexbuildingLevelData.reqLevel3);
                }
            }
        }
        private void InstantiateBuildingUpLimit(int reqType,int reqLevel)
        {
            CoreUtils.assetService.Instantiate("UI_Item_BuildingUpLimit", (gameObject) =>
            {
                Transform transform = view.m_UI_Item_BuildingUpgrade.m_pl_limit_HorizontalLayoutGroup.transform.Find(reqType.ToString());
                if (transform != null)
                {
                    CoreUtils.assetService.Destroy(transform.gameObject);
                }
                gameObject.name = reqType.ToString();
                gameObject.transform.SetParent(view.m_UI_Item_BuildingUpgrade.m_pl_limit_HorizontalLayoutGroup.transform);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                BuildingTypeConfigDefine buildingTypeDefine = CoreUtils.dataService.QueryRecord<BuildingTypeConfigDefine>(reqType);
                UI_Item_BuildingUpLimit_SubView buildingUpLimitView = new UI_Item_BuildingUpLimit_SubView(gameObject.GetComponent<RectTransform>());
                buildingUpLimitView.SetName(LanguageUtils.getTextFormat(300015, reqLevel, LanguageUtils.getText(buildingTypeDefine.l_nameId)));

                string imgRes = m_buildingProxy.GetImgIdByType(reqType);
                buildingUpLimitView.SetIcon(imgRes);
                buildingUpLimitView.AddClickEvent(() =>
                {
                    BuildingInfoEntity buildingInfoEntity = m_buildingProxy.GetBuildingInfoByType(reqType);
                    if (buildingInfoEntity == null)
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ClickEnterCity);
                        CoreUtils.uiManager.ShowUI(UI.s_buildCity, null, reqType);
                    }
                    else
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ShowBuildingMenuAndMoveCameraToBuilding, m_buildingProxy.GetBuildingInfoByType(reqType));
                    }
                    CoreUtils.uiManager.CloseUI(UI.s_buildingUpdate);
                });
            });
        }

        private void ResourcesConsume()
        {
            int objCount = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.childCount;
            for (int i = objCount - 1; i >= 0; i--)
            {
                view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(i).gameObject.SetActive(false);
            }
            GameObject temp = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(0).gameObject;
            int count = 0;

            if (m_nexbuildingLevelData.food > 0)
            {
                GameObject gameObject = null;
                if (count < objCount)
                {
                    gameObject = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(count).gameObject;
                }
                else
                {
                    gameObject = CoreUtils.assetService.Instantiate(temp);
                }
                gameObject.name = EnumCurrencyType.food.ToString();
                gameObject.transform.SetParent(view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform);
                gameObject.SetActive(true);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                UI_Model_ResourcesConsume_SubView ResourcesConsumeView = new UI_Model_ResourcesConsume_SubView(gameObject.GetComponent<RectTransform>());
                ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.food), m_nexbuildingLevelData.food, (m_nexbuildingLevelData.food <= m_playerProxy.CurrentRoleInfo.food));
                ResourcesConsumeView.AddClickEvent(() =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, new long[4] { m_nexbuildingLevelData.food, 0, 0, 0 });
                });
                count++;
            }
            {
                if (m_nexbuildingLevelData.wood > 0)
                {
                    GameObject gameObject = null;
                    if (count < objCount)
                    {
                        gameObject = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(count).gameObject;
                    }
                    else
                    {
                        gameObject = CoreUtils.assetService.Instantiate(temp);
                    }
                    gameObject.name = EnumCurrencyType.wood.ToString();
                    gameObject.transform.SetParent(view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform);
                    gameObject.SetActive(true);
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    UI_Model_ResourcesConsume_SubView ResourcesConsumeView = new UI_Model_ResourcesConsume_SubView(gameObject.GetComponent<RectTransform>());
                    ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.wood), m_nexbuildingLevelData.wood, (m_nexbuildingLevelData.wood <= m_playerProxy.CurrentRoleInfo.wood));
                    ResourcesConsumeView.AddClickEvent(() =>
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, new long[4] { 0, m_nexbuildingLevelData.wood, 0, 0 });
                    });
                    count++;
                }

            }
            {
                if (m_nexbuildingLevelData.stone > 0)
                {
                    GameObject gameObject = null;
                    if (count < objCount)
                    {
                        gameObject = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(count).gameObject;
                    }
                    else
                    {
                        gameObject = CoreUtils.assetService.Instantiate(temp);
                    }
                    gameObject.name = EnumCurrencyType.stone.ToString();
                    gameObject.transform.SetParent(view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform);
                    gameObject.SetActive(true);
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    UI_Model_ResourcesConsume_SubView ResourcesConsumeView = new UI_Model_ResourcesConsume_SubView(gameObject.GetComponent<RectTransform>());
                    ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.stone), m_nexbuildingLevelData.stone, (m_nexbuildingLevelData.stone <= m_playerProxy.CurrentRoleInfo.stone));
                    ResourcesConsumeView.AddClickEvent(() =>
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, new long[4] { 0, 0, m_nexbuildingLevelData.stone, 0 });
                    });
                    count++;
                }
            }
            {
                if (m_nexbuildingLevelData.coin > 0)
                {
                    GameObject gameObject = null;
                    if (count < objCount)
                    {
                        gameObject = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(count).gameObject;
                    }
                    else
                    {
                        gameObject = CoreUtils.assetService.Instantiate(temp);
                    }
                    gameObject.name = EnumCurrencyType.gold.ToString();
                    gameObject.transform.SetParent(view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform);
                    gameObject.SetActive(true);
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    UI_Model_ResourcesConsume_SubView ResourcesConsumeView = new UI_Model_ResourcesConsume_SubView(gameObject.GetComponent<RectTransform>());
                    ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.gold), m_nexbuildingLevelData.coin, (m_nexbuildingLevelData.coin <= m_playerProxy.CurrentRoleInfo.gold));
                    ResourcesConsumeView.AddClickEvent(() =>
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, new long[4] { 0, 0, 0, m_nexbuildingLevelData.coin });
                    });
                    count++;
                }
            }

            {
                if (m_nexbuildingLevelData.denar > 0)
                {
                    GameObject gameObject = null;
                    if (count < objCount)
                    {
                        gameObject = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(count).gameObject;
                    }
                    else
                    {
                        gameObject = CoreUtils.assetService.Instantiate(temp);
                    }
                    gameObject.name = EnumCurrencyType.denar.ToString();
                    gameObject.transform.SetParent(view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform);
                    gameObject.SetActive(true);
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    UI_Model_ResourcesConsume_SubView ResourcesConsumeView = new UI_Model_ResourcesConsume_SubView(gameObject.GetComponent<RectTransform>());
                    ResourcesConsumeView.SetResourcesConsume(m_currencyProxy.GeticonIdByType((int)EnumCurrencyType.denar), m_nexbuildingLevelData.denar, (m_nexbuildingLevelData.denar <= m_playerProxy.CurrentRoleInfo.denar));
                    count++;
                }

            }
            {
                if (m_nexbuildingLevelData.itemType1 > 0)
                {
                    GameObject gameObject = null;
                    if (count < objCount)
                    {
                        gameObject = view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform.GetChild(count).gameObject;
                    }
                    else
                    {
                        gameObject = CoreUtils.assetService.Instantiate(temp);
                    }
                    gameObject.transform.SetParent(view.m_UI_Item_BuildingUpgrade.m_img_resBg_PolygonImage.transform);
                    gameObject.SetActive(true);
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(m_nexbuildingLevelData.itemType1);
                    gameObject.name = LanguageUtils.getText(itemDefine.l_nameID);
                    gameObject.transform.localScale = new Vector3(1, 1, 1);

                    UI_Model_ResourcesConsume_SubView ResourcesConsumeView = new UI_Model_ResourcesConsume_SubView(gameObject.GetComponent<RectTransform>());
                    ResourcesConsumeView.SetResourcesConsume(itemDefine.itemIcon, m_nexbuildingLevelData.itemCnt, (m_nexbuildingLevelData.itemCnt <= m_bagProxy.GetItemNum(m_nexbuildingLevelData.itemType1)));
                    ResourcesConsumeView.AddClickEvent(() =>
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, new long[2] {  m_nexbuildingLevelData.itemType1 , m_nexbuildingLevelData.itemCnt });
                    });
                    count++;
                }
            }
        }
        /// <summary>
        /// 刷新属性升级描述
        /// </summary>
        private void RefreshDescAttr()
        {
            view.m_UI_Item_Buildinglevelup.m_lbl_before_LanguageText.text = LanguageUtils.getTextFormat(180306, m_curbuildingLevelData.level);
            view.m_UI_Item_Buildinglevelup.m_lbl_after_LanguageText.text = m_nexbuildingLevelData.level.ToString("N0");
            view.m_pl_attr_PolygonImage.gameObject.SetActive(true);
            int count = m_buildingDescAttrList.Count;
            for (int i = view.m_pl_attr_PolygonImage.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(view.m_pl_attr_PolygonImage.transform.GetChild(i).gameObject);
            }
            for (int i = 0; i < count; i++)
            {
                GameObject obj = GameObject.Instantiate(m_assetDic["UI_Model_AttrItem"], view.m_pl_attr_PolygonImage.transform);
                obj.transform.localScale = Vector3.one;
                attrItemHeight = obj.GetComponent<RectTransform>().rect.height;
                UI_Model_AttrItemView itemView = MonoHelper.AddHotFixViewComponent<UI_Model_AttrItemView>(obj);
                itemView.m_lbl_name_LanguageText.text = m_buildingDescAttrList[i].Name;
                itemView.m_lbl_crrVaule_LanguageText.supportRichText = true;
                itemView.m_lbl_addVaule_LanguageText.text = string.Empty;
                switch (m_buildingDescAttrList[i].symbol)
                {
                    case BuildingUpdateAttr.Symbol.Plus:
                        itemView.m_lbl_crrVaule_LanguageText.text = LanguageUtils.getTextFormat(180356, m_buildingDescAttrList[i].curValue, m_buildingDescAttrList[i].addValue);
                        break;
                    case BuildingUpdateAttr.Symbol.minus:
                        itemView.m_lbl_crrVaule_LanguageText.text = LanguageUtils.getTextFormat(180362, m_buildingDescAttrList[i].curValue, m_buildingDescAttrList[i].addValue);
                        break;
                    case BuildingUpdateAttr.Symbol.arrows2:
                        itemView.m_lbl_crrVaule_LanguageText.text = LanguageUtils.getTextFormat(180364, m_buildingDescAttrList[i].curValue, m_buildingDescAttrList[i].curValue, m_buildingDescAttrList[i].addValue, m_buildingDescAttrList[i].addValue);
                        break;
                    case BuildingUpdateAttr.Symbol.arrows1:
                        itemView.m_lbl_crrVaule_LanguageText.text = LanguageUtils.getTextFormat(180363, m_buildingDescAttrList[i].curValue, m_buildingDescAttrList[i].addValue);
                        break;
                    default:
                        {
                            Debug.LogError("not find type");
                        }
                        break;
                }
                if (i == count - 1)
                {
                    itemView.m_pl_line_PolygonImage.gameObject.SetActive(false);
                }
                else
                {
                    itemView.m_pl_line_PolygonImage.gameObject.SetActive(true);
                }
            }
            RectTransform rightRectTransform = view.m_pl_Right_VerticalLayoutGroup.GetComponent<RectTransform>();
            RectTransform attrRectTransform = view.m_pl_attr_VerticalLayoutGroup.GetComponent<RectTransform>();
            attrRectTransform.sizeDelta = new Vector2(attrRectTransform.sizeDelta.x, attrItemHeight * count + 30);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_pl_Right_VerticalLayoutGroup.GetComponent<RectTransform>());
            readyAttrItem = true; BuildingFinish();
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
                default:
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
            m_buildingDescAttrList.Clear();
            int buildType = (int)m_buildingInfo.type;
            int buildLevel = (int)m_buildingInfo.level;
            string powerStr = LanguageUtils.getText(180314);
            switch ((EnumCityBuildingType)buildType)
            {
                case EnumCityBuildingType.TownCenter:
                    {
                        BuildingTownCenterDefine define1 = CoreUtils.dataService.QueryRecord<BuildingTownCenterDefine>(buildLevel);
                        BuildingTownCenterDefine define2 = CoreUtils.dataService.QueryRecord<BuildingTownCenterDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.troopsCapacity != define1.troopsCapacity)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(181110);
                                attr.curValue = ClientUtils.FormatComma(define1.troopsCapacity);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.troopsCapacity - define1.troopsCapacity));
                                attr.symbol = define2.troopsCapacity > define1.troopsCapacity ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.troopsDispatchNumber != define1.troopsDispatchNumber)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180313);
                                attr.curValue = ClientUtils.FormatComma(define1.troopsDispatchNumber);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.troopsDispatchNumber - define1.troopsDispatchNumber));
                                attr.symbol = define2.troopsDispatchNumber > define1.troopsDispatchNumber ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.CityWall:
                    {
                        BuildingCityWallDefine define1 = CoreUtils.dataService.QueryRecord<BuildingCityWallDefine>(buildLevel);
                        BuildingCityWallDefine define2 = CoreUtils.dataService.QueryRecord<BuildingCityWallDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.wallDurableMax != define1.wallDurableMax)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180324);
                                attr.curValue = ClientUtils.FormatComma(define1.wallDurableMax);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.wallDurableMax - define1.wallDurableMax));
                                attr.symbol = define2.wallDurableMax > define1.wallDurableMax ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr2 = new BuildingUpdateAttr();
                            attr2.Name = powerStr;
                            attr2.curValue = ClientUtils.FormatComma(GetPower((int)buildType, buildLevel));
                            attr2.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr2.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr2);
                        }
                    }
                    break;
                case EnumCityBuildingType.Farm:
                    {
                        int id1 = GetResId(buildType, buildLevel);
                        BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id1);
                        BuildingResourcesProduceDefine define2 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id1 + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.produceSpeed != define1.produceSpeed)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180702);
                                attr.curValue = ClientUtils.FormatComma(define1.produceSpeed);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.produceSpeed - define1.produceSpeed));
                                attr.symbol = define2.produceSpeed > define1.produceSpeed ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.gatherMax != define1.gatherMax)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180703);
                                attr.curValue = ClientUtils.FormatComma(define1.gatherMax);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.gatherMax - define1.gatherMax));
                                attr.symbol = define2.gatherMax > define1.gatherMax ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.Sawmill:
                    {
                        int id = GetResId(buildType, buildLevel);
                        BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                        BuildingResourcesProduceDefine define2 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.produceSpeed != define1.produceSpeed)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180700);
                                attr.curValue = ClientUtils.FormatComma(define1.produceSpeed);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.produceSpeed - define1.produceSpeed));
                                attr.symbol = define2.produceSpeed > define1.produceSpeed ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.gatherMax != define1.gatherMax)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180701);
                                attr.curValue = ClientUtils.FormatComma(define1.gatherMax);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.gatherMax - define1.gatherMax));
                                attr.symbol = define2.gatherMax > define1.gatherMax ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.Quarry:
                    {
                        int id = GetResId(buildType, buildLevel);
                        BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                        BuildingResourcesProduceDefine define2 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.produceSpeed != define1.produceSpeed)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180706);
                                attr.curValue = ClientUtils.FormatComma(define1.produceSpeed);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.produceSpeed - define1.produceSpeed));
                                attr.symbol = define2.produceSpeed > define1.produceSpeed ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.gatherMax != define1.gatherMax)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180707);
                                attr.curValue = ClientUtils.FormatComma(define1.gatherMax);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.gatherMax - define1.gatherMax));
                                attr.symbol = define2.gatherMax > define1.gatherMax ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.SilverMine:
                    {
                        int id = GetResId(buildType, buildLevel);
                        BuildingResourcesProduceDefine define1 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id);
                        BuildingResourcesProduceDefine define2 = CoreUtils.dataService.QueryRecord<BuildingResourcesProduceDefine>(id + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.produceSpeed != define1.produceSpeed)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180704);
                                attr.curValue = ClientUtils.FormatComma(define1.produceSpeed);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.produceSpeed - define1.produceSpeed));
                                attr.symbol = define2.produceSpeed > define1.produceSpeed ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.gatherMax != define1.gatherMax)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180705);
                                attr.curValue = ClientUtils.FormatComma(define1.gatherMax);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.gatherMax - define1.gatherMax));
                                attr.symbol = define2.gatherMax > define1.gatherMax ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.BuilderHut:
                    {
                        Debug.LogError(" error type");
                    }
                    break;
                case EnumCityBuildingType.GuardTower:
                    {
                        BuildingGuardTowerDefine define1 = CoreUtils.dataService.QueryRecord<BuildingGuardTowerDefine>(buildLevel);
                        BuildingGuardTowerDefine define2 = CoreUtils.dataService.QueryRecord<BuildingGuardTowerDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.warningTowerAttack != define1.warningTowerAttack)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180327);
                                attr.curValue = ClientUtils.FormatComma(define1.warningTowerAttack);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.warningTowerAttack - define1.warningTowerAttack));
                                attr.symbol = define2.warningTowerAttack > define1.warningTowerAttack ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.warningTowerHpMax != define1.warningTowerHpMax)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180328);
                                attr.curValue = ClientUtils.FormatComma(define1.warningTowerHpMax);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.warningTowerHpMax - define1.warningTowerHpMax));
                                attr.symbol = define2.warningTowerHpMax > define1.warningTowerHpMax ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.Academy:
                    {
                        BuildingCampusDefine define1 = CoreUtils.dataService.QueryRecord<BuildingCampusDefine>(buildLevel);
                        BuildingCampusDefine define2 = CoreUtils.dataService.QueryRecord<BuildingCampusDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.researchSpeedMulti != define1.researchSpeedMulti)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180333);
                                attr.curValue = (define1.researchSpeedMulti/1000.0f).ToString("P1");
                                attr.addValue = (Mathf.Abs(define2.researchSpeedMulti - define1.researchSpeedMulti)/1000.0f).ToString("P1");
                                attr.symbol = define2.researchSpeedMulti > define1.researchSpeedMulti ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.Hospital:
                    {
                        BuildingHospitalDefine define1 = CoreUtils.dataService.QueryRecord<BuildingHospitalDefine>(buildLevel);
                        BuildingHospitalDefine define2 = CoreUtils.dataService.QueryRecord<BuildingHospitalDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.armyCnt != define1.armyCnt)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180334);
                                attr.curValue = ClientUtils.FormatComma(define1.armyCnt);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.armyCnt - define1.armyCnt));
                                attr.symbol = define2.armyCnt > define1.armyCnt ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.allHpBuff != define1.allHpBuff)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180335);
                                attr.curValue = ((define1.allHpBuff)/1000.0f).ToString("P1");
                                attr.addValue = ((Mathf.Abs(define2.allHpBuff - define1.allHpBuff)) / 1000.0f).ToString("P1");
                                attr.symbol = define2.allHpBuff > define1.allHpBuff ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.Storage:
                    {
                        BuildingStorageDefine define1 = CoreUtils.dataService.QueryRecord<BuildingStorageDefine>(buildLevel);
                        BuildingStorageDefine define2 = CoreUtils.dataService.QueryRecord<BuildingStorageDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.foodCnt != define1.foodCnt)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180336);
                                attr.curValue = ClientUtils.FormatComma(define1.foodCnt);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.foodCnt - define1.foodCnt));
                                attr.symbol = define2.foodCnt > define1.foodCnt ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.woodCnt != define1.woodCnt)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180337);
                                attr.curValue = ClientUtils.FormatComma(define1.woodCnt);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.woodCnt - define1.woodCnt));
                                attr.symbol = define2.woodCnt > define1.woodCnt ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.stoneCnt != define1.stoneCnt)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180338);
                                attr.curValue = ClientUtils.FormatComma(define1.stoneCnt);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.stoneCnt - define1.stoneCnt));
                                attr.symbol = define2.stoneCnt > define1.stoneCnt ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.goldCnt != define1.goldCnt)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180339);
                                attr.curValue = ClientUtils.FormatComma(define1.goldCnt);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.goldCnt - define1.goldCnt));
                                attr.symbol = define2.goldCnt > define1.goldCnt ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr5 = new BuildingUpdateAttr();
                            attr5.Name = powerStr;
                            attr5.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr5.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr5.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr5);

                        }
                    }
                    break;
                case EnumCityBuildingType.AllianceCenter:
                    {
                        BuildingAllianceCenterDefine define1 = CoreUtils.dataService.QueryRecord<BuildingAllianceCenterDefine>(buildLevel);
                        BuildingAllianceCenterDefine define2 = CoreUtils.dataService.QueryRecord<BuildingAllianceCenterDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.defCapacity != define1.defCapacity)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180340);
                                attr.curValue = ClientUtils.FormatComma(define1.defCapacity);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.defCapacity - define1.defCapacity));
                                attr.symbol = define2.defCapacity > define1.defCapacity ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.helpCnt != define1.helpCnt)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(610018);
                                attr.curValue = ClientUtils.FormatComma(define1.helpCnt);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.helpCnt - define1.helpCnt));
                                attr.symbol = define2.helpCnt > define1.helpCnt ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.Castel:
                    {
                        BuildingCastleDefine define1 = CoreUtils.dataService.QueryRecord<BuildingCastleDefine>(buildLevel);
                        BuildingCastleDefine define2 = CoreUtils.dataService.QueryRecord<BuildingCastleDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.massTroopsCapacity != define1.massTroopsCapacity)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180341);
                                attr.curValue = ClientUtils.FormatComma(define1.massTroopsCapacity);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.massTroopsCapacity - define1.massTroopsCapacity));
                                attr.symbol = define2.massTroopsCapacity > define1.massTroopsCapacity ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.Tavern:
                    {
                        BuildingTavernDefine define1 = CoreUtils.dataService.QueryRecord<BuildingTavernDefine>(buildLevel);
                        BuildingTavernDefine define2 = CoreUtils.dataService.QueryRecord<BuildingTavernDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.goldBoxCD != define1.goldBoxCD)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180342);
                                attr.curValue = ClientUtils.FormatTimeUpgrad(define1.goldBoxCD);
                                attr.addValue = ClientUtils.FormatTimeUpgrad(Mathf.Abs(define2.goldBoxCD - define1.goldBoxCD));
                                attr.symbol = define2.goldBoxCD > define1.goldBoxCD ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.silverBoxCnt != define1.silverBoxCnt)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180343);
                                attr.curValue = ClientUtils.FormatComma(define1.silverBoxCnt);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.silverBoxCnt - define1.silverBoxCnt));
                                attr.symbol = define2.silverBoxCnt > define1.silverBoxCnt ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.TradingPost:
                    {
                        BuildingFreightDefine define1 = CoreUtils.dataService.QueryRecord<BuildingFreightDefine>(buildLevel);
                        BuildingFreightDefine define2 = CoreUtils.dataService.QueryRecord<BuildingFreightDefine>(buildLevel + 1);
                        if (define1 != null)
                        {
                            if (define2.capacity != define1.capacity)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180344);
                                attr.curValue = ClientUtils.FormatComma(define1.capacity);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.capacity - define1.capacity));
                                attr.symbol = define2.capacity > define1.capacity ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.tax != define1.tax)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180345);
                                attr.curValue = (define1.tax / 1000.0f).ToString("P0");
                                attr.addValue = Mathf.Abs(define2.tax - define1.tax) / 1000f * 100f + "%";//ClientUtils.FormatComma(Mathf.Abs(define2.tax - define1.tax));
                                attr.symbol = define2.tax > define1.tax ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.transportSpeedMulti != define1.transportSpeedMulti)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180346);
                                attr.curValue = (define1.transportSpeedMulti / 1000.0f).ToString("P0");
                                attr.addValue =( Mathf.Abs(define2.transportSpeedMulti - define1.transportSpeedMulti) / 1000f).ToString("P0");
                                attr.symbol = define2.transportSpeedMulti > define1.transportSpeedMulti ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr3 = new BuildingUpdateAttr();
                            attr3.Name = powerStr;
                            attr3.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr3.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr3.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr3);
                        }
                    }
                    break;
                case EnumCityBuildingType.ScoutCamp:
                    {
                        BuildingScoutcampDefine define1 = CoreUtils.dataService.QueryRecord<BuildingScoutcampDefine>(buildLevel);
                        BuildingScoutcampDefine define2 = CoreUtils.dataService.QueryRecord<BuildingScoutcampDefine>(buildLevel + 1);
                        if (define1 != null && define2 != null)
                        {
                            if (define2.scoutSpeedMulti != define1.scoutSpeedMulti)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180347);//斥候行军速度提升
                                attr.curValue = (define1.scoutSpeedMulti / 1000.0f).ToString("P1");
                                attr.addValue = (Mathf.Abs(define2.scoutSpeedMulti - define1.scoutSpeedMulti)/1000.0f).ToString("P1");
                                attr.symbol = define2.scoutSpeedMulti > define1.scoutSpeedMulti ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.scoutNumber != define1.scoutNumber)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180349);
                                attr.curValue = ClientUtils.FormatComma(define1.scoutNumber);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.scoutNumber - define1.scoutNumber));
                                attr.symbol = define2.scoutNumber > define1.scoutNumber ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.scoutView != define1.scoutView)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180348);//探索范围
                                attr.curValue = ClientUtils.FormatComma(define1.scoutView);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.scoutView));
                                attr.symbol = BuildingUpdateAttr.Symbol.arrows2;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr4 = new BuildingUpdateAttr();
                            attr4.Name = powerStr;
                            attr4.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr4.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr4.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr4);
                        }
                    }
                    break;
                case EnumCityBuildingType.Barracks:
                    {
                        BuildingBarracksDefine define1 = CoreUtils.dataService.QueryRecord<BuildingBarracksDefine>(buildLevel);
                        BuildingBarracksDefine define2 = CoreUtils.dataService.QueryRecord<BuildingBarracksDefine>(buildLevel + 1);
                        if (define1 != null)
                        {
                            if (define2.infantryTrainNumber != define1.infantryTrainNumber)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180329);
                                attr.curValue = ClientUtils.FormatComma(define1.infantryTrainNumber);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.infantryTrainNumber - define1.infantryTrainNumber));
                                attr.symbol = define2.infantryTrainNumber > define1.infantryTrainNumber ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.infantryAttackMulti != define1.infantryAttackMulti)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180352);
                                attr.curValue = ((define1.infantryAttackMulti) / 1000.0f).ToString("P1");
                                attr.addValue = ((Mathf.Abs(define2.infantryAttackMulti - define1.infantryAttackMulti)) / 1000.0f).ToString("P1");
                                attr.symbol = define2.infantryAttackMulti > define1.infantryAttackMulti ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr4 = new BuildingUpdateAttr();
                            attr4.Name = powerStr;
                            attr4.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr4.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr4.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr4);
                        }
                    }
                    break;
                case EnumCityBuildingType.Stable:
                    {
                        BuildingStableDefine define1 = CoreUtils.dataService.QueryRecord<BuildingStableDefine>(buildLevel);
                        BuildingStableDefine define2 = CoreUtils.dataService.QueryRecord<BuildingStableDefine>(buildLevel + 1);
                        if (define1 != null)
                        {
                            if (define2.cavalryTrainNumber != define1.cavalryTrainNumber)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180330);
                                attr.curValue = ClientUtils.FormatComma(define1.cavalryTrainNumber);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.cavalryTrainNumber - define1.cavalryTrainNumber));
                                attr.symbol = define2.cavalryTrainNumber > define1.cavalryTrainNumber ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.infantryDefenseMulti != define1.infantryDefenseMulti)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180353);
                                attr.curValue = ((define1.infantryDefenseMulti) / 1000.0f).ToString("P1");
                                attr.addValue = ((Mathf.Abs(define2.infantryDefenseMulti - define1.infantryDefenseMulti)) / 1000.0f).ToString("P1");
                                attr.symbol = define2.infantryDefenseMulti > define1.infantryDefenseMulti ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr4 = new BuildingUpdateAttr();
                            attr4.Name = powerStr;
                            attr4.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr4.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr4.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr4);
                        }
                    }
                    break;
                case EnumCityBuildingType.ArcheryRange:
                    {
                        BuildingArcheryrangeDefine define1 = CoreUtils.dataService.QueryRecord<BuildingArcheryrangeDefine>(buildLevel);
                        BuildingArcheryrangeDefine define2 = CoreUtils.dataService.QueryRecord<BuildingArcheryrangeDefine>(buildLevel + 1);
                        if (define1 != null)
                        {
                            if (define2.bowmenTrainNumber != define1.bowmenTrainNumber)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180331);
                                attr.curValue = ClientUtils.FormatComma(define1.bowmenTrainNumber);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.bowmenTrainNumber - define1.bowmenTrainNumber));
                                attr.symbol = define2.bowmenTrainNumber > define1.bowmenTrainNumber ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.infantryHpMaxMulti != define1.infantryHpMaxMulti)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180335);
                                attr.curValue = ((define1.infantryHpMaxMulti) / 1000.0f).ToString("P1");
                                attr.addValue = ((Mathf.Abs(define2.infantryHpMaxMulti - define1.infantryHpMaxMulti)) / 1000.0f).ToString("P1");
                                attr.symbol = define2.infantryHpMaxMulti > define1.infantryHpMaxMulti ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }

                            BuildingUpdateAttr attr4 = new BuildingUpdateAttr();
                            attr4.Name = powerStr;
                            attr4.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr4.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr4.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr4);
                        }
                    }
                    break;
                case EnumCityBuildingType.SiegeWorkshop:
                    {
                        BuildingSiegeWorkshopDefine define1 = CoreUtils.dataService.QueryRecord<BuildingSiegeWorkshopDefine>(buildLevel);
                        BuildingSiegeWorkshopDefine define2 = CoreUtils.dataService.QueryRecord<BuildingSiegeWorkshopDefine>(buildLevel + 1);
                        if (define1 != null)
                        {
                            if (define2.siegeCarTrainNumber != define1.siegeCarTrainNumber)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180332);
                                attr.curValue = ClientUtils.FormatComma(define1.siegeCarTrainNumber);
                                attr.addValue = ClientUtils.FormatComma(Mathf.Abs(define2.siegeCarTrainNumber - define1.siegeCarTrainNumber));
                                attr.symbol = define2.siegeCarTrainNumber > define1.siegeCarTrainNumber ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            if (define2.troopsSpaceMulti != define1.troopsSpaceMulti)
                            {
                                BuildingUpdateAttr attr = new BuildingUpdateAttr();
                                attr.Name = LanguageUtils.getText(180354);
                                attr.curValue = ((define1.troopsSpaceMulti) / 1000.0f).ToString("P1");
                                attr.addValue = ((Mathf.Abs(define2.troopsSpaceMulti - define1.troopsSpaceMulti)) / 1000.0f).ToString("P1");
                                attr.symbol = define2.troopsSpaceMulti > define1.troopsSpaceMulti ? BuildingUpdateAttr.Symbol.Plus : BuildingUpdateAttr.Symbol.minus;
                                m_buildingDescAttrList.Add(attr);
                            }
                            BuildingUpdateAttr attr4 = new BuildingUpdateAttr();
                            attr4.Name = powerStr;
                            attr4.curValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel));
                            attr4.addValue = ClientUtils.FormatComma(GetPower(buildType, buildLevel + 1) - GetPower(buildType, buildLevel));
                            attr4.symbol = BuildingUpdateAttr.Symbol.Plus;
                            m_buildingDescAttrList.Add(attr4);
                        }
                    }
                    break;
            }



        }
        private void OnBack()
        {

            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(false);
            view.m_pl_content_Animator.gameObject.SetActive(true);
            view.m_pl_sv_level_Animator.gameObject.SetActive(false);
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

        private void SendUpGradeBuilding(int needDenar)
        {
            UIHelper.DenarCostRemain(needDenar, () =>
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundBuildingStartLevelup);
                Build_UpGradeBuilding.request req = new Build_UpGradeBuilding.request();
                req.buildingIndex = m_buildingIndex;
                req.immediately = true;
                AppFacade.GetInstance().SendSproto(req);
            });
        }
        #region 建筑各等级详情
        //建筑各等级信息
        private void OnBuildLevelInfo()
        {
            if (view != null && view.m_pl_buildImg != null)
            {
                GameObject m_effectGO = GetEffect("UI_10015", view.m_pl_buildImg.transform);
                if (m_effectGO != null)
                {
                    GameObject.Destroy(m_effectGO);
                }
            }

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
                view.m_sv_levelData_ListView.ForceRefresh();
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
            nodeView.m_img_select_PolygonImage.gameObject.SetActive((index + 1) == m_buildingInfo.level);
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
        public void BuildingFinish()
        {
            if (readyAttrItem && readyLeft)
            {
                if (view.gameObject != null)
                {
                    view.gameObject.SetActive(true);
                }
            }
        }


    }
}