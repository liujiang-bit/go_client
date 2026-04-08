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
    unsafe class Skyunion_GameView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.GameView);

            field = type.GetField("gameObject", flag);
            app.RegisterCLRFieldGetter(field, get_gameObject_0);
            app.RegisterCLRFieldSetter(field, set_gameObject_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_gameObject_0, AssignFromStack_gameObject_0);
            field = type.GetField("vb", flag);
            app.RegisterCLRFieldGetter(field, get_vb_1);
            app.RegisterCLRFieldSetter(field, set_vb_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_vb_1, AssignFromStack_vb_1);
            field = type.GetField("data", flag);
            app.RegisterCLRFieldGetter(field, get_data_2);
            app.RegisterCLRFieldSetter(field, set_data_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_data_2, AssignFromStack_data_2);
            field = type.GetField("IsAllowClickMaskClose", flag);
            app.RegisterCLRFieldGetter(field, get_IsAllowClickMaskClose_3);
            app.RegisterCLRFieldSetter(field, set_IsAllowClickMaskClose_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsAllowClickMaskClose_3, AssignFromStack_IsAllowClickMaskClose_3);


        }



        static object get_gameObject_0(ref object o)
        {
            return ((Skyunion.GameView)o).gameObject;
        }

        static StackObject* CopyToStack_gameObject_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.GameView)o).gameObject;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_gameObject_0(ref object o, object v)
        {
            ((Skyunion.GameView)o).gameObject = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_gameObject_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @gameObject = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.GameView)o).gameObject = @gameObject;
            return ptr_of_this_method;
        }

        static object get_vb_1(ref object o)
        {
            return ((Skyunion.GameView)o).vb;
        }

        static StackObject* CopyToStack_vb_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.GameView)o).vb;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_vb_1(ref object o, object v)
        {
            ((Skyunion.GameView)o).vb = (Skyunion.ViewBinder)v;
        }

        static StackObject* AssignFromStack_vb_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Skyunion.ViewBinder @vb = (Skyunion.ViewBinder)typeof(Skyunion.ViewBinder).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.GameView)o).vb = @vb;
            return ptr_of_this_method;
        }

        static object get_data_2(ref object o)
        {
            return ((Skyunion.GameView)o).data;
        }

        static StackObject* CopyToStack_data_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.GameView)o).data;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static void set_data_2(ref object o, object v)
        {
            ((Skyunion.GameView)o).data = (System.Object)v;
        }

        static StackObject* AssignFromStack_data_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Object @data = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.GameView)o).data = @data;
            return ptr_of_this_method;
        }

        static object get_IsAllowClickMaskClose_3(ref object o)
        {
            return ((Skyunion.GameView)o).IsAllowClickMaskClose;
        }

        static StackObject* CopyToStack_IsAllowClickMaskClose_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.GameView)o).IsAllowClickMaskClose;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsAllowClickMaskClose_3(ref object o, object v)
        {
            ((Skyunion.GameView)o).IsAllowClickMaskClose = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsAllowClickMaskClose_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsAllowClickMaskClose = ptr_of_this_method->Value == 1;
            ((Skyunion.GameView)o).IsAllowClickMaskClose = @IsAllowClickMaskClose;
            return ptr_of_this_method;
        }



    }
}
