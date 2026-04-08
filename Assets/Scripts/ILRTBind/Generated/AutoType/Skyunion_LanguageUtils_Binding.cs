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
    unsafe class Skyunion_LanguageUtils_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.LanguageUtils);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("getText", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getText_0);
            args = new Type[]{typeof(System.Int32), typeof(System.Object[])};
            method = type.GetMethod("getTextFormat", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getTextFormat_1);
            args = new Type[]{};
            method = type.GetMethod("GetLanguage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetLanguage_2);
            args = new Type[]{};
            method = type.GetMethod("LoadCache", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LoadCache_3);
            args = new Type[]{typeof(UnityEngine.SystemLanguage)};
            method = type.GetMethod("SetLanguage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetLanguage_4);
            args = new Type[]{};
            method = type.GetMethod("IsArabic", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsArabic_5);
            args = new Type[]{};
            method = type.GetMethod("ClearCache", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearCache_6);
            args = new Type[]{};
            method = type.GetMethod("SaveCache", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SaveCache_7);


        }


        static StackObject* getText_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @id = ptr_of_this_method->Value;


            var result_of_this_method = Skyunion.LanguageUtils.getText(@id);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getTextFormat_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object[] @arg = (System.Object[])typeof(System.Object[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @id = ptr_of_this_method->Value;


            var result_of_this_method = Skyunion.LanguageUtils.getTextFormat(@id, @arg);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetLanguage_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.LanguageUtils.GetLanguage();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* LoadCache_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            Skyunion.LanguageUtils.LoadCache();

            return __ret;
        }

        static StackObject* SetLanguage_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.SystemLanguage @value = (UnityEngine.SystemLanguage)typeof(UnityEngine.SystemLanguage).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Skyunion.LanguageUtils.SetLanguage(@value);

            return __ret;
        }

        static StackObject* IsArabic_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.LanguageUtils.IsArabic();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* ClearCache_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            Skyunion.LanguageUtils.ClearCache();

            return __ret;
        }

        static StackObject* SaveCache_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            Skyunion.LanguageUtils.SaveCache();

            return __ret;
        }



    }
}
