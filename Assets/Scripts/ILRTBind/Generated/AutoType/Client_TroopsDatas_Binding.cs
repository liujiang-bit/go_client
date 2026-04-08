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
    unsafe class Client_TroopsDatas_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.TroopsDatas);
            args = new Type[]{};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_0);
            args = new Type[]{};
            method = type.GetMethod("GetUnitPrefabName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetUnitPrefabName_1);
            args = new Type[]{typeof(Client.Matrix_Prefab)};
            method = type.GetMethod("InitPrefabs", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitPrefabs_2);
            args = new Type[]{typeof(Client.Troops.ENMU_MATRIX_TYPE), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("InitCfgRowData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitCfgRowData_3);
            args = new Type[]{typeof(Client.Troops.ENMU_MATRIX_TYPE), typeof(System.Int32), typeof(System.Single)};
            method = type.GetMethod("InitCfgRowWidthData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitCfgRowWidthData_4);
            args = new Type[]{typeof(Client.Troops.ENMU_MATRIX_TYPE), typeof(System.Int32), typeof(System.Single)};
            method = type.GetMethod("InitCfgForwardSpacingData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitCfgForwardSpacingData_5);
            args = new Type[]{typeof(Client.Troops.ENMU_MATRIX_TYPE), typeof(System.Int32), typeof(System.Single)};
            method = type.GetMethod("InitCfgBackwardSpacingData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitCfgBackwardSpacingData_6);
            args = new Type[]{typeof(Client.Troops.ENMU_MATRIX_TYPE), typeof(System.Int32), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("InitSquareOffset", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitSquareOffset_7);
            args = new Type[]{typeof(Client.Troops.ENMU_MATRIX_TYPE), typeof(System.Int32), typeof(System.Int32), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("InitNumberBySumData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitNumberBySumData_8);


        }


        static StackObject* Clear_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Clear();

            return __ret;
        }

        static StackObject* GetUnitPrefabName_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetUnitPrefabName();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* InitPrefabs_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Matrix_Prefab @prefabs = (Client.Matrix_Prefab)typeof(Client.Matrix_Prefab).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitPrefabs(@prefabs);

            return __ret;
        }

        static StackObject* InitCfgRowData_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @num = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.Troops.ENMU_MATRIX_TYPE @group = (Client.Troops.ENMU_MATRIX_TYPE)typeof(Client.Troops.ENMU_MATRIX_TYPE).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitCfgRowData(@group, @type, @num);

            return __ret;
        }

        static StackObject* InitCfgRowWidthData_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @num = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.Troops.ENMU_MATRIX_TYPE @group = (Client.Troops.ENMU_MATRIX_TYPE)typeof(Client.Troops.ENMU_MATRIX_TYPE).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitCfgRowWidthData(@group, @type, @num);

            return __ret;
        }

        static StackObject* InitCfgForwardSpacingData_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @num = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.Troops.ENMU_MATRIX_TYPE @group = (Client.Troops.ENMU_MATRIX_TYPE)typeof(Client.Troops.ENMU_MATRIX_TYPE).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitCfgForwardSpacingData(@group, @type, @num);

            return __ret;
        }

        static StackObject* InitCfgBackwardSpacingData_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @num = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.Troops.ENMU_MATRIX_TYPE @group = (Client.Troops.ENMU_MATRIX_TYPE)typeof(Client.Troops.ENMU_MATRIX_TYPE).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitCfgBackwardSpacingData(@group, @type, @num);

            return __ret;
        }

        static StackObject* InitSquareOffset_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.Troops.ENMU_MATRIX_TYPE @group = (Client.Troops.ENMU_MATRIX_TYPE)typeof(Client.Troops.ENMU_MATRIX_TYPE).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitSquareOffset(@group, @type, @x, @y);

            return __ret;
        }

        static StackObject* InitNumberBySumData_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @num = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @rangeMax = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @range = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.Troops.ENMU_MATRIX_TYPE @group = (Client.Troops.ENMU_MATRIX_TYPE)typeof(Client.Troops.ENMU_MATRIX_TYPE).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            Client.TroopsDatas instance_of_this_method = (Client.TroopsDatas)typeof(Client.TroopsDatas).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitNumberBySumData(@group, @type, @range, @rangeMax, @num);

            return __ret;
        }



    }
}
