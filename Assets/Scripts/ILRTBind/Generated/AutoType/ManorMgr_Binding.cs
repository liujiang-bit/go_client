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
    unsafe class ManorMgr_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::ManorMgr);
            args = new Type[]{typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("ClearAllLine_S", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearAllLine_S_0);
            args = new Type[]{typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetMethod("SetStrategicShow_S", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStrategicShow_S_1);
            args = new Type[]{typeof(System.Collections.Generic.List<global::ManorData>), typeof(System.Collections.Generic.List<global::ManorItem>)};
            method = type.GetMethod("UpdateFakeTerritoryS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateFakeTerritoryS_2);
            args = new Type[]{typeof(System.Collections.Generic.List<global::ManorData>)};
            method = type.GetMethod("UpdateTerritoryS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateTerritoryS_3);
            args = new Type[]{};
            method = type.GetMethod("get_manorStrategicLineRoot", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_manorStrategicLineRoot_4);
            args = new Type[]{typeof(UnityEngine.Vector2[]), typeof(System.Single), typeof(System.Single), typeof(UnityEngine.Color), typeof(System.Byte)};
            method = type.GetMethod("CreateLineFromCache_S", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CreateLineFromCache_S_5);


        }


        static StackObject* ClearAllLine_S_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @clearStrategic = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @clearTactical = ptr_of_this_method->Value == 1;


            global::ManorMgr.ClearAllLine_S(@clearTactical, @clearStrategic);

            return __ret;
        }

        static StackObject* SetStrategicShow_S_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isShowLow = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isShowHight = ptr_of_this_method->Value == 1;


            global::ManorMgr.SetStrategicShow_S(@isShowHight, @isShowLow);

            return __ret;
        }

        static StackObject* UpdateFakeTerritoryS_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<global::ManorItem> @fake_manor_item_table = (System.Collections.Generic.List<global::ManorItem>)typeof(System.Collections.Generic.List<global::ManorItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<global::ManorData> @manorInfos = (System.Collections.Generic.List<global::ManorData>)typeof(System.Collections.Generic.List<global::ManorData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::ManorMgr.UpdateFakeTerritoryS(@manorInfos, @fake_manor_item_table);

            return __ret;
        }

        static StackObject* UpdateTerritoryS_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<global::ManorData> @manorInfos = (System.Collections.Generic.List<global::ManorData>)typeof(System.Collections.Generic.List<global::ManorData>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::ManorMgr.UpdateTerritoryS(@manorInfos);

            return __ret;
        }

        static StackObject* get_manorStrategicLineRoot_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::ManorMgr.manorStrategicLineRoot;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CreateLineFromCache_S_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Byte @dir = (byte)ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Color @line_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @width = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @uvStep = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.Vector2[] @vector2_array = (UnityEngine.Vector2[])typeof(UnityEngine.Vector2[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::ManorMgr.CreateLineFromCache_S(@vector2_array, @uvStep, @width, @line_color, @dir);

            return __ret;
        }



    }
}
