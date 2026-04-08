// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年02月25日
// Update Time         :    2020年02月25日
// Class Description   :    PropTip 道具tip
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using System;
using Data;

namespace Game {

    public class PropTip
    {
        public static void Show(int itemId, Transform trans)
        {
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)itemId);
            if(itemDefine==null)
            {
                Debug.LogErrorFormat("ItemDefine not find id:{0}", itemId);
            }
            HelpTip.CreateTip(LanguageUtils.getText(itemDefine.l_nameID), trans).SetStyle(HelpTipData.Style.arrowDown).SetOffset(23).Show();
        }
    }
}