using System;
using UnityEngine;

namespace ROK
{
    public class LodCommon : LodBase
    {
        public override void UpdateLod()
        {
            base.UpdateLod();
            float unitScale = LodScalerMgr.instance.unitScale;
            base.transform.localScale = new Vector3(unitScale, unitScale, unitScale);
        }
    }
}
