using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class SpriteAsset : ScriptableObject
    {
        public int ID;

        public bool _IsStatic;

        public Texture texSource;

        public List<SpriteInforGroup> listSpriteGroup;
    }
}