using System.Collections;
using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;
using Data;
using UnityEngine.UI;

namespace Game
{
    public class StoryCMD : GameCmd
    {
        private StoreProxy storeProxy;

        public StoreProxy StoreProxy()
        {
            if (storeProxy == null)
            {
                storeProxy = AppFacade.GetInstance().RetrieveProxy(Game.StoreProxy.ProxyNAME) as StoreProxy;
            }

            return storeProxy;
        }
        public override void Execute(INotification notification)
        {
            CityBuildingProxy m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            switch (notification.Name)
            {
                case CmdConstant.ShowMask:
                    {
                        RectTransform trans = CoreUtils.uiManager.GetUILayer((int)UILayer.StoryLayer);
                        if (trans != null)
                        {
                            Transform mask = trans.Find("StoryMask");
                            if (mask != null)
                            {
                                mask.gameObject.SetActive(true);
                            }
                            else
                            {
                                GameObject maskObj = new GameObject("StoryMask");
                                maskObj.layer = LayerMask.NameToLayer("UI");
                                maskObj.transform.SetParent(trans);
                                maskObj.transform.SetAsFirstSibling();
                                maskObj.AddComponent<Empty4Raycast>();
                                maskObj.SetActive(true);
                            }

                        }

                    }
                    break;
                case CmdConstant.HideMask:
                    {
                        RectTransform trans = CoreUtils.uiManager.GetUILayer((int)UILayer.StoryLayer);
                        if (trans != null)
                        {
                            Transform mask = trans.Find("StoryMask");
                            if (mask != null)
                            {
                                CoreUtils.assetService.Destroy(mask.gameObject);
                            }
                        }
                    }
                    break;
                case CmdConstant.OnMysteryStoreRefresh:
                    StoreProxy().OnMysteryStoreRefresh();
                    break;

                case Role_MysteryStore.TagName://神秘商店刷新提示
                    if (PlayerProxy.LoadCityFinished)
                    {
                        Tip.CreateTip(new Tip.OtherAssetData(RS.Tip_MysteryStore, null, 2f)).Show();
                    }
                    break;
                default: break;
            }
        }
    }
}

