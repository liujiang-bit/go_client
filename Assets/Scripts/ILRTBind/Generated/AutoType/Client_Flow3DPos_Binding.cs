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
    unsafe class Client_Flow3DPos_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.Flow3DPos);

            field = type.GetField("m_target", flag);
            app.RegisterCLRFieldGetter(field, get_m_target_0);
            app.RegisterCLRFieldSetter(field, set_m_target_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_target_0, AssignFromStack_m_target_0);
            field = type.GetField("invert", flag);
            app.RegisterCLRFieldGetter(field, get_invert_1);
            app.RegisterCLRFieldSetter(field, set_invert_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_invert_1, AssignFromStack_invert_1);


        }



        static object get_m_target_0(ref object o)
        {
            return ((Client.Flow3DPos)o).m_target;
        }

        static StackObject* CopyToStack_m_target_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.Flow3DPos)o).m_target;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_m_target_0(ref object o, object v)
        {
            ((Client.Flow3DPos)o).m_target = (UnityEngine.Transform)v;
        }

        static StackObject* AssignFromStack_m_target_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Transform @m_target = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.Flow3DPos)o).m_target = @m_target;
            return ptr_of_this_method;
        }

        static object get_invert_1(ref object o)
        {
            return ((Client.Flow3DPos)o).invert;
        }

        static StackObject* CopyToStack_invert_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.Flow3DPos)o).invert;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_invert_1(ref object o, object v)
        {
            ((Client.Flow3DPos)o).invert = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_invert_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @invert = ptr_of_this_method->Value == 1;
            ((Client.Flow3DPos)o).invert = @invert;
            return ptr_of_this_method;
        }



    }
}
