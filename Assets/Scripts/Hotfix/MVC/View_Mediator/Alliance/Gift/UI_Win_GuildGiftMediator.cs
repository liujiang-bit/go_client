// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, May 15, 2020
// Update Time         :    Friday, May 15, 2020
// Class Description   :    UI_Win_GuildGiftMediator
// Copyright IGG All rights reserved.
// ===============================================================================

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

namespace Game {
    public class UI_Win_GuildGiftMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Win_GuildGiftMediator";



        private long crrKeyCount = 100;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildGiftMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Win_GuildGiftView view;

        private GuildGiftType m_seletedType = GuildGiftType.common;

        private AllianceTreasureDefine bestGiftConfig;

        private Timer m_timer;

        private long m_passTime = 0;

        private AllianceProxy m_allianceProxy;

        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();


        private List<AllianceGiftTypeDefine> m_comonGiftList =
            CoreUtils.dataService.QueryRecords<AllianceGiftTypeDefine>();

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                GuildGiftInfoEntity.GuildGiftInfoChange,
                Guild_TakeGuildGift.TagName,
                CmdConstant.AllianceGiftRedPoint,
                CmdConstant.AllianceInfoUpdate
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AllianceGiftRedPoint:
                    CheckRedPoint();
                    break;
                case GuildGiftInfoEntity.GuildGiftInfoChange:
                case CmdConstant.AllianceInfoUpdate:
                    UpdateInfo();
                    ReList();
                    break;

