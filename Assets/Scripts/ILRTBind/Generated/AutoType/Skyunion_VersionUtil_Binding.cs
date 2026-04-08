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
    unsafe class Skyunion_VersionUtil_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.VersionUtil);
            args = new Type[]{};
            method = type.GetMethod("GetVersionNumber", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetVersionNumber_0);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("GetVersionNumber", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetVersionNumber_1);
            args = new Type[]{};
            method = type.GetMethod("get_HotfixNumber", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_HotfixNumber_2);
            args = new Type[]{};
            method = type.GetMethod("GetPlatform", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetPlatform_3);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_HotfixNumber", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_HotfixNumber_4);
            args = new Type[]{};
            method = type.GetMethod("GetVersionStr", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetVersionStr_5);

            field = type.GetField("HotfixVersionPath", flag);
            app.RegisterCLRFieldGetter(field, get_HotfixVersionPath_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_HotfixVersionPath_0, null);


        }


        static StackObject* GetVersionNumber_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.VersionUtil.GetVersionNumber();

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetVersionNumber_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @version = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Skyunion.VersionUtil.GetVersionNumber(@version);

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* get_HotfixNumber_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.VersionUtil.HotfixNumber;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetPlatform_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.VersionUtil.GetPlatform();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_HotfixNumber_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;


            Skyunion.VersionUtil.HotfixNumber = value;

            return __ret;
        }

        static StackObject* GetVersionStr_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.VersionUtil.GetVersionStr();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_HotfixVersionPath_0(ref object o)
        {
            return Skyunion.VersionUtil.HotfixVersionPath;
        }

        static StackObject* CopyToStack_HotfixVersionPath_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Skyunion.VersionUtil.HotfixVersionPath;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
