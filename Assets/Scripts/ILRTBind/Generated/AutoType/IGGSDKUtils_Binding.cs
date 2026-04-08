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
    unsafe class IGGSDKUtils_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGSDKUtils);
            args = new Type[]{};
            method = type.GetMethod("shareInstance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, shareInstance_0);
            args = new Type[]{typeof(System.String), typeof(System.String), typeof(System.String), typeof(System.String), typeof(global::IGGSDKUtils.MsgBoxReturnListener.Listener)};
            method = type.GetMethod("ShowMsgBox", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowMsgBox_1);
            args = new Type[]{typeof(System.String), typeof(System.String), typeof(System.String), typeof(global::IGGSDKUtils.MsgBoxReturnListener.Listener)};
            method = type.GetMethod("ShowMsgBox", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowMsgBox_2);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("ShowToast", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowToast_3);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("OpenBrowser", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OpenBrowser_4);
            args = new Type[]{typeof(global::IGGSDKUtils.ShowMsgBoxListener1)};
            method = type.GetMethod("ReplaceShowMsgBox1", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ReplaceShowMsgBox1_5);
            args = new Type[]{typeof(global::IGGSDKUtils.ShowMsgBoxListener2)};
            method = type.GetMethod("ReplaceShowMsgBox2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ReplaceShowMsgBox2_6);
            args = new Type[]{typeof(global::IGGSDKUtils.ShowToastListener)};
            method = type.GetMethod("ReplaceShowToast", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ReplaceShowToast_7);
            args = new Type[]{};
            method = type.GetMethod("getCountryCode", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getCountryCode_8);


        }


        static StackObject* shareInstance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::IGGSDKUtils.shareInstance();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ShowMsgBox_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGSDKUtils.MsgBoxReturnListener.Listener @listener = (global::IGGSDKUtils.MsgBoxReturnListener.Listener)typeof(global::IGGSDKUtils.MsgBoxReturnListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @cancle = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @ok = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @title = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.String @message = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            global::IGGSDKUtils instance_of_this_method = (global::IGGSDKUtils)typeof(global::IGGSDKUtils).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ShowMsgBox(@message, @title, @ok, @cancle, @listener);

            return __ret;
        }

        static StackObject* ShowMsgBox_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGSDKUtils.MsgBoxReturnListener.Listener @listener = (global::IGGSDKUtils.MsgBoxReturnListener.Listener)typeof(global::IGGSDKUtils.MsgBoxReturnListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @ok = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @title = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @message = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::IGGSDKUtils instance_of_this_method = (global::IGGSDKUtils)typeof(global::IGGSDKUtils).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ShowMsgBox(@message, @title, @ok, @listener);

            return __ret;
        }

        static StackObject* ShowToast_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @message = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGSDKUtils instance_of_this_method = (global::IGGSDKUtils)typeof(global::IGGSDKUtils).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ShowToast(@message);

            return __ret;
        }

        static StackObject* OpenBrowser_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @url = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGSDKUtils instance_of_this_method = (global::IGGSDKUtils)typeof(global::IGGSDKUtils).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OpenBrowser(@url);

            return __ret;
        }

        static StackObject* ReplaceShowMsgBox1_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGSDKUtils.ShowMsgBoxListener1 @listener = (global::IGGSDKUtils.ShowMsgBoxListener1)typeof(global::IGGSDKUtils.ShowMsgBoxListener1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGSDKUtils instance_of_this_method = (global::IGGSDKUtils)typeof(global::IGGSDKUtils).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ReplaceShowMsgBox1(@listener);

            return __ret;
        }

        static StackObject* ReplaceShowMsgBox2_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGSDKUtils.ShowMsgBoxListener2 @listener = (global::IGGSDKUtils.ShowMsgBoxListener2)typeof(global::IGGSDKUtils.ShowMsgBoxListener2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGSDKUtils instance_of_this_method = (global::IGGSDKUtils)typeof(global::IGGSDKUtils).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ReplaceShowMsgBox2(@listener);

            return __ret;
        }

        static StackObject* ReplaceShowToast_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGSDKUtils.ShowToastListener @listener = (global::IGGSDKUtils.ShowToastListener)typeof(global::IGGSDKUtils.ShowToastListener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::IGGSDKUtils.ReplaceShowToast(@listener);

            return __ret;
        }

        static StackObject* getCountryCode_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGSDKUtils instance_of_this_method = (global::IGGSDKUtils)typeof(global::IGGSDKUtils).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getCountryCode();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
