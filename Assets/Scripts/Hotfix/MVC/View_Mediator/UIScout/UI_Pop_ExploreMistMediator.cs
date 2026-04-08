// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Pop_ExploreMistMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game
{
    public class UI_Pop_ExploreMistMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Pop_ExploreMistMediator";
        private Vector2 m_pos;

        #endregion

        //IMediatorPlug needs
        public UI_Pop_ExploreMistMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Pop_ExploreMistView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
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
            WarFogMgr.RemoveFadeGroupByType(1);
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        protected override void InitData()
        {
            Vector2 v2 = (Vector2) view.data;
            m_pos = v2;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_StandardButton_Yellow.AddClickEvent(OnSearchClick);
        }

        protected override void BindUIData()
        {
            CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonClickButton3);
            Debug.LogFormat("m_pos:{0}", m_pos);
            FogSystemMediator fogMedia =
                AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            var groupId = fogMedia.GetFadeGroupId(m_pos.x, m_pos.y, WarFogMgr.GROUP_SIZE);

            var fogs = fogMedia.GetGroupFog(m_pos.x, m_pos.y);

            var tilePos = fogMedia.Pos2Tile(m_pos.x, m_pos.y);
            Vector2Int lbPos = new Vector2Int();
            lbPos.x = Mathf.FloorToInt(tilePos.x / WarFogMgr.GROUP_SIZE) * WarFogMgr.GROUP_SIZE * 3;
            lbPos.y = Mathf.FloorToInt(tilePos.y / WarFogMgr.GROUP_SIZE) * WarFogMgr.GROUP_SIZE * 3;
            Vector2Int rtPos = new Vector2Int();
            rtPos.x = Mathf.FloorToInt(tilePos.x / WarFogMgr.GROUP_SIZE + 1) * WarFogMgr.GROUP_SIZE * 3;
            rtPos.y = Mathf.FloorToInt(tilePos.y / WarFogMgr.GROUP_SIZE + 1) * WarFogMgr.GROUP_SIZE * 3;

            view.m_lbl_pos2_LanguageText.text = LanguageUtils.getTextFormat(300032, lbPos.x, lbPos.y);
            view.m_lbl_pos1_LanguageText.text = LanguageUtils.getTextFormat(300032, rtPos.x, rtPos.y);

            var rect = view.m_img_bg_PolygonImage.rectTransform.rect;
            //重点： 世界坐标转屏幕坐标
            Vector3 curPos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), new Vector3(m_pos.x, 0, m_pos.y));
            //var curPos = Input.mousePosition;

            view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(false);
            view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(false);
            view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(false);
            view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(false);

            RectTransform viewRect = view.gameObject.GetComponent<RectTransform>();
            Vector2 localPos;
            RectTransform posRect = view.m_pl_pos.gameObject.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(posRect,
                                                                    curPos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            // 左
            if (localPos.x < viewRect.rect.width / 2)
            {
                // 上
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    localPos.y -= (rect.height / 2);
                    if (localPos.x < rect.width / 2)
                    {
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                }
                // 下
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y += (rect.height / 2);
                    if (localPos.x < rect.width / 2)
                    {
                        localPos.x = rect.width / 2;
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x += (rect.width / 2);
                    view.m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                }
            }
            // 右
            else
            {
                // 上
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    localPos.y -= (rect.height / 2);
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        localPos.x = (viewRect.rect.width - rect.width / 2);
                    }
                    view.m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                }
                // 下
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y += (rect.height / 2);
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        localPos.x = (viewRect.rect.width - rect.width / 2);
                    }
                    view.m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x -= (rect.width / 2);
                    view.m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                }
            }

            //var pos = CoreUtils.uiManager.GetUICamera().ScreenToWorldPoint(curPos);
            //var bgPos = view.m_img_bg_PolygonImage.transform.position;
            //bgPos.x = pos.x;
            //bgPos.y = pos.y;
            view.m_img_bg_PolygonImage.transform.localPosition = localPos;

            view.m_img_mist_GridLayoutGroup.constraintCount = WarFogMgr.GROUP_SIZE;
            var cellsize = view.m_img_mist_PolygonImage.rectTransform.sizeDelta.x / WarFogMgr.GROUP_SIZE;
            view.m_img_mist_GridLayoutGroup.cellSize = new Vector2(cellsize, cellsize);

            view.m_fog_img_Image.color = new Color(1, 1, 1, 0);
            float witdh = view.m_fog_img_Image.sprite.textureRect.width / WarFogMgr.GROUP_SIZE;
            float height = view.m_fog_img_Image.sprite.textureRect.height / WarFogMgr.GROUP_SIZE;
            Vector2 off = view.m_fog_img_Image.sprite.textureRect.min;

            var newSprite = Sprite.Create(view.m_fog_img_Image.sprite.texture, new Rect(off.x, (WarFogMgr.GROUP_SIZE-1)*height+ off.y, witdh, height), new Vector2(0.5f, 0.5f));
            view.m_fog_img_Image.sprite = newSprite;
            for (int i = 1; i < WarFogMgr.GROUP_SIZE * WarFogMgr.GROUP_SIZE; i++)
            {
                int row = i / WarFogMgr.GROUP_SIZE;
                int col = i % WarFogMgr.GROUP_SIZE;
                newSprite = Sprite.Create(view.m_fog_img_Image.sprite.texture, new Rect(col* witdh+ off.x, (WarFogMgr.GROUP_SIZE-row-1) * height+ off.y, witdh, height), new Vector2(0.5f, 0.5f));
                var go = GameObject.Instantiate(view.m_fog_img_Image.gameObject, view.m_img_mist_PolygonImage.transform);
                go.GetComponent<Image>().sprite = newSprite;
            }

            for (int i = 0; i < fogs.Count; i++)
            {
                var fog = fogs[i];
                int nIndex = fog.x + (WarFogMgr.GROUP_SIZE - fog.y - 1) * WarFogMgr.GROUP_SIZE;
                var img = view.m_img_mist_PolygonImage.transform.GetChild(nIndex).gameObject.GetComponent<Image>();
                img.color = Color.white;
            }
        }

        #endregion

        private void OnSearchClick()
        {
            UI_Pop_ScoutSelectMediator.Param param = new UI_Pop_ScoutSelectMediator.Param();
            param.pos = m_pos;
            param.targetIndex = 0;
            CoreUtils.uiManager.ShowUI(UI.s_scoutSelectMenu, null, param);
            CoreUtils.uiManager.CloseUI(UI.s_scoutSearchMenuu);
        }
    }
}