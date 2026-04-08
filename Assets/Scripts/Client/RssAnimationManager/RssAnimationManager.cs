using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using System.Linq;
using System;
using System.Reflection;

namespace Client
{
    public class RssAnimationManager: Skyunion.Module
    {

        public List<RssAniGroup> RssAniGroups = new List<RssAniGroup>();

        public void RemoveAnimationGroup(RssAniGroup rssGroup)
        {
            if(RssAniGroups != null)
            {
                RssAniGroups.Remove(rssGroup);
            }
        }

        public void AddAnimaitonGroup(RssAniGroup rssGroup)
        {
            RssAniGroups.Add(rssGroup);
        }

        public void ClearData()
        {
            for (int i = 0; i < RssAniGroups.Count; i++)
            {
                RssAniGroups[i].ClearData();
            }
            RssAniGroups.Clear();
        }

        public override void Update()
        {
            base.Update();
            for(int i = RssAniGroups.Count-1;i>=0;i--)
            {
                if(RssAniGroups.Count>i)
                {
                    RssAniGroups[i].Update(Time.deltaTime);
                }
            }
        }



    }

}
