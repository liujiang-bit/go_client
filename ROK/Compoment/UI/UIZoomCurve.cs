using System;
using UnityEngine;

namespace ROK
{
    public class UIZoomCurve : MonoBehaviour
    {
        [Tooltip("UI缩放")]
        public AnimationCurve AllUIScaleCurve;

        [Tooltip("UI3D缩放")]
        public AnimationCurve AllUIScaleCurve3D;

        [Tooltip("部队UI")]
        public AnimationCurve TroopCurve;

        [Tooltip("部队普通情况半径")]
        public AnimationCurve TroopRadiusCure;

        [Tooltip("部队战斗情况半径")]
        public AnimationCurve TroopFightRadiusCure;

        [Tooltip("部队战斗连线")]
        public AnimationCurve TroopFightLineCurve;

        [Tooltip("主城名字age1")]
        public AnimationCurve CastleTitleOffsetCurve;

        [Tooltip("主城交战开始age1")]
        public AnimationCurve CastleFightStartOffsetCurve;

        [Tooltip("主城交战结束age1")]
        public AnimationCurve CastleFightEndOffsetCurve;

        [Tooltip("主城名字age2")]
        public AnimationCurve CastleTitleOffsetCurve2;

        [Tooltip("主城名字age3")]
        public AnimationCurve CastleTitleOffsetCurve3;

        [Tooltip("主城名字age4")]
        public AnimationCurve CastleTitleOffsetCurve4;

        [Tooltip("主城名字age5")]
        public AnimationCurve CastleTitleOffsetCurve5;

        [Tooltip("野蛮人部队")]
        public AnimationCurve NpcTroopCurve;

        [Tooltip("资源点等级")]
        public AnimationCurve RssTitleCurve;

        [Tooltip("资源点状态")]
        public AnimationCurve RssStatusCurve;

        [Tooltip("关卡名字")]
        public AnimationCurve PassTitleCurve;

        [Tooltip("关卡交战开始点")]
        public AnimationCurve PassFightStartOffsetCurve;

        [Tooltip("关卡交战结束点")]
        public AnimationCurve PassFightEndOffsetCurve;

        [Tooltip("主权塔名字")]
        public AnimationCurve FortressTitleCurve;

        [Tooltip("主权塔交战开始点")]
        public AnimationCurve FortressFightStartOffsetCurve;

        [Tooltip("主权塔交战结束点")]
        public AnimationCurve FortressFightEndOffsetCurve;

        [Tooltip("联盟建筑名字")]
        public AnimationCurve AsBuildingNameCurve;

        [Tooltip("联盟建筑交战开始点")]
        public AnimationCurve AsBuildingFightStartOffsetCurve;

        [Tooltip("联盟建筑交战结束点")]
        public AnimationCurve AsBuildingFightEndOffsetCurve;

        [Tooltip("联盟建筑HP")]
        public AnimationCurve AsBuildingHpCurve;

        [Tooltip("联盟旗帜名字")]
        public AnimationCurve AsFlagNameCurve;

        [Tooltip("联盟旗帜交战开始点")]
        public AnimationCurve AsFlagFightStartOffsetCurve;

        [Tooltip("联盟旗帜交战结束点")]
        public AnimationCurve AsFlagFightEndOffsetCurve;

        [Tooltip("联盟旗帜HP")]
        public AnimationCurve AsFlagHpCurve;

        [Tooltip("村庄")]
        public AnimationCurve VillageCurve;

        [Tooltip("斥候")]
        public AnimationCurve ScoutCurve;

        [Tooltip("山洞")]
        public AnimationCurve CaveCurve;

        [Tooltip("野蛮人城寨")]
        public AnimationCurve NpcCastleCurve;

        [Tooltip("部队击溃")]
        public AnimationCurve TroopDefeatCurve;

        [Tooltip("联盟资源点")]
        public AnimationCurve VeinCure;
    }
}