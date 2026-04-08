// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Win_AssitResMediator
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
using System;
using UnityEngine.UI;

namespace Game {

    public class TransportData
    {
        public int resourceTypeId;
        public int taxinclusiveValue;//税后值
        public int preTaxValue;//税前值
    }
    public class UI_Win_AssitResMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_AssitResMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private AllianceProxy m_allianceProxy;
        private TroopProxy m_TroopProxy;

        private int m_time;//预估时间

        private long m_targetRid;//目标rid

        private List<TransportData> transportResourceInfos = new List<TransportData>();
        private GrayChildrens makeChildrenGray;

        private BuildingFreightDefine buildingFreightDefine;
        private GuildMemberInfoEntity m_mapObjectInfoEntity;

        private GameObject m_effectGO;
        
        private int foodNum = 0;
        private int woodNum = 0;
        private int stonrNum = 0;
        private int goldNum = 0;
        int maxcount = 0; 
        int tax = 0; 
        #endregion

        //IMediatorPlug needs
        public UI_Win_AssitResMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_AssitResView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {
                Transport_CreateTransport.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Transport_CreateTransport.TagName:
                    {
                        Transport_CreateTransport.response response = notification.Body as Transport_CreateTransport.response;
                        if (response != null)
                        {
                      //      Debug.LogError(response.transportIndex + "ssssssssssssssssssss");
                       //     Debug.LogErrorFormat("{0},,,,{0},,,,{2},,,,,{3}",m_playerProxy.CurrentRoleInfo.food,m_playerProxy.CurrentRoleInfo.wood,m_playerProxy.CurrentRoleInfo.stone,m_playerProxy.CurrentRoleInfo.gold);
                        }
                    }

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

        public override void PrewarmComplete() {

        }

        public override void Update()
        {


        }

        protected override void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            if (view.data is Transport_GetTransport.response)
            {
                Transport_GetTransport.response response = view.data as Transport_GetTransport.response;
                m_time = (int)response.time;
                m_targetRid = response.targetRid;
              //  m_targetRid = 40000011;//TODO:
                
            }
            BuildingInfoEntity buildingInfoEntity = m_cityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.TradingPost);
            if (buildingInfoEntity != null)
            {
                if (buildingInfoEntity.level >= 1)
                {
                    buildingFreightDefine = CoreUtils.dataService.QueryRecord<BuildingFreightDefine>((int)buildingInfoEntity.level);
                }
            }
            makeChildrenGray = view.m_UI_transport.m_root_RectTransform.transform.gameObject.AddComponent<GrayChildrens>();
             m_mapObjectInfoEntity = m_allianceProxy.getMemberInfo(m_targetRid);
            if (m_mapObjectInfoEntity == null)
            {
                Debug.LogError("not find targetRid");
            }
            {
                TransportData transportResourceInfo = new TransportData();
                transportResourceInfo.resourceTypeId = 100;
                transportResourceInfo.taxinclusiveValue = 0;
                transportResourceInfos.Add(transportResourceInfo);
            }
            {
                TransportData transportResourceInfo = new TransportData();
                transportResourceInfo.resourceTypeId = 101;
                transportResourceInfo.taxinclusiveValue = 0;
                transportResourceInfos.Add(transportResourceInfo);

            }
            {
                TransportData transportResourceInfo = new TransportData();
                transportResourceInfo.resourceTypeId = 102;
                transportResourceInfo.taxinclusiveValue = 0;
                transportResourceInfos.Add(transportResourceInfo);

            }
            {
                TransportData transportResourceInfo = new TransportData();
                transportResourceInfo.resourceTypeId = 103;
                transportResourceInfo.taxinclusiveValue = 0;
                transportResourceInfos.Add(transportResourceInfo);
            }

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider.onValueChanged.AddListener((value) => {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
                RefSliderView(EnumResType.Food, value);
            });
            view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider.onValueChanged.AddListener((value) => {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
                RefSliderView(EnumResType.Wood, value);
            });
            view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider.onValueChanged.AddListener((value) => {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
                RefSliderView(EnumResType.Stone, value);
            });
            view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider.onValueChanged.AddListener((value) => {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
                RefSliderView(EnumResType.Gold, value);
            });
            view.m_UI_transport.m_btn_languageButton_GameButton.onClick.AddListener(OnTransportBrnClick);

