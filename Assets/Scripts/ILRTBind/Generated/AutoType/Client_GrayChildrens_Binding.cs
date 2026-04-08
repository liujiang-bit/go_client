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
    unsafe class Client_GrayChildrens_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.GrayChildrens);
            args = new Type[]{};
            method = type.GetMethod("Gray", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Gray_0);
            args = new Type[]{};
            method = type.GetMethod("Normal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Normal_1);
            args = new Type[]{};
            method = type.GetMethod("NormalSkeletonGraphic", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, NormalSkeletonGraphic_2);
            args = new Type[]{};
            method = type.GetMethod("GraySkeletonGraphic", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GraySkeletonGraphic_3);


        }


        static StackObject* Gray_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.GrayChildrens instance_of_this_method = (Client.GrayChildrens)typeof(Client.GrayChildrens).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Gray();

            return __ret;
        }

        static StackObject* Normal_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.GrayChildrens instance_of_this_method = (Client.GrayChildrens)typeof(Client.GrayChildrens).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Normal();

            return __ret;
        }

        static StackObject* NormalSkeletonGraphic_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.GrayChildrens instance_of_this_method = (Client.GrayChildrens)typeof(Client.GrayChildrens).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.NormalSkeletonGraphic();

            return __ret;
        }

        static StackObject* GraySkeletonGraphic_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.GrayChildrens instance_of_this_method = (Client.GrayChildrens)typeof(Client.GrayChildrens).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.GraySkeletonGraphic();

            return __ret;
        }



    }
}
