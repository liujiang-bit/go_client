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
    unsafe class Client_PageView_Binding_ListItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.PageView.ListItem);

            field = type.GetField("go", flag);
            app.RegisterCLRFieldGetter(field, get_go_0);
            app.RegisterCLRFieldSetter(field, set_go_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_go_0, AssignFromStack_go_0);
            field = type.GetField("index", flag);
            app.RegisterCLRFieldGetter(field, get_index_1);
            app.RegisterCLRFieldSetter(field, set_index_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_index_1, AssignFromStack_index_1);


        }



        static object get_go_0(ref object o)
        {
            return ((Client.PageView.ListItem)o).go;
        }

        static StackObject* CopyToStack_go_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.PageView.ListItem)o).go;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_go_0(ref object o, object v)
        {
            ((Client.PageView.ListItem)o).go = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_go_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.PageView.ListItem)o).go = @go;
            return ptr_of_this_method;
        }

        static object get_index_1(ref object o)
        {
            return ((Client.PageView.ListItem)o).index;
        }

        static StackObject* CopyToStack_index_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.PageView.ListItem)o).index;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_index_1(ref object o, object v)
        {
            ((Client.PageView.ListItem)o).index = (System.Int32)v;
        }

        static StackObject* AssignFromStack_index_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @index = ptr_of_this_method->Value;
            ((Client.PageView.ListItem)o).index = @index;
            return ptr_of_this_method;
        }



    }
}
