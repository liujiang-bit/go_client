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
    unsafe class People_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::People);
            args = new Type[]{typeof(global::People), typeof(UnityEngine.GameObject), typeof(UnityEngine.Color)};
            method = type.GetMethod("InitCitizenS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitCitizenS_0);
            args = new Type[]{typeof(global::People), typeof(System.Boolean)};
            method = type.GetMethod("SetUnitFootprintsActiveS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetUnitFootprintsActiveS_1);
            args = new Type[]{typeof(global::People)};
            method = type.GetMethod("ResetUnitPosS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetUnitPosS_2);
            args = new Type[]{};
            method = type.GetMethod("FadeOut", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FadeOut_3);
            args = new Type[]{typeof(global::People), typeof(global::People.ENMU_CITIZEN_STAT), typeof(UnityEngine.Vector2), typeof(UnityEngine.Vector2), typeof(System.Single), typeof(global::People.ENMU_CARRY_RESOURCE_TYPE)};
            method = type.GetMethod("SetStateS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStateS_4);

            field = type.GetField("WorldPaths", flag);
            app.RegisterCLRFieldGetter(field, get_WorldPaths_0);
            app.RegisterCLRFieldSetter(field, set_WorldPaths_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_WorldPaths_0, AssignFromStack_WorldPaths_0);

            app.RegisterCLRCreateArrayInstance(type, s => new global::People[s]);


        }


        static StackObject* InitCitizenS_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @unit_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @unit_path = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::People @self = (global::People)typeof(global::People).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::People.InitCitizenS(@self, @unit_path, @unit_color);

            return __ret;
        }

        static StackObject* SetUnitFootprintsActiveS_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @active = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::People @self = (global::People)typeof(global::People).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::People.SetUnitFootprintsActiveS(@self, @active);

            return __ret;
        }

        static StackObject* ResetUnitPosS_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::People @self = (global::People)typeof(global::People).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::People.ResetUnitPosS(@self);

            return __ret;
        }

        static StackObject* FadeOut_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::People instance_of_this_method = (global::People)typeof(global::People).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.FadeOut();

            return __ret;
        }

        static StackObject* SetStateS_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::People.ENMU_CARRY_RESOURCE_TYPE @carry_resource_type = (global::People.ENMU_CARRY_RESOURCE_TYPE)typeof(global::People.ENMU_CARRY_RESOURCE_TYPE).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @move_speed = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Vector2 @target_pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @target_pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @target_pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Vector2 @current_pos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @current_pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @current_pos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::People.ENMU_CITIZEN_STAT @state = (global::People.ENMU_CITIZEN_STAT)typeof(global::People.ENMU_CITIZEN_STAT).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            global::People @self = (global::People)typeof(global::People).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::People.SetStateS(@self, @state, @current_pos, @target_pos, @move_speed, @carry_resource_type);

            return __ret;
        }


        static object get_WorldPaths_0(ref object o)
        {
            return ((global::People)o).WorldPaths;
        }

        static StackObject* CopyToStack_WorldPaths_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::People)o).WorldPaths;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_WorldPaths_0(ref object o, object v)
        {
            ((global::People)o).WorldPaths = (System.Collections.Generic.List<UnityEngine.Vector2>)v;
        }

        static StackObject* AssignFromStack_WorldPaths_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<UnityEngine.Vector2> @WorldPaths = (System.Collections.Generic.List<UnityEngine.Vector2>)typeof(System.Collections.Generic.List<UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::People)o).WorldPaths = @WorldPaths;
            return ptr_of_this_method;
        }



    }
}
