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
    unsafe class Client_PageView_Binding_FuncTab_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.PageView.FuncTab);

            field = type.GetField("ItemEnter", flag);
            app.RegisterCLRFieldGetter(field, get_ItemEnter_0);
            app.RegisterCLRFieldSetter(field, set_ItemEnter_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ItemEnter_0, AssignFromStack_ItemEnter_0);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_ItemEnter_0(ref object o)
        {
            return ((Client.PageView.FuncTab)o).ItemEnter;
        }

        static StackObject* CopyToStack_ItemEnter_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.PageView.FuncTab)o).ItemEnter;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ItemEnter_0(ref object o, object v)
        {
            ((Client.PageView.FuncTab)o).ItemEnter = (System.Action<Client.PageView.ListItem>)v;
        }

        static StackObject* AssignFromStack_ItemEnter_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<Client.PageView.ListItem> @ItemEnter = (System.Action<Client.PageView.ListItem>)typeof(System.Action<Client.PageView.ListItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.PageView.FuncTab)o).ItemEnter = @ItemEnter;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Client.PageView.FuncTab();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
