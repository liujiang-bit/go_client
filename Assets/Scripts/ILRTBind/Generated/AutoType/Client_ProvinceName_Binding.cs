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
    unsafe class Client_ProvinceName_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.ProvinceName);

            field = type.GetField("m_pos", flag);
            app.RegisterCLRFieldGetter(field, get_m_pos_0);
            app.RegisterCLRFieldSetter(field, set_m_pos_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_pos_0, AssignFromStack_m_pos_0);
            field = type.GetField("m_province_name", flag);
            app.RegisterCLRFieldGetter(field, get_m_province_name_1);
            app.RegisterCLRFieldSetter(field, set_m_province_name_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_province_name_1, AssignFromStack_m_province_name_1);

            app.RegisterCLRCreateDefaultInstance(type, () => new Client.ProvinceName());


        }

        static void WriteBackInstance(ILRuntime.Runtime.Enviorment.AppDomain __domain, StackObject* ptr_of_this_method, IList<object> __mStack, ref Client.ProvinceName instance_of_this_method)
        {
            ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.Object:
                    {
                        __mStack[ptr_of_this_method->Value] = instance_of_this_method;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = instance_of_this_method;
                        }
                        else
                        {
                            var t = __domain.GetType(___obj.GetType()) as CLRType;
                            t.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, instance_of_this_method);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var t = __domain.GetType(ptr_of_this_method->Value);
                        if(t is ILType)
                        {
                            ((ILType)t).StaticInstance[ptr_of_this_method->ValueLow] = instance_of_this_method;
                        }
                        else
                        {
                            ((CLRType)t).SetStaticFieldValue(ptr_of_this_method->ValueLow, instance_of_this_method);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as Client.ProvinceName[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = instance_of_this_method;
                    }
                    break;
            }
        }


        static object get_m_pos_0(ref object o)
        {
            return ((Client.ProvinceName)o).m_pos;
        }

        static StackObject* CopyToStack_m_pos_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ProvinceName)o).m_pos;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_m_pos_0(ref object o, object v)
        {
            Client.ProvinceName ins =(Client.ProvinceName)o;
            ins.m_pos = (UnityEngine.Vector3)v;
            o = ins;
        }

        static StackObject* AssignFromStack_m_pos_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector3 @m_pos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @m_pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @m_pos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            Client.ProvinceName ins =(Client.ProvinceName)o;
            ins.m_pos = @m_pos;
            o = ins;
            return ptr_of_this_method;
        }

        static object get_m_province_name_1(ref object o)
        {
            return ((Client.ProvinceName)o).m_province_name;
        }

        static StackObject* CopyToStack_m_province_name_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.ProvinceName)o).m_province_name;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_m_province_name_1(ref object o, object v)
        {
            Client.ProvinceName ins =(Client.ProvinceName)o;
            ins.m_province_name = (System.String)v;
            o = ins;
        }

        static StackObject* AssignFromStack_m_province_name_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @m_province_name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Client.ProvinceName ins =(Client.ProvinceName)o;
            ins.m_province_name = @m_province_name;
            o = ins;
            return ptr_of_this_method;
        }



    }
}
