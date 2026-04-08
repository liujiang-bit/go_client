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
    unsafe class Client_UIEventTrigger_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.UIEventTrigger);

            field = type.GetField("onDrag", flag);
            app.RegisterCLRFieldGetter(field, get_onDrag_0);
            app.RegisterCLRFieldSetter(field, set_onDrag_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDrag_0, AssignFromStack_onDrag_0);
            field = type.GetField("onPointerUp", flag);
            app.RegisterCLRFieldGetter(field, get_onPointerUp_1);
            app.RegisterCLRFieldSetter(field, set_onPointerUp_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPointerUp_1, AssignFromStack_onPointerUp_1);
            field = type.GetField("onPointerDown", flag);
            app.RegisterCLRFieldGetter(field, get_onPointerDown_2);
            app.RegisterCLRFieldSetter(field, set_onPointerDown_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPointerDown_2, AssignFromStack_onPointerDown_2);
            field = type.GetField("onPointerClick", flag);
            app.RegisterCLRFieldGetter(field, get_onPointerClick_3);
            app.RegisterCLRFieldSetter(field, set_onPointerClick_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPointerClick_3, AssignFromStack_onPointerClick_3);


        }



        static object get_onDrag_0(ref object o)
        {
            return ((Client.UIEventTrigger)o).onDrag;
        }

        static StackObject* CopyToStack_onDrag_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIEventTrigger)o).onDrag;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDrag_0(ref object o, object v)
        {
            ((Client.UIEventTrigger)o).onDrag = (Client.UIEventTrigger.EventTriggerCB1)v;
        }

        static StackObject* AssignFromStack_onDrag_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.UIEventTrigger.EventTriggerCB1 @onDrag = (Client.UIEventTrigger.EventTriggerCB1)typeof(Client.UIEventTrigger.EventTriggerCB1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIEventTrigger)o).onDrag = @onDrag;
            return ptr_of_this_method;
        }

        static object get_onPointerUp_1(ref object o)
        {
            return ((Client.UIEventTrigger)o).onPointerUp;
        }

        static StackObject* CopyToStack_onPointerUp_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIEventTrigger)o).onPointerUp;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPointerUp_1(ref object o, object v)
        {
            ((Client.UIEventTrigger)o).onPointerUp = (Client.UIEventTrigger.EventTriggerCB1)v;
        }

        static StackObject* AssignFromStack_onPointerUp_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.UIEventTrigger.EventTriggerCB1 @onPointerUp = (Client.UIEventTrigger.EventTriggerCB1)typeof(Client.UIEventTrigger.EventTriggerCB1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIEventTrigger)o).onPointerUp = @onPointerUp;
            return ptr_of_this_method;
        }

        static object get_onPointerDown_2(ref object o)
        {
            return ((Client.UIEventTrigger)o).onPointerDown;
        }

        static StackObject* CopyToStack_onPointerDown_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIEventTrigger)o).onPointerDown;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPointerDown_2(ref object o, object v)
        {
            ((Client.UIEventTrigger)o).onPointerDown = (Client.UIEventTrigger.EventTriggerCB1)v;
        }

        static StackObject* AssignFromStack_onPointerDown_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.UIEventTrigger.EventTriggerCB1 @onPointerDown = (Client.UIEventTrigger.EventTriggerCB1)typeof(Client.UIEventTrigger.EventTriggerCB1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIEventTrigger)o).onPointerDown = @onPointerDown;
            return ptr_of_this_method;
        }

        static object get_onPointerClick_3(ref object o)
        {
            return ((Client.UIEventTrigger)o).onPointerClick;
        }

        static StackObject* CopyToStack_onPointerClick_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIEventTrigger)o).onPointerClick;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPointerClick_3(ref object o, object v)
        {
            ((Client.UIEventTrigger)o).onPointerClick = (Client.UIEventTrigger.EventTriggerCB1)v;
        }

        static StackObject* AssignFromStack_onPointerClick_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.UIEventTrigger.EventTriggerCB1 @onPointerClick = (Client.UIEventTrigger.EventTriggerCB1)typeof(Client.UIEventTrigger.EventTriggerCB1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIEventTrigger)o).onPointerClick = @onPointerClick;
            return ptr_of_this_method;
        }



    }
}
