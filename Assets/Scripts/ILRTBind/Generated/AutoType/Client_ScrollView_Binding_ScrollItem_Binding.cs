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
    unsafe class Client_ScrollView_Binding_ScrollItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.ScrollView.ScrollItem);
            args = new Type[]{};
            method = type.GetMethod("GetGameObject", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetGameObject_0);

            field = type.GetField("index", flag);
            app.RegisterCLRFieldGetter(field, get_index_0);
            app.RegisterCLRFieldSetter(field, set_index_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_index_0, AssignFromStack_index_0);
            field = type.GetField("tag", flag);
            app.RegisterCLRFieldGetter(field, get_tag_1);
            app.RegisterCLRFieldSetter(field, set_tag_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_tag_1, AssignFromStack_tag_1);
            field = type.GetField("gameObject", flag);
            app.RegisterCLRFieldGetter(field, get_gameObject_2);
            app.RegisterCLRFieldSetter(field, set_gameObject_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_gameObject_2, AssignFromStack_gameObject_2);


        }


        static StackObject* GetGameObject_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ScrollView.ScrollItem instance_of_this_method = (Client.ScrollView.ScrollItem)typeof(Client.ScrollView.ScrollItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetGameObject();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_index_0(ref object o)
        {
            return ((Client.ScrollView.ScrollItem)o).index;
        }

        static StackObject* CopyToStack_index_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ScrollView.ScrollItem)o).index;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_index_0(ref object o, object v)
        {
            ((Client.ScrollView.ScrollItem)o).index = (System.Int32)v;
        }

        static StackObject* AssignFromStack_index_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @index = ptr_of_this_method->Value;
            ((Client.ScrollView.ScrollItem)o).index = @index;
            return ptr_of_this_method;
        }

        static object get_tag_1(ref object o)
        {
            return ((Client.ScrollView.ScrollItem)o).tag;
        }

        static StackObject* CopyToStack_tag_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ScrollView.ScrollItem)o).tag;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_tag_1(ref object o, object v)
        {
            ((Client.ScrollView.ScrollItem)o).tag = (System.String)v;
        }

        static StackObject* AssignFromStack_tag_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @tag = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ScrollView.ScrollItem)o).tag = @tag;
            return ptr_of_this_method;
        }

        static object get_gameObject_2(ref object o)
        {
            return ((Client.ScrollView.ScrollItem)o).gameObject;
        }

        static StackObject* CopyToStack_gameObject_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ScrollView.ScrollItem)o).gameObject;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_gameObject_2(ref object o, object v)
        {
            ((Client.ScrollView.ScrollItem)o).gameObject = (Client.ScrollView.ItemObject)v;
        }

        static StackObject* AssignFromStack_gameObject_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.ScrollView.ItemObject @gameObject = (Client.ScrollView.ItemObject)typeof(Client.ScrollView.ItemObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ScrollView.ScrollItem)o).gameObject = @gameObject;
            return ptr_of_this_method;
        }



    }
}
