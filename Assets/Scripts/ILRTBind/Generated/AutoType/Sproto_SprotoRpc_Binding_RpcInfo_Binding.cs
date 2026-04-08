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
    unsafe class Sproto_SprotoRpc_Binding_RpcInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Sproto.SprotoRpc.RpcInfo);

            field = type.GetField("requestObj", flag);
            app.RegisterCLRFieldGetter(field, get_requestObj_0);
            app.RegisterCLRFieldSetter(field, set_requestObj_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_requestObj_0, AssignFromStack_requestObj_0);
            field = type.GetField("responseObj", flag);
            app.RegisterCLRFieldGetter(field, get_responseObj_1);
            app.RegisterCLRFieldSetter(field, set_responseObj_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_responseObj_1, AssignFromStack_responseObj_1);
            field = type.GetField("session", flag);
            app.RegisterCLRFieldGetter(field, get_session_2);
            app.RegisterCLRFieldSetter(field, set_session_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_session_2, AssignFromStack_session_2);
            field = type.GetField("type", flag);
            app.RegisterCLRFieldGetter(field, get_type_3);
            app.RegisterCLRFieldSetter(field, set_type_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_type_3, AssignFromStack_type_3);
            field = type.GetField("tag", flag);
            app.RegisterCLRFieldGetter(field, get_tag_4);
            app.RegisterCLRFieldSetter(field, set_tag_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_tag_4, AssignFromStack_tag_4);

            app.RegisterCLRCreateDefaultInstance(type, () => new Sproto.SprotoRpc.RpcInfo());


        }

        static void WriteBackInstance(ILRuntime.Runtime.Enviorment.AppDomain __domain, StackObject* ptr_of_this_method, IList<object> __mStack, ref Sproto.SprotoRpc.RpcInfo instance_of_this_method)
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
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as Sproto.SprotoRpc.RpcInfo[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = instance_of_this_method;
                    }
                    break;
            }
        }


        static object get_requestObj_0(ref object o)
        {
            return ((Sproto.SprotoRpc.RpcInfo)o).requestObj;
        }

        static StackObject* CopyToStack_requestObj_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Sproto.SprotoRpc.RpcInfo)o).requestObj;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_requestObj_0(ref object o, object v)
        {
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.requestObj = (Sproto.SprotoTypeBase)v;
            o = ins;
        }

        static StackObject* AssignFromStack_requestObj_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Sproto.SprotoTypeBase @requestObj = (Sproto.SprotoTypeBase)typeof(Sproto.SprotoTypeBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.requestObj = @requestObj;
            o = ins;
            return ptr_of_this_method;
        }

        static object get_responseObj_1(ref object o)
        {
            return ((Sproto.SprotoRpc.RpcInfo)o).responseObj;
        }

        static StackObject* CopyToStack_responseObj_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Sproto.SprotoRpc.RpcInfo)o).responseObj;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_responseObj_1(ref object o, object v)
        {
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.responseObj = (Sproto.SprotoTypeBase)v;
            o = ins;
        }

        static StackObject* AssignFromStack_responseObj_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Sproto.SprotoTypeBase @responseObj = (Sproto.SprotoTypeBase)typeof(Sproto.SprotoTypeBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.responseObj = @responseObj;
            o = ins;
            return ptr_of_this_method;
        }

        static object get_session_2(ref object o)
        {
            return ((Sproto.SprotoRpc.RpcInfo)o).session;
        }

        static StackObject* CopyToStack_session_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Sproto.SprotoRpc.RpcInfo)o).session;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_session_2(ref object o, object v)
        {
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.session = (System.Nullable<System.Int64>)v;
            o = ins;
        }

        static StackObject* AssignFromStack_session_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Nullable<System.Int64> @session = (System.Nullable<System.Int64>)typeof(System.Nullable<System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.session = @session;
            o = ins;
            return ptr_of_this_method;
        }

        static object get_type_3(ref object o)
        {
            return ((Sproto.SprotoRpc.RpcInfo)o).type;
        }

        static StackObject* CopyToStack_type_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Sproto.SprotoRpc.RpcInfo)o).type;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_type_3(ref object o, object v)
        {
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.type = (Sproto.SprotoRpc.RpcType)v;
            o = ins;
        }

        static StackObject* AssignFromStack_type_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Sproto.SprotoRpc.RpcType @type = (Sproto.SprotoRpc.RpcType)typeof(Sproto.SprotoRpc.RpcType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.type = @type;
            o = ins;
            return ptr_of_this_method;
        }

        static object get_tag_4(ref object o)
        {
            return ((Sproto.SprotoRpc.RpcInfo)o).tag;
        }

        static StackObject* CopyToStack_tag_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Sproto.SprotoRpc.RpcInfo)o).tag;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_tag_4(ref object o, object v)
        {
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.tag = (System.Nullable<System.Int32>)v;
            o = ins;
        }

        static StackObject* AssignFromStack_tag_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Nullable<System.Int32> @tag = (System.Nullable<System.Int32>)typeof(System.Nullable<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Sproto.SprotoRpc.RpcInfo ins =(Sproto.SprotoRpc.RpcInfo)o;
            ins.tag = @tag;
            o = ins;
            return ptr_of_this_method;
        }



    }
}
