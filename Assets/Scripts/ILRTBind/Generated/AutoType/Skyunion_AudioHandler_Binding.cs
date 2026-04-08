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
    unsafe class Skyunion_AudioHandler_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.AudioHandler);
            args = new Type[]{};
            method = type.GetMethod("OnDestroy", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnDestroy_0);

            field = type.GetField("IsDestroyed", flag);
            app.RegisterCLRFieldGetter(field, get_IsDestroyed_0);
            app.RegisterCLRFieldSetter(field, set_IsDestroyed_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsDestroyed_0, AssignFromStack_IsDestroyed_0);
            field = type.GetField("NeedDestroyGameObject", flag);
            app.RegisterCLRFieldGetter(field, get_NeedDestroyGameObject_1);
            app.RegisterCLRFieldSetter(field, set_NeedDestroyGameObject_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_NeedDestroyGameObject_1, AssignFromStack_NeedDestroyGameObject_1);
            field = type.GetField("gameObject", flag);
            app.RegisterCLRFieldGetter(field, get_gameObject_2);
            app.RegisterCLRFieldSetter(field, set_gameObject_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_gameObject_2, AssignFromStack_gameObject_2);


        }


        static StackObject* OnDestroy_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.AudioHandler instance_of_this_method = (Skyunion.AudioHandler)typeof(Skyunion.AudioHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnDestroy();

            return __ret;
        }


        static object get_IsDestroyed_0(ref object o)
        {
            return ((Skyunion.AudioHandler)o).IsDestroyed;
        }

        static StackObject* CopyToStack_IsDestroyed_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.AudioHandler)o).IsDestroyed;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsDestroyed_0(ref object o, object v)
        {
            ((Skyunion.AudioHandler)o).IsDestroyed = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsDestroyed_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsDestroyed = ptr_of_this_method->Value == 1;
            ((Skyunion.AudioHandler)o).IsDestroyed = @IsDestroyed;
            return ptr_of_this_method;
        }

        static object get_NeedDestroyGameObject_1(ref object o)
        {
            return ((Skyunion.AudioHandler)o).NeedDestroyGameObject;
        }

        static StackObject* CopyToStack_NeedDestroyGameObject_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.AudioHandler)o).NeedDestroyGameObject;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_NeedDestroyGameObject_1(ref object o, object v)
        {
            ((Skyunion.AudioHandler)o).NeedDestroyGameObject = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_NeedDestroyGameObject_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @NeedDestroyGameObject = ptr_of_this_method->Value == 1;
            ((Skyunion.AudioHandler)o).NeedDestroyGameObject = @NeedDestroyGameObject;
            return ptr_of_this_method;
        }

        static object get_gameObject_2(ref object o)
        {
            return ((Skyunion.AudioHandler)o).gameObject;
        }

        static StackObject* CopyToStack_gameObject_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.AudioHandler)o).gameObject;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_gameObject_2(ref object o, object v)
        {
            ((Skyunion.AudioHandler)o).gameObject = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_gameObject_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @gameObject = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.AudioHandler)o).gameObject = @gameObject;
            return ptr_of_this_method;
        }



    }
}
