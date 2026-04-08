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
    unsafe class Client_WorldCamera_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.WorldCamera);
            args = new Type[]{};
            method = type.GetMethod("GetCamera", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCamera_0);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("getCameraDxf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getCameraDxf_1);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Action)};
            method = type.GetMethod("SetCameraDxf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCameraDxf_2);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Single), typeof(System.Action)};
            method = type.GetMethod("ViewTerrainPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ViewTerrainPos_3);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetCanDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCanDrag_4);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetCanZoom", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCanZoom_5);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetCanClick", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCanClick_6);
            args = new Type[]{};
            method = type.GetMethod("ClearViewChange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearViewChange_7);
            args = new Type[]{};
            method = type.GetMethod("GetViewCenter", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetViewCenter_8);
            args = new Type[]{typeof(System.Action<System.Single, System.Single, System.Single>)};
            method = type.GetMethod("AddViewChange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddViewChange_9);
            args = new Type[]{typeof(System.Action<System.Single, System.Single, System.Single>)};
            method = type.GetMethod("RemoveViewChange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveViewChange_10);
            args = new Type[]{};
            method = type.GetMethod("getCurrentCameraDxf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getCurrentCameraDxf_11);
            args = new Type[]{};
            method = type.GetMethod("IsAutoMoving", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsAutoMoving_12);
            args = new Type[]{};
            method = type.GetMethod("IsMovingToPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsMovingToPos_13);
            args = new Type[]{};
            method = type.GetMethod("IsSlipping", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsSlipping_14);
            args = new Type[]{};
            method = type.GetMethod("get_isMovingToPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isMovingToPos_15);
            args = new Type[]{typeof(UnityEngine.Camera), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("GetTouchTerrainPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetTouchTerrainPos_16);
            args = new Type[]{};
            method = type.GetMethod("CanDrag", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CanDrag_17);
            args = new Type[]{};
            method = type.GetMethod("getCurrentCameraDist", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getCurrentCameraDist_18);
            args = new Type[]{typeof(System.Action<System.Single, System.Single>)};
            method = type.GetMethod("AddMapClickListner", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddMapClickListner_19);
            args = new Type[]{typeof(System.Action<System.Single, System.Single>)};
            method = type.GetMethod("RemoveMapClickListner", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveMapClickListner_20);
            args = new Type[]{typeof(System.Collections.Generic.List<Client.WorldCamera.cameraInfoItem>)};
            method = type.GetMethod("ResetCamera", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetCamera_21);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("setAdditionHeightForMinDxf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, setAdditionHeightForMinDxf_22);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_isMovingToPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_isMovingToPos_23);
            args = new Type[]{};
            method = type.GetMethod("get_GetTerrainPlane", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_GetTerrainPlane_24);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_AllowTouchWhenMovingOrZooming", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_AllowTouchWhenMovingOrZooming_25);

            field = type.GetField("enableReboundXY", flag);
            app.RegisterCLRFieldGetter(field, get_enableReboundXY_0);
            app.RegisterCLRFieldSetter(field, set_enableReboundXY_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_enableReboundXY_0, AssignFromStack_enableReboundXY_0);
            field = type.GetField("worldMinX", flag);
            app.RegisterCLRFieldGetter(field, get_worldMinX_1);
            app.RegisterCLRFieldSetter(field, set_worldMinX_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_worldMinX_1, AssignFromStack_worldMinX_1);
            field = type.GetField("worldMaxX", flag);
            app.RegisterCLRFieldGetter(field, get_worldMaxX_2);
            app.RegisterCLRFieldSetter(field, set_worldMaxX_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_worldMaxX_2, AssignFromStack_worldMaxX_2);
            field = type.GetField("worldMinY", flag);
            app.RegisterCLRFieldGetter(field, get_worldMinY_3);
            app.RegisterCLRFieldSetter(field, set_worldMinY_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_worldMinY_3, AssignFromStack_worldMinY_3);
            field = type.GetField("worldMaxY", flag);
            app.RegisterCLRFieldGetter(field, get_worldMaxY_4);
            app.RegisterCLRFieldSetter(field, set_worldMaxY_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_worldMaxY_4, AssignFromStack_worldMaxY_4);
            field = type.GetField("customMinDxf", flag);
            app.RegisterCLRFieldGetter(field, get_customMinDxf_5);
            app.RegisterCLRFieldSetter(field, set_customMinDxf_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_customMinDxf_5, AssignFromStack_customMinDxf_5);
            field = type.GetField("customMaxDxf", flag);
            app.RegisterCLRFieldGetter(field, get_customMaxDxf_6);
            app.RegisterCLRFieldSetter(field, set_customMaxDxf_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_customMaxDxf_6, AssignFromStack_customMaxDxf_6);
            field = type.GetField("INVALID_FLOAT_VALUE", flag);
            app.RegisterCLRFieldGetter(field, get_INVALID_FLOAT_VALUE_7);
            app.RegisterCLRFieldSetter(field, set_INVALID_FLOAT_VALUE_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_INVALID_FLOAT_VALUE_7, AssignFromStack_INVALID_FLOAT_VALUE_7);


        }


        static StackObject* GetCamera_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetCamera();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getCameraDxf_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @Id = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getCameraDxf(@Id);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* SetCameraDxf_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @interpolateTime = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @dxf = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetCameraDxf(@dxf, @interpolateTime, @callback);

            return __ret;
        }

        static StackObject* ViewTerrainPos_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @interpolateTime = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @terrainY = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @terrainX = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ViewTerrainPos(@terrainX, @terrainY, @interpolateTime, @callback);

            return __ret;
        }

        static StackObject* SetCanDrag_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isEnable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetCanDrag(@isEnable);

            return __ret;
        }

        static StackObject* SetCanZoom_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isEnable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetCanZoom(@isEnable);

            return __ret;
        }

        static StackObject* SetCanClick_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isEnable = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetCanClick(@isEnable);

            return __ret;
        }

        static StackObject* ClearViewChange_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ClearViewChange();

            return __ret;
        }

        static StackObject* GetViewCenter_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetViewCenter();

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* AddViewChange_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Single, System.Single, System.Single> @callback = (System.Action<System.Single, System.Single, System.Single>)typeof(System.Action<System.Single, System.Single, System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddViewChange(@callback);

            return __ret;
        }

        static StackObject* RemoveViewChange_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Single, System.Single, System.Single> @callback = (System.Action<System.Single, System.Single, System.Single>)typeof(System.Action<System.Single, System.Single, System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveViewChange(@callback);

            return __ret;
        }

        static StackObject* getCurrentCameraDxf_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getCurrentCameraDxf();

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* IsAutoMoving_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsAutoMoving();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsMovingToPos_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsMovingToPos();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsSlipping_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsSlipping();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_isMovingToPos_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isMovingToPos;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetTouchTerrainPos_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Camera @camera = (UnityEngine.Camera)typeof(UnityEngine.Camera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetTouchTerrainPos(@camera, @x, @y);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* CanDrag_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CanDrag();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* getCurrentCameraDist_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.getCurrentCameraDist();

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* AddMapClickListner_19(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Single, System.Single> @callback = (System.Action<System.Single, System.Single>)typeof(System.Action<System.Single, System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddMapClickListner(@callback);

            return __ret;
        }

        static StackObject* RemoveMapClickListner_20(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Single, System.Single> @callback = (System.Action<System.Single, System.Single>)typeof(System.Action<System.Single, System.Single>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveMapClickListner(@callback);

            return __ret;
        }

        static StackObject* ResetCamera_21(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<Client.WorldCamera.cameraInfoItem> @cameras = (System.Collections.Generic.List<Client.WorldCamera.cameraInfoItem>)typeof(System.Collections.Generic.List<Client.WorldCamera.cameraInfoItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetCamera(@cameras);

            return __ret;
        }

        static StackObject* setAdditionHeightForMinDxf_22(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.setAdditionHeightForMinDxf(@value);

            return __ret;
        }

        static StackObject* set_isMovingToPos_23(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.isMovingToPos = value;

            return __ret;
        }

        static StackObject* get_GetTerrainPlane_24(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetTerrainPlane;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* set_AllowTouchWhenMovingOrZooming_25(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.WorldCamera instance_of_this_method = (Client.WorldCamera)typeof(Client.WorldCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AllowTouchWhenMovingOrZooming = value;

            return __ret;
        }


        static object get_enableReboundXY_0(ref object o)
        {
            return ((Client.WorldCamera)o).enableReboundXY;
        }

        static StackObject* CopyToStack_enableReboundXY_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera)o).enableReboundXY;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_enableReboundXY_0(ref object o, object v)
        {
            ((Client.WorldCamera)o).enableReboundXY = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_enableReboundXY_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @enableReboundXY = ptr_of_this_method->Value == 1;
            ((Client.WorldCamera)o).enableReboundXY = @enableReboundXY;
            return ptr_of_this_method;
        }

        static object get_worldMinX_1(ref object o)
        {
            return ((Client.WorldCamera)o).worldMinX;
        }

        static StackObject* CopyToStack_worldMinX_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera)o).worldMinX;
            __ret->ObjectType = ObjectTypes.Double;
            *(double*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_worldMinX_1(ref object o, object v)
        {
            ((Client.WorldCamera)o).worldMinX = (System.Double)v;
        }

        static StackObject* AssignFromStack_worldMinX_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Double @worldMinX = *(double*)&ptr_of_this_method->Value;
            ((Client.WorldCamera)o).worldMinX = @worldMinX;
            return ptr_of_this_method;
        }

        static object get_worldMaxX_2(ref object o)
        {
            return ((Client.WorldCamera)o).worldMaxX;
        }

        static StackObject* CopyToStack_worldMaxX_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera)o).worldMaxX;
            __ret->ObjectType = ObjectTypes.Double;
            *(double*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_worldMaxX_2(ref object o, object v)
        {
            ((Client.WorldCamera)o).worldMaxX = (System.Double)v;
        }

        static StackObject* AssignFromStack_worldMaxX_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Double @worldMaxX = *(double*)&ptr_of_this_method->Value;
            ((Client.WorldCamera)o).worldMaxX = @worldMaxX;
            return ptr_of_this_method;
        }

        static object get_worldMinY_3(ref object o)
        {
            return ((Client.WorldCamera)o).worldMinY;
        }

        static StackObject* CopyToStack_worldMinY_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera)o).worldMinY;
            __ret->ObjectType = ObjectTypes.Double;
            *(double*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_worldMinY_3(ref object o, object v)
        {
            ((Client.WorldCamera)o).worldMinY = (System.Double)v;
        }

        static StackObject* AssignFromStack_worldMinY_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Double @worldMinY = *(double*)&ptr_of_this_method->Value;
            ((Client.WorldCamera)o).worldMinY = @worldMinY;
            return ptr_of_this_method;
        }

        static object get_worldMaxY_4(ref object o)
        {
            return ((Client.WorldCamera)o).worldMaxY;
        }

        static StackObject* CopyToStack_worldMaxY_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera)o).worldMaxY;
            __ret->ObjectType = ObjectTypes.Double;
            *(double*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_worldMaxY_4(ref object o, object v)
        {
            ((Client.WorldCamera)o).worldMaxY = (System.Double)v;
        }

        static StackObject* AssignFromStack_worldMaxY_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Double @worldMaxY = *(double*)&ptr_of_this_method->Value;
            ((Client.WorldCamera)o).worldMaxY = @worldMaxY;
            return ptr_of_this_method;
        }

        static object get_customMinDxf_5(ref object o)
        {
            return ((Client.WorldCamera)o).customMinDxf;
        }

        static StackObject* CopyToStack_customMinDxf_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera)o).customMinDxf;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_customMinDxf_5(ref object o, object v)
        {
            ((Client.WorldCamera)o).customMinDxf = (System.Single)v;
        }

        static StackObject* AssignFromStack_customMinDxf_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @customMinDxf = *(float*)&ptr_of_this_method->Value;
            ((Client.WorldCamera)o).customMinDxf = @customMinDxf;
            return ptr_of_this_method;
        }

        static object get_customMaxDxf_6(ref object o)
        {
            return ((Client.WorldCamera)o).customMaxDxf;
        }

        static StackObject* CopyToStack_customMaxDxf_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera)o).customMaxDxf;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_customMaxDxf_6(ref object o, object v)
        {
            ((Client.WorldCamera)o).customMaxDxf = (System.Single)v;
        }

        static StackObject* AssignFromStack_customMaxDxf_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @customMaxDxf = *(float*)&ptr_of_this_method->Value;
            ((Client.WorldCamera)o).customMaxDxf = @customMaxDxf;
            return ptr_of_this_method;
        }

        static object get_INVALID_FLOAT_VALUE_7(ref object o)
        {
            return Client.WorldCamera.INVALID_FLOAT_VALUE;
        }

        static StackObject* CopyToStack_INVALID_FLOAT_VALUE_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.WorldCamera.INVALID_FLOAT_VALUE;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_INVALID_FLOAT_VALUE_7(ref object o, object v)
        {
            Client.WorldCamera.INVALID_FLOAT_VALUE = (System.Single)v;
        }

        static StackObject* AssignFromStack_INVALID_FLOAT_VALUE_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @INVALID_FLOAT_VALUE = *(float*)&ptr_of_this_method->Value;
            Client.WorldCamera.INVALID_FLOAT_VALUE = @INVALID_FLOAT_VALUE;
            return ptr_of_this_method;
        }



    }
}
