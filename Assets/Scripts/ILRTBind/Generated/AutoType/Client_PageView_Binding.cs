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
    unsafe class Client_PageView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.PageView);
            args = new Type[]{typeof(System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>), typeof(Client.PageView.FuncTab)};
            method = type.GetMethod("SetInitData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetInitData_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FillContent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FillContent_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ScrollPanelToItemIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ScrollPanelToItemIndex_2);

            field = type.GetField("ItemPrefabDataList", flag);
            app.RegisterCLRFieldGetter(field, get_ItemPrefabDataList_0);
            app.RegisterCLRFieldSetter(field, set_ItemPrefabDataList_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ItemPrefabDataList_0, AssignFromStack_ItemPrefabDataList_0);


        }


        static StackObject* SetInitData_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.PageView.FuncTab @funcObj = (Client.PageView.FuncTab)typeof(Client.PageView.FuncTab).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject> @prefabDic = (System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>)typeof(System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.PageView instance_of_this_method = (Client.PageView)typeof(Client.PageView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetInitData(@prefabDic, @funcObj);

            return __ret;
        }

        static StackObject* FillContent_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @listLength = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.PageView instance_of_this_method = (Client.PageView)typeof(Client.PageView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.FillContent(@listLength);

            return __ret;
        }

        static StackObject* ScrollPanelToItemIndex_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.PageView instance_of_this_method = (Client.PageView)typeof(Client.PageView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ScrollPanelToItemIndex(@index);

            return __ret;
        }


        static object get_ItemPrefabDataList_0(ref object o)
        {
            return ((Client.PageView)o).ItemPrefabDataList;
        }

        static StackObject* CopyToStack_ItemPrefabDataList_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.PageView)o).ItemPrefabDataList;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ItemPrefabDataList_0(ref object o, object v)
        {
            ((Client.PageView)o).ItemPrefabDataList = (System.Collections.Generic.List<System.String>)v;
        }

        static StackObject* AssignFromStack_ItemPrefabDataList_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.String> @ItemPrefabDataList = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.PageView)o).ItemPrefabDataList = @ItemPrefabDataList;
            return ptr_of_this_method;
        }



    }
}
