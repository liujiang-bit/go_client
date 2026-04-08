// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月18日
// Update Time         :    2020年6月18日
// Class Description   :    UI_Item_MailType16_SubView 邮件 侦查内容
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_MailType16_SubView : UI_SubView
    {
        private EmailProxy m_emailProxy;
        private bool m_isInit;
        private MailDefine m_emailDefine;
        private EmailInfoEntity m_emailInfo;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            m_emailDefine = mailDefine;
            m_emailInfo = mailInfo;
            if (!m_isInit)
            {
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_isInit = true;
            }
            m_UI_Item_MailTitle.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_nameID), mailInfo.titleContents);
            m_UI_Item_MailTitle.m_lbl_desc_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_subheadingID), mailInfo.subTitleContents);
            m_UI_Item_MailTitle.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(mailInfo.sendTime);
            m_lbl_Content_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(mailDefine.l_mesID), mailInfo.emailContents);

            //标题背景
            if (!string.IsNullOrEmpty(mailDefine.poster))
            {
                ClientUtils.LoadSprite(m_UI_Item_MailTitle.m_img_bg_PolygonImage, mailDefine.poster);
            }
            else
            {
                m_UI_Item_MailTitle.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }

            if (m_emailInfo.scoutReport == null)
            {
                return;
            }

            long targetType = m_emailInfo.scoutReport.targetType;
            if (targetType == 1)//玩家城市
            {
                if (mailDefine.ID == 200100) //成功
                {
                    RefreshPlayerInfo();
                    RefreshRes();
                    RefreshTroops();
                    RefreshReinforce();
                    RefreshMass();
                    RefreshGuardTower();
                }
                else
                {
                    RefreshPlayerInfo();
                    m_pl_resources.gameObject.SetActive(false);
                    m_pl_troops_VerticalLayoutGroup.gameObject.SetActive(false);
                    m_pl_reinforce_VerticalLayoutGroup.gameObject.SetActive(false);
                    m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(false);
                    m_pl_tower.gameObject.SetActive(false);
                }

            }
            else if (targetType == 2) //单人部队
            {
                RefreshPlayerInfo();
                if (mailDefine.ID == 200108) //侦查成功
                {
                    RefreshTroops();
                }
                else//侦查失败
                {
                    m_pl_troops_VerticalLayoutGroup.gameObject.SetActive(false);
                }
                m_pl_resources.gameObject.SetActive(false);
                m_pl_reinforce_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_tower.gameObject.SetActive(false);
            }
            else if (targetType == 3) //集结部队
            {
                RefreshPlayerInfo();
                if (mailDefine.ID == 200106) //侦查成功
                {
                    RefreshMass();
                }
                else//侦查失败
                {
                    m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(false);
                }
                m_pl_resources.gameObject.SetActive(false);
                m_pl_troops_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_reinforce_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_tower.gameObject.SetActive(false);
            }
            else if (targetType == 4)//玩家采集点
            {
                RefreshPlayerInfo();
                if (mailDefine.ID == 200104) //侦查成功
                {
                    RefreshTroops();
                }
                else//侦查失败
                {
                    m_pl_troops_VerticalLayoutGroup.gameObject.SetActive(false);
                }
                m_pl_resources.gameObject.SetActive(false);
                m_pl_reinforce_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_tower.gameObject.SetActive(false);
            }
            else if (targetType == 5 || targetType == 6 || targetType == 7) //联盟旗帜 关卡 圣地
            {
                RefreshPlayerInfo();
                if (mailDefine.ID == 200110 || mailDefine.ID == 200121)//成功
                {
                    RefreshTroops();
                }
                else//失败
                {
                    m_pl_troops_VerticalLayoutGroup.gameObject.SetActive(false);
                }
                m_pl_resources.gameObject.SetActive(false);
                m_pl_reinforce_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(false);
                m_pl_tower.gameObject.SetActive(false);
            }
        }

        //刷新玩家信息 或 建筑信息
        private void RefreshPlayerInfo()
        {
            m_pl_player_message_ArabLayoutCompment.gameObject.SetActive(true);

            m_UI_Model_GuildFlag.gameObject.SetActive(false);
            m_UI_Model_PlayerHead.gameObject.SetActive(false);
            m_img_build_PolygonImage.gameObject.SetActive(false);

            ScoutReportInfo scoutInfo = m_emailInfo.scoutReport;
            ScoutRoleInfo roleInfo = m_emailInfo.scoutReport.scoutRole;

            if (scoutInfo.targetType == 5)
            {
                //联盟建筑
                m_UI_Model_GuildFlag.gameObject.SetActive(true);
                m_UI_Model_GuildFlag.setData(scoutInfo.guildFlagSigns);

                AllianceBuildingTypeDefine allianceBuild = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)scoutInfo.objectTypeId);
                if (allianceBuild != null)
                {
                    //ClientUtils.LoadSprite(m_UI_Model_GuildFlag.m_img_flagIcon_PolygonImage, allianceBuild.iconImg);
                    m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, scoutInfo.guildAbbName, LanguageUtils.getText(allianceBuild.l_nameId));
                }
                else
                {
                    m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, scoutInfo.guildAbbName, "");
                }
            }
            else if (scoutInfo.targetType == 6 || scoutInfo.targetType == 7)
            {
                //关卡 圣地
                m_img_build_PolygonImage.gameObject.SetActive(true);

                StrongHoldDataDefine strongHoldData = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)scoutInfo.objectTypeId);
                if (strongHoldData != null)
                {
                    StrongHoldTypeDefine strongHold = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldData.type);

                    ClientUtils.LoadSprite(m_img_build_PolygonImage, strongHold.icon);
                    m_lbl_name_LanguageText.text = LanguageUtils.getText(strongHold.l_nameId);
                }
                else
                {
                    m_lbl_name_LanguageText.text = "";
                }
            }
            else
            {
                m_UI_Model_PlayerHead.gameObject.SetActive(true);

                //头像
                if (roleInfo != null)
                {
                    m_UI_Model_PlayerHead.gameObject.SetActive(true);
                    m_UI_Model_PlayerHead.LoadPlayerIcon(roleInfo.headId, roleInfo.headFrameID);
                }
                else
                {
                    m_UI_Model_PlayerHead.gameObject.SetActive(false);
                }

                //名称
                if (roleInfo != null && !string.IsNullOrEmpty(roleInfo.name))
                {
                    m_lbl_name_LanguageText.text = string.IsNullOrEmpty(scoutInfo.guildAbbName) ? roleInfo.name :
                               LanguageUtils.getTextFormat(300030, scoutInfo.guildAbbName, roleInfo.name);
                }
                else
                {
                    m_lbl_name_LanguageText.text = "";
                }
            }

            //耐久度
            if (scoutInfo.cityWallDurableLimit > 0)
            {
                m_lbl_wallhp_LanguageText.text = LanguageUtils.getTextFormat(570108,
                                                ClientUtils.FormatComma(scoutInfo.cityWallDurable),
                                                ClientUtils.FormatComma(scoutInfo.cityWallDurableLimit));
            }
            else
            {
                m_lbl_wallhp_LanguageText.text = "";
            }

            //坐标
            if (scoutInfo.pos != null)
            {
                m_UI_Model_Link.SetLinkText(LanguageUtils.getTextFormat(300032, scoutInfo.pos.x / 600, scoutInfo.pos.y / 600));
                m_UI_Model_Link.RemoveAllClickEvent();
                m_UI_Model_Link.AddClickEvent(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_Email);
                    WorldCamera.Instance().ViewTerrainPos(scoutInfo.pos.x / 100, scoutInfo.pos.y / 100, 1000, null);
                    float dxf = WorldCamera.Instance().getCameraDxf("FTE_Scout");
                    WorldCamera.Instance().SetCameraDxf(dxf, 1000, null);
                });
            }
            else
            {
                m_UI_Model_Link.SetLinkText("");
                m_UI_Model_Link.RemoveAllClickEvent();
            }

        }

        //刷新可掠夺资源
        private void RefreshRes()
        {
            ScoutReportInfo scoutInfo = m_emailInfo.scoutReport;
            //可掠夺资源
            m_pl_resources.gameObject.SetActive(scoutInfo.robResourceType == 2);
            if (scoutInfo.robResourceType == 2)
            {
                //m_UI_Model_ResCost.m_UI_Model_ArmyTrainRes_1.gameObject.SetActive(scoutInfo.HasRobFood);
                m_UI_Model_ResCost.m_UI_Model_ArmyTrainRes_1.m_lbl_resCost_num_LanguageText.text = scoutInfo.robFood > 0 ? ClientUtils.CurrencyFormat(scoutInfo.robFood) : "-";
                m_UI_Model_ResCost.m_UI_Model_ArmyTrainRes_2.m_lbl_resCost_num_LanguageText.text = scoutInfo.robWood > 0 ? ClientUtils.CurrencyFormat(scoutInfo.robWood) : "-";
                m_UI_Model_ResCost.m_UI_Model_ArmyTrainRes_3.m_lbl_resCost_num_LanguageText.text = scoutInfo.robStone > 0 ? ClientUtils.CurrencyFormat(scoutInfo.robStone) : "-";
                m_UI_Model_ResCost.m_UI_Model_ArmyTrainRes_4.m_lbl_resCost_num_LanguageText.text = scoutInfo.robGold > 0 ? ClientUtils.CurrencyFormat(scoutInfo.robGold) : "-";

                m_pl_resources_title.m_btn_info_GameButton.onClick.RemoveAllListeners();
                m_pl_resources_title.m_btn_info_GameButton.onClick.AddListener(OnHelpInfo);
            }
        }

        //部队总数
        private void RefreshTroops()
        {
            ScoutReportInfo scoutInfo = m_emailInfo.scoutReport;
            m_pl_troops_VerticalLayoutGroup.gameObject.SetActive(true);

            if (scoutInfo.armySum > 0)
            {
                if (scoutInfo.armySumType == 1) //不详
                {
                    m_pl_troops_title.m_lbl_num_LanguageText.text = LanguageUtils.getText(550125);
                }
                else if (scoutInfo.armySumType == 2)//大概数量不显示ICON
                {
                    m_pl_troops_title.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(550126, ClientUtils.FormatComma(scoutInfo.armySum));
                }
                else if (scoutInfo.armySumType == 3)//大概数量显示ICON
                {
                    m_pl_troops_title.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(550126, ClientUtils.FormatComma(scoutInfo.armySum));
                }
                else if (scoutInfo.armySumType == 4)//精确
                {
                    m_pl_troops_title.m_lbl_num_LanguageText.text = ClientUtils.FormatComma(scoutInfo.armySum);
                }
            }
            else//无
            {
                if (scoutInfo.armySumType == 1) //不详
                {
                    m_pl_troops_title.m_lbl_num_LanguageText.text = LanguageUtils.getText(550125);
                }
                else//无
                {
                    m_pl_troops_title.m_lbl_num_LanguageText.text = LanguageUtils.getText(570029);
                }
            }
            float height = m_pl_troops_title.m_root_RectTransform.rect.height;

            //统帅
            if (scoutInfo.mainHero == null)
            {
                m_pl_troops_heromain.gameObject.SetActive(false);
                m_pl_troops_herosub.gameObject.SetActive(false);

                //无统帅
                m_lbl_none_hero_LanguageText.gameObject.SetActive(true);
                RectTransform tRect = m_lbl_none_hero_LanguageText.GetComponent<RectTransform>(); ;
                height = height + tRect.rect.height;
                //height = height + m_lbl_none_hero_LanguageText.preferredHeight;
            }
            else
            {
                //有统帅
                m_lbl_none_hero_LanguageText.gameObject.SetActive(false);

                m_pl_troops_heromain.gameObject.SetActive(true);
                RefreshHeroInfo(m_pl_troops_heromain, scoutInfo.mainHero);
                height = height + m_pl_troops_heromain.m_root_RectTransform.rect.height;

                if (scoutInfo.deputyHero != null)
                {
                    m_pl_troops_herosub.gameObject.SetActive(true);
                    RefreshHeroInfo(m_pl_troops_herosub, scoutInfo.deputyHero);
                    height = height + m_pl_troops_herosub.m_root_RectTransform.rect.height;
                }
                else
                {
                    m_pl_troops_herosub.gameObject.SetActive(false);
                }
            }

            //部队
            if (scoutInfo.soldiers != null && scoutInfo.soldiers.Count > 0)
            {
                //有部队
                m_lbl_none_troops_LanguageText.gameObject.SetActive(false);

                m_pl_troops_soldiers.gameObject.SetActive(true);
                m_pl_troops_soldiers.m_UI_Item_SoldierHead.gameObject.SetActive(false);

                int childCount = m_pl_troops_soldiers.gameObject.transform.childCount;
                for (int i = childCount - 1; i > 0; i--)
                {
                    CoreUtils.assetService.Destroy(m_pl_troops_soldiers.gameObject.transform.GetChild(i).gameObject);
                }
                List<SoldierInfo> soldiersList = SoldierDataSort(scoutInfo.soldiers);
                for (int i = 0; i < soldiersList.Count; i++)
                {
                    GameObject go = CoreUtils.assetService.Instantiate(m_pl_troops_soldiers.m_UI_Item_SoldierHead.gameObject);
                    go.transform.SetParent(m_pl_troops_soldiers.gameObject.transform);
                    go.SetActive(true);
                    go.transform.localScale = Vector3.one;

                    UI_Item_SoldierHead_SubView subView = new UI_Item_SoldierHead_SubView(go.GetComponent<RectTransform>());
                    subView.Refresh(soldiersList[i], true);
                }
                int num = (int)Mathf.Ceil((float)scoutInfo.soldiers.Count/m_pl_troops_soldiers.m_UI_Item_MailType16Soldiers_GridLayoutGroup.constraintCount);
                height = height+ m_pl_troops_soldiers.m_UI_Item_MailType16Soldiers_GridLayoutGroup.cellSize.y * num;
            }
            else
            {
                //无部队
                m_pl_troops_soldiers.gameObject.SetActive(false);
                if (scoutInfo.armySumType == 3 || scoutInfo.armySumType == 4)
                {
                    m_lbl_none_troops_LanguageText.gameObject.SetActive(true);
                    RectTransform tRect = m_lbl_none_troops_LanguageText.GetComponent<RectTransform>(); ;
                    height = height + tRect.rect.height;
                }
                else
                {
                    m_lbl_none_troops_LanguageText.gameObject.SetActive(false);
                }
                //height = height + m_lbl_none_troops_LanguageText.preferredHeight;
            }

            RectTransform rectTrans = m_pl_troops_VerticalLayoutGroup.GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, height);
        }

        //刷新援军信息
        private void RefreshReinforce()
        {
            m_pl_reinforce_VerticalLayoutGroup.gameObject.SetActive(true);

            ScoutReportInfo scoutInfo = m_emailInfo.scoutReport;

            //统计士兵数量
            int count = 0;
            if (scoutInfo.reinforceSoldiers != null && scoutInfo.reinforceSoldiers.Count > 0)
            {
                for (int i = 0; i < scoutInfo.reinforceSoldiers.Count; i++)
                {
                    count = count + (int)scoutInfo.reinforceSoldiers[i].num;
                }
            }

            if (count > 0)
            {
                if (scoutInfo.reinforceArmySumType == 1) //不详
                {
                    m_pl_reinforce_title.m_lbl_num_LanguageText.text = LanguageUtils.getText(550125);
                }
                else if (scoutInfo.reinforceArmySumType == 2)//大概数量
                {
                    m_pl_reinforce_title.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(550126, ClientUtils.FormatComma(count));
                }
                else if (scoutInfo.reinforceArmySumType == 3)//准确数量
                {
                    m_pl_reinforce_title.m_lbl_num_LanguageText.text = ClientUtils.FormatComma(count);
                }
            }
            else//无
            {
                if (scoutInfo.reinforceArmySumType == 1) //不详
                {
                    m_pl_reinforce_title.m_lbl_num_LanguageText.text = LanguageUtils.getText(550125);
                }
                else
                {
                    m_pl_reinforce_title.m_lbl_num_LanguageText.text = LanguageUtils.getText(570029);
                }
            }


            float height = m_pl_reinforce_title.m_root_RectTransform.rect.height;

            //部队
            if (scoutInfo.reinforceSoldiers != null && scoutInfo.reinforceSoldiers.Count > 0)
            {
                //有部队
                m_lbl_none_reinforce_LanguageText.gameObject.SetActive(false);

                m_pl_reinforce_soldiers.gameObject.SetActive(true);
                m_pl_reinforce_soldiers.m_UI_Item_SoldierHead.gameObject.SetActive(false);

                int childCount = m_pl_reinforce_soldiers.gameObject.transform.childCount;
                for (int i = childCount - 1; i > 0; i--)
                {
                    CoreUtils.assetService.Destroy(m_pl_reinforce_soldiers.gameObject.transform.GetChild(i).gameObject);
                }
                List<SoldierInfo> soldiersList = SoldierDataSort(scoutInfo.reinforceSoldiers);
                for (int i = 0; i < soldiersList.Count; i++)
                {
                    GameObject go = CoreUtils.assetService.Instantiate(m_pl_reinforce_soldiers.m_UI_Item_SoldierHead.gameObject);
                    go.transform.SetParent(m_pl_reinforce_soldiers.gameObject.transform);
                    go.SetActive(true);
                    go.transform.localScale = Vector3.one;

                    UI_Item_SoldierHead_SubView subView = new UI_Item_SoldierHead_SubView(go.GetComponent<RectTransform>());
                    subView.Refresh(soldiersList[i], true);
                    //ArmsDefine armDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)scoutInfo.reinforceSoldiers[i].id);
                    //subView.SetSoldierInfo(armDefine.icon, (int)scoutInfo.reinforceSoldiers[i].num);
                }
                int num = (int)Mathf.Ceil((float)scoutInfo.reinforceSoldiers.Count / m_pl_reinforce_soldiers.m_UI_Item_MailType16Soldiers_GridLayoutGroup.constraintCount);
                height = height + m_pl_reinforce_soldiers.m_UI_Item_MailType16Soldiers_GridLayoutGroup.cellSize.y * num;
            }
            else
            {
                //无部队
                m_pl_reinforce_soldiers.gameObject.SetActive(false);

                if (scoutInfo.reinforceArmySumType != 1)
                {
                    m_lbl_none_reinforce_LanguageText.gameObject.SetActive(true);
                    RectTransform tRect = m_lbl_none_reinforce_LanguageText.GetComponent<RectTransform>(); ;
                    height = height + tRect.rect.height;
                }
                else
                {
                    m_lbl_none_reinforce_LanguageText.gameObject.SetActive(false);
                }
                //height = height + m_lbl_none_reinforce_LanguageText.preferredHeight;
            }

            RectTransform rectTrans = m_pl_reinforce_VerticalLayoutGroup.GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, height);
        }

        //集结部队总数
        private void RefreshMass()
        {
            m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(true);
            ScoutReportInfo scoutInfo = m_emailInfo.scoutReport;

            if (scoutInfo.rallyArmySumType == 0)
            {
                m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(false);
                return;
            }

            //统计士兵数量
            int count = 0;
            if (scoutInfo.rallySoldiers != null && scoutInfo.rallySoldiers.Count > 0)
            {
                for (int i = 0; i < scoutInfo.rallySoldiers.Count; i++)
                {
                    count = count + (int)scoutInfo.rallySoldiers[i].num;
                }
            }

            if (scoutInfo.rallyMainHero == null) //无
            {
                m_pl_mass_title.m_lbl_num_LanguageText.text = LanguageUtils.getText(570029);
            }
            else
            {
                if (scoutInfo.rallyArmySumType == 1) //大概数量
                {
                    m_pl_mass_title.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(550126, ClientUtils.FormatComma(count));
                }
                else if (scoutInfo.rallyArmySumType == 2)//精确数量
                {
                    m_pl_mass_title.m_lbl_num_LanguageText.text = ClientUtils.FormatComma(count);
                }
                else if (scoutInfo.rallyArmySumType == 3)//无正在集结部队
                {
                    m_pl_mass_VerticalLayoutGroup.gameObject.SetActive(false);
                    return;
                }
            }

            float height = m_pl_mass_title.m_root_RectTransform.rect.height;

            //统帅
            if (scoutInfo.rallyMainHero == null)
            {
                m_pl_mass_heromain.gameObject.SetActive(false);
                m_pl_mass_herosub.gameObject.SetActive(false);
            }
            else
            {
                m_pl_mass_heromain.gameObject.SetActive(true);
                RefreshHeroInfo(m_pl_mass_heromain, scoutInfo.rallyMainHero, true);
                height = height + m_pl_mass_heromain.m_root_RectTransform.rect.height;

                if (scoutInfo.rallyDeputyHero != null)
                {
                    m_pl_mass_herosub.gameObject.SetActive(true);
                    RefreshHeroInfo(m_pl_mass_herosub, scoutInfo.rallyDeputyHero, true);
                    height = height + m_pl_mass_herosub.m_root_RectTransform.rect.height;
                }
                else
                {
                    m_pl_mass_herosub.gameObject.SetActive(false);
                }
            }

            //部队
            if (scoutInfo.rallySoldiers != null && scoutInfo.rallySoldiers.Count > 0)
            {
                //有部队
                m_lbl_none_mass_LanguageText.gameObject.SetActive(false);

                m_pl_mass_soldiers.gameObject.SetActive(true);
                m_pl_mass_soldiers.m_UI_Item_SoldierHead.gameObject.SetActive(false);

                int childCount = m_pl_mass_soldiers.gameObject.transform.childCount;
                for (int i = childCount - 1; i > 0; i--)
                {
                    CoreUtils.assetService.Destroy(m_pl_mass_soldiers.gameObject.transform.GetChild(i).gameObject);
                }
                List<SoldierInfo> soldiersList = SoldierDataSort(scoutInfo.rallySoldiers);
                for (int i = 0; i < soldiersList.Count; i++)
                {
                    GameObject go = CoreUtils.assetService.Instantiate(m_pl_mass_soldiers.m_UI_Item_SoldierHead.gameObject);
                    go.transform.SetParent(m_pl_mass_soldiers.gameObject.transform);
                    go.SetActive(true);
                    go.transform.localScale = Vector3.one;

                    UI_Item_SoldierHead_SubView subView = new UI_Item_SoldierHead_SubView(go.GetComponent<RectTransform>());
                    subView.Refresh(soldiersList[i], true);
                    //ArmsDefine armDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)scoutInfo.rallySoldiers[i].id);
                    //subView.SetSoldierInfo(armDefine.icon, (int)scoutInfo.rallySoldiers[i].num);
                }
                int num = (int)Mathf.Ceil((float)scoutInfo.rallySoldiers.Count / m_pl_mass_soldiers.m_UI_Item_MailType16Soldiers_GridLayoutGroup.constraintCount);
                height = height + m_pl_mass_soldiers.m_UI_Item_MailType16Soldiers_GridLayoutGroup.cellSize.y * num;
            }
            else
            {
                //无部队
                m_pl_mass_soldiers.gameObject.SetActive(false);

                m_lbl_none_mass_LanguageText.gameObject.SetActive(true);
                RectTransform tRect = m_lbl_none_mass_LanguageText.GetComponent<RectTransform>(); ;
                height = height + tRect.rect.height;
                //height = height + m_lbl_none_mass_LanguageText.preferredHeight;
            }

            RectTransform rectTrans = m_pl_mass_VerticalLayoutGroup.GetComponent<RectTransform>();
            rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, height);
        }

        //刷新箭塔信息
        private void RefreshGuardTower()
        {
            ScoutReportInfo scoutInfo = m_emailInfo.scoutReport;
            if (scoutInfo.guardTowerLevel < 1)
            {
                m_pl_tower.gameObject.SetActive(false);
                return;
            }
            else
            {
                m_pl_tower.gameObject.SetActive(true);
            }

            m_pl_tower.gameObject.SetActive(true);
            m_lbl_towerhp_LanguageText.text = LanguageUtils.getTextFormat(570109, scoutInfo.guardTowerHp, scoutInfo.guardTowerHpLimit);
            m_lbl_towername_LanguageText.text = LanguageUtils.getTextFormat(550133, scoutInfo.guardTowerLevel);
            
            //根据时代显示箭塔icon
            CityAgeSizeDefine define = CoreUtils.dataService.QueryRecord<CityAgeSizeDefine>((int)scoutInfo.roleAge);
            if (define != null)
            {
                ClientUtils.LoadSprite(m_img_tower_PolygonImage, define.towerMail);
            }
        }

        //刷新英雄信息
        private void RefreshHeroInfo(UI_Item_MailHero_SubView heroView, DefendHeroInfo heroInfo, bool isMass = false)
        {
            var define = CoreUtils.dataService.QueryRecord<HeroDefine>((int)heroInfo.heroId);
            heroView.m_UI_Model_CaptainHead.SetHero(heroInfo.heroId, 0);
            if (isMass) //集结 特殊处理
            {
                if (string.IsNullOrEmpty(m_emailInfo.scoutReport.guildAbbName))
                {
                    heroView.m_lbl_name_LanguageText.text = LanguageUtils.getText(define.l_nameID);
                } else
                {
                    heroView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(570110, m_emailInfo.scoutReport.guildAbbName,
                        m_emailInfo.scoutReport.scoutRole.name, LanguageUtils.getText(define.l_nameID));
                }
                
            }
            else
            {
                heroView.m_lbl_name_LanguageText.text = LanguageUtils.getText(define.l_nameID);
            }
            if (heroInfo.star > 0)
            {
                //显示统帅星级 技能
                heroView.m_pl_stars_GridLayoutGroup.gameObject.SetActive(true);
                heroView.m_pl_skill_GridLayoutGroup.gameObject.SetActive(true);

                heroView.m_UI_Model_HeadStar1.SetShow2(heroInfo.star > 0);
                heroView.m_UI_Model_HeadStar2.SetShow2(heroInfo.star > 1);
                heroView.m_UI_Model_HeadStar3.SetShow2(heroInfo.star > 2);
                heroView.m_UI_Model_HeadStar4.SetShow2(heroInfo.star > 3);
                heroView.m_UI_Model_HeadStar5.SetShow2(heroInfo.star > 4);
                heroView.m_UI_Model_HeadStar6.SetShow2(heroInfo.star > 5);

                heroView.m_UI_Item_CaptainSkill1.gameObject.SetActive(false);
                heroView.m_UI_Item_CaptainSkill2.gameObject.SetActive(false);
                heroView.m_UI_Item_CaptainSkill3.gameObject.SetActive(false);
                heroView.m_UI_Item_CaptainSkill4.gameObject.SetActive(false);
                heroView.m_UI_Item_CaptainSkill5.gameObject.SetActive(false);

                List<UI_Item_CaptainSkill_SubView> skillList = new List<UI_Item_CaptainSkill_SubView>();
                skillList.Add(heroView.m_UI_Item_CaptainSkill1);
                skillList.Add(heroView.m_UI_Item_CaptainSkill2);
                skillList.Add(heroView.m_UI_Item_CaptainSkill3);
                skillList.Add(heroView.m_UI_Item_CaptainSkill4);
                skillList.Add(heroView.m_UI_Item_CaptainSkill5);
                if (heroInfo.skills != null)
                {
                    int count = heroInfo.skills.Count;
                    if (count > skillList.Count)
                    {
                        Debug.LogErrorFormat("技能列表数据超出范围：{0}", count);
                    }
                    for (int i = 0; i < skillList.Count; i++)
                    {
                        if (i < count)
                        {
                            skillList[i].gameObject.SetActive(true);
                            skillList[i].SetSkillInfo(heroInfo.skills[i], (int)heroInfo.star);
                        }
                        else
                        {
                            skillList[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                //隐藏统帅星级 技能
                heroView.m_pl_stars_GridLayoutGroup.gameObject.SetActive(false);
                heroView.m_pl_skill_GridLayoutGroup.gameObject.SetActive(false);
            }
        }

        //可掠夺帮助信息
        private void OnHelpInfo()
        {
            HelpTip.CreateTip(LanguageUtils.getText(550124), 
                              m_pl_resources_title.m_btn_info_GameButton.GetComponent<RectTransform>())
                              .SetStyle(HelpTipData.Style.arrowUp)
                              .SetOffset(23)
                              .SetWidth(500)
                              .Show();
        }

        //士兵数据排序
        private List<SoldierInfo> SoldierDataSort(List<SoldierInfo> soldierList)
        {
            List<SoldierInfo> lastList = new List<SoldierInfo>();
            lastList.AddRange(soldierList);

            lastList.Sort(delegate (SoldierInfo x, SoldierInfo y)
            {
                int re = y.level.CompareTo(x.level);
                if (re == 0)
                {
                    re = x.type.CompareTo(y.type);
                    if (re == 0)
                    {
                        re = y.id.CompareTo(x.id);
                    }
                }
                return re;
            });

            return lastList;
        }
    }
}