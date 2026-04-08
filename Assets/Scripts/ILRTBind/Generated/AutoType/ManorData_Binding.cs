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
    unsafe class ManorData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::ManorData);

            field = type.GetField("type", flag);
            app.RegisterCLRFieldGetter(field, get_type_0);
            app.RegisterCLRFieldSetter(field, set_type_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_type_0, AssignFromStack_type_0);
            field = type.GetField("width", flag);
            app.RegisterCLRFieldGetter(field, get_width_1);
            app.RegisterCLRFieldSetter(field, set_width_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_width_1, AssignFromStack_width_1);
            field = type.GetField("uvStep", flag);
            app.RegisterCLRFieldGetter(field, get_uvStep_2);
            app.RegisterCLRFieldSetter(field, set_uvStep_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_uvStep_2, AssignFromStack_uvStep_2);
            field = type.GetField("list", flag);
            app.RegisterCLRFieldGetter(field, get_list_3);
            app.RegisterCLRFieldSetter(field, set_list_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_list_3, AssignFromStack_list_3);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_type_0(ref object o)
        {
            return ((global::ManorData)o).type;
        }

        static StackObject* CopyToStack_type_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ManorData)o).type;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_type_0(ref object o, object v)
        {
            ((global::ManorData)o).type = (System.String)v;
        }

        static StackObject* AssignFromStack_type_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @type = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::ManorData)o).type = @type;
            return ptr_of_this_method;
        }

        static object get_width_1(ref object o)
        {
            return ((global::ManorData)o).width;
        }

        static StackObject* CopyToStack_width_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ManorData)o).width;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_width_1(ref object o, object v)
        {
            ((global::ManorData)o).width = (System.Single)v;
        }

        static StackObject* AssignFromStack_width_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @width = *(float*)&ptr_of_this_method->Value;
            ((global::ManorData)o).width = @width;
            return ptr_of_this_method;
        }

        static object get_uvStep_2(ref object o)
        {
            return ((global::ManorData)o).uvStep;
        }

        static StackObject* CopyToStack_uvStep_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ManorData)o).uvStep;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_uvStep_2(ref object o, object v)
        {
            ((global::ManorData)o).uvStep = (System.Single)v;
        }

        static StackObject* AssignFromStack_uvStep_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @uvStep = *(float*)&ptr_of_this_method->Value;
            ((global::ManorData)o).uvStep = @uvStep;
            return ptr_of_this_method;
        }

        static object get_list_3(ref object o)
        {
            return ((global::ManorData)o).list;
        }

        static StackObject* CopyToStack_list_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::ManorData)o).list;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_list_3(ref object o, object v)
        {
            ((global::ManorData)o).list = (System.Collections.Generic.List<global::ManorItem>)v;
        }

        static StackObject* AssignFromStack_list_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::ManorItem> @list = (System.Collections.Generic.List<global::ManorItem>)typeof(System.Collections.Generic.List<global::ManorItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::ManorData)o).list = @list;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new global::ManorData();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
