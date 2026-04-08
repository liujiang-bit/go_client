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
    unsafe class Client_GridCollideItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.GridCollideItem);
            args = new Type[]{typeof(Client.GridCollideItem)};
            method = type.GetMethod("ResetInitLocalPosS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetInitLocalPosS_0);

            field = type.GetField("m_priority", flag);
            app.RegisterCLRFieldGetter(field, get_m_priority_0);
            app.RegisterCLRFieldSetter(field, set_m_priority_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_priority_0, AssignFromStack_m_priority_0);
            field = type.GetField("size", flag);
            app.RegisterCLRFieldGetter(field, get_size_1);
            app.RegisterCLRFieldSetter(field, set_size_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_size_1, AssignFromStack_size_1);
            field = type.GetField("m_auto_registre", flag);
            app.RegisterCLRFieldGetter(field, get_m_auto_registre_2);
            app.RegisterCLRFieldSetter(field, set_m_auto_registre_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_auto_registre_2, AssignFromStack_m_auto_registre_2);


        }


        static StackObject* ResetInitLocalPosS_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.GridCollideItem @self = (Client.GridCollideItem)typeof(Client.GridCollideItem).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.GridCollideItem.ResetInitLocalPosS(@self);

            return __ret;
        }


        static object get_m_priority_0(ref object o)
        {
            return ((Client.GridCollideItem)o).m_priority;
        }

        static StackObject* CopyToStack_m_priority_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.GridCollideItem)o).m_priority;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_m_priority_0(ref object o, object v)
        {
            ((Client.GridCollideItem)o).m_priority = (System.Int32)v;
        }

        static StackObject* AssignFromStack_m_priority_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @m_priority = ptr_of_this_method->Value;
            ((Client.GridCollideItem)o).m_priority = @m_priority;
            return ptr_of_this_method;
        }

        static object get_size_1(ref object o)
        {
            return ((Client.GridCollideItem)o).size;
        }

        static StackObject* CopyToStack_size_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.GridCollideItem)o).size;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_size_1(ref object o, object v)
        {
            ((Client.GridCollideItem)o).size = (UnityEngine.Vector2)v;
        }

        static StackObject* AssignFromStack_size_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector2 @size = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @size, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @size = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            ((Client.GridCollideItem)o).size = @size;
            return ptr_of_this_method;
        }

        static object get_m_auto_registre_2(ref object o)
        {
            return ((Client.GridCollideItem)o).m_auto_registre;
        }

        static StackObject* CopyToStack_m_auto_registre_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.GridCollideItem)o).m_auto_registre;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_m_auto_registre_2(ref object o, object v)
        {
            ((Client.GridCollideItem)o).m_auto_registre = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_m_auto_registre_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @m_auto_registre = ptr_of_this_method->Value == 1;
            ((Client.GridCollideItem)o).m_auto_registre = @m_auto_registre;
            return ptr_of_this_method;
        }



    }
}
