using Client;
using IGGSDKConstant;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    
    public class GameHelper
    {
        private static float m_lastPlayTime;

        public static void PlaySoundSlider()
        {
            if (Time.realtimeSinceStartup - m_lastPlayTime >= 0.1f)
            {
                m_lastPlayTime = Time.realtimeSinceStartup;
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            }
        }

        public static IGGTranslator GetTranslator()
        {
            IGGTranslator translator;
            switch (LanguageUtils.GetLanguage())
            {
                case SystemLanguage.Arabic:
                    translator = new IGGTranslator(IGGLanguage.auto, IGGLanguage.Ar);
                    break;
                case SystemLanguage.English:
                    translator = new IGGTranslator(IGGLanguage.auto, IGGLanguage.En);
                    break;
                case SystemLanguage.Turkish:
                    translator = new IGGTranslator(IGGLanguage.auto, IGGLanguage.Tr);
                    break;
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                    translator = new IGGTranslator(IGGLanguage.auto, IGGLanguage.Zh_CN);
                    break;
                default:
                    translator = new IGGTranslator(IGGLanguage.auto, IGGLanguage.Ar);
                    break;
            }
            return translator;
        }

        //坐标跳转
        public static void CoordinateJump(int x, int y)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
            WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 1000f, null);
            WorldCamera.Instance().ViewTerrainPos(x * 6 + 3, y * 6 + 3, 1000f, null);
        }
    }

}