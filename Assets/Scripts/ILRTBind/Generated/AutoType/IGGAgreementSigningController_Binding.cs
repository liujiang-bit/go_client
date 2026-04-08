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
    unsafe class IGGAgreementSigningController_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGAgreementSigningController);
            args = new Type[]{typeof(global::IGGAgreementSigning.IGGStatusRequestListener.Listener)};
            method = type.GetMethod("informAsap", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, informAsap_0);
            args = new Type[]{typeof(global::IGGAgreementSigning.IGGStatusRequestListener.Listener)};
            method = type.GetMethod("informKindly", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, informKindly_1);


        }


        static StackObject* informAsap_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementSigning.IGGStatusRequestListener.Listener @listener = (global::IGGAgreementSigning.IGGStatusRequestListener.Listener)typeof(global::IGGAgreementSigning.IGGStatusRequestListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAgreementSigningController instance_of_this_method = (global::IGGAgreementSigningController)typeof(global::IGGAgreementSigningController).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.informAsap(@listener);

            return __ret;
        }

        static StackObject* informKindly_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementSigning.IGGStatusRequestListener.Listener @listener = (global::IGGAgreementSigning.IGGStatusRequestListener.Listener)typeof(global::IGGAgreementSigning.IGGStatusRequestListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAgreementSigningController instance_of_this_method = (global::IGGAgreementSigningController)typeof(global::IGGAgreementSigningController).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.informKindly(@listener);

            return __ret;
        }



    }
}
