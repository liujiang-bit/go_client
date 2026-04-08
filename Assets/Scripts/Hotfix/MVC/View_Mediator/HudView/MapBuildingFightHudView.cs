using Client;
using DG.Tweening;
using Hotfix;
using Skyunion;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class MapBuildingFightHudView : FightUiHudView
    {
        private Transform m_hudRoot;
        private UI_Tip_WorldObjectPvPView m_buildingCmdView;
        private MapViewLevel m_viewLevel = MapViewLevel.Continental;
        private GameObject m_targetObject;
        private MapObjectInfoEntity mapObjectInfoEntity;

        public void Create(int id, GameObject go, MapObjectInfoEntity data)
        {
            HUDUI hud = HUDUI
                .Register(UI_Tip_WorldObjectPvPView.VIEW_NAME, typeof(UI_Tip_WorldObjectPvPView),
                    HUDLayer.world, go.gameObject).SetData(id)
                .SetInitCallback(OnHudFightCallBack)
                .SetUpdateCallback(UpdateFightHUDUI)
                .SetCameraLodDist(500f, 3000f, (isOn, view) =>
                {
                    if (isOn)
                    {
                        WorldMapLogicMgr.Instance.BattleUIEffectHandler.Remove(id);

                        m_buildingCmdView.m_pl_beTarget_Animator.Play("Empty");
                        m_buildingCmdView.m_UI_Model_CaptainHead.Play("Empty");
                    }
                }).SetPosOffset(new Vector2(0, -30f));
            ClientUtils.hudManager.ShowHud(hud);
            this.m_targetObject = go;
            this.mapObjectInfoEntity = data;
            HuduiView = hud;
        }


        public void UpdateBuildingHead()
        {
            if (mapObjectInfoEntity.mainHeroId == 0)
            {
                m_buildingCmdView.m_UI_Model_CaptainHead.LoadHeadDefID((int) mapObjectInfoEntity.guardTowerLevel);
            }
            else
            {
                m_buildingCmdView.m_UI_Model_CaptainHead.LoadHeadID(mapObjectInfoEntity.mainHeroId); 
            }
            m_buildingCmdView.m_UI_Model_CaptainHead.SetFightArmyRare(mapObjectInfoEntity);
        }

        public void SetDefMaxValue()
        {
            HUDHelp.SetBloodProgress(m_buildingCmdView.m_pl_sd_GameSlider_GameSlider, m_buildingCmdView.m_img_Fill_PolygonImage, 1, 1);
        }

        private void UpdateFightHUDUI(HUDUI hudui)
        {
            if (this.m_buildingCmdView == null)
            {
                return;
            }

            if (mapObjectInfoEntity == null)
            {
                return;
            }

            m_buildingCmdView.m_UI_Item_WorldArmyCmdAp.SetImgActive(mapObjectInfoEntity.maxSp, mapObjectInfoEntity.sp);

            if (mapObjectInfoEntity.armyCountMax == 0)
            {
                HUDHelp.SetBloodProgress(m_buildingCmdView.m_pl_sd_GameSlider_GameSlider, m_buildingCmdView.m_img_Fill_PolygonImage, 1, 1);
            }
            else
            {
                HUDHelp.SetBloodProgress(m_buildingCmdView.m_pl_sd_GameSlider_GameSlider, m_buildingCmdView.m_img_Fill_PolygonImage, mapObjectInfoEntity.armyCountMax, mapObjectInfoEntity.armyCount);
            }

        }

        private void OnHudFightCallBack(HUDUI info)
        {
            UI_Tip_WorldObjectPvPView uiView = info.gameView as UI_Tip_WorldObjectPvPView;
            int id = (int) info.data;
            this.m_buildingCmdView = uiView;
            m_buildingCmdView.gameObject.name = string.Format("{0}_{1}", "BuildingFight", id);
            m_buildingCmdView.gameObject.GetComponent<MapElementUI>().enabled = false;
            HUDHelp.SetBloodProgress(m_buildingCmdView.m_pl_sd_GameSlider_GameSlider, m_buildingCmdView.m_img_Fill_PolygonImage, 1, 1);
            UpdateBuildingHead();
            m_buildingCmdView.m_pl_buildingHp.gameObject.SetActive(false);
            if (mapObjectInfoEntity.rssType == RssType.City)
            {
                string guildName = string.Empty;
                if (!string.IsNullOrEmpty(mapObjectInfoEntity.guildAbbName))
                {
                    guildName=string.Format("[{0}]", mapObjectInfoEntity.guildAbbName);
                }

                m_buildingCmdView.m_lbl_playerName_LanguageText.text = string.Format("{0}{1}", guildName, mapObjectInfoEntity.cityName);
                m_buildingCmdView.m_lbl_playerName_LanguageText.color = HUDHelp.GetCityColor(mapObjectInfoEntity);

                m_buildingCmdView.m_lbl_level_LanguageText.text =   mapObjectInfoEntity.cityLevel.ToString();
                m_buildingCmdView.m_pl_bg0_PolygonImage.gameObject.SetActive(true);
                m_buildingCmdView.m_pl_bg1_PolygonImage.transform.DOLocalMoveY(-28.2f, 0f);
                m_buildingCmdView.m_pl_bg1_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_buildingCmdView.m_lbl_playerName_LanguageText.text = string.Empty;
                m_buildingCmdView.m_pl_bg0_PolygonImage.gameObject.SetActive(false);
                m_buildingCmdView.m_pl_bg1_PolygonImage.transform.DOLocalMoveY(-7.2f, 0f);
                info.SetPosOffset(new Vector2(0,-30f));
                m_buildingCmdView.m_lbl_level_LanguageText.text = string.Empty;
                m_buildingCmdView.m_pl_bg1_PolygonImage.gameObject.SetActive(false);
            }
            m_buildingCmdView.m_pl_ap.gameObject.SetActive(mapObjectInfoEntity.mainHeroId != 0);
        }

        public override void ShowHeadStatus()
        {
            base.ShowHeadStatus();
            if (battleUiData == null)
            {
                return;
            }

            if (m_buildingCmdView == null)
            {
                return;
            }

            switch (battleUiData.type)
            {
                case BattleUIType.BattleUI_HeadChangeScale:
                    m_buildingCmdView.m_pl_captainhead_Animator.Play("UseSkill");
                    //Debug.LogError("播放头像变大");
                    break;
                case BattleUIType.BattleUI_ShowHeadEffect:
                    //Debug.LogError("播放ui特效");                 
                    WorldMapLogicMgr.Instance.BattleUIEffectHandler.Play(battleUiData.id, RS.BattleHudHeadEffect,
                        m_buildingCmdView.m_UI_Model_CaptainHead.gameObject.transform);
                    break;
                case BattleUIType.BattleUI_ShowViceHeadEffect:
                    //Debug.LogError("播放ui特效");                 
                    WorldMapLogicMgr.Instance.BattleUIEffectHandler.Play(battleUiData.id, RS.BattleHudHeadEffect,
                        m_buildingCmdView.m_UI_SubCaptain.gameObject.transform);
                    break;
                case BattleUIType.BattleUI_ShowViceHead:
                    m_buildingCmdView.m_UI_SubCaptain.gameObject.SetActive(true);
                    m_buildingCmdView.m_pl_subCaptain_Animator.Play("UseSkill");
                    WorldMapLogicMgr.Instance.BattleUIEffectHandler.Play(battleUiData.id, RS.BattleHudHeadEffect,
                        m_buildingCmdView.m_UI_SubCaptain.gameObject.transform);
                    break;
                case BattleUIType.BattleUI_ShowBeAttack:
                    m_buildingCmdView.m_pl_beTarget_Animator.Play("Flash");
                    break;
                case BattleUIType.BattleUI_ShowBeAttackIcon:
                    m_buildingCmdView.m_UI_Model_CaptainHead.Play("Flash");
                    m_buildingCmdView.m_pl_captainhead_Animator.transform.DOShakePosition(5, new Vector3(0, 3, 0));
                    break;
            }
        }

        public override void FadeOut()
        {
            base.FadeOut();
            this.Close();
        }

        /// <summary>
        /// 更新Hud的跟随物体
        /// </summary>
        /// <param name="targetObj"></param>
        public void UpdateHudTargetObj(GameObject targetObj)
        {
            HuduiView.targetObj = targetObj;
            this.m_targetObject = targetObj;
        }
    }
}