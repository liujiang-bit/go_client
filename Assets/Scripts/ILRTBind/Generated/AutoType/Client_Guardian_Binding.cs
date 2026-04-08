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
    unsafe class Client_Guardian_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.Guardian);
            args = new Type[]{typeof(Client.Guardian), typeof(Client.Troops.ENMU_SQUARE_STAT), typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector2), typeof(System.Single)};
            method = type.GetMethod("SetStateS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStateS_0);
            args = new Type[]{typeof(Client.Guardian)};
            method = type.GetMethod("FadeOut_S", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FadeOut_S_1);
            args = new Type[]{typeof(Client.Guardian), typeof(System.String)};
            method = type.GetMethod("TriggerSkillS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TriggerSkillS_2);
            args = new Type[]{typeof(Client.Guardian), typeof(System.String), typeof(UnityEngine.Color)};
            method = type.GetMethod("InitFormationS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitFormationS_3);
            args = new Type[]{typeof(Client.Guardian), typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector2)};
            method = type.GetMethod("InitPositionS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitPositionS_4);
            args = new Type[]{typeof(Client.Guardian), typeof(Client.Troops.ENUM_FORMATION_CAMP)};
            method = type.GetMethod("SetCampS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCampS_5);


        }


        static StackObject* SetStateS_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @move_speed = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector2 @target_pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @target_pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @target_pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Vector2 @current_pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @current_pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @current_pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Client.Troops.ENMU_SQUARE_STAT @state = (Client.Troops.ENMU_SQUARE_STAT)typeof(Client.Troops.ENMU_SQUARE_STAT).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.Guardian @self = (Client.Guardian)typeof(Client.Guardian).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Guardian.SetStateS(@self, @state, @current_pos, @target_pos, @move_speed);

            return __ret;
        }

        static StackObject* FadeOut_S_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Guardian @self = (Client.Guardian)typeof(Client.Guardian).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Guardian.FadeOut_S(@self);

            return __ret;
        }

        static StackObject* TriggerSkillS_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @param = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Guardian @self = (Client.Guardian)typeof(Client.Guardian).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Guardian.TriggerSkillS(@self, @param);

            return __ret;
        }

        static StackObject* InitFormationS_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @unit_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @formation_info = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.Guardian @self = (Client.Guardian)typeof(Client.Guardian).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Guardian.InitFormationS(@self, @formation_info, @unit_color);

            return __ret;
        }

        static StackObject* InitPositionS_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @target_pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @target_pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @target_pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector2 @current_pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @current_pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @current_pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.Guardian @self = (Client.Guardian)typeof(Client.Guardian).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Guardian.InitPositionS(@self, @current_pos, @target_pos);

            return __ret;
        }

        static StackObject* SetCampS_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Troops.ENUM_FORMATION_CAMP @camp = (Client.Troops.ENUM_FORMATION_CAMP)typeof(Client.Troops.ENUM_FORMATION_CAMP).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Guardian @self = (Client.Guardian)typeof(Client.Guardian).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Guardian.SetCampS(@self, @camp);

            return __ret;
        }



    }
}
