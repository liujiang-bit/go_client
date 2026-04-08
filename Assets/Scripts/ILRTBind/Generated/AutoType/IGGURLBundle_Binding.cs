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
    unsafe class IGGURLBundle_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGURLBundle);
            args = new Type[]{};
            method = type.GetMethod("shareInstance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, shareInstance_0);
            args = new Type[]{typeof(global::IGGURLBundle.IGGURLBundleListener.Listener)};
            method = type.GetMethod("serviceURL", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, serviceURL_1);
            args = new Type[]{typeof(global::IGGURLBundle.IGGURLBundleListener.Listener)};
            method = type.GetMethod("forumURL", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, forumURL_2);
            args = new Type[]{typeof(global::IGGURLBundle.IGGURLBundleListener.Listener)};
            method = type.GetMethod("livechatURL", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, livechatURL_3);
            args = new Type[]{typeof(global::IGGURLBundle.IGGURLBundleListener.Listener)};
            method = type.GetMethod("paymentLivechatURL", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, paymentLivechatURL_4);


        }


        static StackObject* shareInstance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::IGGURLBundle.shareInstance();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* serviceURL_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGURLBundle.IGGURLBundleListener.Listener @listener = (global::IGGURLBundle.IGGURLBundleListener.Listener)typeof(global::IGGURLBundle.IGGURLBundleListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGURLBundle instance_of_this_method = (global::IGGURLBundle)typeof(global::IGGURLBundle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.serviceURL(@listener);

            return __ret;
        }

        static StackObject* forumURL_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGURLBundle.IGGURLBundleListener.Listener @listener = (global::IGGURLBundle.IGGURLBundleListener.Listener)typeof(global::IGGURLBundle.IGGURLBundleListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGURLBundle instance_of_this_method = (global::IGGURLBundle)typeof(global::IGGURLBundle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.forumURL(@listener);

            return __ret;
        }

        static StackObject* livechatURL_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGURLBundle.IGGURLBundleListener.Listener @listener = (global::IGGURLBundle.IGGURLBundleListener.Listener)typeof(global::IGGURLBundle.IGGURLBundleListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGURLBundle instance_of_this_method = (global::IGGURLBundle)typeof(global::IGGURLBundle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.livechatURL(@listener);

            return __ret;
        }

        static StackObject* paymentLivechatURL_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGURLBundle.IGGURLBundleListener.Listener @listener = (global::IGGURLBundle.IGGURLBundleListener.Listener)typeof(global::IGGURLBundle.IGGURLBundleListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGURLBundle instance_of_this_method = (global::IGGURLBundle)typeof(global::IGGURLBundle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.paymentLivechatURL(@listener);

            return __ret;
        }



    }
}
