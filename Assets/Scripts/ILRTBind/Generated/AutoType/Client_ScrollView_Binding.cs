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
    unsafe class Client_ScrollView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.ScrollView);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetItemByIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetItemByIndex_0);
            args = new Type[]{typeof(Client.ScrollView.ScrollItem)};
            method = type.GetMethod("RemoveItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveItem_1);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(Client.ScrollView.FuncTab)};
            method = type.GetMethod("SetInitData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetInitData_2);
            args = new Type[]{typeof(System.Single), typeof(System.String)};
            method = type.GetMethod("AddItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddItem_3);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_4);
            args = new Type[]{typeof(System.Int32), typeof(System.Single), typeof(System.String)};
            method = type.GetMethod("InsertItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InsertItem_5);
            args = new Type[]{};
            method = type.GetMethod("RefreshShowRect", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RefreshShowRect_6);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("MovePanelToItemIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, MovePanelToItemIndex_7);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ScrollPanelToItemInidex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ScrollPanelToItemInidex_8);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("LocateItemPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LocateItemPosition_9);

            field = type.GetField("ItemPrefabDataList", flag);
            app.RegisterCLRFieldGetter(field, get_ItemPrefabDataList_0);
            app.RegisterCLRFieldSetter(field, set_ItemPrefabDataList_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ItemPrefabDataList_0, AssignFromStack_ItemPrefabDataList_0);


        }


        static StackObject* GetItemByIndex_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetItemByIndex(@index);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* RemoveItem_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ScrollView.ScrollItem @item = (Client.ScrollView.ScrollItem)typeof(Client.ScrollView.ScrollItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveItem(@item);

            return __ret;
        }

        static StackObject* SetInitData_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ScrollView.FuncTab @funcTab = (Client.ScrollView.FuncTab)typeof(Client.ScrollView.FuncTab).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @prefab = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetInitData(@prefab, @funcTab);

            return __ret;
        }

        static StackObject* AddItem_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @tag = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @itemHeight = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AddItem(@itemHeight, @tag);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Clear_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @completed = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Clear(@completed);

            return __ret;
        }

        static StackObject* InsertItem_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @tag = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @itemHeight = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.InsertItem(@index, @itemHeight, @tag);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* RefreshShowRect_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RefreshShowRect();

            return __ret;
        }

        static StackObject* MovePanelToItemIndex_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MovePanelToItemIndex(@index);

            return __ret;
        }

        static StackObject* ScrollPanelToItemInidex_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ScrollPanelToItemInidex(@index);

            return __ret;
        }

        static StackObject* LocateItemPosition_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ScrollView instance_of_this_method = (Client.ScrollView)typeof(Client.ScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.LocateItemPosition(@index);

            return __ret;
        }


        static object get_ItemPrefabDataList_0(ref object o)
        {
            return ((Client.ScrollView)o).ItemPrefabDataList;
        }

        static StackObject* CopyToStack_ItemPrefabDataList_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ScrollView)o).ItemPrefabDataList;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ItemPrefabDataList_0(ref object o, object v)
        {
            ((Client.ScrollView)o).ItemPrefabDataList = (System.Collections.Generic.List<System.String>)v;
        }

        static StackObject* AssignFromStack_ItemPrefabDataList_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.String> @ItemPrefabDataList = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ScrollView)o).ItemPrefabDataList = @ItemPrefabDataList;
            return ptr_of_this_method;
        }



    }
}
