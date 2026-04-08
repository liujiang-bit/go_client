using System;
using System.Collections.Generic;
using Data;
using Game;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Hotfix
{
    public enum GuildNotify
    {
        None = 0,

        /// <summary>
        /// 成员加入
        /// </summary>
        MEMBER_JOIN=1,

        /// <summary>
        /// 移除成员
        /// </summary>
        KICK_MEMBER=2,

        /// <summary>
        /// 任命官职
        /// </summary>
        APPOINT_OFFICER=3,

        /// <summary>
        /// 联盟帮助
        /// </summary>
        HELP=4,

        /// <summary>
        /// 联盟解散
        /// </summary>
        DISBAND=5,

        /// <summary>
        /// 联盟成员对城市发起集结
        /// </summary>
        RALLY_CITY=6,

        /// <summary>
        /// 联盟成员对联盟旗帜发起集结
        /// </summary>
        RALLY_FLAG=7,

        /// <summary>
        /// 联盟成员对联盟要塞发起集结
        /// </summary>
        RALLY_FORTRESS=8,

        /// <summary>
        /// 联盟成员对部队发起集结
        /// </summary>
        RALLY_ARMY=9,

        /// <summary>
        /// 联盟成员取消集结
        /// </summary>
        CANCLE_RALLY=10,

        /// <summary>
        // 联盟成员被集结 
        /// </summary>
        CITY_RALLYED=11,

        /// <summary>
        ///联盟成员对野蛮人城寨发起集结
        /// </summary>
        RALLY_MONSTER_CITY=12,

        /// <summary>
        ///联盟旗帜被集结
        /// </summary>
        FLAG_RALLYED=13,
        /// <summary>
        ///联盟要塞被集结
        /// </summary>  
        FORTRESS_RALLYED=14,
        /// <summary>
        ///人数不足集结取消 
        /// </summary> 
        RALLY_MEMBER_NOT_ENOUGH = 15,
        /// <summary>
        ///联盟对无人圣地发起集结
        /// </summary>
        RALLY_NO_GUILD_HOLY_LAND = 16,
        /// <summary>
        ///联盟对其他联盟圣地发起集结
        /// </summary>
        RALLY_GUILD_HOLY_LAND = 17,
        /// <summary>
        ///联盟圣地被集结
        /// </summary>
        HOLY_LAND_RALLYED = 18, 
        
        /// <summary>
        /// 联盟成员对联盟建筑发起集结
        /// </summary>
        RALLY_GUILD_BUILD=19,
        
        /// <summary>
        /// 联盟建筑被集结
        /// </summary>
        GUILD_BUILD_RALLYED=20
    }


    public class BattleBroadcastsHandler : IBattleBroadcastsHandler
    {
        private readonly List<string> lsName = new List<string>();
        private readonly List<string> lsStringArg = new List<string>();
        private readonly List<int> guildFlag =new List<int>();
        private AllianceProxy m_AllianceProxy;


        public void Init()
        {
            m_AllianceProxy= AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
        }

        public void Show(int id, object parm)
        {
            Guild_GuildNotify.request info = parm as Guild_GuildNotify.request;
            if (info == null)
            {
                return;
            }

            string str = string.Empty;
            lsStringArg.Clear(); 
            guildFlag.Clear();
            if (info.HasStringArg)
            {            
                foreach (var des in info.stringArg)
                {
                    lsStringArg.Add(des);
                }
            }

            if (info.HasNotifyOperate)
            {
                switch ((GuildNotify) info.notifyOperate)
                {
                    case GuildNotify.RALLY_CITY:
                        if (lsStringArg.Count >= 3)
                        {
                            str = LanguageUtils.getTextFormat(730321, lsStringArg[0], lsStringArg[1],lsStringArg[2]);
                        }else if (lsStringArg.Count == 2)
                        {
                            str = LanguageUtils.getTextFormat(730354, lsStringArg[0], lsStringArg[1]);                                                   
                        }
                        break;
                    case GuildNotify.RALLY_FLAG:
                        str = LanguageUtils.getTextFormat(730283, lsStringArg[0],lsStringArg[1]);
                        break;
                    case GuildNotify.RALLY_FORTRESS:
                        str = LanguageUtils.getTextFormat(730284, lsStringArg[0],lsStringArg[1]);
                        break;
                    case GuildNotify.RALLY_ARMY:
                        str = LanguageUtils.getTextFormat(730322, lsStringArg[0], lsStringArg[1]);
                        break;
                    case GuildNotify.CANCLE_RALLY:
                        str = LanguageUtils.getTextFormat(730267, lsStringArg[0]);
                        break;
                    case GuildNotify.CITY_RALLYED:
                        guildFlag.Add(int.Parse(lsStringArg[0]));
                        guildFlag.Add(int.Parse(lsStringArg[1]));
                        guildFlag.Add(int.Parse(lsStringArg[2]));
                        guildFlag.Add(int.Parse(lsStringArg[3]));
                        str = LanguageUtils.getTextFormat(730271, lsStringArg[4], lsStringArg[5]);
                        break;
                    case GuildNotify.RALLY_MONSTER_CITY:
                        int nameId  = int.Parse(lsStringArg[1]);
                        MonsterDefine monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>(nameId);
                        string name=String.Empty;;
                        if (monsterDefine != null)
                        {
                            name = LanguageUtils.getText(monsterDefine.l_nameId);
                        }
                        str = LanguageUtils.getTextFormat(730358, lsStringArg[0], name);
                        break;
                    case GuildNotify.FLAG_RALLYED:
                        str = LanguageUtils.getTextFormat(730272, lsStringArg[0]);
                        break;
                    case GuildNotify.FORTRESS_RALLYED:
                        str = LanguageUtils.getTextFormat(730273, lsStringArg[0]);
                        break;                
                    case GuildNotify.RALLY_MEMBER_NOT_ENOUGH:
                        str = LanguageUtils.getText(730270);     
                        break;
                    case GuildNotify.RALLY_NO_GUILD_HOLY_LAND:
                        int holdNameId = int.Parse(lsStringArg[1]);
                        str = LanguageUtils.getTextFormat(730358, lsStringArg[0], GetStrongHoldName(holdNameId));                      
                        break;
                    case GuildNotify.RALLY_GUILD_HOLY_LAND:
                        int holdNameId1 = int.Parse(lsStringArg[2]);   
                        str = LanguageUtils.getTextFormat(730366, lsStringArg[0], lsStringArg[1],GetStrongHoldName(holdNameId1));                                              
                        break;
                    case  GuildNotify.HOLY_LAND_RALLYED:                        
                        guildFlag.Add(int.Parse(lsStringArg[0]));
                        guildFlag.Add(int.Parse(lsStringArg[1]));
                        guildFlag.Add(int.Parse(lsStringArg[2]));
                        guildFlag.Add(int.Parse(lsStringArg[3]));                           
                        int holdNameId2= int.Parse(lsStringArg[5]);
                        str = LanguageUtils.getTextFormat(730358, lsStringArg[4],GetStrongHoldName(holdNameId2));        
                        break;
                    case  GuildNotify.RALLY_GUILD_BUILD:
                        int type = int.Parse(lsStringArg[2]);
                        var type1 = m_AllianceProxy.GetBuildServerTypeToConfigType(type);   
                        AllianceBuildingTypeDefine define   = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type1);
                        if (define != null)
                        {
                            int nameID = define.l_nameId;
                            string guildName = LanguageUtils.getText(nameID);
                            str = LanguageUtils.getTextFormat(730283, lsStringArg[0],lsStringArg[1],guildName);      
                        }
                        break;
                   case GuildNotify.GUILD_BUILD_RALLYED:
                       guildFlag.Add(int.Parse(lsStringArg[0]));
                       guildFlag.Add(int.Parse(lsStringArg[1]));
                       guildFlag.Add(int.Parse(lsStringArg[2]));
                       guildFlag.Add(int.Parse(lsStringArg[3]));                                            
                       int type2 = int.Parse(lsStringArg[5]);
                       var type3 = m_AllianceProxy.GetBuildServerTypeToConfigType(type2);   
                       AllianceBuildingTypeDefine define1   = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type3);
                       if (define1 != null)
                       {
                           int nameID = define1.l_nameId;
                           string guildName = LanguageUtils.getText(nameID);
                           str = LanguageUtils.getTextFormat(730272, lsStringArg[4],guildName);      
                       }
                       
                       break;
                }
            }

            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            var asset = new Tip.OtherAssetData("UI_Common_Aggregation",(obj)=>{
                                            
                UI_Common_AggregationView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_AggregationView>(obj);
                var data = m_AllianceProxy.GetAlliance();
                tipView.m_lbl_message_LanguageText.text = str;
                tipView.m_UI_Model_GuildFlag.m_img_flag_noali_PolygonImage.gameObject.SetActive(data == null);
                if (data != null)
                {
                    if (guildFlag.Count > 0)
                    {
                        tipView.m_UI_Model_GuildFlag.setData(guildFlag);   
                    }
                    else
                    {                    
                        tipView.m_UI_Model_GuildFlag.setData(data.signs);   
                    }

                }
                
            });
            Tip.CreateTip(asset).Show();
        }

        public void Hide(int id)
        {
            
        }

        public void Clear()
        {
            lsName.Clear();
            lsStringArg.Clear();
            guildFlag.Clear();
        }

        private string GetStrongHoldName(int nameId)
        {
            string name= String.Empty;
            StrongHoldTypeDefine strongHoldTypeDefine=CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(nameId);
           
            if (strongHoldTypeDefine != null)
            {
                name=LanguageUtils.getText(strongHoldTypeDefine.l_nameId);
            }
            return name;
        }
    }
}