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
    unsafe class Client_LongPressBtn_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.LongPressBtn);

            field = type.GetField("action", flag);
            app.RegisterCLRFieldGetter(field, get_action_0);
            app.RegisterCLRFieldSetter(field, set_action_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_action_0, AssignFromStack_action_0);
            field = type.GetField("releaseAction", flag);
            app.RegisterCLRFieldGetter(field, get_releaseAction_1);
            app.RegisterCLRFieldSetter(field, set_releaseAction_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_releaseAction_1, AssignFromStack_releaseAction_1);
            field = type.GetField("reqHoldTimeFristTime", flag);
            app.RegisterCLRFieldGetter(field, get_reqHoldTimeFristTime_2);
            app.RegisterCLRFieldSetter(field, set_reqHoldTimeFristTime_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_reqHoldTimeFristTime_2, AssignFromStack_reqHoldTimeFristTime_2);
            field = type.GetField("reqHoldTimeOtherTime", flag);
            app.RegisterCLRFieldGetter(field, get_reqHoldTimeOtherTime_3);
            app.RegisterCLRFieldSetter(field, set_reqHoldTimeOtherTime_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_reqHoldTimeOtherTime_3, AssignFromStack_reqHoldTimeOtherTime_3);


        }



        static object get_action_0(ref object o)
        {
            return ((Client.LongPressBtn)o).action;
        }

        static StackObject* CopyToStack_action_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.LongPressBtn)o).action;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_action_0(ref object o, object v)
        {
            ((Client.LongPressBtn)o).action = (UnityEngine.Events.UnityAction)v;
        }

        static StackObject* AssignFromStack_action_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Events.UnityAction @action = (UnityEngine.Events.UnityAction)typeof(UnityEngine.Events.UnityAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.LongPressBtn)o).action = @action;
            return ptr_of_this_method;
        }

        static object get_releaseAction_1(ref object o)
        {
            return ((Client.LongPressBtn)o).releaseAction;
        }

        static StackObject* CopyToStack_releaseAction_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.LongPressBtn)o).releaseAction;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_releaseAction_1(ref object o, object v)
        {
            ((Client.LongPressBtn)o).releaseAction = (UnityEngine.Events.UnityAction)v;
        }

        static StackObject* AssignFromStack_releaseAction_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Events.UnityAction @releaseAction = (UnityEngine.Events.UnityAction)typeof(UnityEngine.Events.UnityAction).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.LongPressBtn)o).releaseAction = @releaseAction;
            return ptr_of_this_method;
        }

        static object get_reqHoldTimeFristTime_2(ref object o)
        {
            return ((Client.LongPressBtn)o).reqHoldTimeFristTime;
        }

        static StackObject* CopyToStack_reqHoldTimeFristTime_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.LongPressBtn)o).reqHoldTimeFristTime;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_reqHoldTimeFristTime_2(ref object o, object v)
        {
            ((Client.LongPressBtn)o).reqHoldTimeFristTime = (System.Single)v;
        }

        static StackObject* AssignFromStack_reqHoldTimeFristTime_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @reqHoldTimeFristTime = *(float*)&ptr_of_this_method->Value;
            ((Client.LongPressBtn)o).reqHoldTimeFristTime = @reqHoldTimeFristTime;
            return ptr_of_this_method;
        }

        static object get_reqHoldTimeOtherTime_3(ref object o)
        {
            return ((Client.LongPressBtn)o).reqHoldTimeOtherTime;
        }

        static StackObject* CopyToStack_reqHoldTimeOtherTime_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.LongPressBtn)o).reqHoldTimeOtherTime;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_reqHoldTimeOtherTime_3(ref object o, object v)
        {
            ((Client.LongPressBtn)o).reqHoldTimeOtherTime = (System.Single)v;
        }

        static StackObject* AssignFromStack_reqHoldTimeOtherTime_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @reqHoldTimeOtherTime = *(float*)&ptr_of_this_method->Value;
            ((Client.LongPressBtn)o).reqHoldTimeOtherTime = @reqHoldTimeOtherTime;
            return ptr_of_this_method;
        }



    }
}
