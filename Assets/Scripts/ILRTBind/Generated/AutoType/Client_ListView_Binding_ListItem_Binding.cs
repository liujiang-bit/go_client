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
    unsafe class Client_ListView_Binding_ListItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.ListView.ListItem);
            args = new Type[]{};
            method = type.GetMethod("HasGameObject", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, HasGameObject_0);

            field = type.GetField("index", flag);
            app.RegisterCLRFieldGetter(field, get_index_0);
            app.RegisterCLRFieldSetter(field, set_index_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_index_0, AssignFromStack_index_0);
            field = type.GetField("data", flag);
            app.RegisterCLRFieldGetter(field, get_data_1);
            app.RegisterCLRFieldSetter(field, set_data_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_data_1, AssignFromStack_data_1);
            field = type.GetField("go", flag);
            app.RegisterCLRFieldGetter(field, get_go_2);
            app.RegisterCLRFieldSetter(field, set_go_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_go_2, AssignFromStack_go_2);
            field = type.GetField("isInit", flag);
            app.RegisterCLRFieldGetter(field, get_isInit_3);
            app.RegisterCLRFieldSetter(field, set_isInit_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_isInit_3, AssignFromStack_isInit_3);
            field = type.GetField("startPos", flag);
            app.RegisterCLRFieldGetter(field, get_startPos_4);
            app.RegisterCLRFieldSetter(field, set_startPos_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_startPos_4, AssignFromStack_startPos_4);
            field = type.GetField("prefabName", flag);
            app.RegisterCLRFieldGetter(field, get_prefabName_5);
            app.RegisterCLRFieldSetter(field, set_prefabName_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_prefabName_5, AssignFromStack_prefabName_5);


        }


        static StackObject* HasGameObject_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ListView.ListItem instance_of_this_method = (Client.ListView.ListItem)typeof(Client.ListView.ListItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.HasGameObject();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_index_0(ref object o)
        {
            return ((Client.ListView.ListItem)o).index;
        }

        static StackObject* CopyToStack_index_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.ListItem)o).index;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_index_0(ref object o, object v)
        {
            ((Client.ListView.ListItem)o).index = (System.Int32)v;
        }

        static StackObject* AssignFromStack_index_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @index = ptr_of_this_method->Value;
            ((Client.ListView.ListItem)o).index = @index;
            return ptr_of_this_method;
        }

        static object get_data_1(ref object o)
        {
            return ((Client.ListView.ListItem)o).data;
        }

        static StackObject* CopyToStack_data_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.ListItem)o).data;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_data_1(ref object o, object v)
        {
            ((Client.ListView.ListItem)o).data = (System.Object)v;
        }

        static StackObject* AssignFromStack_data_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @data = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView.ListItem)o).data = @data;
            return ptr_of_this_method;
        }

        static object get_go_2(ref object o)
        {
            return ((Client.ListView.ListItem)o).go;
        }

        static StackObject* CopyToStack_go_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.ListItem)o).go;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_go_2(ref object o, object v)
        {
            ((Client.ListView.ListItem)o).go = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_go_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView.ListItem)o).go = @go;
            return ptr_of_this_method;
        }

        static object get_isInit_3(ref object o)
        {
            return ((Client.ListView.ListItem)o).isInit;
        }

        static StackObject* CopyToStack_isInit_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.ListItem)o).isInit;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isInit_3(ref object o, object v)
        {
            ((Client.ListView.ListItem)o).isInit = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isInit_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isInit = ptr_of_this_method->Value == 1;
            ((Client.ListView.ListItem)o).isInit = @isInit;
            return ptr_of_this_method;
        }

        static object get_startPos_4(ref object o)
        {
            return ((Client.ListView.ListItem)o).startPos;
        }

        static StackObject* CopyToStack_startPos_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.ListItem)o).startPos;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_startPos_4(ref object o, object v)
        {
            ((Client.ListView.ListItem)o).startPos = (System.Single)v;
        }

        static StackObject* AssignFromStack_startPos_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @startPos = *(float*)&ptr_of_this_method->Value;
            ((Client.ListView.ListItem)o).startPos = @startPos;
            return ptr_of_this_method;
        }

        static object get_prefabName_5(ref object o)
        {
            return ((Client.ListView.ListItem)o).prefabName;
        }

        static StackObject* CopyToStack_prefabName_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView.ListItem)o).prefabName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_prefabName_5(ref object o, object v)
        {
            ((Client.ListView.ListItem)o).prefabName = (System.String)v;
        }

        static StackObject* AssignFromStack_prefabName_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @prefabName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView.ListItem)o).prefabName = @prefabName;
            return ptr_of_this_method;
        }



    }
}
