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
    unsafe class IGGAgreementSigning_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGAgreementSigning);
            args = new Type[]{};
            method = type.GetMethod("signing", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, signing_0);
            args = new Type[]{typeof(global::IGGAgreementSigning.IGGAssignedAgreementsRequestListener.Listener)};
            method = type.GetMethod("requestAssignedAgreements", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, requestAssignedAgreements_1);
            args = new Type[]{};
            method = type.GetMethod("termination", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, termination_2);
            args = new Type[]{typeof(global::IGGAgreementSigningFile), typeof(global::IGGAgreementSigning.IGGSigningListener.Listener)};
            method = type.GetMethod("sign", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, sign_3);


        }


        static StackObject* signing_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementSigning instance_of_this_method = (global::IGGAgreementSigning)typeof(global::IGGAgreementSigning).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.signing();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* requestAssignedAgreements_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementSigning.IGGAssignedAgreementsRequestListener.Listener @listener = (global::IGGAgreementSigning.IGGAssignedAgreementsRequestListener.Listener)typeof(global::IGGAgreementSigning.IGGAssignedAgreementsRequestListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAgreementSigning instance_of_this_method = (global::IGGAgreementSigning)typeof(global::IGGAgreementSigning).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.requestAssignedAgreements(@listener);

            return __ret;
        }

        static StackObject* termination_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementSigning instance_of_this_method = (global::IGGAgreementSigning)typeof(global::IGGAgreementSigning).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.termination();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* sign_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAgreementSigning.IGGSigningListener.Listener @listener = (global::IGGAgreementSigning.IGGSigningListener.Listener)typeof(global::IGGAgreementSigning.IGGSigningListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAgreementSigningFile @signingFile = (global::IGGAgreementSigningFile)typeof(global::IGGAgreementSigningFile).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::IGGAgreementSigning instance_of_this_method = (global::IGGAgreementSigning)typeof(global::IGGAgreementSigning).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.sign(@signingFile, @listener);

            return __ret;
        }



    }
}
