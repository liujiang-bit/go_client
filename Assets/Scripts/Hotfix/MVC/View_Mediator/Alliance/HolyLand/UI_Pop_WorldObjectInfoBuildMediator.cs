// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    UI_Pop_WorldObjectInfoBuildMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game
{
    public class UI_Pop_WorldObjectInfoBuildMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Pop_WorldObjectInfoBuildMediator";

        private RectTransform _contentBGRT;

        RectTransform contentBGRT
        {
            get
            {
                if (_contentBGRT == null)
                {
                    _contentBGRT = view.m_content_bg_PolygonImage.GetComponent<RectTransform>();
                }
                return _contentBGRT;
            }
        }
        
        private RectTransform _contentRT;

        RectTransform contentRT
        {
            get
            {
                if (_contentRT == null)
                {
                    _contentRT = view.m_pl_content_Animator.GetComponent<RectTransform>();
                }
                return _contentRT;
            }
        }

        private int lan_pos = 300032;
        private int lan_none = 570029;
        private int lan_occupyGain = 500784;
        private int lan_king = 500785;
        private int lan_allianceAbb = 730138;
        private int lan_checkPointDesc = 500786;

        private WorldMapObjectProxy m_worldProxy;
        private AllianceProxy m_allianceProxy;
        private MapObjectInfoEntity m_mapData;
        private long m_objectId;

        private UI_Item_IconAndTime_SubView.BuildingState m_buildingState;

        private StrongHoldTypeDefine strongHoldTypeDefine;

        private List<UI_Model_Item_SubView> m_modelItemSubViews = new List<UI_Model_Item_SubView>(); 
        
        private float[] bgSize = new float[] {430, 320};

        private float[] btnsY = new float[] {-170, -60};

        #endregion

        //IMediatorPlug needs
        public UI_Pop_WorldObjectInfoBuildMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Pop_WorldObjectInfoBuildView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.MapObjectChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.MapObjectChange:
                    MapObjectInfoEntity mapItemInfo = notification.Body as MapObjectInfoEntity;
                    
                    if (mapItemInfo != null && mapItemInfo.objectId == m_mapData.objectId)
                    {
                        m_mapData = mapItemInfo;
                        UpdateView();
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
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
            // 在保护期时，检测保护时间是否已到
//            if (m_buildingState == UI_Item_IconAndTime_SubView.BuildingState.InitProtecting || m_buildingState == UI_Item_IconAndTime_SubView.BuildingState.Protecting)
//            {
//                long serverTime = ServerTimeModule.Instance.GetServerTime();
//                long remainTime = m_mapData.holyLandFinishTime - serverTime;
//                if (remainTime < 0)
//                {
//                    m_mapData = m_worldProxy.GetWorldMapObjectByobjectId(m_objectId); 
//                    UpdateView();
//                }    
//            }
        }

        protected override void InitData()
        {
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_objectId = (long)view.data;
            m_mapData = m_worldProxy.GetWorldMapObjectByobjectId(m_objectId);

            m_modelItemSubViews.Add(view.m_UI_Model_Item);
            if (m_mapData == null)
            {
                CoreUtils.uiManager.CloseUI(UI.s_WorldObjectInfoBuild);
                return;
            }
            UpdateView();
            UpdatePopPos();

            // 功能介绍
            if (strongHoldTypeDefine != null)
            {
                StrongHoldGroup holdGroup = (StrongHoldGroup)strongHoldTypeDefine.group;

                if (holdGroup == StrongHoldGroup.Chancel || holdGroup == StrongHoldGroup.HolyPlace ||
                    holdGroup == StrongHoldGroup.Shrine || holdGroup == StrongHoldGroup.Fane)
                {
                    //奇观group1~4 
                    int diff = (int)holdGroup - 1;
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.WonderGroup1 + diff);
                }
                else
                {
                    //关卡
                    AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.CustomsPass);
                }
            }
        }

        protected override void BindUIEvent()
        {
            view.m_btn_reinforce.AddClickEvent(OnClickReinforce);
            view.m_btn_attack.AddClickEvent(OnClickAttack);
            view.m_btn_gather.AddClickEvent(OnClickGather);
            view.m_btn_look.AddClickEvent(OnClickLook);
            view.m_btn_descinfo_GameButton.onClick.AddListener(OnClickDescInfo);
            view.m_btn_more_GameButton.onClick.AddListener(OnClickMore);
            view.m_btn_kingset_GameButton.onClick.AddListener(OnClickKingset);
        }

        protected override void BindUIData()
        {
            view.m_UI_Common_PopFun.InitSubView(m_mapData);
        }

        #endregion


        void UpdatePopPos()
        {
            UIHelper.SelfAdaptPopViewPos(m_mapData.gameobject, view.gameObject.GetComponent<RectTransform>(),
                view.m_pl_pos.GetComponent<RectTransform>(),
                view.m_pl_content_Animator.GetComponent<RectTransform>(),
                view.m_img_arrowSideL_PolygonImage.gameObject,
                view.m_img_arrowSideR_PolygonImage.gameObject,
                view.m_img_arrowSideTop_PolygonImage.gameObject,
                view.m_img_arrowSideButtom_PolygonImage.gameObject
            );
        }

        void UpdateView()
        {
            m_buildingState = (UI_Item_IconAndTime_SubView.BuildingState) m_mapData.holyLandStatus;

            bool Occupied = m_mapData.guildId == 0 ? false : true;
            view.m_btn_more_GameButton.gameObject.SetActive(Occupied);

            StrongHoldDataDefine dataDefine =
                CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int) m_mapData.strongHoldId);
            
            strongHoldTypeDefine =
                CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(dataDefine.type);
            
            // 是否是自己的圣地
            bool isOwn = m_allianceProxy.HasGuildHolyLand((int) m_mapData.strongHoldId);
            view.m_btns_1_ArabLayoutCompment.gameObject.SetActive(isOwn);
            view.m_btns_2_ArabLayoutCompment.gameObject.SetActive(!isOwn);
            if (m_mapData.objectPos != null)
            {
                view.m_lbl_position_LanguageText.text = LanguageUtils.getTextFormat(lan_pos,
      PosHelper.ServerUnitToClientPos(m_mapData.objectPos.x),
      PosHelper.ServerUnitToClientPos(m_mapData.objectPos.y));
            }
            if (strongHoldTypeDefine == null)
            {
                Debug.Log("strongHoldTypeDefine == null " +  m_mapData.strongHoldId);
                return;
            }
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(strongHoldTypeDefine.l_nameId);
            
            view.m_UI_Item_IconAndTime.Refresh(m_buildingState, m_mapData.holyLandFinishTime,
                strongHoldTypeDefine.iconImg);

            RefreshFirstOrder();

            if (m_mapData.guildId > 0)
            {
                // 显示联盟简称
                view.m_lbl_content_LanguageText.text =
                    LanguageUtils.getTextFormat(lan_allianceAbb, m_mapData.guildAbbName);
            }
            else
            {
                view.m_lbl_content_LanguageText.text = LanguageUtils.getText(lan_none);
            }

            StrongHoldGroup holdGroup = (StrongHoldGroup) strongHoldTypeDefine.@group;

            view.m_pl_type1_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_pl_type2_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_pl_type3_ArabLayoutCompment.gameObject.SetActive(false);

            // 圣所、圣坛、圣祠处理
            if (holdGroup == StrongHoldGroup.Chancel || holdGroup == StrongHoldGroup.HolyPlace ||
                holdGroup == StrongHoldGroup.Shrine)
            {
                view.m_pl_type2_ArabLayoutCompment.gameObject.SetActive(true);
                RefreshBuff();
            }
            else if (holdGroup == StrongHoldGroup.Fane) // 国王处理
            {
                view.m_pl_type3_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_lbl_kingName_LanguageText.text = m_mapData.kingName;
            }
            else // 关卡处理
            {
                view.m_pl_type1_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(lan_checkPointDesc);
            }
        }

        void RefreshBuff()
        {
            int buffCountIndex = 0;
            List<int> buffDataLst = new List<int>()
            {
                strongHoldTypeDefine.buffData1,
                strongHoldTypeDefine.buffData2,
                strongHoldTypeDefine.buffData3,
            };

            view.m_icon_buff1_PolygonImage.gameObject.SetActive(false);
            view.m_icon_buff2_PolygonImage.gameObject.SetActive(false);
            
            for (int i = 0; i < buffDataLst.Count; i++)
            {
                int buffId = buffDataLst[i];
                if (buffId != 0)
                {
                    CityBuffDefine define = CoreUtils.dataService.QueryRecord<CityBuffDefine>(buffId);
                    if (buffCountIndex == 0)
                    {
                        buffCountIndex++;
                        RefreshBuffData(view.m_lbl_buffatt1_LanguageText, view.m_icon_buff1_PolygonImage, define);
                    }
                    else if (buffCountIndex == 1)
                    {
                        buffCountIndex++;
                        RefreshBuffData(view.m_lbl_buffatt2_LanguageText, view.m_icon_buff2_PolygonImage, define);
                    }
                    else
                    {
                        CoreUtils.logService.Warn("圣地===  预制现在最多只支持2条buff配置！");
                    }
                }
            }
        }

        void RefreshBuffData(LanguageText text, PolygonImage image, CityBuffDefine define)
        {
            try
            {
                image.gameObject.SetActive(true);
                object[] objs = new object[define.tagData.Count];
                for (int i = 0; i < define.tagData.Count; i++)
                {
                    objs[i] = define.tagData[i];
                }

                text.text = LanguageUtils.getTextFormat(define.tag, objs);
                ClientUtils.LoadSprite(image, define.tagIcon);
            }
            catch (System.Exception e)
            {
                CoreUtils.logService.Error($"[RefreshBuffData] 报错  info:[{e.ToString()}]");
            }
           
        }

        // 刷新首占
        void RefreshFirstOrder()
        {
            bool isFirstOrder = false;
            
            if (m_buildingState == UI_Item_IconAndTime_SubView.BuildingState.NotUnlock 
                || m_buildingState == UI_Item_IconAndTime_SubView.BuildingState.InitProtecting 
                || m_buildingState == UI_Item_IconAndTime_SubView.BuildingState.InitFighting)
            {
                isFirstOrder = true;
                
                // 刷新首占奖励
                UIHelper.RefreshItemPackage(strongHoldTypeDefine.firstRewardShow, m_modelItemSubViews, view.m_UI_Model_Item);
            }

            int index = 0;
            
            view.m_pl_reward.gameObject.SetActive(isFirstOrder);
            
            if (!isFirstOrder)
            {
                index = 1;
            }
            
            Vector2 vec2 = contentBGRT.sizeDelta;
            contentBGRT.sizeDelta = new Vector2(vec2.x, bgSize[index]);
            contentRT.sizeDelta = new Vector2(vec2.x, bgSize[index]);
            vec2 = view.m_btns_1_ArabLayoutCompment.GetComponent<RectTransform>().localPosition;
            view.m_btns_1_ArabLayoutCompment.GetComponent<RectTransform>().localPosition = new Vector2(vec2.x, btnsY[index]);
            vec2 = view.m_btns_2_ArabLayoutCompment.GetComponent<RectTransform>().localPosition;
            view.m_btns_2_ArabLayoutCompment.GetComponent<RectTransform>().localPosition = new Vector2(vec2.x, btnsY[index]);
        }

        private void CloseSelf()
        {
            CoreUtils.uiManager.CloseUI(UI.s_WorldObjectInfoBuild);
        }

        #region 按钮点击

        // 点击增援
        void OnClickReinforce()
        {
            CloseSelf();
            FightHelper.Instance.Reinforce(m_mapData);
        }

        // 攻击
        void OnClickAttack()
        {
            CloseSelf();
            FightHelper.Instance.Attack((int)m_mapData.objectId);

        }

        // 集结
        void OnClickGather()
        {
            CloseSelf();
            FightHelper.Instance.Concentrate((int)m_mapData.objectId);
        }

        // 侦擦
        void OnClickLook()
        {
            CloseSelf();
            FightHelper.Instance.Scout(m_mapData.objectPos.x, m_mapData.objectPos.y, (int)m_mapData.objectId);
            
        }

        // 信息描述
        void OnClickDescInfo()
        {
            CoreUtils.uiManager.ShowUI(UI.s_WorldObjectInfoBuildInfo, null, strongHoldTypeDefine.l_desc2);
        }

        // 更多
        void OnClickMore()
        {
            CloseSelf();
            CoreUtils.uiManager.ShowUI(UI.s_AllianceInfo, null, m_mapData.guildId);
        }

        // 国王设置
        void OnClickKingset()
        {
            Tip.CreateTip(100045).Show();
        }

        #endregion
    }
}