using Newtonsoft.Json;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AgreementCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            var signing = SDKBridge.shareInstance().getAgreementSigning().signing();
            Debug.Log("informAsap");
            signing.informAsap(OnIGGAgreementSigingLoadAsp);
        }

        void OnIGGAgreementSigingLoadAsp(SDKException exception, SDKAgreementSigningStatus var2)
        {
            if (exception.isNone())
            {
                Debug.Log($"OnIGGAgreementSigingLoadAsp1:{var2.getAgreeType()}");
                if (!var2.isNull())
                {
                    var signingFile = var2.prepareFileToBeSigned();
                    if (!signingFile.isNull())
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Agreement, null, signingFile);
                        return;
                    }
                    Debug.Log("mSDKAgreementSigningFile.isNull()");
                }
            }

            // 首次安装需要使用A方案协议
            string key = "FirstLogin_" + SDKSession.currentSession.getIGGId();
            if (PlayerPrefs.GetInt(key, 1) == 1)
            {
                PlayerPrefs.SetInt(key, 0);
                PlayerPrefs.Save();
                SendNotification(CmdConstant.HotfixUpteCheck);
            }
            // 非首次登陆需要使用B方案协议
            else
            {
                Debug.Log("informKindly");
                var signing = SDKBridge.shareInstance().getAgreementSigning().signing();
                signing.informKindly(OnIGGAgreementSigingLoadKindly);
            }
        }

        void OnIGGAgreementSigingLoadKindly(SDKException exception, SDKAgreementSigningStatus var2)
        {
            if (exception.isNone())
            {
                Debug.Log($"OnIGGAgreementSigingLoadKindly1:{var2.getAgreeType()}");
                if (!var2.isNull())
                {
                    var signingFile = var2.prepareFileToBeSigned();
                    if (!signingFile.isNull())
                    {
                        CoreUtils.uiManager.ShowUI(UI.s_Agreement, null, signingFile);
                        return;
                    }
                    Debug.Log("mSDKAgreementSigningFile.isNull()");
                }
            }
            Debug.Log($"OnIGGAgreementSigingLoadKindly2:{var2.getAgreeType()}");
            SendNotification(CmdConstant.HotfixUpteCheck);
        }
    }
}
