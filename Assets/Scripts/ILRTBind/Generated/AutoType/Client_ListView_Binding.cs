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
    unsafe class Client_ListView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.ListView);
            args = new Type[]{typeof(System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>), typeof(Client.ListView.FuncTab)};
            method = type.GetMethod("SetInitData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetInitData_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FillContent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FillContent_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ScrollList2IdxCenter", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ScrollList2IdxCenter_2);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetItemByIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetItemByIndex_3);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("RefreshItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RefreshItem_4);
            args = new Type[]{};
            method = type.GetMethod("GetContainerPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetContainerPos_5);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("ScrollToPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ScrollToPos_6);
            args = new Type[]{};
            method = type.GetMethod("ForceRefresh", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ForceRefresh_7);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ScrollPanelToItemIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ScrollPanelToItemIndex_8);
            args = new Type[]{typeof(System.Int32), typeof(System.Single)};
            method = type.GetMethod("UpdateItemSize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateItemSize_9);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("MovePanelToItemIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, MovePanelToItemIndex_10);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("MovePanelToPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, MovePanelToPos_11);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("RefreshAndRestPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RefreshAndRestPos_12);
            args = new Type[]{};
            method = type.GetMethod("SetContainerLayout", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetContainerLayout_13);
            args = new Type[]{};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_14);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("RemoveAt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveAt_15);
            args = new Type[]{typeof(UnityEngine.UI.ScrollRect)};
            method = type.GetMethod("SetParent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetParent_16);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("ShowContentAt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowContentAt_17);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("SetContainerPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetContainerPos_18);
            args = new Type[]{typeof(Client.ListView)};
            method = type.GetMethod("SetParentListView", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetParentListView_19);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetItemSizeByIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetItemSizeByIndex_20);
            args = new Type[]{typeof(Client.ScrollBaseView)};
            method = type.GetMethod("SetParentView", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetParentView_21);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("Insert", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Insert_22);

            field = type.GetField("ItemPrefabDataList", flag);
            app.RegisterCLRFieldGetter(field, get_ItemPrefabDataList_0);
            app.RegisterCLRFieldSetter(field, set_ItemPrefabDataList_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ItemPrefabDataList_0, AssignFromStack_ItemPrefabDataList_0);
            field = type.GetField("onDragBegin", flag);
            app.RegisterCLRFieldGetter(field, get_onDragBegin_1);
            app.RegisterCLRFieldSetter(field, set_onDragBegin_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDragBegin_1, AssignFromStack_onDragBegin_1);
            field = type.GetField("onDragEnd", flag);
            app.RegisterCLRFieldGetter(field, get_onDragEnd_2);
            app.RegisterCLRFieldSetter(field, set_onDragEnd_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDragEnd_2, AssignFromStack_onDragEnd_2);
            field = type.GetField("listContainer", flag);
            app.RegisterCLRFieldGetter(field, get_listContainer_3);
            app.RegisterCLRFieldSetter(field, set_listContainer_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_listContainer_3, AssignFromStack_listContainer_3);
            field = type.GetField("autoScrollTime", flag);
            app.RegisterCLRFieldGetter(field, get_autoScrollTime_4);
            app.RegisterCLRFieldSetter(field, set_autoScrollTime_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_autoScrollTime_4, AssignFromStack_autoScrollTime_4);
            field = type.GetField("layoutType", flag);
            app.RegisterCLRFieldGetter(field, get_layoutType_5);
            app.RegisterCLRFieldSetter(field, set_layoutType_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_layoutType_5, AssignFromStack_layoutType_5);


        }


        static StackObject* SetInitData_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ListView.FuncTab @funcObj = (Client.ListView.FuncTab)typeof(Client.ListView.FuncTab).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject> @prefabDic = (System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>)typeof(System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
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
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.FillContent(@listLength);

            return __ret;
        }

        static StackObject* ScrollList2IdxCenter_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ScrollList2IdxCenter(@index);

            return __ret;
        }

        static StackObject* GetItemByIndex_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetItemByIndex(@index);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* RefreshItem_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RefreshItem(@index);

            return __ret;
        }

        static StackObject* GetContainerPos_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetContainerPos();

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ScrollToPos_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @dest = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ScrollToPos(@dest);

            return __ret;
        }

        static StackObject* ForceRefresh_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ForceRefresh();

            return __ret;
        }

        static StackObject* ScrollPanelToItemIndex_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ScrollPanelToItemIndex(@index);

            return __ret;
        }

        static StackObject* UpdateItemSize_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @size = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateItemSize(@index, @size);

            return __ret;
        }

        static StackObject* MovePanelToItemIndex_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MovePanelToItemIndex(@index);

            return __ret;
        }

        static StackObject* MovePanelToPos_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @pos = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MovePanelToPos(@pos);

            return __ret;
        }

        static StackObject* RefreshAndRestPos_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @count = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RefreshAndRestPos(@count);

            return __ret;
        }

        static StackObject* SetContainerLayout_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetContainerLayout();

            return __ret;
        }

        static StackObject* Clear_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Clear();

            return __ret;
        }

        static StackObject* RemoveAt_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveAt(@index);

            return __ret;
        }

        static StackObject* SetParent_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.ScrollRect @parent = (UnityEngine.UI.ScrollRect)typeof(UnityEngine.UI.ScrollRect).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetParent(@parent);

            return __ret;
        }

        static StackObject* ShowContentAt_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @pos = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ShowContentAt(@pos);

            return __ret;
        }

        static StackObject* SetContainerPos_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @pos = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetContainerPos(@pos);

            return __ret;
        }

        static StackObject* SetParentListView_19(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ListView @parent = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetParentListView(@parent);

            return __ret;
        }

        static StackObject* GetItemSizeByIndex_20(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetItemSizeByIndex(@index);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* SetParentView_21(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.ScrollBaseView @parent = (Client.ScrollBaseView)typeof(Client.ScrollBaseView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetParentView(@parent);

            return __ret;
        }

        static StackObject* Insert_22(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.ListView instance_of_this_method = (Client.ListView)typeof(Client.ListView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Insert(@index);

            return __ret;
        }


        static object get_ItemPrefabDataList_0(ref object o)
        {
            return ((Client.ListView)o).ItemPrefabDataList;
        }

        static StackObject* CopyToStack_ItemPrefabDataList_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView)o).ItemPrefabDataList;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ItemPrefabDataList_0(ref object o, object v)
        {
            ((Client.ListView)o).ItemPrefabDataList = (System.Collections.Generic.List<System.String>)v;
        }

        static StackObject* AssignFromStack_ItemPrefabDataList_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.String> @ItemPrefabDataList = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView)o).ItemPrefabDataList = @ItemPrefabDataList;
            return ptr_of_this_method;
        }

        static object get_onDragBegin_1(ref object o)
        {
            return ((Client.ListView)o).onDragBegin;
        }

        static StackObject* CopyToStack_onDragBegin_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView)o).onDragBegin;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDragBegin_1(ref object o, object v)
        {
            ((Client.ListView)o).onDragBegin = (System.Action<UnityEngine.EventSystems.PointerEventData>)v;
        }

        static StackObject* AssignFromStack_onDragBegin_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.EventSystems.PointerEventData> @onDragBegin = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView)o).onDragBegin = @onDragBegin;
            return ptr_of_this_method;
        }

        static object get_onDragEnd_2(ref object o)
        {
            return ((Client.ListView)o).onDragEnd;
        }

        static StackObject* CopyToStack_onDragEnd_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView)o).onDragEnd;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDragEnd_2(ref object o, object v)
        {
            ((Client.ListView)o).onDragEnd = (System.Action<UnityEngine.EventSystems.PointerEventData>)v;
        }

        static StackObject* AssignFromStack_onDragEnd_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<UnityEngine.EventSystems.PointerEventData> @onDragEnd = (System.Action<UnityEngine.EventSystems.PointerEventData>)typeof(System.Action<UnityEngine.EventSystems.PointerEventData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView)o).onDragEnd = @onDragEnd;
            return ptr_of_this_method;
        }

        static object get_listContainer_3(ref object o)
        {
            return ((Client.ListView)o).listContainer;
        }

        static StackObject* CopyToStack_listContainer_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView)o).listContainer;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_listContainer_3(ref object o, object v)
        {
            ((Client.ListView)o).listContainer = (UnityEngine.RectTransform)v;
        }

        static StackObject* AssignFromStack_listContainer_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.RectTransform @listContainer = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView)o).listContainer = @listContainer;
            return ptr_of_this_method;
        }

        static object get_autoScrollTime_4(ref object o)
        {
            return ((Client.ListView)o).autoScrollTime;
        }

        static StackObject* CopyToStack_autoScrollTime_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView)o).autoScrollTime;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_autoScrollTime_4(ref object o, object v)
        {
            ((Client.ListView)o).autoScrollTime = (System.Single)v;
        }

        static StackObject* AssignFromStack_autoScrollTime_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @autoScrollTime = *(float*)&ptr_of_this_method->Value;
            ((Client.ListView)o).autoScrollTime = @autoScrollTime;
            return ptr_of_this_method;
        }

        static object get_layoutType_5(ref object o)
        {
            return ((Client.ListView)o).layoutType;
        }

        static StackObject* CopyToStack_layoutType_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ListView)o).layoutType;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_layoutType_5(ref object o, object v)
        {
            ((Client.ListView)o).layoutType = (Client.ListView.ListViewLayoutType)v;
        }

        static StackObject* AssignFromStack_layoutType_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.ListView.ListViewLayoutType @layoutType = (Client.ListView.ListViewLayoutType)typeof(Client.ListView.ListViewLayoutType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.ListView)o).layoutType = @layoutType;
            return ptr_of_this_method;
        }



    }
}
