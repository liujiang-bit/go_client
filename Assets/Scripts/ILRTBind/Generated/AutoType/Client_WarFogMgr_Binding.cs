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
    unsafe class Client_WarFogMgr_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.WarFogMgr);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("RemoveFadeGroupByType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveFadeGroupByType_0);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("CreateFadeGroup", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CreateFadeGroup_1);
            args = new Type[]{typeof(System.Int32), typeof(System.Int64[]), typeof(UnityEngine.Transform)};
            method = type.GetMethod("InitFogSystem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitFogSystem_2);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("HasFogAt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, HasFogAt_3);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("OpenFog", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OpenFog_4);
            args = new Type[]{typeof(UnityEngine.Vector2Int[])};
            method = type.GetMethod("CrateFadeFog", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CrateFadeFog_5);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("CloseFog", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CloseFog_6);
            args = new Type[]{typeof(System.Int32), typeof(System.Byte[]), typeof(UnityEngine.Transform)};
            method = type.GetMethod("InitFogSystem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitFogSystem_7);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("GetGroupIdByTile", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetGroupIdByTile_8);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("BuildConnections", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, BuildConnections_9);
            args = new Type[]{};
            method = type.GetMethod("IsAllFogClear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsAllFogClear_10);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("CanExploreTile", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CanExploreTile_11);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Collections.Generic.Dictionary<System.Int32, System.Boolean>)};
            method = type.GetMethod("FindFogClosestAt", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FindFogClosestAt_12);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("RemoveTempOpenFog", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveTempOpenFog_13);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("AddTempOpenFog", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddTempOpenFog_14);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ChangeLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ChangeLevel_15);
            args = new Type[]{};
            method = type.GetMethod("IsAllFogOpen", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsAllFogOpen_16);
            args = new Type[]{typeof(System.Int32), typeof(System.Single), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("FindGroupForUseItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FindGroupForUseItem_17);

            field = type.GetField("GROUP_SIZE", flag);
            app.RegisterCLRFieldGetter(field, get_GROUP_SIZE_0);
            app.RegisterCLRFieldSetter(field, set_GROUP_SIZE_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_GROUP_SIZE_0, AssignFromStack_GROUP_SIZE_0);
            field = type.GetField("FogNumber", flag);
            app.RegisterCLRFieldGetter(field, get_FogNumber_1);
            app.RegisterCLRFieldSetter(field, set_FogNumber_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_FogNumber_1, AssignFromStack_FogNumber_1);


        }


        static StackObject* RemoveFadeGroupByType_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @type = ptr_of_this_method->Value;


            Client.WarFogMgr.RemoveFadeGroupByType(@type);

            return __ret;
        }

        static StackObject* CreateFadeGroup_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @groupSize = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @id = ptr_of_this_method->Value;


            var result_of_this_method = Client.WarFogMgr.CreateFadeGroup(@id, @type, @groupSize);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* InitFogSystem_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @lowLevelTrans = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64[] @unlockedData = (System.Int64[])typeof(System.Int64[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @mapSize = ptr_of_this_method->Value;


            Client.WarFogMgr.InitFogSystem(@mapSize, @unlockedData, @lowLevelTrans);

            return __ret;
        }

        static StackObject* HasFogAt_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @withTemp = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @x = ptr_of_this_method->Value;


            var result_of_this_method = Client.WarFogMgr.HasFogAt(@x, @y, @withTemp);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* OpenFog_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;


            Client.WarFogMgr.OpenFog(@x, @y);

            return __ret;
        }

        static StackObject* CrateFadeFog_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2Int[] @tiles = (UnityEngine.Vector2Int[])typeof(UnityEngine.Vector2Int[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.WarFogMgr.CrateFadeFog(@tiles);

            return __ret;
        }

        static StackObject* CloseFog_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;


            Client.WarFogMgr.CloseFog(@x, @y);

            return __ret;
        }

        static StackObject* InitFogSystem_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @lowLevelTrans = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Byte[] @unlockedData = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @mapSize = ptr_of_this_method->Value;


            Client.WarFogMgr.InitFogSystem(@mapSize, @unlockedData, @lowLevelTrans);

            return __ret;
        }

        static StackObject* GetGroupIdByTile_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;


            var result_of_this_method = Client.WarFogMgr.GetGroupIdByTile(@x, @y);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* BuildConnections_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @clear = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @x = ptr_of_this_method->Value;


            Client.WarFogMgr.BuildConnections(@x, @y, @clear);

            return __ret;
        }

        static StackObject* IsAllFogClear_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.WarFogMgr.IsAllFogClear();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* CanExploreTile_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @IsJumpHasFog = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @x = ptr_of_this_method->Value;


            var result_of_this_method = Client.WarFogMgr.CanExploreTile(@x, @y, @IsJumpHasFog);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* FindFogClosestAt_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.Dictionary<System.Int32, System.Boolean> @ignoreGroupDic = (System.Collections.Generic.Dictionary<System.Int32, System.Boolean>)typeof(System.Collections.Generic.Dictionary<System.Int32, System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @x = ptr_of_this_method->Value;


            var result_of_this_method = Client.WarFogMgr.FindFogClosestAt(@x, @y, @ignoreGroupDic);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* RemoveTempOpenFog_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;


            Client.WarFogMgr.RemoveTempOpenFog(@x, @y);

            return __ret;
        }

        static StackObject* AddTempOpenFog_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;


            var result_of_this_method = Client.WarFogMgr.AddTempOpenFog(@x, @y);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* ChangeLevel_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @level = ptr_of_this_method->Value;


            Client.WarFogMgr.ChangeLevel(@level);

            return __ret;
        }

        static StackObject* IsAllFogOpen_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.WarFogMgr.IsAllFogOpen();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* FindGroupForUseItem_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @x = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @ratio = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @groupSize = ptr_of_this_method->Value;


            var result_of_this_method = Client.WarFogMgr.FindGroupForUseItem(@groupSize, @ratio, @x, @y);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }


        static object get_GROUP_SIZE_0(ref object o)
        {
            return Client.WarFogMgr.GROUP_SIZE;
        }

        static StackObject* CopyToStack_GROUP_SIZE_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.WarFogMgr.GROUP_SIZE;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_GROUP_SIZE_0(ref object o, object v)
        {
            Client.WarFogMgr.GROUP_SIZE = (System.Int32)v;
        }

        static StackObject* AssignFromStack_GROUP_SIZE_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @GROUP_SIZE = ptr_of_this_method->Value;
            Client.WarFogMgr.GROUP_SIZE = @GROUP_SIZE;
            return ptr_of_this_method;
        }

        static object get_FogNumber_1(ref object o)
        {
            return Client.WarFogMgr.FogNumber;
        }

        static StackObject* CopyToStack_FogNumber_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.WarFogMgr.FogNumber;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_FogNumber_1(ref object o, object v)
        {
            Client.WarFogMgr.FogNumber = (System.Int32)v;
        }

        static StackObject* AssignFromStack_FogNumber_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @FogNumber = ptr_of_this_method->Value;
            Client.WarFogMgr.FogNumber = @FogNumber;
            return ptr_of_this_method;
        }



    }
}