            view.m_UI_Model_Window_Type1.AddCloseEvent(OnCloseBtnClick);
            view.m_UI_Item_AssitResItem1.m_ipt_count_GameInput.onEndEdit.AddListener((txt) =>{
                OnEditEnd(txt , EnumResType.Food , view.m_UI_Item_AssitResItem1.m_ipt_count_GameInput , view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider);
            });
            view.m_UI_Item_AssitResItem2.m_ipt_count_GameInput.onEndEdit.AddListener((txt) => {
                OnEditEnd(txt , EnumResType.Wood , view.m_UI_Item_AssitResItem2.m_ipt_count_GameInput, view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider);
            });
            view.m_UI_Item_AssitResItem3.m_ipt_count_GameInput.onEndEdit.AddListener((txt) => {
                OnEditEnd(txt , EnumResType.Stone , view.m_UI_Item_AssitResItem3.m_ipt_count_GameInput, view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider);
            });
            view.m_UI_Item_AssitResItem4.m_ipt_count_GameInput.onEndEdit.AddListener((txt) =>{
                OnEditEnd(txt , EnumResType.Gold , view.m_UI_Item_AssitResItem4.m_ipt_count_GameInput, view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider);
            });
        }

        protected override void BindUIData()
        {
        
            tax = buildingFreightDefine.tax;
            maxcount = (int)Math.Round((buildingFreightDefine.capacity / (1000f - tax) * 1000f), 0);
            view.m_UI_transport.SetNum(ClientUtils.FormatCountDown(m_time));
            float width1 = view.m_UI_transport.m_lbl_line2_LanguageText.preferredWidth;
            view.m_UI_transport.m_lbl_line2_LanguageText.GetComponent<RectTransform>().sizeDelta = new Vector2(width1, 27.7f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(view.m_UI_transport.m_pl_line2_HorizontalLayoutGroup.GetComponent<RectTransform>());
            view.m_UI_transport.SetText(LanguageUtils.getText(730258));
            view.m_lbl_tax_LanguageText.text = LanguageUtils.getTextFormat(184000, (tax / 1000f).ToString("P0"));
            if (m_mapObjectInfoEntity != null)
            {
                view.m_lbl_name_LanguageText.text = m_mapObjectInfoEntity.name;
                view.m_UI_PlayerHead.LoadPlayerIcon(m_mapObjectInfoEntity.headId, m_mapObjectInfoEntity.headFrameID);
            }

            ClientUtils.TextSetColor(view.m_lbl_taxRate_LanguageText, "#A52A2A");


            view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider.minValue = 0;
            view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider.maxValue = m_playerProxy.CurrentRoleInfo.food;
            if (m_playerProxy.CurrentRoleInfo.food == 0)
            {
                view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider.interactable = false;
            }
            view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider.minValue = 0;
            view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider.maxValue = m_playerProxy.CurrentRoleInfo.wood;
            if (m_playerProxy.CurrentRoleInfo.wood == 0)
            {
                view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider.interactable = false;
            }
            view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider.minValue = 0;
            view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider.maxValue = m_playerProxy.CurrentRoleInfo.stone;
            if (m_playerProxy.CurrentRoleInfo.stone == 0)
            {
                view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider.interactable = false;
            }
            view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider.minValue = 0;
            view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider.maxValue = m_playerProxy.CurrentRoleInfo.gold;
            if (m_playerProxy.CurrentRoleInfo.gold == 0)
            {
                view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider.interactable = false;
            }

            view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider.value = 0;
            view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider.value = 0;
            view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider.value = 0;
            view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider.value = 0;
            RefSliderView(EnumResType.Food, 0);
            RefSliderView(EnumResType.Wood, 0);
            RefSliderView(EnumResType.Stone, 0);
            RefSliderView(EnumResType.Gold, 0);

            CoreUtils.assetService.Instantiate("UI_10031", (go) =>
            {
                if (m_effectGO != null)
                {
                    CoreUtils.assetService.Destroy(m_effectGO);
                    m_effectGO = null;
                }

                m_effectGO = go;
                go.transform.SetParent(view.m_pl_headEffect);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            });
        }

        #endregion

        private void OnEditEnd(string txt,EnumResType type ,GameInput ipt , GameSlider sl)
        {
            long.TryParse(txt.Replace(",",""), out var value);
            RefSliderView(type, value);
            RefreshSliderByInput(type, value ,ipt);
            ipt.text = ClientUtils.FormatComma((int)sl.value);
        }
        private void OnCloseBtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_assitRes);
        }
        private void OnTransportBrnClick()
        {
            
            var count = m_TroopProxy.GetAllTroopCount();
            var max = m_TroopProxy.GetTroopDispatchNum();
            if (count >= max)
            {
                Tip.CreateTip(184030).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
            
            if (!m_allianceProxy.HasJionAlliance())
            {
            }
            // m_targetRid
            if (m_allianceProxy.getMemberInfo(m_targetRid) == null)
            {
                Tip.CreateTip(732019).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }
                
          //  Debug.LogErrorFormat("{0},,{0},,{2},,,,{3}", m_playerProxy.CurrentRoleInfo.food, m_playerProxy.CurrentRoleInfo.wood, m_playerProxy.CurrentRoleInfo.stone, m_playerProxy.CurrentRoleInfo.gold);
            
            Transport_CreateTransport.request request = new Transport_CreateTransport.request();
            request.transportResourceInfo = new List<TransportResourceInfo>();
            transportResourceInfos.ForEach((TransportData) => {
                TransportResourceInfo transportResourceInfo = new TransportResourceInfo();
                transportResourceInfo.resourceTypeId = TransportData.resourceTypeId;
                transportResourceInfo.load = TransportData.preTaxValue;
                request.transportResourceInfo.Add(transportResourceInfo);
            });
            request.targetRid = m_targetRid;
            var isZero = true;
            foreach (var v in request.transportResourceInfo)
            {
                if (v.load > 0)
                {
                    isZero = false;
                    break;
                }
            }

            if (!isZero)
            {
                CoreUtils.uiManager.CloseUI(UI.s_assitRes);
                AppFacade.GetInstance().SendSproto(request);
            }

        }

        private void RefreshSliderByInput(EnumResType type ,float value , GameInput ipt)
        {
//            if (float.TryParse(txt,out var value))
//            {
                if (value <= 0)
                {
                    value = 0;
                }
                switch (type)
                {
                    case EnumResType.Food:
                        view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider.value = value;
                        break;
                    case EnumResType.Wood:
                        view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider.value = value;
                        break;
                    case EnumResType.Stone:
                        view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider.value = value;
                        break;
                    case EnumResType.Gold:
                        view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider.value = value;
                        break;
                }
//            }
        }
        
        private void RefSliderView(EnumResType enumResType, float value)
        {
            int count = 0;
            int taxcount = 0;
            taxcount = (int)value - Mathf.FloorToInt(value * tax / 1000f + 0.5f);
            switch (enumResType)
            {
                case EnumResType.Food:
                    transportResourceInfos[0].taxinclusiveValue = (int)taxcount;
                    transportResourceInfos[0].preTaxValue = (int)value;
                    transportResourceInfos.ForEach((transportResourceInfo) => {
                        count += (int)transportResourceInfo.taxinclusiveValue;
                    });
                    if (count >= buildingFreightDefine.capacity)
                    {
                        count = buildingFreightDefine.capacity;
                        taxcount = buildingFreightDefine.capacity - (int)(transportResourceInfos[1].taxinclusiveValue + transportResourceInfos[2].taxinclusiveValue + transportResourceInfos[3].taxinclusiveValue);
                         value = maxcount- (int)(transportResourceInfos[1].preTaxValue + transportResourceInfos[2].preTaxValue + transportResourceInfos[3].preTaxValue);
                        view.m_UI_Item_AssitResItem1.m_pb_bar_GameSlider.value = value ;
                        transportResourceInfos[0].taxinclusiveValue = (int)taxcount;
                        transportResourceInfos[0].preTaxValue = (int)value;
                    }
                    view.m_UI_Item_AssitResItem1.m_ipt_count_GameInput.text = value.ToString("N0");
                    break;
                case EnumResType.Wood:
                    transportResourceInfos[1].taxinclusiveValue = (int)taxcount;
                    transportResourceInfos[1].preTaxValue = (int)value;
                    transportResourceInfos.ForEach((transportResourceInfo) => {
                        count += (int)transportResourceInfo.taxinclusiveValue;
                    });
                    if (count >= buildingFreightDefine.capacity)
                    {
                        count = buildingFreightDefine.capacity;
                        taxcount = buildingFreightDefine.capacity - (int)(transportResourceInfos[0].taxinclusiveValue + transportResourceInfos[2].taxinclusiveValue + transportResourceInfos[3].taxinclusiveValue);
                        value = maxcount - (int)(transportResourceInfos[0].preTaxValue + transportResourceInfos[2].preTaxValue + transportResourceInfos[3].preTaxValue);
                        view.m_UI_Item_AssitResItem2.m_pb_bar_GameSlider.value = value ;
                        transportResourceInfos[1].taxinclusiveValue = (int)taxcount;
                        transportResourceInfos[1].preTaxValue = (int)value;
                    }
                    view.m_UI_Item_AssitResItem2.m_ipt_count_GameInput.text = value.ToString("N0");
                    break;
                case EnumResType.Stone:
                    transportResourceInfos[2].taxinclusiveValue = (int)taxcount;
                    transportResourceInfos[2].preTaxValue = (int)value;
                    transportResourceInfos.ForEach((transportResourceInfo) => {
                        count += (int)transportResourceInfo.taxinclusiveValue;
                    });
                    if (count >= buildingFreightDefine.capacity)
                    {
                        count = buildingFreightDefine.capacity;
                        taxcount = buildingFreightDefine.capacity - (int)(transportResourceInfos[0].taxinclusiveValue + transportResourceInfos[1].taxinclusiveValue + transportResourceInfos[3].taxinclusiveValue);
                        value = maxcount - (int)(transportResourceInfos[0].preTaxValue + transportResourceInfos[1].preTaxValue + transportResourceInfos[3].preTaxValue);
                        view.m_UI_Item_AssitResItem3.m_pb_bar_GameSlider.value = value;
                        transportResourceInfos[2].taxinclusiveValue = (int)taxcount;
                        transportResourceInfos[2].preTaxValue = (int)value;
                    }
                    view.m_UI_Item_AssitResItem3.m_ipt_count_GameInput.text = value.ToString("N0");
                    break;
                case EnumResType.Gold:
                    transportResourceInfos[3].taxinclusiveValue = (int)taxcount;
                    transportResourceInfos[3].preTaxValue = (int)value;
                    transportResourceInfos.ForEach((transportResourceInfo) => {
                        count += (int)transportResourceInfo.taxinclusiveValue;
                    });
                    if (count >= buildingFreightDefine.capacity)
                    {
                        count = buildingFreightDefine.capacity;
                        taxcount = buildingFreightDefine.capacity - (int)(transportResourceInfos[0].taxinclusiveValue + transportResourceInfos[1].taxinclusiveValue + transportResourceInfos[2].taxinclusiveValue);
                        value = maxcount - (int)(transportResourceInfos[0].preTaxValue + transportResourceInfos[1].preTaxValue + transportResourceInfos[2].preTaxValue);
                        view.m_UI_Item_AssitResItem4.m_pb_bar_GameSlider.value = value;
                        transportResourceInfos[3].taxinclusiveValue = (int)taxcount;
                        transportResourceInfos[3].preTaxValue = (int)value;
                    }
                    view.m_UI_Item_AssitResItem4.m_ipt_count_GameInput.text = value.ToString("N0");
                    break;
                default:
                    Debug.LogError(" not find type");
                    break;
            }

        
            view.m_lbl_capatiy_LanguageText.text = LanguageUtils.getTextFormat(300001, count.ToString("N0"), buildingFreightDefine.capacity.ToString("N0"));

            int nRate = 0;
            transportResourceInfos.ForEach((transportResourceInfo) => {
                nRate += (int)transportResourceInfo.preTaxValue - transportResourceInfo.taxinclusiveValue;
            });

            view.m_lbl_taxRate_LanguageText.text = nRate.ToString("N0");

            var isZero = true;
            foreach (var v in transportResourceInfos)
            {
                if ( v.taxinclusiveValue > 0)
                {
                    isZero = false;
                    break;
                }
            }
            if (isZero)
            {
                makeChildrenGray.Gray();
                view.m_UI_transport.m_btn_languageButton_GameButton.interactable = false;
            }
            else
            {
                makeChildrenGray.Normal();
                view.m_UI_transport.m_btn_languageButton_GameButton.interactable = true;
            }
        }

        /// <summary>
        /// 税前转税后
        /// </summary>
        /// <returns></returns>
        private int Taxinclusive(int preTax)
        {
            return  (int)Math.Round((preTax * (1000 - tax) / 1000f), 0);
        }

        private int PreTax(long  taxinclusive)
        {
            return  (int)Math.Round((((float)taxinclusive )/ (1000 - tax) * 1000f), 0);
        }
    }
}