
using System;
using System.Globalization;
using Hotfix;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class GuideManager : TSingleton<GuideManager>
    {
        public bool IsGuideSoldierTrain;    //引导训练士兵
        public bool IsGuideSoldierGet;      //引导训练士兵领取
        public bool IsGuideBuildScoutCamp;  //引导建造斥候营地

        public bool IsGuideFightBarbarian;        //是否引导攻击野蛮人战斗
        public bool IsGuideFightSecondBarbarian;  //是否引导攻击野蛮人第二场战斗
        public bool IsExploreFogGuide;            //迷雾探险引导


        public void ClearData()
        {
            IsGuideSoldierTrain = false;
            IsGuideSoldierGet = false;
            IsGuideBuildScoutCamp = false;
            IsExploreFogGuide = false;

            IsGuideFightBarbarian = false;
            IsGuideFightSecondBarbarian = false;
        }
    }
}

