using System;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public class MapSoundHandler : IMapSoundHandler
    {
        public void Init()
        {
        }

        public void Clear()
        {
        }

        public void AddMapSound(RssType type)
        {
            string soundAssetName = SoundFactoy(type);
            if (!string.IsNullOrEmpty(soundAssetName))
            {
                CoreUtils.audioService.PlayOneShot(soundAssetName);
            }
        }

        public void RemoveSound(RssType type)
        {
            string soundAssetName = SoundFactoy(type);
            if (!string.IsNullOrEmpty(soundAssetName))
            {
                CoreUtils.audioService.StopByName(soundAssetName);
            }
        }

        public void PlayTouchMyCity()
        {
            CoreUtils.audioService.PlayOneShot(RS.Sound_Ui_SelectSelf);
        }

        private string SoundFactoy(RssType type)
        {
            string soundName = string.Empty;
            switch (type)
            {
                case RssType.Farmland:
                case RssType.GuildFood:
                case RssType.GuildFoodResCenter:                
                    soundName = "Sound_Ui_SelectFram";
                    break;
                case RssType.Wood:
                case RssType.GuildWood:
                case RssType.GuildWoodResCenter:
                    soundName = "Sound_Ui_SelectWood";
                    break;
                case RssType.Stone:
                case RssType.GuildStone:
                case RssType.GuildGoldResCenter:
                    soundName = "Sound_Ui_SelectStone";
                    break;
                case RssType.Gold:
                case RssType.GuildGold:
                case RssType.GuildGemResCenter:
                    soundName = "Sound_Ui_SelectGold";
                    break;
                case RssType.Village:
                    soundName = "Sound_Ui_SelectVillage";
                    break;
                case RssType.Cave:
                    soundName = "Sound_Ui_SelectCave";
                    break;
                case RssType.Gem:
                    soundName = "Sound_Ui_SelectDiamond";
                    break;
                case RssType.BarbarianCitadel:
                case RssType.CheckPoint:
                    soundName = "Sound_Ui_SelectWalled";
                    break;
                case RssType.Rune:
                    soundName = "Sound_Ui_SelectRune";
                    break;
                
                case RssType.Guardian:
                    //soundName = "Sound_Ui_SelectHoly1";
                    break;
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                    soundName = "Sound_Ui_SelectFort";
                    break;
                case RssType.GuildFlag:
                    soundName = "Sound_Ui_SelectFlag";
                    break;
                case RssType.City:
                //    soundName = "Sound_Ui_SelectSelf";//点击城市没有音效
                    break;
                case RssType.Altar:
                case RssType.Sanctuary:
                case RssType.Shrine:
                case RssType.HolyLand:
                    soundName = "Sound_Ui_SelectHoly0";
                    break;
                case RssType.LostTemple:
                    soundName = "Sound_Ui_SelectHoly1";                   
                    break;
            }

            return soundName;
        }
    }
}