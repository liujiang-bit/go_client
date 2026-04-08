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
    unsafe class IGGAgreementTerminationController_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGAgreementTerminationController);
            args = new Type[]{typeof(global::IGGAgreementTerminationController.IGGAssignedAgreementsRequestForTerminationListener.Listener)};
            method = type.GetMethod("requestAssignedAgreements", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, requestAssignedAgreements_0);
            args = new Type[]{typeof(global::IGGAgreementTerminationController.IGGTerminateRequestListener.Listener)};
            method = type.GetMethod("terminate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, terminate_1);


        }


        static StackObject* requestAssignedAgreements_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementTerminationController.IGGAssignedAgreementsRequestForTerminationListener.Listener @listener = (global::IGGAgreementTerminationController.IGGAssignedAgreementsRequestForTerminationListener.Listener)typeof(global::IGGAgreementTerminationController.IGGAssignedAgreementsRequestForTerminationListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAgreementTerminationController instance_of_this_method = (global::IGGAgreementTerminationController)typeof(global::IGGAgreementTerminationController).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.requestAssignedAgreements(@listener);

            return __ret;
        }

        static StackObject* terminate_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementTerminationController.IGGTerminateRequestListener.Listener @listener = (global::IGGAgreementTerminationController.IGGTerminateRequestListener.Listener)typeof(global::IGGAgreementTerminationController.IGGTerminateRequestListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAgreementTerminationController instance_of_this_method = (global::IGGAgreementTerminationController)typeof(global::IGGAgreementTerminationController).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.terminate(@listener);

            return __ret;
        }



    }
}
