using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ILRuntime.Other;

namespace Skyunion
{
    [NeedAdaptorAttribute]
    public abstract class UIPopValue
    {
        public abstract bool InvokePopMethod();
    }
}

