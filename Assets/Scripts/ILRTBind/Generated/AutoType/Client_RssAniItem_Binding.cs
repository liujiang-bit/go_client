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
    unsafe class Client_RssAniItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.RssAniItem);

            field = type.GetField("m_gameObject", flag);
            app.RegisterCLRFieldGetter(field, get_m_gameObject_0);
            app.RegisterCLRFieldSetter(field, set_m_gameObject_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_gameObject_0, AssignFromStack_m_gameObject_0);
            field = type.GetField("m_baseScale", flag);
            app.RegisterCLRFieldGetter(field, get_m_baseScale_1);
            app.RegisterCLRFieldSetter(field, set_m_baseScale_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_baseScale_1, AssignFromStack_m_baseScale_1);


        }



        static object get_m_gameObject_0(ref object o)
        {
            return ((Client.RssAniItem)o).m_gameObject;
        }

        static StackObject* CopyToStack_m_gameObject_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.RssAniItem)o).m_gameObject;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_m_gameObject_0(ref object o, object v)
        {
            ((Client.RssAniItem)o).m_gameObject = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_m_gameObject_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @m_gameObject = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.RssAniItem)o).m_gameObject = @m_gameObject;
            return ptr_of_this_method;
        }

        static object get_m_baseScale_1(ref object o)
        {
            return ((Client.RssAniItem)o).m_baseScale;
        }

        static StackObject* CopyToStack_m_baseScale_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.RssAniItem)o).m_baseScale;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_m_baseScale_1(ref object o, object v)
        {
            ((Client.RssAniItem)o).m_baseScale = (UnityEngine.Vector3)v;
        }

        static StackObject* AssignFromStack_m_baseScale_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector3 @m_baseScale = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @m_baseScale, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @m_baseScale = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            ((Client.RssAniItem)o).m_baseScale = @m_baseScale;
            return ptr_of_this_method;
        }



    }
}
