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
    unsafe class Client_MapManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.MapManager);
            args = new Type[]{typeof(UnityEngine.Vector2)};
            method = type.GetMethod("GetMapProvinceName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetMapProvinceName_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetMapWidth", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetMapWidth_1);
            args = new Type[]{typeof(System.String), typeof(System.Int32), typeof(System.Int32), typeof(System.Action)};
            method = type.GetMethod("ReadMapBriefDataFromFile2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ReadMapBriefDataFromFile2_2);
            args = new Type[]{typeof(System.String), typeof(System.Action)};
            method = type.GetMethod("ReadMapDataFromFile2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ReadMapDataFromFile2_3);
            args = new Type[]{};
            method = type.GetMethod("ClearMapTileBrief", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearMapTileBrief_4);
            args = new Type[]{};
            method = type.GetMethod("GetMapProvinceNameStruct", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetMapProvinceNameStruct_5);
            args = new Type[]{};
            method = type.GetMethod("Update", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Update_6);
            args = new Type[]{typeof(System.String), typeof(System.Single), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("AddCity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddCity_7);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("RemoveCity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveCity_8);
            args = new Type[]{};
            method = type.GetMethod("ClearCiity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearCiity_9);
            args = new Type[]{typeof(UnityEngine.Vector2)};
            method = type.GetMethod("GetMapZoneLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetMapZoneLevel_10);
            args = new Type[]{typeof(UnityEngine.Vector2)};
            method = type.GetMethod("GetMapZone", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetMapZone_11);


        }


        static StackObject* GetMapProvinceName_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetMapProvinceName(@pos);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetMapWidth_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @width = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetMapWidth(@width);

            return __ret;
        }

        static StackObject* ReadMapBriefDataFromFile2_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @server_y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @server_x = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @map_data_path = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ReadMapBriefDataFromFile2(@map_data_path, @server_x, @server_y, @action);

            return __ret;
        }

        static StackObject* ReadMapDataFromFile2_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @tile_data_path = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ReadMapDataFromFile2(@tile_data_path, @action);

            return __ret;
        }

        static StackObject* ClearMapTileBrief_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ClearMapTileBrief();

            return __ret;
        }

        static StackObject* GetMapProvinceNameStruct_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetMapProvinceNameStruct();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Update_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Update();

            return __ret;
        }

        static StackObject* AddCity_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @remove_grove_distance = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @city_pos_y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @city_pos_x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @city_id = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddCity(@city_id, @city_pos_x, @city_pos_y, @remove_grove_distance);

            return __ret;
        }

        static StackObject* RemoveCity_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @city_id = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveCity(@city_id);

            return __ret;
        }

        static StackObject* ClearCiity_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ClearCiity();

            return __ret;
        }

        static StackObject* GetMapZoneLevel_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetMapZoneLevel(@pos);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetMapZone_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.MapManager instance_of_this_method = (Client.MapManager)typeof(Client.MapManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetMapZone(@pos);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }



    }
}
