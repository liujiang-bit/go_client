using Client;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using System;
using UnityEngine;

namespace Game
{
    public class OpenUiCommonParam
    {
        public int OpenUiId;
        public int IntData;
        public int IntData2;
    }

    public class OpenUICMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OpenUI:
                    OpenUICMD.OnOpenUI((int)notification.Body);
                    break;
                case CmdConstant.OpenUI2:
                    var param = notification.Body as object[];
                    OpenUICMD.OnOpenUI((int)param[0], param[1]);
                    break;
                default:
                    break;
            }
        }

        public static void OnOpenUI(int openid, object data = null)
        {
            OpenUiDefine define = CoreUtils.dataService.QueryRecord<OpenUiDefine>(openid);
            if(define==null)
            {
                return;
            }
            if (SystemOpen.IsSystemClose(define.systemOpen))
            {
                return;
            }
            //只需要打开界面的走default
            switch (openid)
            {
                case 1000://市政厅-数据信息显示界面（升级界面里的感叹号）
                    {
                        CityBuildingProxy cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                        BuildingInfoEntity buildingInfoEntity = cityBuildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.CityWall);
                        CoreUtils.uiManager.ShowUI(UI.s_buildingUpdate, null, new UpGradeData(buildingInfoEntity.buildingIndex, 1));
                    }
                    break;
                case 1001://市政厅-增益效果界面（经济）
                    CoreUtils.uiManager.ShowUI(UI.s_GainEffect,null,0);
                    break;
                case 1002://市政厅-增益效果界面（军事）
                    CoreUtils.uiManager.ShowUI(UI.s_GainEffect, null, 1);
                    break;
                case 1003://市政厅-装扮界面-城堡皮肤
                    break;
                case 1004://市政厅-装扮界面-铭牌皮肤
                    break;
                case 1022:
                    CoreUtils.uiManager.ShowUI(UI.s_scoutWin, null);
                    break;
                case 1027:
                    CoreUtils.uiManager.ShowUI(UI.s_theWall, null);
                    break;
                case 1013://学院-经济科技
                    CoreUtils.uiManager.ShowUI(UI.s_ResearchMain,()=>
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ChangeResearchMainToggle, true);
                    });
                    break;
                case 1014://学院-军事科技
                    CoreUtils.uiManager.ShowUI(UI.s_ResearchMain, () =>
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.ChangeResearchMainToggle, false);
                    });
                    break;
                case 1015://铁匠铺-材料生产界面
                    if (data == null)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Production);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, data);
                    }
                    break;
                case 1016://铁匠铺-材料和图纸合成界面-材料合成
                    if (data == null)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Mix);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, data);
                    }
                    break;
                case 1024:
                    StoreProxy m_storeProxy = AppFacade.GetInstance().RetrieveProxy(StoreProxy.ProxyNAME) as StoreProxy;
                    m_storeProxy.OpenMysteryStore();
                    break;
                case 1039://铁匠铺-材料和图纸合成界面-图纸合成
                    if (data == null)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Mix);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, data);
                    }
                    break;
                case 1017://铁匠铺-材料分解界面
                    if (data == null)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, MaterialPageType.Resolve);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Material, null, data);
                    }
                    break;
                case 1018://铁匠铺-锻造界面
                    if (data == null)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Equip, null, 1);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Equip, null, data);
                    }
                    break;
                case 1019://铁匠铺-分解界面
                    if (data == null)
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Equip, null, 2);
                    }
                    else
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Equip, null, data);
                    }
                    break;
                case 1030:
                    Debug.LogError("建筑升级界面需要定制具体规则");
                    break;
                case 4006:
                case 4007:
                case 4008:
                case 4009:
                case 4010:
                    int searchType = 0;
                    if (openid == 4006)
                    {
                        searchType = (int)SearchType.Barbarian;
                    }
                    else if (openid == 4007)
                    {
                        searchType = (int)SearchType.Farmland;
                    }
                    else if (openid == 4008)
                    {
                        searchType = (int)SearchType.Mill;
                    }
                    else if (openid == 4009)
                    {
                        searchType = (int)SearchType.Stone;
                    }
                    else if (openid == 4010)
                    {
                        searchType = (int)SearchType.Gold;
                    }
                    CoreUtils.uiManager.OpenUIByID(4001, null, searchType);
                    break;
                case 1028:
                    CoreUtils.uiManager.ShowUI(UI.s_theTower);
                    break;
                case 3002:
                    CoreUtils.uiManager.ShowUI(UI.s_itemExchange, null, data);
                    break;
                case 7000:
                    CoreUtils.uiManager.ShowUI(UI.s_Charge, null, EnumRechargeListPageType.ChargeFirst);
                    break;
                case 7001:
                    CoreUtils.uiManager.ShowUI(UI.s_Charge, null, EnumRechargeListPageType.ChargeRiseRoad);
                    break;
                case 7002:
                    CoreUtils.uiManager.ShowUI(UI.s_Charge, null, EnumRechargeListPageType.ChargeSuperGift);
                    break;
                case 7003:
                    CoreUtils.uiManager.ShowUI(UI.s_Charge, null, EnumRechargeListPageType.ChargeDayCheap);
                    break;
                case 7004:
                    CoreUtils.uiManager.ShowUI(UI.s_Charge, null, EnumRechargeListPageType.ChargeGemShop);
                    break;
                case 7005:
                    CoreUtils.uiManager.ShowUI(UI.s_Charge, null, EnumRechargeListPageType.ChargeCitySupply);
                    break;
                case 7006:
                    CoreUtils.uiManager.ShowUI(UI.s_Charge, null, EnumRechargeListPageType.ChargeGrowing);
                    break;
                case 8000:
                    CoreUtils.uiManager.ShowUI(UI.s_PlayerChangeCivilization, null, 1);
                    break;
                case 14000:
                case 14050:
                case 14100:
                case 14150:
                    CoreUtils.uiManager.ShowUI(UI.s_eventDate, null, data);//活动
                    break;
                case 14051://创角活动
                    CoreUtils.uiManager.ShowUI(UI.s_newRoleActivity);
                    break;
                case 15000:
                    CoreUtils.uiManager.ShowUI(UI.s_Vip, null, 1);
                    break;
                case 15010:
                    CoreUtils.uiManager.ShowUI(UI.s_Vip, null, 10);
                    break;
                case 5002: //联盟帮助
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceHelp);
                    break;
                case 5003: //联盟科技
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceResearchMain);
                    break;
                case 5004: //联盟领土
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceTerrtroy);
                    break;
                case 4002://城外迷雾探索界面
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
                        FogSystemMediator fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;

                        var cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                        var worldPos = fogMedia.FindClosestAtWorldPos(cityBuildingProxy.RolePos.x, cityBuildingProxy.RolePos.y);

                        int fgId = fogMedia.GetFadeGroupId(worldPos.x, worldPos.y, WarFogMgr.GROUP_SIZE);
                        WarFogMgr.RemoveFadeGroupByType(FogSystemMediator.FADE_TYPE_CLICK);
                        WarFogMgr.CreateFadeGroup(fgId, 1, 5);

                        CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
                        WorldCamera.Instance().ViewTerrainPos(worldPos.x, worldPos.y, 1000, null);
                        float dxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
                        WorldCamera.Instance().SetCameraDxf(dxf, 1000, () =>
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_scoutSearchMenuu, () =>
                            {
                                UI_Pop_ExploreMistView UI_Pop_ExploreMistView = CoreUtils.uiManager.GetUI(4002).View as UI_Pop_ExploreMistView;
                                FingerTargetParam param = new FingerTargetParam();
                                param.AreaTarget = UI_Pop_ExploreMistView.m_UI_Model_StandardButton_Yellow.m_btn_languageButton_GameButton.gameObject;
                                param.ArrowDirection = (int)EnumArrorDirection.Up;
                                CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                            }, worldPos);
                        });
                    }
                    break;
                case 1025://医院
                    {
                        CityBuildingProxy cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                        BuildingInfoEntity buildingInfoEntity = cityBuildingProxy.GetMinBuildingInfoByType((long)EnumCityBuildingType.Hospital);
                        if (buildingInfoEntity != null)
                        {
                            CoreUtils.uiManager.ShowUI(UI.s_hospitalInfo, null, buildingInfoEntity.buildingIndex);
                        }
                    }
                    break;
                default:
                    CoreUtils.uiManager.OpenUIByID(openid);
                    break;
            }
        }
    }
}