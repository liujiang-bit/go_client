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
    unsafe class Client_ScrollView_Binding_ItemObject_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.ScrollView.ItemObject);

            field = type.GetField("isInit", flag);
            app.RegisterCLRFieldGetter(field, get_isInit_0);
            app.RegisterCLRFieldSetter(field, set_isInit_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_isInit_0, AssignFromStack_isInit_0);


        }



        static object get_isInit_0(ref object o)
        {
            return ((Client.ScrollView.ItemObject)o).isInit;
        }

        static StackObject* CopyToStack_isInit_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ScrollView.ItemObject)o).isInit;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isInit_0(ref object o, object v)
        {
            ((Client.ScrollView.ItemObject)o).isInit = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isInit_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isInit = ptr_of_this_method->Value == 1;
            ((Client.ScrollView.ItemObject)o).isInit = @isInit;
            return ptr_of_this_method;
        }



    }
}
