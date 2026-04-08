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
    unsafe class Client_LevelDetailCastle_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.LevelDetailCastle);
            args = new Type[]{typeof(Client.LevelDetailCastle)};
            method = type.GetMethod("ForceUpdateScaleS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ForceUpdateScaleS_0);
            args = new Type[]{typeof(Client.LevelDetailCastle), typeof(System.Single)};
            method = type.GetMethod("SetMaxScaleS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetMaxScaleS_1);


        }


        static StackObject* ForceUpdateScaleS_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.LevelDetailCastle @self = (Client.LevelDetailCastle)typeof(Client.LevelDetailCastle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.LevelDetailCastle.ForceUpdateScaleS(@self);

            return __ret;
        }

        static StackObject* SetMaxScaleS_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @max_scale = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.LevelDetailCastle @self = (Client.LevelDetailCastle)typeof(Client.LevelDetailCastle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.LevelDetailCastle.SetMaxScaleS(@self, @max_scale);

            return __ret;
        }



    }
}
