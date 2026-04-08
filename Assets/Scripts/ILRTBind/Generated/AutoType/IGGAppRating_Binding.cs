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
    unsafe class IGGAppRating_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::IGGAppRating);
            args = new Type[]{typeof(global::IGGAppRating.IGGRequestReviewListener.Listener1), typeof(global::IGGAppRating.IGGRequestReviewListener.Listener2), typeof(global::IGGAppRating.IGGRequestReviewListener.Listener3), typeof(global::IGGAppRating.IGGRequestReviewListener.Listener4)};
            method = type.GetMethod("requestReview", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, requestReview_0);


        }


        static StackObject* requestReview_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::IGGAppRating.IGGRequestReviewListener.Listener4 @onStarndardModeEnabled = (global::IGGAppRating.IGGRequestReviewListener.Listener4)typeof(global::IGGAppRating.IGGRequestReviewListener.Listener4).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::IGGAppRating.IGGRequestReviewListener.Listener3 @onMinimizedModeEnabled = (global::IGGAppRating.IGGRequestReviewListener.Listener3)typeof(global::IGGAppRating.IGGRequestReviewListener.Listener3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::IGGAppRating.IGGRequestReviewListener.Listener2 @onError = (global::IGGAppRating.IGGRequestReviewListener.Listener2)typeof(global::IGGAppRating.IGGRequestReviewListener.Listener2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::IGGAppRating.IGGRequestReviewListener.Listener1 @onDisabled = (global::IGGAppRating.IGGRequestReviewListener.Listener1)typeof(global::IGGAppRating.IGGRequestReviewListener.Listener1).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::IGGAppRating instance_of_this_method = (global::IGGAppRating)typeof(global::IGGAppRating).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.requestReview(@onDisabled, @onError, @onMinimizedModeEnabled, @onStarndardModeEnabled);

            return __ret;
        }



    }
}
