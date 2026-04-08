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
    unsafe class ManorLod3Data_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ManorLod3Data);

            field = type.GetField("points", flag);
            app.RegisterCLRFieldGetter(field, get_points_0);
            app.RegisterCLRFieldSetter(field, set_points_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_points_0, AssignFromStack_points_0);
            field = type.GetField("color", flag);
            app.RegisterCLRFieldGetter(field, get_color_1);
            app.RegisterCLRFieldSetter(field, set_color_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_color_1, AssignFromStack_color_1);
            field = type.GetField("dir", flag);
            app.RegisterCLRFieldGetter(field, get_dir_2);
            app.RegisterCLRFieldSetter(field, set_dir_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_dir_2, AssignFromStack_dir_2);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_points_0(ref object o)
        {
            return ((global::ManorLod3Data)o).points;
        }

        static StackObject* CopyToStack_points_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ManorLod3Data)o).points;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_points_0(ref object o, object v)
        {
            ((global::ManorLod3Data)o).points = (UnityEngine.Vector2[])v;
        }

        static StackObject* AssignFromStack_points_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector2[] @points = (UnityEngine.Vector2[])typeof(UnityEngine.Vector2[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::ManorLod3Data)o).points = @points;
            return ptr_of_this_method;
        }

        static object get_color_1(ref object o)
        {
            return ((global::ManorLod3Data)o).color;
        }

        static StackObject* CopyToStack_color_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ManorLod3Data)o).color;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_color_1(ref object o, object v)
        {
            ((global::ManorLod3Data)o).color = (UnityEngine.Color)v;
        }

        static StackObject* AssignFromStack_color_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Color @color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::ManorLod3Data)o).color = @color;
            return ptr_of_this_method;
        }

        static object get_dir_2(ref object o)
        {
            return ((global::ManorLod3Data)o).dir;
        }

        static StackObject* CopyToStack_dir_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ManorLod3Data)o).dir;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_dir_2(ref object o, object v)
        {
            ((global::ManorLod3Data)o).dir = (System.Byte)v;
        }

        static StackObject* AssignFromStack_dir_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Byte @dir = (byte)ptr_of_this_method->Value;
            ((global::ManorLod3Data)o).dir = @dir;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new global::ManorLod3Data();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
