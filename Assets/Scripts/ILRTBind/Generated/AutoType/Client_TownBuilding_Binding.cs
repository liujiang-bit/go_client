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
    unsafe class Client_TownBuilding_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.TownBuilding);
            args = new Type[]{typeof(UnityEngine.Color)};
            method = type.GetMethod("SetColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetColor_0);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("SetColliderOrder", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetColliderOrder_1);

            field = type.GetField("colliders", flag);
            app.RegisterCLRFieldGetter(field, get_colliders_0);
            app.RegisterCLRFieldSetter(field, set_colliders_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_colliders_0, AssignFromStack_colliders_0);


        }


        static StackObject* SetColor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @c = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.TownBuilding instance_of_this_method = (Client.TownBuilding)typeof(Client.TownBuilding).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetColor(@c);

            return __ret;
        }

        static StackObject* SetColliderOrder_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.TownBuilding instance_of_this_method = (Client.TownBuilding)typeof(Client.TownBuilding).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetColliderOrder(@y);

            return __ret;
        }


        static object get_colliders_0(ref object o)
        {
            return ((Client.TownBuilding)o).colliders;
        }

        static StackObject* CopyToStack_colliders_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TownBuilding)o).colliders;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_colliders_0(ref object o, object v)
        {
            ((Client.TownBuilding)o).colliders = (UnityEngine.BoxCollider[])v;
        }

        static StackObject* AssignFromStack_colliders_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.BoxCollider[] @colliders = (UnityEngine.BoxCollider[])typeof(UnityEngine.BoxCollider[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.TownBuilding)o).colliders = @colliders;
            return ptr_of_this_method;
        }



    }
}
