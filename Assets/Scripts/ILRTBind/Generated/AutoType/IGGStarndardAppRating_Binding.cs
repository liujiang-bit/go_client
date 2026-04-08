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
    unsafe class IGGStarndardAppRating_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGStarndardAppRating);
            args = new Type[]{typeof(global::IGGAppRating.IGGAppRatingResultListener.Listener)};
            method = type.GetMethod("like", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, like_0);
            args = new Type[]{typeof(global::IGGAppRating.IGGFeedbackWebPageURLResultListener.Listener)};
            method = type.GetMethod("getFeedbackWebPageURL", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getFeedbackWebPageURL_1);
            args = new Type[]{typeof(global::IGGAppRatingFeedback), typeof(global::IGGAppRating.IGGFeedbackResultListener.Listener)};
            method = type.GetMethod("feedback", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, feedback_2);


        }


        static StackObject* like_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAppRating.IGGAppRatingResultListener.Listener @listener = (global::IGGAppRating.IGGAppRatingResultListener.Listener)typeof(global::IGGAppRating.IGGAppRatingResultListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGStarndardAppRating instance_of_this_method = (global::IGGStarndardAppRating)typeof(global::IGGStarndardAppRating).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.like(@listener);

            return __ret;
        }

        static StackObject* getFeedbackWebPageURL_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAppRating.IGGFeedbackWebPageURLResultListener.Listener @listener = (global::IGGAppRating.IGGFeedbackWebPageURLResultListener.Listener)typeof(global::IGGAppRating.IGGFeedbackWebPageURLResultListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGStarndardAppRating instance_of_this_method = (global::IGGStarndardAppRating)typeof(global::IGGStarndardAppRating).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.getFeedbackWebPageURL(@listener);

            return __ret;
        }

        static StackObject* feedback_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAppRating.IGGFeedbackResultListener.Listener @listener = (global::IGGAppRating.IGGFeedbackResultListener.Listener)typeof(global::IGGAppRating.IGGFeedbackResultListener.Listener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAppRatingFeedback @feedback = (global::IGGAppRatingFeedback)typeof(global::IGGAppRatingFeedback).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::IGGStarndardAppRating instance_of_this_method = (global::IGGStarndardAppRating)typeof(global::IGGStarndardAppRating).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.feedback(@feedback, @listener);

            return __ret;
        }



    }
}
