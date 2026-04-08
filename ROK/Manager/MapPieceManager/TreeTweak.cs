using System;
using UnityEngine;

namespace ROK
{
    public class TreeTweak : MonoBehaviour
    {
        public void TweekTreeRot()
        {
            base.transform.eulerAngles = new Vector3(45f, 0f, 0f);
        }
    }
}