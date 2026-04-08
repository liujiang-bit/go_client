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
    unsafe class IGGAgreementTerminationAlert_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGAgreementTerminationAlert);
            args = new Type[]{};
            method = type.GetMethod("getLocalizedCaption", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getLocalizedCaption_0);
            args = new Type[]{};
            method = type.GetMethod("getLocalizedTitle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getLocalizedTitle_1);
            args = new Type[]{};
            method = type.GetMethod("getLocalizedActionDismiss", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getLocalizedActionDismiss_2);


        }


        static StackObject* getLocalizedCaption_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementTerminationAlert instance_of_this_method = (global::IGGAgreementTerminationAlert)typeof(global::IGGAgreementTerminationAlert).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getLocalizedCaption();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getLocalizedTitle_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementTerminationAlert instance_of_this_method = (global::IGGAgreementTerminationAlert)typeof(global::IGGAgreementTerminationAlert).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getLocalizedTitle();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getLocalizedActionDismiss_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementTerminationAlert instance_of_this_method = (global::IGGAgreementTerminationAlert)typeof(global::IGGAgreementTerminationAlert).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getLocalizedActionDismiss();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
