using PureMVC.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using Data;
using SprotoType;
using Client;
using System;
using DG.Tweening;

namespace Game
{
    public class GlobalEffectMediator : GameMediator
    {

        #region Member
        private Dictionary<string, GameObject> m_assets = new Dictionary<string, GameObject>();

        //正在进行战力飘飞动画
        public static int IsPlayingPowerUpEffect = 0;

        //正在飘飞的货币动画
        public static int IsPlayingFoodEffect = 0;
        public static int IsPlayingWoodEffect = 0;
        public static int IsPlayingStoneEffect = 0;
        public static int IsPlayingGoldEffect = 0;
        public static int IsPlayingDenarEffect = 0;
        public static int IsPlayingConquerorMedalEffect = 0;

        public ConfigDefine m_config;

        private Dictionary<int, CurrencyDefine> m_currencyDefines;
        public Dictionary<int, CurrencyDefine> CurrencyDefine
        {
            get
            {
                if (m_currencyDefines == null)
                {
                    m_currencyDefines = new Dictionary<int, CurrencyDefine>();
                    foreach (var item in CoreUtils.dataService.QueryRecords<CurrencyDefine>())
                    {
                        m_currencyDefines[item.ID] = item;
                    }
                }
                return m_currencyDefines;
            }
        }
        //连续播放的持续时间
        public static float QuickShowTime = 0.1f;
        private Queue<long> m_actionForceDequeue = new Queue<long>();//行动力扣除队列
        private bool m_IsActionForceDequeue = false;
        private string iconActionForce = string.Empty;
        #endregion

        public static string NameMediator = "GlobalEffectMediator";

