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
    unsafe class Client_ListView_Binding_FuncTab_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.ListView.FuncTab);

            field = type.GetField("ItemEnter", flag);
            app.RegisterCLRFieldGetter(field, get_ItemEnter_0);
            app.RegisterCLRFieldSetter(field, set_ItemEnter_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ItemEnter_0, AssignFromStack_ItemEnter_0);
            field = type.GetField("GetItemPrefabName", flag);
            app.RegisterCLRFieldGetter(field, get_GetItemPrefabName_1);
            app.RegisterCLRFieldSetter(field, set_GetItemPrefabName_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_GetItemPrefabName_1, AssignFromStack_GetItemPrefabName_1);
            field = type.GetField("GetItemSize", flag);
            app.RegisterCLRFieldGetter(field, get_GetItemSize_2);
            app.RegisterCLRFieldSetter(field, set_GetItemSize_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_GetItemSize_2, AssignFromStack_GetItemSize_2);
            field = type.GetField("ItemRemove", flag);
            app.RegisterCLRFieldGetter(field, get_ItemRemove_3);
            app.RegisterCLRFieldSetter(field, set_ItemRemove_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_ItemRemove_3, AssignFromStack_ItemRemove_3);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_ItemEnter_0(ref object o)
        {
            return ((Client.ListView.FuncTab)o).ItemEnter;
        }

        static StackObject* CopyToStack_ItemEnter_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.FuncTab)o).ItemEnter;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ItemEnter_0(ref object o, object v)
        {
            ((Client.ListView.FuncTab)o).ItemEnter = (System.Action<Client.ListView.ListItem>)v;
        }

        static StackObject* AssignFromStack_ItemEnter_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<Client.ListView.ListItem> @ItemEnter = (System.Action<Client.ListView.ListItem>)typeof(System.Action<Client.ListView.ListItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView.FuncTab)o).ItemEnter = @ItemEnter;
            return ptr_of_this_method;
        }

        static object get_GetItemPrefabName_1(ref object o)
        {
            return ((Client.ListView.FuncTab)o).GetItemPrefabName;
        }

        static StackObject* CopyToStack_GetItemPrefabName_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.FuncTab)o).GetItemPrefabName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_GetItemPrefabName_1(ref object o, object v)
        {
            ((Client.ListView.FuncTab)o).GetItemPrefabName = (Client.ListView.FuncTab.ReturnString)v;
        }

        static StackObject* AssignFromStack_GetItemPrefabName_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.ListView.FuncTab.ReturnString @GetItemPrefabName = (Client.ListView.FuncTab.ReturnString)typeof(Client.ListView.FuncTab.ReturnString).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView.FuncTab)o).GetItemPrefabName = @GetItemPrefabName;
            return ptr_of_this_method;
        }

        static object get_GetItemSize_2(ref object o)
        {
            return ((Client.ListView.FuncTab)o).GetItemSize;
        }

        static StackObject* CopyToStack_GetItemSize_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.FuncTab)o).GetItemSize;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_GetItemSize_2(ref object o, object v)
        {
            ((Client.ListView.FuncTab)o).GetItemSize = (Client.ListView.FuncTab.ReturnFloat)v;
        }

        static StackObject* AssignFromStack_GetItemSize_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.ListView.FuncTab.ReturnFloat @GetItemSize = (Client.ListView.FuncTab.ReturnFloat)typeof(Client.ListView.FuncTab.ReturnFloat).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView.FuncTab)o).GetItemSize = @GetItemSize;
            return ptr_of_this_method;
        }

        static object get_ItemRemove_3(ref object o)
        {
            return ((Client.ListView.FuncTab)o).ItemRemove;
        }

        static StackObject* CopyToStack_ItemRemove_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.FuncTab)o).ItemRemove;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ItemRemove_3(ref object o, object v)
        {
            ((Client.ListView.FuncTab)o).ItemRemove = (System.Action<Client.ListView.ListItem>)v;
        }

        static StackObject* AssignFromStack_ItemRemove_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<Client.ListView.ListItem> @ItemRemove = (System.Action<Client.ListView.ListItem>)typeof(System.Action<Client.ListView.ListItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView.FuncTab)o).ItemRemove = @ItemRemove;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Client.ListView.FuncTab();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