                case Guild_TakeGuildGift.TagName:


                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                            
                    }
                    else
                    {
                        Guild_TakeGuildGift.response info = notification.Body as Guild_TakeGuildGift.response;


                        switch (info.type)
                        {
                            case 1:

                                ClientUtils.PlaySpine(view.m_pl_box.m_UI_Model_AnimationBox_SkeletonGraphic,view.m_pl_box.m_UI_Model_AnimationBox_SkeletonGraphic.initialSkinName,"UE_TreasureBox_open");
                                
                                Timer.Register(1.5f, () =>
                                {
                                    //珍贵
                                    UpdateInfo();

                                    var reawrdProxy =
                                        AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as
                                            RewardGroupProxy;


                                    var rewardGroupDatas = reawrdProxy.GetRewardDataByRewardInfo(info.rewards);

                                    RewardGetData rewardGetData = new RewardGetData();
                                    rewardGetData.rewardGroupDataList = rewardGroupDatas;

                                    AppFacade.GetInstance().SendNotification(CmdConstant.RewardGet, rewardGetData);
                                });


                              
//                            CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin, null, info.rewards);
                                break;
                            case 2:
                                if (info.HasRewards)
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_rewardGetWin, null, info.rewards);
                                }
                                break;
                            case 3:

                                if (info.HasRewards==false)
                                {
                                    return;
                                }
                                GlobalEffectMediator mt =
                                    AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as
                                        GlobalEffectMediator;

                                MGetViewList().RefreshItem(m_index);

                                var item = MGetViewList().GetItemByIndex(m_index);

                                UI_Item_GuildGiftBoxView itemView =
                                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildGiftBoxView>(item.go);
                                GuildGiftInfoEntity data;


                                mt.FlyItemEffect((int) info.rewards.items[0].itemId,
                                    (int) info.rewards.items[0].itemNum,
                                    itemView.m_UI_Model_RewardGet.m_root_RectTransform);

                                RssAniGroup rsskey = RssAniGroup.Register()
                                    .SetStartPos(itemView.m_lbl_keyCount_LanguageText.GetComponent<RectTransform>())
                                    .SetEndPos(view.m_btn_key_GameButton.transform as RectTransform);
                                CoreUtils.assetService.Instantiate("UI_Item_GuildGiftBoxCur1", (obj) =>
                                {
                                    obj.transform.SetParent(CoreUtils.uiManager.GetUILayer((int) UILayer.TipLayer));
                                    rsskey.SetAniTime(0.1f, 1)
                                        .FlyItem(obj, Vector3.one);
                                });

                                RssAniGroup rssGem = RssAniGroup.Register()
                                    .SetStartPos(itemView.m_lbl_sourceCount_LanguageText.GetComponent<RectTransform>())
                                    .SetEndPos(view.m_btn_topPoint_GameButton.transform as RectTransform);
                                CoreUtils.assetService.Instantiate("UI_Item_GuildGiftBoxCur2", (obj) =>
                                {
                                    obj.transform.SetParent(CoreUtils.uiManager.GetUILayer((int) UILayer.TipLayer));

                                    rssGem.SetAniTime(0.1f, 1).FlyItem(obj, Vector3.one);
                                });

                                if (m_index > 0 && MGetList().Count - m_index > 1)
                                {
                                    var posY = MGetViewList().GetContainerPos();

                                    MGetViewList().ScrollToPos(posY + 100);
                                }

                                break;
                        }
                    }

                   
                    
                 
                    break;

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
            StopTime();
        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
        }

        protected override void BindUIEvent()
        {

        }


        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);

            view.m_btn_topPoint_GameButton.onClick.AddListener(onGiftTip);
            view.m_btn_question_GameButton.onClick.AddListener(onHelpTip);

            view.m_btn_delete_GameButton.onClick.AddListener(onDelAllPassGift);
            view.m_btn_getAll_GameButton.onClick.AddListener(onGetAllGift);

            view.m_ck_normal_GameToggle.onValueChanged.AddListener(onCommonTag);
            view.m_ck_rara_GameToggle.onValueChanged.AddListener(onUnCommonTag);

            view.m_btn_Box_GameButton.onClick.AddListener(onBigGiftHelp);

            view.m_btn_key_GameButton.onClick.AddListener(onBestGiftKeyGetOrHelp);
            
            SortGift(m_allianceProxy.MGiftCommons);
            SortGift(m_allianceProxy.MGiftUnCommons);

            m_preLoadRes.AddRange(view.m_sv_list_common_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                
                view.m_sv_list_common_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_common_ListView.FillContent(m_allianceProxy.MGiftCommons.Count);
                
                
                ListView.FuncTab funcTab2 = new ListView.FuncTab();
                funcTab2.ItemEnter = ViewItemByIndexUnCommon;
                
                view.m_sv_list_uncommon_ListView.SetInitData(m_assetDic, funcTab2);
                view.m_sv_list_uncommon_ListView.FillContent(m_allianceProxy.MGiftUnCommons.Count);
            });

            UpdateInfo();

            CheckRedPoint();

            m_timer = Timer.Register(1, OnTime, null, true, true);
            MGetViewList();
            
            view.m_sv_list_uncommon_ListView.gameObject.SetActive(false);
           
        }

        private int m_index;

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            if (scrollItem.go == null)
            {
                return;
            }

            UI_Item_GuildGiftBoxView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildGiftBoxView>(scrollItem.go);
            GuildGiftInfoEntity data;

            
            data = m_allianceProxy.MGiftCommons[scrollItem.index];
          

            ItemRef(itemView,data,scrollItem);
           
        }
        
        
        void ViewItemByIndexUnCommon(ListView.ListItem scrollItem)
        {
            if (scrollItem.go == null)
            {
                return;
            }

            UI_Item_GuildGiftBoxView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildGiftBoxView>(scrollItem.go);
            GuildGiftInfoEntity data;

            
                data = m_allianceProxy.MGiftUnCommons[scrollItem.index];
           

            ItemRef(itemView,data,scrollItem);
           
        }

        private void ItemRef(UI_Item_GuildGiftBoxView itemView,GuildGiftInfoEntity data,ListView.ListItem scrollItem)
        {
             var rewardConfig = CoreUtils.dataService.QueryRecord<AllianceGiftRewardDefine>((int) data.giftId);

            var giftConfig = CoreUtils.dataService.QueryRecord<AllianceGiftTypeDefine>(rewardConfig.giftType);

            bool hasGetGift = data.status == 2;

            long passTime = (giftConfig.lastTime + data.sendTime) - ServerTimeModule.Instance.GetServerTime();

            //领取按钮
            itemView.m_UI_get.gameObject.SetActive(!hasGetGift && passTime > 0);

            //钥匙和宝石
            itemView.m_pl_reward_ArabLayoutCompment.gameObject.SetActive(hasGetGift);

            //宝箱名称
            itemView.m_lbl_boxName_LanguageText.text = LanguageUtils.getText(giftConfig.l_nameId);





            if (hasGetGift)
            {
                //已领取
                var itemConfig = CoreUtils.dataService.QueryRecord<ItemDefine>((int) data.itemId);

                itemView.m_UI_Model_RewardGet.SetScale(0.6f);
                itemView.m_UI_Model_RewardGet.RefreshItem(itemConfig, (int) data.itemNum);

                itemView.m_lbl_keyCount_LanguageText.text = rewardConfig.keyPoint.ToString();
                itemView.m_lbl_sourceCount_LanguageText.text = rewardConfig.giftPoint.ToString();

                itemView.m_lbl_passTime_LanguageText.gameObject.SetActive(false);

                if (string.IsNullOrEmpty(data.buyRoleName) && data.sendType != 1)
                {
                    itemView.m_lbl_desc_LanguageText.text = "";
                }
                else
                {
                    itemView.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(733008, data.buyRoleName,
                        LanguageUtils.getText((int)data.packageNameId));
                }

                itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(762150);
            }
            else
            {
                itemView.m_UI_Model_RewardGet.GuildGift(giftConfig.iconImg);

                itemView.m_UI_get.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_get.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {

                    m_index = scrollItem.index;
                    m_allianceProxy.SendGetGift(data.giftIndex);

                });


                itemView.m_lbl_passTime_LanguageText.gameObject.SetActive(passTime > 0);
                if (passTime > 0)
                {
                    itemView.m_lbl_passTime_LanguageText.text =
                        LanguageUtils.getTextFormat(733007, ClientUtils.FormatTimeSplit((int) passTime));
                }
                else
                {
                    itemView.m_lbl_passTime_LanguageText.text = LanguageUtils.getText(733010);
                    
                    itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(733010);
                }


                if (string.IsNullOrEmpty(data.buyRoleName) && data.sendType!=1)
                {
                    if (m_seletedType == GuildGiftType.common)
                    {
                        itemView.m_lbl_desc_LanguageText.text =
                            "";
                    }
                    else
                    {
                        //联盟成员购买了“<b>{0}</b>”
                        itemView.m_lbl_desc_LanguageText.text =
                            LanguageUtils.getTextFormat(733009, LanguageUtils.getText(giftConfig.l_nameId));
                    }

                   
                }
                else
                {
                    //{0}购买了礼包“<b>{1}</b>”
                    itemView.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(733008, data.buyRoleName,
                        LanguageUtils.getText((int)data.packageNameId));
                }
            }
        }



        private void onBigGiftHelp()
        {


            if (m_allianceProxy.MTreasures.Count > 0)
            {
                m_allianceProxy.SendGetBestGift();
            }
            else
            {
                HelpTip.CreateTip(4019, view.m_btn_Box_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
            }
        }

        private void onBestGiftKeyGetOrHelp()
        {

            if (bestGiftConfig != null && crrKeyCount == bestGiftConfig.reqPoints)
            {
                m_allianceProxy.SendGetBestGift();
            }
            else
            {
                HelpTip.CreateTip(4018, view.m_btn_key_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).Show();
            }

        }



        private void onCommonTag(bool value)
        {
            view.m_btn_getAll_GameButton.gameObject.SetActive(value);
            if (value)
            {
                m_seletedType = GuildGiftType.common;
            }

            ReList();
        }

        private void onUnCommonTag(bool value)
        {
            view.m_btn_getAll_GameButton.gameObject.SetActive(!value);

            if (value)
            {
                m_seletedType = GuildGiftType.uncommon;
            }

            ReList();
        }

        //清楚所有过期礼物
        private void onDelAllPassGift()
        {

            Alert.CreateAlert(733006).SetRightButton(() => { m_allianceProxy.SendGiftDelAll(); }).SetLeftButton(null)
                .Show();

        }

        private void onGetAllGift()
        {
            if (MGetList().Count>0)
            {
                m_allianceProxy.SendGetAllGift();
            }
            
        }

        private void onHelpTip()
        {
            HelpTip.CreateTip(4017, view.m_btn_question_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp)
                .Show();
        }

        private void onGiftTip()
        {
            HelpTip.CreateTip(4016, view.m_btn_topPoint_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp)
                .Show();
        }

        private void UpdateInfo()
        {
            //top
            long crrGiftLevel = m_allianceProxy.GetAlliance().giftLevel;
            long crrGiftCount = m_allianceProxy.GiftPoint;

            var giftConfig = CoreUtils.dataService.QueryRecord<AllianceGiftLevelDefine>((int) crrGiftLevel);
            view.m_lbl_boxLevel_LanguageText.text = LanguageUtils.getTextFormat(730115, crrGiftLevel); //等级1
            view.m_lbl_giftbarText_LanguageText.text = LanguageUtils.getTextFormat(730290,
                ClientUtils.FormatComma(crrGiftCount), ClientUtils.FormatComma(giftConfig.exp)); //当前礼物点数 / 升级所需点数
            view.m_pb_giftBar_GameSlider.value = (float) crrGiftCount / giftConfig.exp;


            //down

            int treasureID = giftConfig.treasureId;
            crrKeyCount = m_allianceProxy.KeyPoint;

            var treasureConfig = CoreUtils.dataService.QueryRecord<AllianceTreasureDefine>(treasureID);

            view.m_lbl_boxName_LanguageText.text = LanguageUtils.getText(treasureConfig.l_nameId);


            if (m_allianceProxy.TreasureCount > 0)
            {
                ClientUtils.PlaySpine(view.m_pl_box.m_UI_Model_AnimationBox_SkeletonGraphic, treasureConfig.imgShow,
                    "UE_TreasureBox_stay",true);
            }
            else
            {
                ClientUtils.PlaySpine(view.m_pl_box.m_UI_Model_AnimationBox_SkeletonGraphic, treasureConfig.imgShow,
                    "UE_TreasureBox_close");
                
            }
             

            view.m_lbl_boxbarText_LanguageText.text = LanguageUtils.getTextFormat(730290,
                ClientUtils.FormatComma(crrKeyCount), ClientUtils.FormatComma(treasureConfig.reqPoints));

            view.m_pb_boxBar_GameSlider.value = (float) crrKeyCount / treasureConfig.reqPoints;



            if (m_allianceProxy.TreasureCount > 0)
            {

                GuildTreasureInfoEntity info = m_allianceProxy.MTreasures.Values.ToArray()[0];

                var tConfig = CoreUtils.dataService.QueryRecord<AllianceTreasureDefine>((int) info.treasureId);

                m_passTime = tConfig.lastTime + info.sendTime - ServerTimeModule.Instance.GetServerTime();

            }

            view.m_UI_Redpoint.gameObject.SetActive(m_allianceProxy.TreasureCount > 0);
            view.m_UI_Redpoint.m_lbl_num_LanguageText.text = m_allianceProxy.TreasureCount.ToString();


            view.m_lbl_passTime_LanguageText.gameObject.SetActive(m_allianceProxy.TreasureCount > 0);


        }


        private void UpdateRedPoint()
        {
            
        }

        private void OnTime()
        {


            if (m_passTime > 0 && m_allianceProxy.MTreasures.Count>0)
            {
                GuildTreasureInfoEntity info = m_allianceProxy.MTreasures.Values.ToArray()[0];

                var tConfig = CoreUtils.dataService.QueryRecord<AllianceTreasureDefine>((int) info.treasureId);

                m_passTime = tConfig.lastTime + info.sendTime - ServerTimeModule.Instance.GetServerTime();
                view.m_lbl_passTime_LanguageText.text =
                    LanguageUtils.getTextFormat(733007, ClientUtils.FormatTimeSplit((int) m_passTime));
            }
            else
            {
                view.m_lbl_passTime_LanguageText.text = "";
            }

            int len = MGetList().Count;
          
            for (int i = 0; i < len; i++)
            {
                var item = MGetViewList().GetItemByIndex(i);

                if (item != null)
                {
                    if (m_seletedType == GuildGiftType.common)
                    {

                        ViewItemByIndex(item);
                    }
                    else
                    {
                        ViewItemByIndexUnCommon(item);
                    }
                }
            }

        }

        public List<GuildGiftInfoEntity> MGetList(){
            List<GuildGiftInfoEntity> list;
            if (m_seletedType == GuildGiftType.common)
            {
                list = m_allianceProxy.MGiftCommons;
                
            }
            else
            {
                list = m_allianceProxy.MGiftUnCommons;
            }
            return list;
        }

        public ListView MGetViewList()
        {
            ListView list;
            if (m_seletedType == GuildGiftType.common)
            {
                list = view.m_sv_list_common_ListView;
                view.m_sv_list_common_ListView.gameObject.SetActive(true);
                view.m_sv_list_uncommon_ListView.gameObject.SetActive(false);
            }
            else
            {
                list = view.m_sv_list_uncommon_ListView;
                view.m_sv_list_common_ListView.gameObject.SetActive(false);
                view.m_sv_list_uncommon_ListView.gameObject.SetActive(true);
            }
            
            view.m_lbl_nogift_LanguageText.gameObject.SetActive(MGetList().Count==0);

            return list;
        }


        private void SortGift(List<GuildGiftInfoEntity> list )
        {
            list.Sort(((entity, infoEntity) =>
            {

                int res = -infoEntity.status.CompareTo(entity.status);

                if (res == 0)
                {
                    return infoEntity.sendTime.CompareTo(entity.sendTime);
                }
                return res;
                    
            }));
        }
        

        private void StopTime()
        {
            if ( m_timer!=null)
            {
                m_timer.Cancel();
                m_timer = null ;
                m_passTime = 0 ;
            }
        }

        private void CheckRedPoint()
        {
            view.m_UI_redpoint_normal.ShowSmallRedPoint(m_allianceProxy.GetGiftRedCommon());
            view.m_UI_redpoint_rara.ShowSmallRedPoint(m_allianceProxy.GetGiftRedUnCommon());
        }

        private void ReList()
        {

            List<GuildGiftInfoEntity> list = MGetList();

            CheckRedPoint();

            MGetViewList().FillContent(list.Count);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceGift);
        }

        #endregion
    }
}