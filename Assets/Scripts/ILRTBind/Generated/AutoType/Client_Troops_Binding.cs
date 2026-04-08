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
    unsafe class Client_Troops_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.Troops);
            args = new Type[]{};
            method = type.GetMethod("GetFormationType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetFormationType_0);
            args = new Type[]{typeof(Client.Troops.ENMU_SQUARE_STAT), typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector2), typeof(System.Single)};
            method = type.GetMethod("SetState", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetState_1);
            args = new Type[]{typeof(Client.Troops), typeof(System.String)};
            method = type.GetMethod("ReservedFunc1S", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ReservedFunc1S_2);
            args = new Type[]{typeof(Client.Troops), typeof(System.String)};
            method = type.GetMethod("SetFormationInfoS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetFormationInfoS_3);
            args = new Type[]{typeof(Client.Troops), typeof(System.String), typeof(UnityEngine.Vector3)};
            method = type.GetMethod("TriggerSkillS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TriggerSkillS_4);
            args = new Type[]{typeof(Client.Troops)};
            method = type.GetMethod("FadeOut_S", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FadeOut_S_5);
            args = new Type[]{typeof(Client.Troops), typeof(UnityEngine.Color)};
            method = type.GetMethod("ChangeUnitColorS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ChangeUnitColorS_6);
            args = new Type[]{typeof(Client.Troops), typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector2)};
            method = type.GetMethod("InitPositionS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitPositionS_7);
            args = new Type[]{typeof(Client.Troops), typeof(System.Single)};
            method = type.GetMethod("SetRadiusS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetRadiusS_8);
            args = new Type[]{typeof(Client.Troops), typeof(System.String), typeof(UnityEngine.Color)};
            method = type.GetMethod("InitFormationS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitFormationS_9);
            args = new Type[]{typeof(Client.Troops)};
            method = type.GetMethod("FadeIn_S", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FadeIn_S_10);
            args = new Type[]{typeof(Client.Troops), typeof(System.String)};
            method = type.GetMethod("SetHeroLevelUpEffectiveS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetHeroLevelUpEffectiveS_11);
            args = new Type[]{typeof(Client.Troops), typeof(Client.Troops.ENMU_SQUARE_STAT), typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector2), typeof(System.Single)};
            method = type.GetMethod("SetStateS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStateS_12);
            args = new Type[]{typeof(Client.Troops)};
            method = type.GetMethod("GetShowPositionS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetShowPositionS_13);
            args = new Type[]{typeof(Client.Troops), typeof(Client.Troops.ENUM_FORMATION_CAMP)};
            method = type.GetMethod("SetCampS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCampS_14);


        }


        static StackObject* GetFormationType_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Troops instance_of_this_method = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetFormationType();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetState_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
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
            Client.Troops instance_of_this_method = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetState(@state, @current_pos, @target_pos, @move_speed);

            return __ret;
        }

        static StackObject* ReservedFunc1S_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @param = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.ReservedFunc1S(@self, @param);

            return __ret;
        }

        static StackObject* SetFormationInfoS_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @formation_info = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.SetFormationInfoS(@self, @formation_info);

            return __ret;
        }

        static StackObject* TriggerSkillS_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @pos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @param = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.TriggerSkillS(@self, @param, @pos);

            return __ret;
        }

        static StackObject* FadeOut_S_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.FadeOut_S(@self);

            return __ret;
        }

        static StackObject* ChangeUnitColorS_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @unit_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.ChangeUnitColorS(@self, @unit_color);

            return __ret;
        }

        static StackObject* InitPositionS_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
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
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.InitPositionS(@self, @current_pos, @target_pos);

            return __ret;
        }

        static StackObject* SetRadiusS_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @r = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.SetRadiusS(@self, @r);

            return __ret;
        }

        static StackObject* InitFormationS_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
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
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.InitFormationS(@self, @formation_info, @unit_color);

            return __ret;
        }

        static StackObject* FadeIn_S_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.FadeIn_S(@self);

            return __ret;
        }

        static StackObject* SetHeroLevelUpEffectiveS_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @effectName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.SetHeroLevelUpEffectiveS(@self, @effectName);

            return __ret;
        }

        static StackObject* SetStateS_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
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
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.SetStateS(@self, @state, @current_pos, @target_pos, @move_speed);

            return __ret;
        }

        static StackObject* GetShowPositionS_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.Troops.GetShowPositionS(@self);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* SetCampS_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.Troops.ENUM_FORMATION_CAMP @camp = (Client.Troops.ENUM_FORMATION_CAMP)typeof(Client.Troops.ENUM_FORMATION_CAMP).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.Troops @self = (Client.Troops)typeof(Client.Troops).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.Troops.SetCampS(@self, @camp);

            return __ret;
        }



    }
}