        public GlobalEffectMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        public GlobalEffectMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
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
            IsPlayingPowerUpEffect = 0;
        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            iconActionForce = CurrencyDefine[105].iconID;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            ClientUtils.PreLoadRes(CoreUtils.uiManager.GetCanvas().gameObject, new List<string>
                    {
                        m_config.flyingFoodRes,
                        m_config.flyingWoodRes,
                        m_config.flyingStoneRes,
                        m_config.flyingGlodRes,
                        m_config.flyingDenarRes,
                        m_config.flyingMedalRes,
                        "UE_ResFly_ActionPoint"
                    }, OnLoadFinish);
        }

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assets = asset;
        }

        #endregion
        public void FlyPowerUpEffect(GameObject go,RectTransform Pos,Vector3 scale,Action callback=null, float effect_scale = 1f, bool isCopyGo = true)
        {
            if (go == null)
            {
                Debug.LogError("飘飞物体为空");
            }
            else
            {
                if (Pos.gameObject == null)
                {
                    Debug.LogError("FlyPowerUpEffect StartPos Object is null");
                    return;
                }
                IsPlayingPowerUpEffect += 1;
                MainInterfaceMediator mt = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
                if (mt != null)
                {
                    RssAniGroup rss = RssAniGroup.Register().SetStartPos(Pos).SetEndPos(mt.view.m_UI_Item_PlayerPowerInfo.m_btn_powerShow_PolygonImage.rectTransform);
                    GameObject copyGO = null;
                    if (isCopyGo)
                    {
                        copyGO = GameObject.Instantiate(go, CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    }
                    else
                    {
                        copyGO = go;
                    }
                    copyGO.SetActive(false);
                    RectTransform rectTrans = copyGO.GetComponent<RectTransform>();
                    rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
                    CoreUtils.assetService.Instantiate("UI_10019", (effect) =>
                    {
                        copyGO.SetActive(true);
                        effect.transform.SetParent(copyGO.transform);
                        effect.transform.localScale = Vector3.one;
                        effect.transform.localPosition = Vector3.zero;
                        effect.gameObject.SetActive(true);
                        var trails = effect.GetComponentsInChildren<TrailRenderer>();
                        Timer.Register(0.5f, () =>
                        {
                            effect.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                            effect.transform.localScale = new Vector3(effect_scale, effect_scale, effect_scale);
                            //effect.transform.localPosition = Vector3.zero;
                            rss.m_rssAniItems[0].m_gameObject = effect;
                            rss.m_rssAniItems[0].m_baseScale = new Vector3(effect_scale, effect_scale, effect_scale);
                            effect.gameObject.SetActive(true);
                            if (trails != null)
                            {
                                for (int i = 0; i < trails.Length; i++)
                                {
                                    trails[i].gameObject.SetActive(false);
                                }
                            }
                            Timer.Register(0.2f, () =>
                            {
                                GameObject.DestroyImmediate(copyGO);
                                if (trails != null)
                                {
                                    for (int i = 0; i < trails.Length; i++)
                                    {
                                        trails[i].gameObject.SetActive(true);
                                    }
                                }
                            });
                        });
                        if (copyGO != null)
                        {
                            rss.SetAniTime(0.7f, 0.7f).SetAniDoneCallBack(() =>
                            {
                                IsPlayingPowerUpEffect -= 1;
                                AppFacade.GetInstance().SendNotification(CmdConstant.ForceUpdatePlayerPower);
                                callback?.Invoke();
                            }).FlyItem(copyGO, scale, 1, false);
                        }
                    });
                }
            }
        }

        public void FlyPowerUpNoEffect(GameObject go, RectTransform Pos, Vector3 scale, Action callback = null, float effect_scale = 1f, bool isCopyGo = true)
        {
            if (go == null)
            {
                Debug.LogError("飘飞物体为空");
            }
            else
            {
                if (Pos.gameObject == null)
                {
                    Debug.LogError("FlyPowerUpEffect StartPos Object is null");
                    return;
                }
                IsPlayingPowerUpEffect += 1;
                MainInterfaceMediator mt = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
                if (mt != null)
                {
                    RssAniGroup rss = RssAniGroup.Register().SetStartPos(Pos).SetEndPos(mt.view.m_UI_Item_PlayerPowerInfo.m_btn_powerShow_PolygonImage.rectTransform);
                    GameObject copyGO = null;
                    if (isCopyGo)
                    {
                        copyGO = GameObject.Instantiate(go, CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    }
                    else
                    {
                        copyGO = go;
                    }
                    copyGO.SetActive(false);
                    RectTransform rectTrans = copyGO.GetComponent<RectTransform>();
                    rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
                    Timer.Register(0.02f, () =>
                    {
                        copyGO.SetActive(true);
                        Timer.Register(0.5f, () =>
                        {
                            rss.m_rssAniItems[0].m_gameObject = copyGO;
                            rss.m_rssAniItems[0].m_baseScale = Vector3.one;

                            Timer.Register(0.2f, () =>
                            {
                                copyGO.SetActive(false);
                            });
                        });
                        if (copyGO != null)
                        {
                            rss.SetAniTime(0.7f, 0.7f).SetAniDoneCallBack(() =>
                            {
                                IsPlayingPowerUpEffect -= 1;
                                AppFacade.GetInstance().SendNotification(CmdConstant.ForceUpdatePlayerPower);
                                callback?.Invoke();
                            }).FlyItem(copyGO, scale, 1, false);
                        }
                    });
                }
            }
        }

        #region 货币飘飞

        public void FlyUICurrency(int currencyID, long num, Vector3 startPos, Vector3 endPos, Action callBack = null,bool uiPos = false,
            bool worldPos = true)
        {
            int flyItemNum = CaculateFlyItemNum(num);
            float time1 = m_config.flyingTimePhase1 / 1000f;
            float time2 = m_config.flyingTimePhase2 / 1000f;
            float scale1 = m_config.flyingInitialZoom / 1000f;
            float scale2 = m_config.flyingFinishZoom / 1000f;
            string assetName = GetCurrencyRes(currencyID);
            switch (currencyID)
            {
                case (int)EnumCurrencyType.food: CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[0]); break;
                case (int)EnumCurrencyType.wood: CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[1]); break;
                case (int)EnumCurrencyType.stone: CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[2]); break;
                case (int)EnumCurrencyType.gold: CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[3]); break;
                case (int)EnumCurrencyType.denar: CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[4]); break;
                case (int)EnumCurrencyType.conquerorMedal: CoreUtils.audioService.PlayOneShot(RS.HarvestRssSound[5]); break;
                case (int)EnumCurrencyType.activePoint:  break;
                default: break;
            }
   
            if (m_assets.ContainsKey(assetName))
            {
                GameObject go = m_assets[assetName];
                List<GameObject> targets = new List<GameObject>();
                Canvas tipLayer = CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer).GetComponent<Canvas>();
                for (int i = 0; i < flyItemNum; i++)
                {
                    GameObject tmp = CoreUtils.assetService.Instantiate(go);
                    tmp.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    tmp.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
                    SpriteRenderer sr = tmp.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sortingOrder = tipLayer.sortingOrder + i * 50;
                    }
                    targets.Add(tmp);
                }
                if (!uiPos)
                {
                    if (worldPos)
                    {
                        var startVec = GetUIPos(startPos);
                        var endVec = GetUIPos(endPos);
                        startPos.Set(startVec.x, startVec.y, 0);
                        endPos.Set(endVec.x, endVec.y, 0);
                    }
                    else
                    {
                        var startVec = GetUIPosFromScreen(startPos);
                        var endVec = GetUIPosFromScreen(endPos);
                        startPos.Set(startVec.x, startVec.y, 0);
                        endPos.Set(endVec.x, endVec.y, 0);
                    }
                }

                switch (currencyID)
                {
                    case (int)EnumCurrencyType.food: IsPlayingFoodEffect++; break;
                    case (int)EnumCurrencyType.wood: IsPlayingWoodEffect++; break;
                    case (int)EnumCurrencyType.stone: IsPlayingStoneEffect++; break;
                    case (int)EnumCurrencyType.gold: IsPlayingGoldEffect++; break;
                    case (int)EnumCurrencyType.denar: IsPlayingDenarEffect++; break;
                    case (int)EnumCurrencyType.conquerorMedal: IsPlayingConquerorMedalEffect++;break;
                    default:break;
                }

                RssAniGroup rss = RssAniGroup.Register(startPos, endPos).SetScale2D(scale1, scale2).SetAniTime(time1, time2)
                    .SetStartReachCallback(() =>
                    {
                        AppFacade.GetInstance().SendNotification(CmdConstant.PlayCurrencyPopAni, new CurrencyPopAniParam { type = (EnumCurrencyType)currencyID, play = true });
                    })
                    .SetAniDoneCallBack(() =>
                {
                    switch (currencyID)
                    {
                        case (int)EnumCurrencyType.food: IsPlayingFoodEffect--; break;
                        case (int)EnumCurrencyType.wood: IsPlayingWoodEffect--; break;
                        case (int)EnumCurrencyType.stone: IsPlayingStoneEffect--; break;
                        case (int)EnumCurrencyType.gold: IsPlayingGoldEffect--; break;
                        case (int)EnumCurrencyType.denar: IsPlayingDenarEffect--; break;
                        case (int)EnumCurrencyType.conquerorMedal:IsPlayingConquerorMedalEffect--;break;
                        default: break;
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.PlayCurrencyPopAni, new CurrencyPopAniParam { type = (EnumCurrencyType)currencyID, play = false });
                    AppFacade.GetInstance().SendNotification(CmdConstant.FlyUpdatePlayerCurrency, currencyID);
                    callBack?.Invoke();
                }).SetAniGameObjects(targets.ToArray());
            }
            else
            {
                CoreUtils.logService.Error("货币飘飞表现预制加载失败：" + currencyID);
                callBack?.Invoke();
            }
        }
        public void FlyUICurrency(int currencyID, long num, Vector3 startPos, Action callBack = null)
        {
            Vector2 pos = GetUIPos(startPos);
            CurrencyDefine currencyDefine = CurrencyDefine[currencyID];
            FlyUICurrency(currencyID,num,pos,GetTargetUIPos(currencyDefine),callBack,true,false);
        }

        public void FlyUICurrencyFromWorld(EnumCurrencyType currencyType, long num, Vector3 startPos, Action callBack = null)
        {
            FlyUICurrency((int)currencyType, num, startPos, callBack);
        }


        private Vector3 GetTargetUIPos(CurrencyDefine currencyDefine)
        {
            MainInterfaceMediator mt = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
            switch ((EnumCurrencyType)currencyDefine.ID)
            {
                case EnumCurrencyType.food:
                    if (mt != null)
                    {
                        return GetUIPos(mt.view.m_UI_Item_PlayerResources.m_UI_Model_food.m_img_icon_PolygonImage.transform.position);
                    }
                    return new Vector3(m_config.flyingFood_X, m_config.flyingFood_Y);
                case EnumCurrencyType.wood:
                    if (mt != null)
                    {
                        return GetUIPos(mt.view.m_UI_Item_PlayerResources.m_UI_Model_wood.m_img_icon_PolygonImage.transform.position);
                    }
                    return new Vector3(m_config.flyingWood_X, m_config.flyingWood_Y);
                case EnumCurrencyType.stone:
                    if (mt != null)
                    {
                        return GetUIPos(mt.view.m_UI_Item_PlayerResources.m_UI_Model_stone.m_img_icon_PolygonImage.transform.position);
                    }
                    return new Vector3(m_config.flyingStone_X, m_config.flyingStone_Y);
                case EnumCurrencyType.gold:
                    if (mt != null)
                    {
                        return GetUIPos(mt.view.m_UI_Item_PlayerResources.m_UI_Model_gold.m_img_icon_PolygonImage.transform.position);
                    }
                    return new Vector3(m_config.flyingGlod_X, m_config.flyingGlod_Y);
                case EnumCurrencyType.denar:
                case EnumCurrencyType.conquerorMedal:
                    if (mt != null)
                    {
                        return GetUIPos(mt.view.m_UI_Item_PlayerResources.m_UI_Model_gem.m_img_icon_PolygonImage.transform.position);
                    }
                    return Vector3.zero;
                default:
                    CoreUtils.logService.Error("未增加该飘飞表现目标的最终位置");
                    return Vector3.zero;
            }
        }

        private Vector2 GetUIPos(Vector3 worldPos)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(worldPos), CoreUtils.uiManager.GetUICamera(), out pos);
            return pos;
        }

        private Vector2 GetUIPosFromScreen(Vector2 screenPos)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, screenPos, CoreUtils.uiManager.GetUICamera(), out pos);
            return pos;
        }

        private Vector2 GetUIPosFromWorld(Vector3 worldPos)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), worldPos);
            return pos;
        }

        private string GetCurrencyRes(int id)
        {
            switch ((EnumCurrencyType)id)
            {
                case EnumCurrencyType.food:
                    return m_config.flyingFoodRes;
                case EnumCurrencyType.wood:
                    return m_config.flyingWoodRes;
                case EnumCurrencyType.stone:
                    return m_config.flyingStoneRes;
                case EnumCurrencyType.gold:
                    return m_config.flyingGlodRes;
                case EnumCurrencyType.denar:
                    return m_config.flyingDenarRes;
                case EnumCurrencyType.conquerorMedal:
                    return m_config.flyingMedalRes;
                case EnumCurrencyType.activePoint:
                    return "UE_ResFly_ActionPoint";
                default:
                    CoreUtils.logService.Error("未添加该飘飞资源");
                    return string.Empty;
            }
        }

        private int CaculateFlyItemNum(long originNum)
        {
            if (originNum <= 10)
            {
                return (int)originNum;
            }
            if (originNum <= 1000)
            {
                return m_config.flyingNum_1;
            }
            else if (originNum <= 10000)
            {
                return m_config.flyingNum_2;
            }
            else if (originNum <= 50000)
            {
                return m_config.flyingNum_3;
            }
            else if (originNum <= 150000)
            {
                return m_config.flyingNum_4;
            }
            else if (originNum <= 500000)
            {
                return m_config.flyingNum_5;
            }
            else if (originNum <= 1500000)
            {
                return m_config.flyingNum_6;
            }
            else
            {
                return m_config.flyingNum_7;
            }
        }

        #endregion
        /// <summary>
        /// 行动力扣除
        /// </summary>
        /// <param name="go"></param>
        /// <param name="Pos"></param>
        /// <param name="scale"></param>
        /// <param name="callback"></param>
        public void FlyActionForceEffect(long temp)
        {
            m_actionForceDequeue.Enqueue(temp);
            if (m_IsActionForceDequeue)
            {
                return;
            }
            FlyActionForceEffectDequeue();
        }
        private void FlyActionForceEffectDequeue()
        {
            if (m_actionForceDequeue.Count <= 0)
            {
                return;
            }
                m_IsActionForceDequeue = true;
            long actionForce = m_actionForceDequeue.Dequeue();
            Timer.Register(QuickShowTime, () =>
            {
                m_IsActionForceDequeue = false;
                FlyActionForceEffectDequeue();
            });
            MainInterfaceMediator mt = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
            if (mt != null)
            {
                ClientUtils.UIAddEffect(RS.ActionForceFly, mt.view.m_UI_Item_PlayerPowerInfo.m_pb_ap_ArabLayoutCompment.transform, (go)=> {
                    UI_Item_EneryUse_SubView uI_Item_EneryUse_SubView = new UI_Item_EneryUse_SubView(go.transform as RectTransform);
                    uI_Item_EneryUse_SubView.m_lbl_use_LanguageText.text = actionForce.ToString();
                }, true, 2);
            }
        }
        public void FlyItemEffect(int itemID, int overlay, Vector3 pos, Action callback = null)
        {
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
            if (itemDefine == null)
            {
                Debug.LogError("错误的ItemID：" + itemID);
                callback?.Invoke();
                return;
            }
            MainInterfaceMediator mt = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
            RssAniGroup rss = RssAniGroup.Register().SetStartPos(pos).SetEndPos(mt.view.m_UI_Model_Item.m_root_RectTransform);
            CoreUtils.assetService.Instantiate("UI_Item_Bag", (item) =>
            {
                ItemBagView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBagView>(item);
                itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                if (itemDefine.l_topID < 1)
                {
                    itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.transform.localScale = Vector3.zero;
                }
                else
                {
                    itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.transform.localScale = Vector3.one;
                    itemView.m_UI_Model_Item.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), ClientUtils.FormatComma(itemDefine.topData));
                }
                if (overlay < 1)
                {
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = string.Empty;
                }
                else
                {
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(overlay);
                }
                ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_quality_PolygonImage, itemView.m_UI_Model_Item.GetQualityImg(itemDefine.quality));
                ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_icon_PolygonImage, itemDefine.itemIcon, false, () =>
                {
                    item.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    float time1 = m_config.flyingTimePhase1 / (float)1000;
                    float time2 = m_config.flyingTimePhase2 / (float)1000;
                    rss.SetAniTime(time1, time2).SetAniDoneCallBack(() =>
                    {
                        callback?.Invoke();
                    }).SetScale2D(1,0.5f).FlyItem(item, Vector3.one * 0.65f, 1, false);
                });
            });
        }
        public void FlyItemEffect(int itemID, int overlay, Vector3 pos, Transform endTransform, Action callback = null)
        {
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
            if (itemDefine == null)
            {
                Debug.LogError("错误的ItemID：" + itemID);
                callback?.Invoke();
                return;
            }
            MainInterfaceMediator mt = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
            RssAniGroup rss = RssAniGroup.Register().SetStartPos(pos).SetEndPos(endTransform.GetComponent<RectTransform>());
            CoreUtils.assetService.Instantiate("UI_Item_Bag", (item) =>
            {
                ItemBagView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBagView>(item);
                itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                if (itemDefine.l_topID < 1)
                {
                    itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.transform.localScale = Vector3.zero;
                }
                else
                {
                    itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.transform.localScale = Vector3.one;
                    itemView.m_UI_Model_Item.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), ClientUtils.FormatComma(itemDefine.topData));
                }
                if (overlay < 1)
                {
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = string.Empty;
                }
                else
                {
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(overlay);
                }
                ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_quality_PolygonImage, itemView.m_UI_Model_Item.GetQualityImg(itemDefine.quality));
                ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_icon_PolygonImage, itemDefine.itemIcon, false, () =>
                {
                    item.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    float time1 = m_config.flyingTimePhase1 / (float)1000;
                    float time2 = m_config.flyingTimePhase2 / (float)1000;
                    rss.SetAniTime(time1, time2).SetAniDoneCallBack(() =>
                    {
                        callback?.Invoke();
                    }).SetScale2D(1, 0.5f).FlyItem(item, Vector3.one * 0.65f, 1, false);
                });
            });
        }
        public void FlyItemEffect(int itemID, int overlay, RectTransform pos , Action callback = null)
        {
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
            if (itemDefine==null)
            {
                Debug.LogError("错误的ItemID："+itemID);
                callback?.Invoke();
                return;
            }
            MainInterfaceMediator mt = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
            if (mt == null)
            {
                return;
            }
            if (mt.view == null || mt.view.m_UI_Model_Item.m_root_RectTransform == null)
            {
                return;
            }
            RssAniGroup rss = RssAniGroup.Register().SetStartPos(pos).SetEndPos(mt.view.m_UI_Model_Item.m_root_RectTransform);
            CoreUtils.assetService.Instantiate("UI_Item_Bag", (item) =>
            {
                if (item == null)
                {
                    return;
                }
                ItemBagView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBagView>(item);
                itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                if (itemDefine.l_topID < 1)
                {
                    itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.transform.localScale = Vector3.zero;
                }
                else
                {
                    itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.transform.localScale = Vector3.one;
                    itemView.m_UI_Model_Item.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), ClientUtils.FormatComma(itemDefine.topData));
                }
                if (overlay < 1)
                {
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = string.Empty;
                }
                else
                {
                    itemView.m_UI_Model_Item.m_lbl_count_LanguageText.text = ClientUtils.FormatComma(overlay);
                }
                ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_quality_PolygonImage, itemView.m_UI_Model_Item.GetQualityImg(itemDefine.quality));
                ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_icon_PolygonImage, itemDefine.itemIcon,false,()=>
                {
                    if (item == null)
                    {
                        return;
                    }
                    item.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    float time1 = m_config.flyingTimePhase1 / (float)1000;
                    float time2 = m_config.flyingTimePhase2 / (float)1000;
                    rss.SetAniTime(time1, time2).SetAniDoneCallBack(() =>
                    {
                        callback?.Invoke();
                    }).SetScale2D(1, 0.5f).FlyItem(item,Vector3.one*0.65f,1,false);
                });
            });
        }

        public void FlyBuffEffect(MapItemTypeDefine mapItemType, Vector2 startPos, RectTransform targetTransform, Action callback = null)
        {
            if (mapItemType == null)
            {
                callback?.Invoke();
                return;
            }
            RssAniGroup rss = RssAniGroup.Register().SetStartPos(startPos).SetEndPos(targetTransform);
            CoreUtils.assetService.Instantiate("UI_Item_Bag", (item) =>
            {
                ItemBagView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemBagView>(item);
                itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                itemView.m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(false);
                itemView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
                itemView.m_UI_Model_Item.m_lbl_count_LanguageText.gameObject.SetActive(false);
                itemView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);

                ClientUtils.LoadSprite(itemView.m_UI_Model_Item.m_img_icon_PolygonImage, mapItemType.headIcon, false, () =>
                {
                    item.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    float time1 = m_config.flyingTimePhase1 / (float)1000;
                    float time2 = m_config.flyingTimePhase2 / (float)1000;
                    rss.SetAniTime(time1, time2).SetAniDoneCallBack(() =>
                    {
                        callback?.Invoke();
                    }).FlyItem(item, Vector3.one, 1, false);
                });
            });
        }

        public void FlyHeroStarUpEffect(int itemID, int overlay, RectTransform startPos, RectTransform endPos, Action callback = null)
        {
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
            if (itemDefine == null)
            {
                Debug.LogError("错误的ItemID：" + itemID);
                callback?.Invoke();
                return;
            }
            
            RssAniGroup rss = RssAniGroup.Register().SetStartPos(startPos).SetEndPos(endPos);
            CoreUtils.assetService.Instantiate("UI_10019", (item) =>
            {
                    item.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                    float time1 = m_config.flyingTimePhase1 / (float)1000;
                    float time2 = m_config.flyingTimePhase2 / (float)1000;
                    rss.SetAniTime(time1, time2).SetAniDoneCallBack(() =>
                    {
                        callback?.Invoke();
                    }).SetScale2D(1, 0.5f).FlyItem(item, Vector3.one * 0.65f, 1, false);
            });
        }
        public void FlypResolveEffect( RectTransform startPos, RectTransform endPos, Action callback = null)
        {
            RssAniGroup rss = RssAniGroup.Register().SetStartPos(startPos).SetEndPos(endPos);
            CoreUtils.assetService.Instantiate("UI_10019", (item) =>
            {
               Transform UI_10018 =  item.transform.Find("UI_10018_fly");
                Transform UI_10017 = item.transform.Find("UI_10017");
                if (UI_10018 != null)
                {
                  //  UI_10018.localScale = new Vector3(0.45f, 0.45f,0.45f);
                    UI_10018.gameObject.SetActive(false);
                }
                if (UI_10017 != null)
                {
                    UI_10017.gameObject.SetActive(true);
                }
                item.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                float time1 = (m_config.flyingTimePhase1 / (float)1000)*0.8f;
                float time2 = (m_config.flyingTimePhase2 / (float)1000) * 0.8f;
                rss.SetAniTime(time1, time2).SetAniDoneCallBack(() =>
                {
                    if (UI_10018 != null)
                    {
                        UI_10018.gameObject.SetActive(true);
                    }
                    if (UI_10017 != null)
                    {
                        UI_10017.gameObject.SetActive(false);
                    }
                    Timer.Register(1, () =>
                    {
                        if (item != null)
                        {
                            CoreUtils.assetService.Destroy(item);
                        }
                    });
                    callback?.Invoke();
                }).SetScale2D(1, 1).SetDestroy(false).FlyItem(item, Vector3.one , 1, false);
            });
        }
    }
}

