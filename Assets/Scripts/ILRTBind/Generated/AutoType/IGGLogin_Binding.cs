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
    unsafe class IGGLogin_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGLogin);
            args = new Type[]{};
            method = type.GetMethod("shareInstance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, shareInstance_0);
            args = new Type[]{typeof(global::IGGLogin.IGGLoginDelegate.Listener)};
            method = type.GetMethod("setLoginDelegate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setLoginDelegate_1);
            args = new Type[]{typeof(global::IGGLogin.IGGLoginListener.Listener)};
            method = type.GetMethod("AutoLogin", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AutoLogin_2);
            args = new Type[]{typeof(global::IGGLogin.CheckStateBox)};
            method = type.GetMethod("setCheckStateBox", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setCheckStateBox_3);
            args = new Type[]{typeof(global::IGGLogin.IGGLoginListener.Listener)};
            method = type.GetMethod("GuestLogin", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GuestLogin_4);
            args = new Type[]{typeof(global::IGGLogin.IGGLoginListener.Listener)};
            method = type.GetMethod("SwitchToIGGPassport", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SwitchToIGGPassport_5);
            args = new Type[]{typeof(global::IGGLogin.IGGLoginListener.Listener2)};
            method = type.GetMethod("BindToIGGPassport", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, BindToIGGPassport_6);


        }


        static StackObject* shareInstance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::IGGLogin.shareInstance();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* setLoginDelegate_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGLogin.IGGLoginDelegate.Listener @callback = (global::IGGLogin.IGGLoginDelegate.Listener)typeof(global::IGGLogin.IGGLoginDelegate.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGLogin instance_of_this_method = (global::IGGLogin)typeof(global::IGGLogin).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setLoginDelegate(@callback);

            return __ret;
        }

        static StackObject* AutoLogin_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGLogin.IGGLoginListener.Listener @listener = (global::IGGLogin.IGGLoginListener.Listener)typeof(global::IGGLogin.IGGLoginListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGLogin instance_of_this_method = (global::IGGLogin)typeof(global::IGGLogin).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AutoLogin(@listener);

            return __ret;
        }

        static StackObject* setCheckStateBox_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGLogin.CheckStateBox @checkStateBox = (global::IGGLogin.CheckStateBox)typeof(global::IGGLogin.CheckStateBox).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGLogin instance_of_this_method = (global::IGGLogin)typeof(global::IGGLogin).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setCheckStateBox(@checkStateBox);

            return __ret;
        }

        static StackObject* GuestLogin_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGLogin.IGGLoginListener.Listener @listener = (global::IGGLogin.IGGLoginListener.Listener)typeof(global::IGGLogin.IGGLoginListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGLogin instance_of_this_method = (global::IGGLogin)typeof(global::IGGLogin).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.GuestLogin(@listener);

            return __ret;
        }

        static StackObject* SwitchToIGGPassport_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGLogin.IGGLoginListener.Listener @listener = (global::IGGLogin.IGGLoginListener.Listener)typeof(global::IGGLogin.IGGLoginListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGLogin instance_of_this_method = (global::IGGLogin)typeof(global::IGGLogin).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SwitchToIGGPassport(@listener);

            return __ret;
        }

        static StackObject* BindToIGGPassport_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGLogin.IGGLoginListener.Listener2 @listener = (global::IGGLogin.IGGLoginListener.Listener2)typeof(global::IGGLogin.IGGLoginListener.Listener2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGLogin instance_of_this_method = (global::IGGLogin)typeof(global::IGGLogin).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.BindToIGGPassport(@listener);

            return __ret;
        }



    }
}
