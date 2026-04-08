// =============================================================================== 
// Author              :    
// Create Time         :    Tuesday, March 13, 2018
// Update Time         :    Tuesday, March 13, 2018
// Class Description   :    TradeShopColElement
// Copyright IGG All rights reserved.
// ===============================================================================

using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class BagColElement : GameView
    {
        public List<ItemBagView> ElemItemList = new List<ItemBagView>();
        public List<int> ElemIndexList = new List<int>();
    }
}