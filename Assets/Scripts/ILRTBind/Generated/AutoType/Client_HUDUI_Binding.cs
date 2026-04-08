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
    unsafe class Client_HUDUI_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.HUDUI);
            args = new Type[]{typeof(System.String), typeof(System.Type), typeof(Client.HUDLayer), typeof(UnityEngine.GameObject)};
            method = type.GetMethod("Register", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Register_0);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("SetTargetGameObject", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetTargetGameObject_1);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetPositionAutoAnchor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetPositionAutoAnchor_2);
            args = new Type[]{typeof(System.Action<Client.HUDUI>)};
            method = type.GetMethod("SetInitCallback", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetInitCallback_3);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetScaleAutoAnchor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetScaleAutoAnchor_4);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetAllowUpdatePos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetAllowUpdatePos_5);
            args = new Type[]{typeof(System.Action<Client.HUDUI>), typeof(System.Single)};
            method = type.GetMethod("SetUpdateCallback", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetUpdateCallback_6);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Action<System.Boolean, Client.HUDUI>), typeof(System.Boolean)};
            method = type.GetMethod("SetCameraLodDist", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCameraLodDist_7);
            args = new Type[]{typeof(UnityEngine.Vector3)};
            method = type.GetMethod("UpdateTargetPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateTargetPos_8);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.Vector3), typeof(Client.HUDLayer)};
            method = type.GetMethod("Register", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Register_9);
            args = new Type[]{};
            method = type.GetMethod("Close", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Close_10);
            args = new Type[]{};
            method = type.GetMethod("Show", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Show_11);
            args = new Type[]{typeof(UnityEngine.Vector2)};
            method = type.GetMethod("SetPosOffset", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetPosOffset_12);
            args = new Type[]{typeof(System.Object)};
            method = type.GetMethod("SetData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetData_13);

            field = type.GetField("gameView", flag);
            app.RegisterCLRFieldGetter(field, get_gameView_0);
            app.RegisterCLRFieldSetter(field, set_gameView_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_gameView_0, AssignFromStack_gameView_0);
            field = type.GetField("uiObj", flag);
            app.RegisterCLRFieldGetter(field, get_uiObj_1);
            app.RegisterCLRFieldSetter(field, set_uiObj_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_uiObj_1, AssignFromStack_uiObj_1);
            field = type.GetField("targetObj", flag);
            app.RegisterCLRFieldGetter(field, get_targetObj_2);
            app.RegisterCLRFieldSetter(field, set_targetObj_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_targetObj_2, AssignFromStack_targetObj_2);
            field = type.GetField("data", flag);
            app.RegisterCLRFieldGetter(field, get_data_3);
            app.RegisterCLRFieldSetter(field, set_data_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_data_3, AssignFromStack_data_3);
            field = type.GetField("bDispose", flag);
            app.RegisterCLRFieldGetter(field, get_bDispose_4);
            app.RegisterCLRFieldSetter(field, set_bDispose_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_bDispose_4, AssignFromStack_bDispose_4);
            field = type.GetField("assetLoadFinish", flag);
            app.RegisterCLRFieldGetter(field, get_assetLoadFinish_5);
            app.RegisterCLRFieldSetter(field, set_assetLoadFinish_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_assetLoadFinish_5, AssignFromStack_assetLoadFinish_5);
            field = type.GetField("viewData", flag);
            app.RegisterCLRFieldGetter(field, get_viewData_6);
            app.RegisterCLRFieldSetter(field, set_viewData_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_viewData_6, AssignFromStack_viewData_6);


        }


        static StackObject* Register_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @target = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDLayer @layer = (Client.HUDLayer)typeof(Client.HUDLayer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Type @viewClass = (System.Type)typeof(System.Type).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @assetName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.HUDUI.Register(@assetName, @viewClass, @layer, @target);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetTargetGameObject_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetTargetGameObject(@go);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetPositionAutoAnchor_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @autoAnchorPos = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetPositionAutoAnchor(@autoAnchorPos);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetInitCallback_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<Client.HUDUI> @initCallBack = (System.Action<Client.HUDUI>)typeof(System.Action<Client.HUDUI>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetInitCallback(@initCallBack);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetScaleAutoAnchor_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @autoAnchorScale = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetScaleAutoAnchor(@autoAnchorScale);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetAllowUpdatePos_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isBool = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetAllowUpdatePos(@isBool);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetUpdateCallback_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @interval = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<Client.HUDUI> @updateCallBack = (System.Action<Client.HUDUI>)typeof(System.Action<Client.HUDUI>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetUpdateCallback(@updateCallBack, @interval);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetCameraLodDist_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @AutoSetActiveByLodChange = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Boolean, Client.HUDUI> @onOutLodRange = (System.Action<System.Boolean, Client.HUDUI>)typeof(System.Action<System.Boolean, Client.HUDUI>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @maxDxf = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @minDxf = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetCameraLodDist(@minDxf, @maxDxf, @onOutLodRange, @AutoSetActiveByLodChange);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* UpdateTargetPos_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @pos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.UpdateTargetPos(@pos);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Register_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.HUDLayer @layer = (Client.HUDLayer)typeof(Client.HUDLayer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @targetPos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @targetPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @targetPos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @assetName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.HUDUI.Register(@assetName, @targetPos, @layer);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Close_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Close();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Show_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.Show();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetPosOffset_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @posOffset = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @posOffset, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @posOffset = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetPosOffset(@posOffset);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetData_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object @data = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.HUDUI instance_of_this_method = (Client.HUDUI)typeof(Client.HUDUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetData(@data);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_gameView_0(ref object o)
        {
            return ((Client.HUDUI)o).gameView;
        }

        static StackObject* CopyToStack_gameView_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.HUDUI)o).gameView;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_gameView_0(ref object o, object v)
        {
            ((Client.HUDUI)o).gameView = (Skyunion.GameView)v;
        }

        static StackObject* AssignFromStack_gameView_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Skyunion.GameView @gameView = (Skyunion.GameView)typeof(Skyunion.GameView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.HUDUI)o).gameView = @gameView;
            return ptr_of_this_method;
        }

        static object get_uiObj_1(ref object o)
        {
            return ((Client.HUDUI)o).uiObj;
        }

        static StackObject* CopyToStack_uiObj_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.HUDUI)o).uiObj;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_uiObj_1(ref object o, object v)
        {
            ((Client.HUDUI)o).uiObj = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_uiObj_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @uiObj = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.HUDUI)o).uiObj = @uiObj;
            return ptr_of_this_method;
        }

        static object get_targetObj_2(ref object o)
        {
            return ((Client.HUDUI)o).targetObj;
        }

        static StackObject* CopyToStack_targetObj_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.HUDUI)o).targetObj;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_targetObj_2(ref object o, object v)
        {
            ((Client.HUDUI)o).targetObj = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_targetObj_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @targetObj = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.HUDUI)o).targetObj = @targetObj;
            return ptr_of_this_method;
        }

        static object get_data_3(ref object o)
        {
            return ((Client.HUDUI)o).data;
        }

        static StackObject* CopyToStack_data_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.HUDUI)o).data;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_data_3(ref object o, object v)
        {
            ((Client.HUDUI)o).data = (System.Object)v;
        }

        static StackObject* AssignFromStack_data_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @data = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.HUDUI)o).data = @data;
            return ptr_of_this_method;
        }

        static object get_bDispose_4(ref object o)
        {
            return ((Client.HUDUI)o).bDispose;
        }

        static StackObject* CopyToStack_bDispose_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.HUDUI)o).bDispose;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_bDispose_4(ref object o, object v)
        {
            ((Client.HUDUI)o).bDispose = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_bDispose_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @bDispose = ptr_of_this_method->Value == 1;
            ((Client.HUDUI)o).bDispose = @bDispose;
            return ptr_of_this_method;
        }

        static object get_assetLoadFinish_5(ref object o)
        {
            return ((Client.HUDUI)o).assetLoadFinish;
        }

        static StackObject* CopyToStack_assetLoadFinish_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.HUDUI)o).assetLoadFinish;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_assetLoadFinish_5(ref object o, object v)
        {
            ((Client.HUDUI)o).assetLoadFinish = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_assetLoadFinish_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @assetLoadFinish = ptr_of_this_method->Value == 1;
            ((Client.HUDUI)o).assetLoadFinish = @assetLoadFinish;
            return ptr_of_this_method;
        }

        static object get_viewData_6(ref object o)
        {
            return ((Client.HUDUI)o).viewData;
        }

        static StackObject* CopyToStack_viewData_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.HUDUI)o).viewData;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_viewData_6(ref object o, object v)
        {
            ((Client.HUDUI)o).viewData = (System.Object)v;
        }

        static StackObject* AssignFromStack_viewData_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @viewData = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.HUDUI)o).viewData = @viewData;
            return ptr_of_this_method;
        }



    }
}
