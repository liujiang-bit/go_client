using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using Skyunion;
using U3D.Threading;
using UnityEngine;

namespace Game
{
    public class StartUpCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            // pc 模式下默认打开日记
            if (HotfixUtil.IsDebugable())
            {
                if (GameObject.Find("IngameDebugConsole") == null)
                {
                    CoreUtils.assetService.Instantiate("IngameDebugConsole", (gameObject) => { });
                    CoreUtils.assetService.Instantiate("Graphy", (gameObject) => { });
                }
            }
            // 用于主线程执行函数的任务
            Dispatcher.Initialize();

            // 用于自动设置画质
            QualitySetting.Init();

            //>加载音量
            LoadVolume();

            // 加载缓存的语言
            LanguageUtils.LoadCache();

            // 替换接口
            IGGSDKUtils.shareInstance().ReplaceShowMsgBox1((string message, string title, string ok, string cancle, IGGSDKUtils.MsgBoxReturnListener.Listener listener)=>
            {
                var alert = Alert.CreateAlert(message, title);
                alert.SetRightButton(() => 
                {
                    listener(true);
                }, ok);
                alert.SetLeftButton(() =>
                {
                    listener(false);
                }, cancle);
                alert.Show();
            });
            IGGSDKUtils.shareInstance().ReplaceShowMsgBox2((string message, string title, string ok, IGGSDKUtils.MsgBoxReturnListener.Listener listener) =>
            {
                var alert = Alert.CreateAlert(message, title);
                alert.SetRightButton(() =>
                {
                    listener(true);
                }, ok);
                alert.Show();
            });
            IGGSDKUtils.ReplaceShowToast((string message) =>
            {
                Tip.CreateTip(message).Show();
            });
            if (LanguageUtils.GetLanguage() == SystemLanguage.Unknown)
            {
                // 获取本机对应配置表的默认语言
                var defLanguage = GetDefaultLanguage();
                // 先设置为默认语言
                LanguageUtils.SetLanguage(defLanguage);
                // 弹出语言选择框
                CoreUtils.uiManager.ShowUI(UI.s_Pop_LanguageSet, null, LanguageSetMediator.OpenType.Start);
            }
            else
            {
                SendNotification(CmdConstant.ReloadGame);
            }
        }

        private void LoadVolume()
        {
            float bgm = PlayerPrefs.GetFloat(RS.BGMVolume, 1f);
            float sfx = PlayerPrefs.GetFloat(RS.SfxVolume, 1f);
            CoreUtils.audioService.SetSfxVolume(sfx);
            CoreUtils.audioService.SetMusicVolume(bgm);
        }
        private SystemLanguage GetDefaultLanguage()
        {
            int deviceLan = (int)Application.systemLanguage;
            var defaultLan = SystemLanguage.English;
            var langages = CoreUtils.dataService.QueryRecords<Data.LanguageSetDefine>();
            for (int i = 0; i < langages.Count; i++)
            {
                var lanConfig = langages[i];
                if (lanConfig.enumSwitch == 0)
                    continue;
                for (int j = 0; j < lanConfig.telephone.Count; j++)
                {
                    if (lanConfig.telephone[j] == (int)deviceLan)
                    {
                        return (SystemLanguage)lanConfig.ID;
                    }
                    if (lanConfig.telephone[j] == (int)SystemLanguage.Unknown)
                    {
                        defaultLan = (SystemLanguage)lanConfig.ID;
                    }
                }
            }

            return defaultLan;
        }
    }
}

