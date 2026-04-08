using Client;
using Data;
using Hotfix.Utils;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class PlayerCmd : GameCmd
    {
        PlayerProxy m_playerProxy;
        BagProxy m_bagProxy;
        CityBuildingProxy m_cityBuildingProxy;

        private Timer m_summonObjectTimer = null;

        public override void Execute(INotification notification)
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            switch (notification.Name)
            {
                case Item_ItemInfo.TagName:
                    m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                    m_bagProxy.UpdateItemInfo(notification);
                    break;
                case Build_BuildingInfo.TagName:
                    {
                        m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                        Build_BuildingInfo.request request = notification.Body as Build_BuildingInfo.request;

                        m_cityBuildingProxy.UpdateCityBuildInfo(notification);

                        break;
                    }
                case Item_ItemUse.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            ErrorCodeHelper.ShowErrorCodeTip(error);
                            return;
                        }
                        var result = notification.Body as Item_ItemUse.response;
                        if (result == null)
                        {
                            return;
                        }
                        //ClientUtils.Print(result);
                        ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)result.itemId);
                        if (itemDefine != null)
                        {
                            string str = LanguageUtils.getTextFormat(128012, LanguageUtils.getText(itemDefine.l_nameID), ClientUtils.FormatComma(result.itemNum));
                            Tip.CreateTip(str).Show();

                            if (itemDefine.itemFunction == 10) //王国地图
                            {
                                if (result.HasRewardInfo)
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_itemCollection, null, result);
                                }
                            }
                            else if (itemDefine.itemFunction == 13)//工人招募
                            {
                                if (result.status == 1)//成功扩充第二队列
                                {
                                    Tip.CreateTip(180523).Show();
                                }
                                else if (result.status == 2)//第二队列时间延长
                                {
                                    Tip.CreateTip(180534).Show();
                                }
                                else if (result.status == 3)//道具回收处理
                                {
                                    if (result.HasRewardInfo)
                                    {
                                        CoreUtils.uiManager.ShowUI(UI.s_itemCollection, null, result);
                                    }
                                }
                            }
                            else if (itemDefine.itemFunction == 12) //召唤怪物
                            {
                                if (result.pos == null)
                                {
                                    Debug.LogError("Item_ItemUse error. pos null.");
                                    return;
                                }

                                float x = result.pos.x / 100f;
                                float y = result.pos.y / 100f;

                                var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);

                                WorldCamera.Instance().ViewTerrainPos(x, y, config.cameraMoveTime, null);
                                float Firstdxf = WorldCamera.Instance().getCameraDxf("map_tactical");
                                WorldCamera.Instance().SetCameraDxf(Firstdxf, config.cameraMoveTime, () =>
                                {
                                    FogSystemMediator fogMedia = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                                    if (fogMedia.HasFogAtWorldPos(x, y))
                                    {
                                        Tip.CreateTip(LanguageUtils.getTextFormat(500102, LanguageUtils.getText(700020), LanguageUtils.getText(170011)), Tip.TipStyle.Middle).Show();
                                        return;
                                    }

                                    CancleSummonEffectTimer();
                                    if (!createSummonEffect(result.objectIndex))
                                    {
                                        CreateSummonEffectTimer(result.objectIndex);
                                    }
                                });

                                RssProxy m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
                                m_RssProxy.SendMapMove((long)x, (long)y);
                            }
                            else
                            {

                                if (result.HasRewardInfo)
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin, null, result.rewardInfo);
                                }
                            }
                        }
                        AppFacade.GetInstance().SendNotification(CmdConstant.ItemUse);
                    }
                    break;
                case CmdConstant.SystemDayTimeChange:
                    m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                    m_bagProxy.ClearForgeEquipRedDot();
                    break;
                default:
                    break;
            }
        }

        private void CancleSummonEffectTimer()
        {
            if (m_summonObjectTimer != null)
            {
                m_summonObjectTimer.Cancel();
                m_summonObjectTimer = null;
            }
        }

        private void CreateSummonEffectTimer(long objectId)
        {
            CancleSummonEffectTimer();

            m_summonObjectTimer = Timer.Register(0, null, (time) =>
            {
                if (createSummonEffect(objectId))
                {
                    CancleSummonEffectTimer();
                }
            }, true);
        }

        private bool createSummonEffect(long objectId)
        {
            WorldMapObjectProxy m_worldMapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            MapObjectInfoEntity obj = m_worldMapProxy.GetWorldMapObjectByobjectId(objectId);
            if (obj != null && obj.gameobject != null)
            {
                CoreUtils.assetService.Instantiate("operation_2023", (go) =>
                {
                    if (obj == null || obj.gameobject == null)
                    {
                        CoreUtils.assetService.Destroy(go);
                        return;
                    }

                    go.transform.SetParent(obj.gameobject.transform);
                    go.transform.position = Vector3.zero;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                });
                return true;
            }
            return false;
        }
    }
}