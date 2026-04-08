// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月29日
// Update Time         :    2019年12月29日
// Class Description   :    UI_Model_CaptainHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using Hotfix;

namespace Game {
    public partial class UI_Model_CaptainHead_SubView : UI_SubView
    {
        private PlayerProxy m_playerProxy;

        protected override void BindEvent()
        {
            base.BindEvent();
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

        }

        public void SetHero(HeroProxy.Hero hero)
        {
            if (hero == null) return;
            ClientUtils.LoadSprite(m_img_char_PolygonImage, hero.config.heroIcon);
            SetRare(hero.config.rare);
        }

        public void SetHero(long id, long lv)
        {
            SetHero((int)id);
            SetLevel(lv);
        }

        public void SetHero(int id)
        {
            var define = CoreUtils.dataService.QueryRecord<HeroDefine>(id);
            if (define != null)
            {
                ClientUtils.LoadSprite(m_img_char_PolygonImage, define.heroIcon);
                SetRare(define.rare);
            }
        }

        public void SetHero(HeroProxy.Hero hero, long lv)
        {
            if (hero == null) return;
            ClientUtils.LoadSprite(m_img_char_PolygonImage, hero.config.heroIcon);
            SetRare(hero.config.rare);
            SetLevel(lv);
        }

        public void SetIcon(string name)
        {
            ClientUtils.LoadSprite(m_img_char_PolygonImage, name);
        }

        public void SetFightArmyRare(string rare)
        {
            ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage, rare);
        }
        
        public void SetFightArmyRare(ArmyData data)
        {
            ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage, GetFightArmyRare(data));
        }

        public void SetFightArmyRare(MapObjectInfoEntity info)
        {
            ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage, GetFightArmyRare(info));
        }


        public string LoadHeadID(long id,long lv=0)
        {
            var define = CoreUtils.dataService.QueryRecord<HeroDefine>((int) id);
            if (define!=null)
            {
                if (define.rare > 0 && RS.HeroQualityBg.Length>=define.rare)
                {
                    ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage,RS.HeroQualityBg[define.rare-1]);
                }
                ClientUtils.LoadSprite(m_img_char_PolygonImage, define.heroIcon);
                return LanguageUtils.getTextFormat(300015,lv,LanguageUtils.getText(define.l_nameID));
            }
            return "";
        }
        public string LoadHeadID(long id, bool heroAwake )
        {
            var define = CoreUtils.dataService.QueryRecord<HeroDefine>((int)id);
            if (define != null)
            {
                if (define.rare > 0 && RS.HeroQualityBg.Length >= define.rare)
                {
                    ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage, RS.HeroQualityBg[define.rare - 1]);
                }
                ClientUtils.LoadSprite(m_img_char_PolygonImage, define.heroIcon);
                return  LanguageUtils.getText(define.l_nameID);
            }
            return "";
        }

        public void LoadHeadDefID(int level)
        {
            var define = CoreUtils.dataService.QueryRecord<BuildingGuardTowerDefine>(level);
            if (define != null)
            {
                ClientUtils.LoadSprite(m_img_char_PolygonImage, define.towerHead);
            }
        }

        public void SetRare(int rare)
        {
            ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage, RS.HeroQualityBg[rare-1]);

        }

        public void SetDefRare()
        {
            ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage, RS.HeroQualityBg[1]);
        }

        public void SetLevel(int level)
        {
            this.m_lbl_lv_LanguageText.text = level.ToString();
        }

        public void SetLevel(long level)
        {
            this.m_lbl_lv_LanguageText.text = level.ToString();
        }
        public void SetEmpty()
        {
            this.m_lbl_lv_LanguageText.text = "";
            this.m_img_lvbg_PolygonImage.gameObject.SetActive(false);
            this.m_img_char_PolygonImage.gameObject.SetActive(false);
        }

        public void Play(string name)
        {
            if (this.gameObject != null)
            {
                Animator ani = this.gameObject.GetComponent<Animator>();
                if (ani != null)
                {
                    ani.Play(name);
                }                
            }            
        }

        private string GetFightArmyRare(ArmyData dataContent)
        {
            return GetFightArmyRare(dataContent.guild,dataContent.armyRid);

        }

        private string GetFightArmyRare(MapObjectInfoEntity mapObjectInfoEntity)
        {
            long rid = 0;
            switch (mapObjectInfoEntity.rssType)
            {
                case RssType.City:
                    rid = mapObjectInfoEntity.cityRid;
                    break;
                case RssType.Troop:
                    rid = mapObjectInfoEntity.armyRid;
                    break;
                case RssType.BarbarianCitadel:
                case RssType.Monster:
                case RssType.Guardian:
                case RssType.SummonAttackMonster:
                case RssType.SummonConcentrateMonster:
                    return RS.MonsterFightArmyFrame;
                default:
                    rid = mapObjectInfoEntity.collectRid;
                    break;
            }

            return GetFightArmyRare(mapObjectInfoEntity.guildId,rid);
        }

        private string GetFightArmyRare(long guildID, long rid)
        {
            if (rid == 0 && guildID == 0)
            {
                CoreUtils.logService.Warn("战斗头像边框数据获取错误！！所属Rid和联盟ID都为0");
                return "";   
            }
            if (rid == m_playerProxy.Rid)
            {
                return RS.PlayerFightArmyFrame;
            }
            else if (guildID!=0 && guildID == m_playerProxy.CurrentRoleInfo.guildId)
            {
                return RS.AllianceFightArmyFrame;
            }else
            {
                return RS.OtherMapFightFrame;
            }
        }

        public void SetArmyRare(ArmyData armyData)
        {
            ClientUtils.LoadSprite(m_UI_Model_CaptainHead_PolygonImage, GetArmyRare(armyData));
        }

        private string GetArmyRare(ArmyData armyData)
        {
            if (armyData == null)
            {
                return  RS.HudHeroWhite;
            }

            if (armyData.isPlayerHave)
            {
                return RS.HudHeroGreen;
            }
            
            if (GuideProxy.IsGuideing)
            {
                return RS.HudHeroBlue;
            }
            Color color = HUDHelp.GetOtherTroopColor(armyData);
            if(color.Equals(RS.red))
            {
                return RS.HudHeroRed;
            }
            if (color.Equals(RS.blue))
            {
                return RS.HudHeroBlue;
            }
            
            return  RS.HudHeroWhite;
        }
    }
}