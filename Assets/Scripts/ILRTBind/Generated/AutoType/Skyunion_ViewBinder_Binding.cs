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
    unsafe class Skyunion_ViewBinder_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.ViewBinder);
            args = new Type[]{typeof(System.String), typeof(Skyunion.GameView), typeof(System.Action)};
            method = type.GetMethod("Create", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Create_0);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("Create", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Create_1);

            field = type.GetField("openAniEndCallback", flag);
            app.RegisterCLRFieldGetter(field, get_openAniEndCallback_0);
            app.RegisterCLRFieldSetter(field, set_openAniEndCallback_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_openAniEndCallback_0, AssignFromStack_openAniEndCallback_0);
            field = type.GetField("onWinFocusCallback", flag);
            app.RegisterCLRFieldGetter(field, get_onWinFocusCallback_1);
            app.RegisterCLRFieldSetter(field, set_onWinFocusCallback_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onWinFocusCallback_1, AssignFromStack_onWinFocusCallback_1);
            field = type.GetField("onWinCloseCallback", flag);
            app.RegisterCLRFieldGetter(field, get_onWinCloseCallback_2);
            app.RegisterCLRFieldSetter(field, set_onWinCloseCallback_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_onWinCloseCallback_2, AssignFromStack_onWinCloseCallback_2);
            field = type.GetField("onPrewarmCallback", flag);
            app.RegisterCLRFieldGetter(field, get_onPrewarmCallback_3);
            app.RegisterCLRFieldSetter(field, set_onPrewarmCallback_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_onPrewarmCallback_3, AssignFromStack_onPrewarmCallback_3);
            field = type.GetField("onMenuBackCallback", flag);
            app.RegisterCLRFieldGetter(field, get_onMenuBackCallback_4);
            app.RegisterCLRFieldSetter(field, set_onMenuBackCallback_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_onMenuBackCallback_4, AssignFromStack_onMenuBackCallback_4);


        }


        static StackObject* Create_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @cbAction = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.GameView @view = (Skyunion.GameView)typeof(Skyunion.GameView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @assetName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Skyunion.ViewBinder.Create(@assetName, @view, @cbAction);

            return __ret;
        }

        static StackObject* Create_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Skyunion.ViewBinder.Create(@go);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_openAniEndCallback_0(ref object o)
        {
            return ((Skyunion.ViewBinder)o).openAniEndCallback;
        }

        static StackObject* CopyToStack_openAniEndCallback_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.ViewBinder)o).openAniEndCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_openAniEndCallback_0(ref object o, object v)
        {
            ((Skyunion.ViewBinder)o).openAniEndCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_openAniEndCallback_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @openAniEndCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.ViewBinder)o).openAniEndCallback = @openAniEndCallback;
            return ptr_of_this_method;
        }

        static object get_onWinFocusCallback_1(ref object o)
        {
            return ((Skyunion.ViewBinder)o).onWinFocusCallback;
        }

        static StackObject* CopyToStack_onWinFocusCallback_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.ViewBinder)o).onWinFocusCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onWinFocusCallback_1(ref object o, object v)
        {
            ((Skyunion.ViewBinder)o).onWinFocusCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_onWinFocusCallback_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @onWinFocusCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.ViewBinder)o).onWinFocusCallback = @onWinFocusCallback;
            return ptr_of_this_method;
        }

        static object get_onWinCloseCallback_2(ref object o)
        {
            return ((Skyunion.ViewBinder)o).onWinCloseCallback;
        }

        static StackObject* CopyToStack_onWinCloseCallback_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.ViewBinder)o).onWinCloseCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onWinCloseCallback_2(ref object o, object v)
        {
            ((Skyunion.ViewBinder)o).onWinCloseCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_onWinCloseCallback_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @onWinCloseCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.ViewBinder)o).onWinCloseCallback = @onWinCloseCallback;
            return ptr_of_this_method;
        }

        static object get_onPrewarmCallback_3(ref object o)
        {
            return ((Skyunion.ViewBinder)o).onPrewarmCallback;
        }

        static StackObject* CopyToStack_onPrewarmCallback_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.ViewBinder)o).onPrewarmCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onPrewarmCallback_3(ref object o, object v)
        {
            ((Skyunion.ViewBinder)o).onPrewarmCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_onPrewarmCallback_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @onPrewarmCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.ViewBinder)o).onPrewarmCallback = @onPrewarmCallback;
            return ptr_of_this_method;
        }

        static object get_onMenuBackCallback_4(ref object o)
        {
            return ((Skyunion.ViewBinder)o).onMenuBackCallback;
        }

        static StackObject* CopyToStack_onMenuBackCallback_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.ViewBinder)o).onMenuBackCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onMenuBackCallback_4(ref object o, object v)
        {
            ((Skyunion.ViewBinder)o).onMenuBackCallback = (System.Func<System.Boolean>)v;
        }

        static StackObject* AssignFromStack_onMenuBackCallback_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Func<System.Boolean> @onMenuBackCallback = (System.Func<System.Boolean>)typeof(System.Func<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.ViewBinder)o).onMenuBackCallback = @onMenuBackCallback;
            return ptr_of_this_method;
        }



    }
}
