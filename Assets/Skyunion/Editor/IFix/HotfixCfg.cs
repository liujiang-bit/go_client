using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System;
namespace IFix
{
    [Configure]
    public class InterpertConfig
    {
        [IFix]
        static IEnumerable<Type> ToProcess
        {
            get
            {
                return (from type in Assembly.Load("HotFix").GetTypes()
                        where /*(type.Namespace == "Game" || type.Namespace == "Hotfix") && */!type.Name.Contains("<")
                        select type);
            }
        }
    }
}
