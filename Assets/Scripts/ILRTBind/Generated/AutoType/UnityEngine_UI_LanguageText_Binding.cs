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
    unsafe class UnityEngine_UI_LanguageText_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UnityEngine.UI.LanguageText);
            args = new Type[]{};
            method = type.GetMethod("get_BaseText", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_BaseText_0);
            args = new Type[]{};
            method = type.GetMethod("UpdateLanguage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateLanguage_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("set_languageId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_languageId_2);

            field = type.GetField("isArabicText", flag);
            app.RegisterCLRFieldGetter(field, get_isArabicText_0);
            app.RegisterCLRFieldSetter(field, set_isArabicText_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_isArabicText_0, AssignFromStack_isArabicText_0);


        }


        static StackObject* get_BaseText_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.LanguageText instance_of_this_method = (UnityEngine.UI.LanguageText)typeof(UnityEngine.UI.LanguageText).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.BaseText;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* UpdateLanguage_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.LanguageText instance_of_this_method = (UnityEngine.UI.LanguageText)typeof(UnityEngine.UI.LanguageText).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateLanguage();

            return __ret;
        }

        static StackObject* set_languageId_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @value = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.LanguageText instance_of_this_method = (UnityEngine.UI.LanguageText)typeof(UnityEngine.UI.LanguageText).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.languageId = value;

            return __ret;
        }


        static object get_isArabicText_0(ref object o)
        {
            return ((UnityEngine.UI.LanguageText)o).isArabicText;
        }

        static StackObject* CopyToStack_isArabicText_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((UnityEngine.UI.LanguageText)o).isArabicText;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isArabicText_0(ref object o, object v)
        {
            ((UnityEngine.UI.LanguageText)o).isArabicText = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isArabicText_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isArabicText = ptr_of_this_method->Value == 1;
            ((UnityEngine.UI.LanguageText)o).isArabicText = @isArabicText;
            return ptr_of_this_method;
        }



    }
}
