// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, May 13, 2020
// Update Time         :    Wednesday, May 13, 2020
// Class Description   :    UI_Item_GuildResearchSubItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Item_GuildResearchSubItem_SubView : UI_SubView
    {
         private AllianceStudyDefine m_res;

        protected override void BindEvent()
        {
            m_btn_research_GameButton.onClick.AddListener(OnUpdateResource);
        }

        private bool isDebug = false;

        private GrayChildrens cp;

        private Vector3 s_initPos = Vector3.zero;
        private Vector3 s_initScale = Vector3.zero;
        
        
        private bool HasReachPreStudy(AllianceStudyDefine data, AllianceResarchProxy proxy)
        {
            int countPre = 0;
            int totalPre = 0;
            if (data.preconditionStudy1 >0)
            {
                totalPre++;
                int preLv = proxy.GetCrrTechnologyLv(data.preconditionStudy1);

                if (preLv>=data.preconditionLv1)
                {
                    countPre++;
                }
               
            }
                
            if (data.preconditionStudy2 >0)
            {
                totalPre++;
                int preLv = proxy.GetCrrTechnologyLv(data.preconditionStudy2);

                if (preLv>=data.preconditionLv2)
                {
                    countPre++;
                }
               
            }
                
            if (data.preconditionStudy3 >0)
            {
                totalPre++;
                int preLv = proxy.GetCrrTechnologyLv(data.preconditionStudy3);

                if (preLv>=data.preconditionLv3)
                {
                    countPre++;
                }
               
            }
                
            if (data.preconditionStudy4>0)
            {
                totalPre++;
                int preLv = proxy.GetCrrTechnologyLv(data.preconditionStudy4);

                if (preLv>=data.preconditionLv4)
                {
                    countPre++;
                }
               
            }

            return countPre == totalPre;
        }

        
        private int CheckSonReachPreStudy(List<AllianceStudyDefine> crossCol,AllianceStudyDefine data, AllianceResarchProxy proxy)
        {
            int line = 1;
            for (int i = 0; i < crossCol.Count; i++)
            {
                var cross = crossCol[i];

                if (line ==1 && cross.preconditionStudy1 == data.studyType)
                {
                    int preLv = proxy.GetCrrTechnologyLv(cross.preconditionStudy1);
                    
                    if (preLv < cross.preconditionLv1)
                    {
                        line = 0;
                        continue;
                    }
                }
                
                if (line ==1 && cross.preconditionStudy2 == data.studyType)
                {
                    int preLv = proxy.GetCrrTechnologyLv(cross.preconditionStudy2);


                    if (preLv < cross.preconditionLv2)
                    {
                        line = 0;
                        continue;
                    }
                }
                
                if (line ==1 && cross.preconditionStudy3 == data.studyType)
                {
                    int preLv = proxy.GetCrrTechnologyLv(cross.preconditionStudy3);


                    if (preLv < cross.preconditionLv3)
                    {
                        line = 0;
                        continue;
                    }
                }
                
                if (line ==1 && cross.preconditionStudy4 == data.studyType)
                {
                    int preLv = proxy.GetCrrTechnologyLv(cross.preconditionStudy4);


                    if (preLv < cross.preconditionLv4)
                    {
                        line = 0;
                        continue;
                    }
                }
                
                
            }

            return line;
        }


        public void setData(AllianceStudyDefine data, int crrLv, int maxLv, AllianceResarchProxy proxy,int tagType,int jumpStudyID )
        {
            m_res = data;
            
            
            m_pl_mark_ArabLayoutCompment.gameObject.SetActive(proxy.GetMarkType()==data.studyType);

            if (jumpStudyID>0 && jumpStudyID == data.studyType)
            {
                FingerTargetParam param = new FingerTargetParam();
                param.AreaTarget = m_img_icon_PolygonImage.gameObject;
                param.ArrowDirection = (int) EnumArrorDirection.Up;
                CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
            }


            if (s_initPos.Equals(Vector3.zero))
            {
                s_initPos = this.m_img_line_PolygonImage.transform.localPosition;
                s_initScale = this.m_img_line_PolygonImage.transform.localScale;
            }
            long resEndTime =proxy.GetTechEndTime() ;

            if (resEndTime > 0 && proxy.GetCrrTechnologying()==data.studyType)
            {
                m_pl_effect.gameObject.SetActive(true);
            }
            else
            {
                m_pl_effect.gameObject.SetActive(false);
            }

            cp = m_btn_research_GameButton.GetComponent<GrayChildrens>();
            
            
            m_img_initiativeSkill_PolygonImage.gameObject.SetActive(data.skillType==1);


            if (isDebug == false)
            {
                if (HasReachPreStudy(data,proxy))
                {
                    ClientUtils.LoadSprite(m_btn_research_PolygonImage, RS.ResearchBG[1]);
                    cp.Normal();
                }
                else
                {
                    ClientUtils.LoadSprite(m_btn_research_PolygonImage, RS.ResearchBG[0]);
                    cp.Gray();
                }
            }
            

            //m_btn_research_GameButton.onClick.AddListener(OnUpdateResource);
            

            var spImg = data.icon;
            var spLanID = data.l_nameID;

            m_lbl_barText_LanguageText.text = string.Format("{0}/{1}", crrLv, maxLv);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(spLanID);
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, spImg);
            m_pb_bar_GameSlider.value = (float)crrLv / maxLv;


            // if (m_pb_bar_GameSlider.value>0.5f)
            // {
            //     ClientUtils.TextSetColor(m_lbl_barText_LanguageText, RS.ResearchItemFontColor[1]);
            // }
            // else
            // {
            //     ClientUtils.TextSetColor(m_lbl_barText_LanguageText, RS.ResearchItemFontColor[0]);
            // }

            int line = 1;
            
            if (crrLv==0)
            {
                line = 0;
            }

            if (line == 0)
            {
                this.gameObject.transform.SetAsFirstSibling();
            }
           


            m_img_line_PolygonImage.gameObject.SetActive(false);

            var sons = proxy.GetTechnologySub(data.studyType);


            if (sons == null)
            {
                m_img_line_PolygonImage.gameObject.SetActive(false);
                m_img_line2_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                if (isDebug)
                {
                    m_lbl_name_LanguageText.text += "S:" + sons.Count;
                    m_lbl_name_LanguageText.text += "L:" + m_res.location;
                    m_lbl_barText_LanguageText.text = "";
                }

                m_img_line_PolygonImage.gameObject.SetActive(true);
                //子列
                List<AllianceStudyDefine> subCol = new List<AllianceStudyDefine>();
                //跨列的
                List<AllianceStudyDefine> crossCol = null;

                foreach (var s in sons)
                {
                    if (isDebug)
                    {
                        m_lbl_barText_LanguageText.text += LanguageUtils.getText(s.l_nameID) + s.columns + ",";
                    }

                    if (data.columns + 2 == s.columns)
                    {
                        if (crossCol == null)
                        {
                            crossCol = new List<AllianceStudyDefine>();
                        }

                        crossCol.Add(s);
                    }
                    else if (data.columns + 1 == s.columns)
                    {
                        subCol.Add(s);
                    }
                }

                m_img_line_PolygonImage.rectTransform.localScale = s_initScale;
                m_img_line_PolygonImage.rectTransform.localPosition = s_initPos;
                
                
                if (crrLv==maxLv  && crrLv >0)
                {
                    line = 1;
                }
                else
                {
                    line = CheckSonReachPreStudy(sons, data, proxy);
                }
                
                if (line == 0)
                {
                    this.gameObject.transform.SetAsFirstSibling();
                }


                //第一种情况 子集都在一个列里面 多多映射
                if (crossCol == null)
                {
                    m_img_line_PolygonImage.gameObject.SetActive(true);
                    m_img_line2_PolygonImage.gameObject.SetActive(false);

                    if (subCol.Count == 1)
                    {
                        //横线
                        if (subCol[0].location == m_res.location)
                        {
                            ClientUtils.LoadSprite(m_img_line_PolygonImage, RS.GuildResearchLine[tagType-1][line][subCol.Count], true);
                        }
                        else
                        {
                            //多对一   上下线镜像
                            bool isMir = m_res.location > subCol[0].location;
                            float offY = -1f;            //镜像图片位移
                            if (isDebug)
                            {
                                if (isMir)
                                {
                                    m_lbl_barText_LanguageText.text += m_res.location + "◤" + subCol[0].location + ",";
                                }
                                else
                                {
                                    m_lbl_barText_LanguageText.text += m_res.location + "◣" + subCol[0].location + ",";
                                }
                            }

                            if (isMir)
                            {
                                //垂直镜像
                                m_img_line_PolygonImage.rectTransform.localScale = new Vector3(s_initScale.x, -1, s_initScale.z);
                                offY = 1f;
                            }

                            int count = Mathf.Max(m_res.location, subCol[0].location) -
                                        Mathf.Min(m_res.location, subCol[0].location);

                            string spname = RS.ResearchLineUpToDown[count][line];


                            if (count==2 && subCol[0].location==5)
                            {
                                spname = RS.ResearchLineUpToDown21[line];
                            }
                            
                            //斜线情况 上到下 长线
                            ClientUtils.LoadSprite(m_img_line_PolygonImage, spname, true,
                                () =>
                                {
                                    m_img_line_PolygonImage.rectTransform.localPosition = new Vector3(s_initPos.x,
                                        s_initPos.y +(offY*(m_img_line_PolygonImage.sprite.rect.height / 2)) -offY*2.5f, s_initPos.z);
                                });
                           
                        }
                        
                    }
                    else
                    {

                        if (m_res.location == 5 && subCol.Count==2)
                        {
                            //1分2中点大个
                            ClientUtils.LoadSprite(m_img_line_PolygonImage, RS.GuildResearch1_2[line], true);
                        }
                        else
                        {
                            //1分多
                            ClientUtils.LoadSprite(m_img_line_PolygonImage, RS.GuildResearchLine[tagType-1][line][subCol.Count], true);
                        }

                        
                    }
                }
                else
                {
                    //跨列的 需要用2条线来绘制   List的Cache size 属性需要设置为2个ItemWith的大小
                    m_img_line_PolygonImage.gameObject.SetActive(subCol.Count > 0);
                    m_img_line2_PolygonImage.gameObject.SetActive(true);

                    if (subCol.Count > 0)
                    {
                        string line1 = RS.ResearchSubAndCrossLine[line][subCol.Count];
                            
                        if (m_res.studyType == 121)
                        {
                            //1-2 这个比较特殊 大个的1-2
                            line1 = RS.ResearchLineSp1TO2[line];
                        }
                        ClientUtils.LoadSprite(m_img_line_PolygonImage,line1 ,
                            true, (
                                () =>
                                {
                                    if (subCol.Count == 1)
                                    {
                                        //下到上线偏移
                                        m_img_line_PolygonImage.rectTransform.localPosition = new Vector3(s_initPos.x,
                                            s_initPos.y + m_img_line_PolygonImage.sprite.rect.height / 2, s_initPos.z);
                                    }
                                }));
                    }

                    //直线
                    if (crossCol.Count == 1)
                    {
                        if (m_res.location == crossCol[0].location)
                        {
                            //这个图片需要9宫格缩放
                            ClientUtils.LoadSprite(m_img_line2_PolygonImage, RS.GuildResearchLine[tagType-1][line][1], false);
                            m_img_line2_PolygonImage.rectTransform.sizeDelta = new Vector2(540f, 5f);
                        }
                    }
                    else if (crossCol.Count == 4)
                    {
                        //一分四 这个图片需要9宫格缩放  重新设置图片大小为Item的高  Item的宽度+原来的宽度  跨列了需要扩大一列
                        ClientUtils.LoadSprite(m_img_line2_PolygonImage, RS.GuildResearchLine[tagType-1][line][4], false);
                        m_img_line2_PolygonImage.rectTransform.sizeDelta = new Vector2(540f, 446f);
                    }
                }
            }
        }

        private void OnUpdateResource()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceResearchUpdate, null, m_res);
        }
    }
}