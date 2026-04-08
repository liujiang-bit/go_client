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
    unsafe class WegamersSDK_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::WegamersSDK);
            args = new Type[]{};
            method = type.GetMethod("shareInstance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, shareInstance_0);
            args = new Type[]{typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(global::WeGamersSDKParams.SkinType), typeof(global::WegamersSDK.WGSDKInitListener.Listener2), typeof(global::WegamersSDK.WGMsgReddotListener.Listener)};
            method = type.GetMethod("CheckState", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CheckState_1);
            args = new Type[]{};
            method = type.GetMethod("startBrowser", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, startBrowser_2);


        }


        static StackObject* shareInstance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::WegamersSDK.shareInstance();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CheckState_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::WegamersSDK.WGMsgReddotListener.Listener @listener2 = (global::WegamersSDK.WGMsgReddotListener.Listener)typeof(global::WegamersSDK.WGMsgReddotListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::WegamersSDK.WGSDKInitListener.Listener2 @listener1 = (global::WegamersSDK.WGSDKInitListener.Listener2)typeof(global::WegamersSDK.WGSDKInitListener.Listener2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::WeGamersSDKParams.SkinType @type = (global::WeGamersSDKParams.SkinType)typeof(global::WeGamersSDKParams.SkinType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @key = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.String @sdkid = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            System.String @accountId = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 8);
            global::WegamersSDK instance_of_this_method = (global::WegamersSDK)typeof(global::WegamersSDK).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CheckState(@accountId, @name, @sdkid, @key, @type, @listener1, @listener2);

            return __ret;
        }

        static StackObject* startBrowser_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::WegamersSDK instance_of_this_method = (global::WegamersSDK)typeof(global::WegamersSDK).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.startBrowser();

            return __ret;
        }



    }
}
