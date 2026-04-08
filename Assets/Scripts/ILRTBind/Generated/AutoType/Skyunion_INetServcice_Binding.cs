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
    unsafe class Skyunion_INetServcice_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.INetServcice);
            args = new Type[]{typeof(System.Action<Skyunion.NetEvent, System.Int32>), typeof(System.Action<System.IO.MemoryStream>), typeof(Skyunion.ProtocolResolverDelegate)};
            method = type.GetMethod("CreateClient", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CreateClient_0);


        }


        static StackObject* CreateClient_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.ProtocolResolverDelegate @protocolResolver = (Skyunion.ProtocolResolverDelegate)typeof(Skyunion.ProtocolResolverDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.IO.MemoryStream> @reciveEvent = (System.Action<System.IO.MemoryStream>)typeof(System.Action<System.IO.MemoryStream>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<Skyunion.NetEvent, System.Int32> @connectEvent = (System.Action<Skyunion.NetEvent, System.Int32>)typeof(System.Action<Skyunion.NetEvent, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.INetServcice instance_of_this_method = (Skyunion.INetServcice)typeof(Skyunion.INetServcice).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CreateClient(@connectEvent, @reciveEvent, @protocolResolver);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
