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
    unsafe class Skyunion_IUIManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.IUIManager);
            args = new Type[]{typeof(Skyunion.UIInfo), typeof(System.Action), typeof(System.Object)};
            method = type.GetMethod("ShowUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowUI_0);
            args = new Type[]{typeof(Skyunion.UIPopValue)};
            method = type.GetMethod("RemoveUIPopStack", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveUIPopStack_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetUILayer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetUILayer_2);
            args = new Type[]{typeof(Skyunion.UIPopValue)};
            method = type.GetMethod("AddUIPopStack", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddUIPopStack_3);
            args = new Type[]{typeof(Skyunion.UIInfo)};
            method = type.GetMethod("ExistUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ExistUI_4);
            args = new Type[]{};
            method = type.GetMethod("GetUICamera", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetUICamera_5);
            args = new Type[]{};
            method = type.GetMethod("GetCanvas", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCanvas_6);
            args = new Type[]{typeof(Skyunion.OnShowUI)};
            method = type.GetMethod("AddShowUIListener", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddShowUIListener_7);
            args = new Type[]{typeof(Skyunion.OnCloseUI)};
            method = type.GetMethod("AddCloseUIListener", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddCloseUIListener_8);
            args = new Type[]{typeof(Skyunion.UIInfo), typeof(System.Boolean), typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("CloseUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseUI_9);
            args = new Type[]{typeof(System.Int32), typeof(System.Action), typeof(System.Object)};
            method = type.GetMethod("OpenUIByID", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OpenUIByID_10);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetUI_11);
            args = new Type[]{typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("CloseAll", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseAll_12);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetGuideStatus", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetGuideStatus_13);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ExistUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ExistUI_14);
            args = new Type[]{typeof(Skyunion.UILayer), typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("CloseLayerUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseLayerUI_15);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("SetExitGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetExitGame_16);
            args = new Type[]{typeof(Skyunion.UILayer)};
            method = type.GetMethod("LayerCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LayerCount_17);
            args = new Type[]{};
            method = type.GetMethod("IsHasPopView", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsHasPopView_18);
            args = new Type[]{typeof(System.Int32), typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("CloseGroupUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseGroupUI_19);
            args = new Type[]{typeof(Skyunion.UIInfo)};
            method = type.GetMethod("HideUI", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, HideUI_20);


        }


        static StackObject* ShowUI_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @data = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action @callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Skyunion.UIInfo @uiInfo = (Skyunion.UIInfo)typeof(Skyunion.UIInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ShowUI(@uiInfo, @callback, @data);

            return __ret;
        }

        static StackObject* RemoveUIPopStack_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.UIPopValue @ui = (Skyunion.UIPopValue)typeof(Skyunion.UIPopValue).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveUIPopStack(@ui);

            return __ret;
        }

        static StackObject* GetUILayer_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @layerIndex = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetUILayer(@layerIndex);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AddUIPopStack_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.UIPopValue @ui = (Skyunion.UIPopValue)typeof(Skyunion.UIPopValue).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddUIPopStack(@ui);

            return __ret;
        }

        static StackObject* ExistUI_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.UIInfo @uiInfo = (Skyunion.UIInfo)typeof(Skyunion.UIInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ExistUI(@uiInfo);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetUICamera_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetUICamera();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetCanvas_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetCanvas();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AddShowUIListener_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.OnShowUI @param = (Skyunion.OnShowUI)typeof(Skyunion.OnShowUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddShowUIListener(@param);

            return __ret;
        }

        static StackObject* AddCloseUIListener_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.OnCloseUI @param = (Skyunion.OnCloseUI)typeof(Skyunion.OnCloseUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddCloseUIListener(@param);

            return __ret;
        }

        static StackObject* CloseUI_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @returnToLogin = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @IsForceClose = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Boolean @IsManual = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.UIInfo @uiInfo = (Skyunion.UIInfo)typeof(Skyunion.UIInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseUI(@uiInfo, @IsManual, @IsForceClose, @returnToLogin);

            return __ret;
        }

        static StackObject* OpenUIByID_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @data = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action @callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @uiid = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OpenUIByID(@uiid, @callback, @data);

            return __ret;
        }

        static StackObject* GetUI_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @uiId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetUI(@uiId);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CloseAll_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @returnToLogin = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isForceClose = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseAll(@isForceClose, @returnToLogin);

            return __ret;
        }

        static StackObject* SetGuideStatus_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isGuide = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetGuideStatus(@isGuide);

            return __ret;
        }

        static StackObject* ExistUI_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @uiId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ExistUI(@uiId);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* CloseLayerUI_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @returnToLogin = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isForceClose = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Skyunion.UILayer @uiLayer = (Skyunion.UILayer)typeof(Skyunion.UILayer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseLayerUI(@uiLayer, @isForceClose, @returnToLogin);

            return __ret;
        }

        static StackObject* SetExitGame_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @param = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetExitGame(@param);

            return __ret;
        }

        static StackObject* LayerCount_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.UILayer @layer = (Skyunion.UILayer)typeof(Skyunion.UILayer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.LayerCount(@layer);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* IsHasPopView_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsHasPopView();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* CloseGroupUI_19(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @returnToLogin = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isForceClose = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @group = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CloseGroupUI(@group, @isForceClose, @returnToLogin);

            return __ret;
        }

        static StackObject* HideUI_20(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.UIInfo @uiInfo = (Skyunion.UIInfo)typeof(Skyunion.UIInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IUIManager instance_of_this_method = (Skyunion.IUIManager)typeof(Skyunion.IUIManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.HideUI(@uiInfo);

            return __ret;
        }



    }
}
