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
    unsafe class Client_Matrix_Prefab_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.Matrix_Prefab);

            field = type.GetField("id", flag);
            app.RegisterCLRFieldGetter(field, get_id_0);
            app.RegisterCLRFieldSetter(field, set_id_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_id_0, AssignFromStack_id_0);
            field = type.GetField("name", flag);
            app.RegisterCLRFieldGetter(field, get_name_1);
            app.RegisterCLRFieldSetter(field, set_name_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_name_1, AssignFromStack_name_1);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_id_0(ref object o)
        {
            return ((Client.Matrix_Prefab)o).id;
        }

        static StackObject* CopyToStack_id_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.Matrix_Prefab)o).id;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_id_0(ref object o, object v)
        {
            ((Client.Matrix_Prefab)o).id = (System.Int32)v;
        }

        static StackObject* AssignFromStack_id_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @id = ptr_of_this_method->Value;
            ((Client.Matrix_Prefab)o).id = @id;
            return ptr_of_this_method;
        }

        static object get_name_1(ref object o)
        {
            return ((Client.Matrix_Prefab)o).name;
        }

        static StackObject* CopyToStack_name_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.Matrix_Prefab)o).name;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_name_1(ref object o, object v)
        {
            ((Client.Matrix_Prefab)o).name = (System.String)v;
        }

        static StackObject* AssignFromStack_name_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.Matrix_Prefab)o).name = @name;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Client.Matrix_Prefab();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
