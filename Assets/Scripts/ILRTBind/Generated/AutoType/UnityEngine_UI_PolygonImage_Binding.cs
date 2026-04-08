using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class UnityEngine_UI_PolygonImage_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UnityEngine.UI.PolygonImage);

            field = type.GetField("assetName", flag);
            app.RegisterCLRFieldGetter(field, get_assetName_0);
            app.RegisterCLRFieldSetter(field, set_assetName_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_assetName_0, AssignFromStack_assetName_0);


        }



        static object get_assetName_0(ref object o)
        {
            return ((UnityEngine.UI.PolygonImage)o).assetName;
        }

        static StackObject* CopyToStack_assetName_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((UnityEngine.UI.PolygonImage)o).assetName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_assetName_0(ref object o, object v)
        {
            ((UnityEngine.UI.PolygonImage)o).assetName = (System.String)v;
        }

        static StackObject* AssignFromStack_assetName_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @assetName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((UnityEngine.UI.PolygonImage)o).assetName = @assetName;
            return ptr_of_this_method;
        }



    }
}
