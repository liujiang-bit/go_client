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
    unsafe class Skyunion_BehaviourBinder_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.BehaviourBinder);

            field = type.GetField("updateCallback", flag);
            app.RegisterCLRFieldGetter(field, get_updateCallback_0);
            app.RegisterCLRFieldSetter(field, set_updateCallback_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_updateCallback_0, AssignFromStack_updateCallback_0);
            field = type.GetField("fixedUpdateCallback", flag);
            app.RegisterCLRFieldGetter(field, get_fixedUpdateCallback_1);
            app.RegisterCLRFieldSetter(field, set_fixedUpdateCallback_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_fixedUpdateCallback_1, AssignFromStack_fixedUpdateCallback_1);
            field = type.GetField("lateUpdateCallback", flag);
            app.RegisterCLRFieldGetter(field, get_lateUpdateCallback_2);
            app.RegisterCLRFieldSetter(field, set_lateUpdateCallback_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_lateUpdateCallback_2, AssignFromStack_lateUpdateCallback_2);


        }



        static object get_updateCallback_0(ref object o)
        {
            return ((Skyunion.BehaviourBinder)o).updateCallback;
        }

        static StackObject* CopyToStack_updateCallback_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.BehaviourBinder)o).updateCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_updateCallback_0(ref object o, object v)
        {
            ((Skyunion.BehaviourBinder)o).updateCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_updateCallback_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @updateCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.BehaviourBinder)o).updateCallback = @updateCallback;
            return ptr_of_this_method;
        }

        static object get_fixedUpdateCallback_1(ref object o)
        {
            return ((Skyunion.BehaviourBinder)o).fixedUpdateCallback;
        }

        static StackObject* CopyToStack_fixedUpdateCallback_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.BehaviourBinder)o).fixedUpdateCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_fixedUpdateCallback_1(ref object o, object v)
        {
            ((Skyunion.BehaviourBinder)o).fixedUpdateCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_fixedUpdateCallback_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @fixedUpdateCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.BehaviourBinder)o).fixedUpdateCallback = @fixedUpdateCallback;
            return ptr_of_this_method;
        }

        static object get_lateUpdateCallback_2(ref object o)
        {
            return ((Skyunion.BehaviourBinder)o).lateUpdateCallback;
        }

        static StackObject* CopyToStack_lateUpdateCallback_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.BehaviourBinder)o).lateUpdateCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_lateUpdateCallback_2(ref object o, object v)
        {
            ((Skyunion.BehaviourBinder)o).lateUpdateCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_lateUpdateCallback_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @lateUpdateCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.BehaviourBinder)o).lateUpdateCallback = @lateUpdateCallback;
            return ptr_of_this_method;
        }



    }
}
