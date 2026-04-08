// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月24日
// Update Time         :    2020年4月24日
// Class Description   :    UI_Text_SkillAtk_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using DG.Tweening;

namespace Game {
    public partial class UI_Text_SkillAtk_SubView : UI_SubView
    {
        
        public void SetDes(string des ,Vector2 v2,bool randomFlag = false)
        {
            if (this.m_Txt_word_LanguageText==null)
            {
                return;
            }

            this.m_Txt_word_LanguageText.gameObject.SetActive(false);
            this.m_Txt_word_LanguageText.gameObject.SetActive(true);
            this.m_Txt_word_LanguageText.text = des;
            float y = Random.Range(0, 50f);
            float x = Random.Range(0, 60f);

            if (randomFlag)
            {
                x = 0;
                y = 0;
            }

            int dir = Random.Range(0, 100);
            Vector3 v3= Vector3.zero;;
            if (dir <= 50)
            {
                 v3= new Vector3(v2.x+x,v2.y + y);
            }
            else
            {
                 v3= new Vector3(v2.x-x,v2.y + y);
            }
            this.m_UI_Text_SkillAtk.transform.DOLocalMove(v3,2f);
        }

        public void RestPos()  
        {
            if (this.m_Txt_word_LanguageText != null)
            {
                this.m_Txt_word_LanguageText.gameObject.SetActive(false);
                this.m_UI_Text_SkillAtk.transform.DOLocalMove(Vector3.zero, 0f);
            }
        }
    }
}