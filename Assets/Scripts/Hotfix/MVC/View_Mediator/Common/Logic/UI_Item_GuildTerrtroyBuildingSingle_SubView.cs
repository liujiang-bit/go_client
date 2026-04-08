// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 16, 2020
// Update Time         :    Thursday, April 16, 2020
// Class Description   :    UI_Item_GuildTerrtroyBuildingSingle_SubView
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
    public partial class UI_Item_GuildTerrtroyBuildingSingle_SubView : UI_SubView
    {


        private int type;
        private AllianceBuildingTypeDefine config;
        private AllianceProxy proxy;

        private PosInfo posInfo;


        private long  m_buildID;

        private void CheckUIShow(bool isShow)
        {
            this.m_pl_content.gameObject.SetActive(isShow);

            if (isShow==false)
            {
                return;
            }
            
            if (this.m_pl_content.transform.childCount>0)
            {
                var transform = this.m_pl_content.transform.GetChild(0);

                if (transform.name == config.imgShow)
                {
                    return;
                }
                else
                {
                    CoreUtils.assetService.Destroy(transform.gameObject);
                }
            }
//            Debug.Log(config.imgShow);
            
            CoreUtils.assetService.Instantiate(config.imgShow, (obj) =>
            {
                if (obj!=null)
                {
                    int childCount = this.m_pl_content.transform.childCount;
                    if (childCount > 0)
                    {
                        for (int i = childCount - 1; i >= 0; i--)
                        {
                            CoreUtils.assetService.Destroy(this.m_pl_content.transform.GetChild(i).gameObject);
                        }
                    }
                    obj.transform.SetParent(this.m_pl_content.transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one*0.4f;
                    obj.name = config.imgShow;
                    
                    if (config.type == 3)
                    {
                        AllianceProxy.SetUIFlag(proxy.GetAlliance().signs,obj);
                    }
                }
            });
        }

        private int GetBuildPro(AllianceBuildingTypeDefine config,GuildBuildInfoEntity buildInfo)
        {
            
            if (buildInfo.buildFinishTime==0)
            {
                return (int)((float)buildInfo.buildProgress/config.S*100);
            }
            
            float rate = ((float)config.S - buildInfo.buildProgress) /
                         (buildInfo.buildFinishTime - buildInfo.buildProgressTime);
            

            int pro = Mathf.FloorToInt((buildInfo.buildProgress+rate * (ServerTimeModule.Instance.GetServerTime()-buildInfo.buildProgressTime))/config.S*100f);

            if (pro>100)
            {
                pro = 100;
            }

            if (pro<0)
            {
                pro = 0;
            }
            
            return pro;
        }
        
        private int GetBuildPro(AllianceBuildingTypeDefine config,GuildBuildInfo buildInfo)
        {

            if (buildInfo.buildFinishTime==0)
            {
                return (int)((float)buildInfo.buildProgress/config.S*100);
            }
            
            float rate = ((float)config.S - buildInfo.buildProgress) /
                         (buildInfo.buildFinishTime - buildInfo.buildProgressTime);

            int pro = Mathf.FloorToInt((buildInfo.buildProgress+rate * (ServerTimeModule.Instance.GetServerTime()-buildInfo.buildProgressTime))/config.S*100f);

            if (pro>100)
            {
                pro = 100;
            }

            if (pro<0)
            {
                pro = 0;
            }
            
            return pro;
        }

        public void setData(int type,AllianceBuildingTypeDefine config,AllianceProxy proxy,int lineindex,int col, GuildBuildInfo subFlagData=null)
        {

            if (proxy==null)
            {
                return;
            }
            
            this.type = type;
            this.config = config;
            this.proxy = proxy;
            this.posInfo = null;
            this.m_lbl_name_LanguageText.text = LanguageUtils.getText(config.l_nameId);
            this.m_btn_info_GameButton.onClick.RemoveAllListeners();
            this.m_btn_info_GameButton.onClick.AddListener(OnTipInfo);
            
            this.m_btn_link.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            
            this.m_btn_link.m_btn_treaty_GameButton.onClick.AddListener(onJumpMap);
            
            this.m_btn_army_GameButton.onClick.RemoveAllListeners();
            this.m_btn_army_GameButton.onClick.AddListener(onJumpMapArmy);
            
            
            this.m_btn_build.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
            this.m_btn_build.m_btn_languageButton_GameButton.onClick.AddListener(onCreate);
            
            this.m_btn_plus_GameButton.onClick.RemoveAllListeners();
            this.m_btn_plus_GameButton.onClick.AddListener(onCreate);


            this.m_btn_plus_GameButton.gameObject.SetActive(true);

            switch (type)
            {
                case 0:
                {
                    //要塞
                    GuildBuildInfoEntity buidInfo = proxy.GetFortressesByType(config.type);



                    bool hasPreBuild = false;

                    if (config.preBuilding1 > 0)
                    {
                        GuildBuildInfoEntity preBuildInfo = proxy.GetFortressesByType(config.preBuilding1);

                        hasPreBuild = preBuildInfo == null;
                    }
                    

                    bool canBuild = proxy.GetAlliance().memberNum >= config.playerNum &&
                                    proxy.GetAlliance().power >= config.alliancePower && buidInfo == null &&
                                    !hasPreBuild;


                    this.m_pl_build.gameObject.SetActive(canBuild);
                    this.m_pl_data.gameObject.SetActive(buidInfo != null);
                    this.m_lbl_limit_LanguageText.gameObject.SetActive(!canBuild && buidInfo==null);


                    CheckUIShow(canBuild || buidInfo != null);
                    
                    this.m_btn_plus_GameButton.gameObject.SetActive(false);
                    
                    if (buidInfo != null)
                    {
                        this.m_btn_army_GameButton.gameObject.SetActive(buidInfo.isReinforce==false && buidInfo.status ==(long) GuildBuildState.building);
                        this.m_btn_link.m_UI_Model_Link_LanguageText.text =
                            LanguageUtils.getTextFormat(300032, Mathf.FloorToInt(buidInfo.pos.x/600),Mathf.FloorToInt(buidInfo.pos.y/600));
                        this.m_lbl_state_LanguageText.text = RS.GetGuildBuildState(buidInfo);
                        
                        ClientUtils.TextSetColor(this.m_lbl_state_LanguageText,RS.GetGuildBuildStateColor(buidInfo));
//                            LanguageUtils.getText(RS.AllianceBuildStatie[buidInfo.status]);
                        
                        var state = (GuildBuildState) buidInfo.status;

                        if (state !=GuildBuildState.building)
                        {
                            switch (state)
                            {
                                case GuildBuildState.fire:
                                    float durable = buidInfo.durable- (ServerTimeModule.Instance.GetServerTime() - buidInfo.burnTime) *
                                                    buidInfo.burnSpeed / 100f;
                                
                                    //耐久度
                                    this.m_lbl_process_LanguageText.text =
                                        LanguageUtils.getTextFormat(732072, Mathf.FloorToInt(durable/buidInfo.durableLimit*100f)); //耐久度

                                    break;
                                case GuildBuildState.fix:


                                    var buildInfo = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)buidInfo.type);

                                    float durablefix = buidInfo.durable+ (ServerTimeModule.Instance.GetServerTime() - buidInfo.durableRecoverTime) *
                                                       buildInfo.durableUp/3600f;

                                    if (durablefix>buidInfo.durableLimit)
                                    {
                                        durablefix = buidInfo.durableLimit;
                                    }
                                    
                                    this.m_lbl_process_LanguageText.text =
                                        LanguageUtils.getTextFormat(732072, Mathf.FloorToInt(durablefix/buidInfo.durableLimit*100f)); //耐久度
                                    break;
                                default:
                                    //耐久度
                                    this.m_lbl_process_LanguageText.text =
                                        LanguageUtils.getTextFormat(732072, Mathf.FloorToInt(buidInfo.durable/buidInfo.durableLimit*100f)); //耐久度

                                    break;
                            }
                        }
                        else
                        {
                            
                          
                            this.m_lbl_process_LanguageText.text =
                                LanguageUtils.getTextFormat(732094, GetBuildPro(config,buidInfo)); //建造进度
                        }

                        posInfo = buidInfo.pos;
                        m_buildID = buidInfo.objectIndex;
                    }
                    else
                    {
                        if (canBuild == false)
                        {
                            this.m_lbl_limit_LanguageText.text =
                                LanguageUtils.getTextFormat(732001, config.playerNum, config.alliancePower);
                        }
                        else
                        {
                            
                        }
                    }
                }
                    

                    break;

                case 1:
                    //资源中心
                {
                    GuildBuildInfo buidInfo = proxy.GetResCenter().resourceCenter;
                    bool canBuild = proxy.GetAlliance().memberNum >= config.playerNum &&
                                    proxy.GetFlagInfoEntity().flagNum >= config.preNum1 && buidInfo == null ;
                   
                    this.m_pl_build.gameObject.SetActive(canBuild);
                    this.m_pl_data.gameObject.SetActive(buidInfo != null&& buidInfo.type == config.type);
                    this.m_lbl_limit_LanguageText.gameObject.SetActive(!canBuild);
                    
                    CheckUIShow(canBuild|| buidInfo != null && buidInfo.type == config.type );//
                    
                    this.m_btn_plus_GameButton.gameObject.SetActive(false);
                    
                    
                    
                    if (buidInfo != null  )
                    {
                        if (buidInfo.type == config.type)
                        {
                            this.m_btn_army_GameButton.gameObject.SetActive(buidInfo.isReinforce == false && buidInfo.status ==(long) GuildBuildState.building);
                            this.m_btn_link.m_UI_Model_Link_LanguageText.text =
                                LanguageUtils.getTextFormat(300032, Mathf.FloorToInt(buidInfo.pos.x/600),Mathf.FloorToInt(buidInfo.pos.y/600));
                            
                            if (buidInfo.status != (long) GuildBuildState.building)
                            {
                                //采集中
                                this.m_lbl_state_LanguageText.text =
                                    LanguageUtils.getText(732088);
                                
                                //储量
                                var resCenter = proxy.GetResCenter();
                                long passTime = ServerTimeModule.Instance.GetServerTime() - resCenter.collectTime;
                                long collectRes = passTime * resCenter.collectSpeed / 10000;
                                this.m_lbl_process_LanguageText.text =
                                    LanguageUtils.getTextFormat(732041, ClientUtils.FormatComma(resCenter.resource-collectRes)); //剩余数量
                            }
                            else
                            {
                                this.m_lbl_state_LanguageText.text = RS.GetGuildBuildState(buidInfo);
                                ClientUtils.TextSetColor(this.m_lbl_state_LanguageText,RS.GetGuildBuildStateColor(buidInfo));
                                this.m_lbl_process_LanguageText.text =
                                    LanguageUtils.getTextFormat(732094, GetBuildPro(config,buidInfo)); //建造进度
                            }

                            posInfo = buidInfo.pos;
                            m_buildID = buidInfo.objectIndex;
                            this.m_lbl_limit_LanguageText.text = "";
                            
                            ClientUtils.TextSetColor(this.m_lbl_state_LanguageText,RS.GetGuildBuildStateColor(buidInfo));
                        }
                        else
                        {
                            //只能一个
                            this.m_lbl_limit_LanguageText.text = LanguageUtils.getText(732091);
                        }
                    }
                    else
                    {
                      
                        if (canBuild == false)
                        {
                            m_lbl_name_LanguageText.text = LanguageUtils.getText(config.l_nameId);
                            this.m_lbl_limit_LanguageText.text =
                                LanguageUtils.getTextFormat(732090, config.playerNum,  config.preNum1);
                            
                        }
                        
                    }
                }


                    break;

                case 2:
                    //旗帜
                    if (lineindex == 0 && col==0)
                    {
                        this.m_pl_build.gameObject.SetActive(true);
                        this.m_pl_data.gameObject.SetActive(false);
                        this.m_lbl_limit_LanguageText.gameObject.SetActive(false);
                        this.m_btn_build.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                        this.m_btn_build.m_btn_languageButton_GameButton.onClick.AddListener(onCreate);
                    }
                    else
                    {
                       
                        this.m_btn_army_GameButton.gameObject.SetActive(subFlagData.isReinforce==false && subFlagData.status ==(long) GuildBuildState.building);
                        this.m_pl_build.gameObject.SetActive(false);
                        this.m_pl_data.gameObject.SetActive(true);
                        this.m_lbl_limit_LanguageText.gameObject.SetActive(false);
                        
                        this.m_btn_link.m_UI_Model_Link_LanguageText.text =
                            LanguageUtils.getTextFormat(300032, Mathf.FloorToInt(subFlagData.pos.x/600),Mathf.FloorToInt(subFlagData.pos.y/600));
                        this.m_lbl_state_LanguageText.text = RS.GetGuildBuildState(subFlagData);
                        
                        ClientUtils.TextSetColor(this.m_lbl_state_LanguageText,RS.GetGuildBuildStateColor(subFlagData));

                        var state = (GuildBuildState) subFlagData.status;
                        if (state !=GuildBuildState.building)
                        {
                            
                            switch (state)
                            {
                                case GuildBuildState.fire:
                                    float durable = subFlagData.durable- (ServerTimeModule.Instance.GetServerTime() - subFlagData.burnTime) *
                                                    subFlagData.burnSpeed / 100f;
                                
                                    //耐久度
                                    this.m_lbl_process_LanguageText.text =
                                        LanguageUtils.getTextFormat(732072, Mathf.FloorToInt(durable/subFlagData.durableLimit*100f)); //耐久度

                                    break;
                                case GuildBuildState.fix:


                                    var buildInfo = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)subFlagData.type);

                                    float durablefix = subFlagData.durable+ (ServerTimeModule.Instance.GetServerTime() - subFlagData.durableRecoverTime) *
                                                       buildInfo.durableUp/3600f;
                                    
                                    if (durablefix>subFlagData.durableLimit)
                                    {
                                        durablefix = subFlagData.durableLimit;
                                    }
                                    
                                    this.m_lbl_process_LanguageText.text =
                                        LanguageUtils.getTextFormat(732072, Mathf.FloorToInt(durablefix/subFlagData.durableLimit*100f)); //耐久度
                                    break;
                                default:
                                    //耐久度
                                    this.m_lbl_process_LanguageText.text =
                                        LanguageUtils.getTextFormat(732072, Mathf.FloorToInt(subFlagData.durable/subFlagData.durableLimit*100f)); //耐久度

                                    break;
                            }
                        }
                        else
                        {
                            this.m_lbl_process_LanguageText.text =
                                LanguageUtils.getTextFormat(732094,GetBuildPro(config,subFlagData));//建造进度
                        }

                       

                        posInfo = subFlagData.pos;
                        m_buildID = subFlagData.objectIndex;

                    }
                    CheckUIShow(subFlagData!=null);


                    break;
                
                case 3:
                    
                    break;
            }
        }

        private void onCreate()
        {
            if (proxy.GetSelfRoot(GuildRoot.createBuild)==false && config.imgShowIndex != 2 )//不是旗帜
            {
                Tip.CreateTip(730137).Show();
                return;
            }else if (proxy.GetSelfRoot(GuildRoot.createFlag)==false && config.imgShowIndex == 2)
            {
                Tip.CreateTip(730137).Show();
                return;
            }

            if (config.imgShowIndex == 2)//旗帜
            {
                if (config.preBuilding1 > 0)
                {
                    var allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                    GuildBuildInfoEntity preBuildInfo = allianceProxy.GetFortressesByType(config.preBuilding1);
                    if (preBuildInfo == null)
                    {
                        Tip.CreateTip(732100).Show();
                        return;
                    }
                }

                var flag = proxy.GetFlagInfoEntity();

                if (flag.flagNum >= flag.flagLimit)
                {
                    Tip.CreateTip(732010).Show();
                    return;
                }
            }

            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            if (config.imgShowIndex == 1)//资源中心
            {
                if (playerProxy.CurrentRoleInfo.level<proxy.Config.allianceResourcePointReqLevel)
                {
                    Tip.CreateTip(732023, proxy.Config.allianceResourcePointReqLevel).Show();
                    return;
                }
            }


            CoreUtils.uiManager.CloseGroupUI(UI.ALLIANCE_GRPOP,true,true);
            CityBuffProxy cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;

            if (cityBuffProxy.CheckGuildBuildCreatePre())
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceCreateBuildRes, null, config);
                AppFacade.GetInstance().SendNotification(CmdConstant.HideMainCityUI, EnumMainModule.All);
                float dxf = WorldCamera.Instance().getCameraDxf("dispatch");
                WorldCamera.Instance().SetCameraDxf(dxf, 1000, () => { });
            }
        }

        private void onJumpMap()
        {
            if (posInfo!=null)
            {
                CoreUtils.uiManager.CloseGroupUI(UI.ALLIANCE_GRPOP);
                
                WorldCamera.Instance().ViewTerrainPos(posInfo.x/100, posInfo.y/100, 0, () =>
                {
                    float dxf = WorldCamera.Instance().getCameraDxf("dispatch");
                    WorldCamera.Instance().SetCameraDxf(dxf, 1000, () => { });
                });
                
                
            }
        }
        
        private void onJumpMapArmy()
        {
            if (posInfo!=null)
            {
                CoreUtils.uiManager.CloseGroupUI(UI.ALLIANCE_GRPOP);
                WorldCamera.Instance().ViewTerrainPos(posInfo.x/100, posInfo.y/100, 0, () =>
                {
                    float dxf = WorldCamera.Instance().getCameraDxf("dispatch");
                    WorldCamera.Instance().SetCameraDxf(dxf, 1000, () =>
                    {
                        Debug.Log("前往建筑"+m_buildID); 
                        FightHelper.Instance.Reinfore((int)m_buildID,0,(int) m_buildID,0,0,true);
                    });
                });              
            }
        }


        public void OnTipInfo()
        {
            int tip = 0;
            switch (type)
            {
                case 0:
                    //要塞

                    if (config.type == 1) //主要塞
                    {
                        tip = 4012;
                    }
                    else
                    {
                        tip = 4013;
                    }


                    break;
                case 1:

                    tip = 4015;
                    break;
                
                case 2:
                    tip = 4011;
                    break;
                case 3:
                    tip = 4011;
                    break;
                default:
                    
                    break;
            }

            if (tip>0)
            {
                HelpTip.CreateTip(tip,this.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).Show();
            }
        }
    }
}