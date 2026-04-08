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
    unsafe class IGGTranslator_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGTranslator);
            args = new Type[]{typeof(global::IGGTranslationSource), typeof(global::IGGTranslator.IGGTranslatorListener.Listener1), typeof(global::IGGTranslator.IGGTranslatorListener.Listener2)};
            method = type.GetMethod("translateText", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, translateText_0);
            args = new Type[]{typeof(System.Collections.Generic.List<global::IGGTranslationSource>), typeof(global::IGGTranslator.IGGTranslatorListener.Listener1), typeof(global::IGGTranslator.IGGTranslatorListener.Listener2)};
            method = type.GetMethod("translateTexts", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, translateTexts_1);

            args = new Type[]{typeof(System.String), typeof(System.String)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }


        static StackObject* translateText_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGTranslator.IGGTranslatorListener.Listener2 @listener2 = (global::IGGTranslator.IGGTranslatorListener.Listener2)typeof(global::IGGTranslator.IGGTranslatorListener.Listener2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGTranslator.IGGTranslatorListener.Listener1 @listener1 = (global::IGGTranslator.IGGTranslatorListener.Listener1)typeof(global::IGGTranslator.IGGTranslatorListener.Listener1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::IGGTranslationSource @source = (global::IGGTranslationSource)typeof(global::IGGTranslationSource).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::IGGTranslator instance_of_this_method = (global::IGGTranslator)typeof(global::IGGTranslator).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.translateText(@source, @listener1, @listener2);

            return __ret;
        }

        static StackObject* translateTexts_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGTranslator.IGGTranslatorListener.Listener2 @listener2 = (global::IGGTranslator.IGGTranslatorListener.Listener2)typeof(global::IGGTranslator.IGGTranslatorListener.Listener2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGTranslator.IGGTranslatorListener.Listener1 @listener1 = (global::IGGTranslator.IGGTranslatorListener.Listener1)typeof(global::IGGTranslator.IGGTranslatorListener.Listener1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Collections.Generic.List<global::IGGTranslationSource> @list = (System.Collections.Generic.List<global::IGGTranslationSource>)typeof(System.Collections.Generic.List<global::IGGTranslationSource>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::IGGTranslator instance_of_this_method = (global::IGGTranslator)typeof(global::IGGTranslator).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.translateTexts(@list, @listener1, @listener2);

            return __ret;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @targetLanguage = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @sourceLanguage = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new global::IGGTranslator(@sourceLanguage, @targetLanguage);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
