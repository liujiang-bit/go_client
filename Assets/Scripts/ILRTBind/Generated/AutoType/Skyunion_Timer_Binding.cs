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
    unsafe class Skyunion_Timer_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.Timer);
            args = new Type[]{};
            method = type.GetMethod("Cancel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Cancel_0);
            args = new Type[]{typeof(System.Single), typeof(System.Action), typeof(System.Action<System.Single>), typeof(System.Boolean), typeof(System.Boolean), typeof(UnityEngine.MonoBehaviour)};
            method = type.GetMethod("Register", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Register_1);
            args = new Type[]{typeof(Skyunion.Timer)};
            method = type.GetMethod("Cancel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Cancel_2);
            args = new Type[]{};
            method = type.GetMethod("Pause", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Pause_3);
            args = new Type[]{};
            method = type.GetMethod("Resume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Resume_4);
            args = new Type[]{};
            method = type.GetMethod("get_isPaused", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isPaused_5);
            args = new Type[]{};
            method = type.GetMethod("get_isCancelled", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isCancelled_6);


        }


        static StackObject* Cancel_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.Timer instance_of_this_method = (Skyunion.Timer)typeof(Skyunion.Timer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Cancel();

            return __ret;
        }

        static StackObject* Register_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.MonoBehaviour @autoDestroyOwner = (UnityEngine.MonoBehaviour)typeof(UnityEngine.MonoBehaviour).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @useRealTime = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Boolean @isLooped = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Action<System.Single> @onUpdate = (System.Action<System.Single>)typeof(System.Action<System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Action @onComplete = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            System.Single @duration = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = Skyunion.Timer.Register(@duration, @onComplete, @onUpdate, @isLooped, @useRealTime, @autoDestroyOwner);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Cancel_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.Timer @timer = (Skyunion.Timer)typeof(Skyunion.Timer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Skyunion.Timer.Cancel(@timer);

            return __ret;
        }

        static StackObject* Pause_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.Timer instance_of_this_method = (Skyunion.Timer)typeof(Skyunion.Timer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Pause();

            return __ret;
        }

        static StackObject* Resume_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.Timer instance_of_this_method = (Skyunion.Timer)typeof(Skyunion.Timer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Resume();

            return __ret;
        }

        static StackObject* get_isPaused_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.Timer instance_of_this_method = (Skyunion.Timer)typeof(Skyunion.Timer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isPaused;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_isCancelled_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.Timer instance_of_this_method = (Skyunion.Timer)typeof(Skyunion.Timer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isCancelled;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }



    }
}
