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
    unsafe class IGGSDKConstant_IGGLanguage_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(IGGSDKConstant.IGGLanguage);

            field = type.GetField("auto", flag);
            app.RegisterCLRFieldGetter(field, get_auto_0);
            app.RegisterCLRFieldSetter(field, set_auto_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_auto_0, AssignFromStack_auto_0);
            field = type.GetField("Ar", flag);
            app.RegisterCLRFieldGetter(field, get_Ar_1);
            app.RegisterCLRFieldSetter(field, set_Ar_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_Ar_1, AssignFromStack_Ar_1);
            field = type.GetField("En", flag);
            app.RegisterCLRFieldGetter(field, get_En_2);
            app.RegisterCLRFieldSetter(field, set_En_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_En_2, AssignFromStack_En_2);
            field = type.GetField("Tr", flag);
            app.RegisterCLRFieldGetter(field, get_Tr_3);
            app.RegisterCLRFieldSetter(field, set_Tr_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_Tr_3, AssignFromStack_Tr_3);
            field = type.GetField("Zh_CN", flag);
            app.RegisterCLRFieldGetter(field, get_Zh_CN_4);
            app.RegisterCLRFieldSetter(field, set_Zh_CN_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_Zh_CN_4, AssignFromStack_Zh_CN_4);


        }



        static object get_auto_0(ref object o)
        {
            return IGGSDKConstant.IGGLanguage.auto;
        }

        static StackObject* CopyToStack_auto_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGLanguage.auto;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_auto_0(ref object o, object v)
        {
            IGGSDKConstant.IGGLanguage.auto = (System.String)v;
        }

        static StackObject* AssignFromStack_auto_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @auto = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGLanguage.auto = @auto;
            return ptr_of_this_method;
        }

        static object get_Ar_1(ref object o)
        {
            return IGGSDKConstant.IGGLanguage.Ar;
        }

        static StackObject* CopyToStack_Ar_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGLanguage.Ar;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Ar_1(ref object o, object v)
        {
            IGGSDKConstant.IGGLanguage.Ar = (System.String)v;
        }

        static StackObject* AssignFromStack_Ar_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @Ar = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGLanguage.Ar = @Ar;
            return ptr_of_this_method;
        }

        static object get_En_2(ref object o)
        {
            return IGGSDKConstant.IGGLanguage.En;
        }

        static StackObject* CopyToStack_En_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGLanguage.En;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_En_2(ref object o, object v)
        {
            IGGSDKConstant.IGGLanguage.En = (System.String)v;
        }

        static StackObject* AssignFromStack_En_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @En = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGLanguage.En = @En;
            return ptr_of_this_method;
        }

        static object get_Tr_3(ref object o)
        {
            return IGGSDKConstant.IGGLanguage.Tr;
        }

        static StackObject* CopyToStack_Tr_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGLanguage.Tr;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Tr_3(ref object o, object v)
        {
            IGGSDKConstant.IGGLanguage.Tr = (System.String)v;
        }

        static StackObject* AssignFromStack_Tr_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @Tr = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGLanguage.Tr = @Tr;
            return ptr_of_this_method;
        }

        static object get_Zh_CN_4(ref object o)
        {
            return IGGSDKConstant.IGGLanguage.Zh_CN;
        }

        static StackObject* CopyToStack_Zh_CN_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = IGGSDKConstant.IGGLanguage.Zh_CN;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Zh_CN_4(ref object o, object v)
        {
            IGGSDKConstant.IGGLanguage.Zh_CN = (System.String)v;
        }

        static StackObject* AssignFromStack_Zh_CN_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @Zh_CN = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            IGGSDKConstant.IGGLanguage.Zh_CN = @Zh_CN;
            return ptr_of_this_method;
        }



    }
}
