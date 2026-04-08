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
    unsafe class IGGSDKConstant_IGGDefault_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(IGGSDKConstant.IGGDefault);

            field = type.GetField("AppConfigIP", flag);
            app.RegisterCLRFieldGetter(field, get_AppConfigIP_0);
            app.RegisterCLRFieldSetter(field, set_AppConfigIP_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_AppConfigIP_0, AssignFromStack_AppConfigIP_0);
            field = type.GetField("IGGID", flag);
            app.RegisterCLRFieldGetter(field, get_IGGID_1);
            app.RegisterCLRFieldSetter(field, set_IGGID_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_IGGID_1, AssignFromStack_IGGID_1);
            field = type.GetField("Token", flag);
            app.RegisterCLRFieldGetter(field, get_Token_2);
            app.RegisterCLRFieldSetter(field, set_Token_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_Token_2, AssignFromStack_Token_2);


        }



        static object get_AppConfigIP_0(ref object o)
        {
            return IGGSDKConstant.IGGDefault.AppConfigIP;
        }

        static StackObject* CopyToStack_AppConfigIP_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGDefault.AppConfigIP;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_AppConfigIP_0(ref object o, object v)
        {
            IGGSDKConstant.IGGDefault.AppConfigIP = (System.String)v;
        }

        static StackObject* AssignFromStack_AppConfigIP_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @AppConfigIP = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGDefault.AppConfigIP = @AppConfigIP;
            return ptr_of_this_method;
        }

        static object get_IGGID_1(ref object o)
        {
            return IGGSDKConstant.IGGDefault.IGGID;
        }

        static StackObject* CopyToStack_IGGID_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGDefault.IGGID;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_IGGID_1(ref object o, object v)
        {
            IGGSDKConstant.IGGDefault.IGGID = (System.String)v;
        }

        static StackObject* AssignFromStack_IGGID_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @IGGID = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGDefault.IGGID = @IGGID;
            return ptr_of_this_method;
        }

        static object get_Token_2(ref object o)
        {
            return IGGSDKConstant.IGGDefault.Token;
        }

        static StackObject* CopyToStack_Token_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGDefault.Token;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Token_2(ref object o, object v)
        {
            IGGSDKConstant.IGGDefault.Token = (System.String)v;
        }

        static StackObject* AssignFromStack_Token_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @Token = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGDefault.Token = @Token;
            return ptr_of_this_method;
        }



    }
}
