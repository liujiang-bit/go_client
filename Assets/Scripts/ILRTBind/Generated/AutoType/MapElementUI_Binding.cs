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
    unsafe class MapElementUI_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::MapElementUI);
            args = new Type[]{typeof(System.Boolean), typeof(System.Single)};
            method = type.GetMethod("SetUIStatus", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetUIStatus_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetUIFadeShow", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetUIFadeShow_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetUIType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetUIType_2);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Single), typeof(System.Single), typeof(System.Single), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("SetBattlePos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetBattlePos_3);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("SetPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetPosition_4);
            args = new Type[]{};
            method = type.GetMethod("InitData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitData_5);


        }


        static StackObject* SetUIStatus_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @smooth_rate = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @in_battle = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::MapElementUI instance_of_this_method = (global::MapElementUI)typeof(global::MapElementUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetUIStatus(@in_battle, @smooth_rate);

            return __ret;
        }

        static StackObject* SetUIFadeShow_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @fade_type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::MapElementUI instance_of_this_method = (global::MapElementUI)typeof(global::MapElementUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetUIFadeShow(@fade_type);

            return __ret;
        }

        static StackObject* SetUIType_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @ui_type = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::MapElementUI instance_of_this_method = (global::MapElementUI)typeof(global::MapElementUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetUIType(@ui_type);

            return __ret;
        }

        static StackObject* SetBattlePos_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 8);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @stanceOffset = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @world_z = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @world_y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @world_x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Single @dir_z = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            System.Single @dir_y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            System.Single @dir_x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 8);
            global::MapElementUI instance_of_this_method = (global::MapElementUI)typeof(global::MapElementUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetBattlePos(@dir_x, @dir_y, @dir_z, @world_x, @world_y, @world_z, @stanceOffset);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* SetPosition_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @world_z = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @world_y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @world_x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::MapElementUI instance_of_this_method = (global::MapElementUI)typeof(global::MapElementUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetPosition(@world_x, @world_y, @world_z);

            return __ret;
        }

        static StackObject* InitData_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::MapElementUI instance_of_this_method = (global::MapElementUI)typeof(global::MapElementUI).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitData();

            return __ret;
        }



    }
}
