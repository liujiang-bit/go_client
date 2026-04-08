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
    unsafe class Client_UIClickListener_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.UIClickListener);

            field = type.GetField("onPointerDown", flag);
            app.RegisterCLRFieldGetter(field, get_onPointerDown_0);
            app.RegisterCLRFieldSetter(field, set_onPointerDown_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPointerDown_0, AssignFromStack_onPointerDown_0);
            field = type.GetField("onPointerUp", flag);
            app.RegisterCLRFieldGetter(field, get_onPointerUp_1);
            app.RegisterCLRFieldSetter(field, set_onPointerUp_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPointerUp_1, AssignFromStack_onPointerUp_1);
            field = type.GetField("onPointerClick", flag);
            app.RegisterCLRFieldGetter(field, get_onPointerClick_2);
            app.RegisterCLRFieldSetter(field, set_onPointerClick_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPointerClick_2, AssignFromStack_onPointerClick_2);


        }



        static object get_onPointerDown_0(ref object o)
        {
            return ((Client.UIClickListener)o).onPointerDown;
        }

        static StackObject* CopyToStack_onPointerDown_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIClickListener)o).onPointerDown;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPointerDown_0(ref object o, object v)
        {
            ((Client.UIClickListener)o).onPointerDown = (Client.UIClickListener.EventTriggerCB1)v;
        }

        static StackObject* AssignFromStack_onPointerDown_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.UIClickListener.EventTriggerCB1 @onPointerDown = (Client.UIClickListener.EventTriggerCB1)typeof(Client.UIClickListener.EventTriggerCB1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIClickListener)o).onPointerDown = @onPointerDown;
            return ptr_of_this_method;
        }

        static object get_onPointerUp_1(ref object o)
        {
            return ((Client.UIClickListener)o).onPointerUp;
        }

        static StackObject* CopyToStack_onPointerUp_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIClickListener)o).onPointerUp;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPointerUp_1(ref object o, object v)
        {
            ((Client.UIClickListener)o).onPointerUp = (Client.UIClickListener.EventTriggerCB1)v;
        }

        static StackObject* AssignFromStack_onPointerUp_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.UIClickListener.EventTriggerCB1 @onPointerUp = (Client.UIClickListener.EventTriggerCB1)typeof(Client.UIClickListener.EventTriggerCB1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIClickListener)o).onPointerUp = @onPointerUp;
            return ptr_of_this_method;
        }

        static object get_onPointerClick_2(ref object o)
        {
            return ((Client.UIClickListener)o).onPointerClick;
        }

        static StackObject* CopyToStack_onPointerClick_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIClickListener)o).onPointerClick;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPointerClick_2(ref object o, object v)
        {
            ((Client.UIClickListener)o).onPointerClick = (Client.UIClickListener.EventTriggerCB1)v;
        }

        static StackObject* AssignFromStack_onPointerClick_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.UIClickListener.EventTriggerCB1 @onPointerClick = (Client.UIClickListener.EventTriggerCB1)typeof(Client.UIClickListener.EventTriggerCB1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIClickListener)o).onPointerClick = @onPointerClick;
            return ptr_of_this_method;
        }



    }
}
