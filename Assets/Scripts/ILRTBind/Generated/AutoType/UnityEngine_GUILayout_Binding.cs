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
    unsafe class UnityEngine_GUILayout_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(UnityEngine.GUILayout);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("Height", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Height_0);
            args = new Type[]{typeof(System.Int32), typeof(System.String[]), typeof(UnityEngine.GUILayoutOption[])};
            method = type.GetMethod("Toolbar", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Toolbar_1);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Single), typeof(UnityEngine.GUILayoutOption[])};
            method = type.GetMethod("HorizontalSlider", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, HorizontalSlider_2);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("Width", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Width_3);
            args = new Type[]{typeof(UnityEngine.GUILayoutOption[])};
            method = type.GetMethod("BeginVertical", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, BeginVertical_4);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.GUILayoutOption[])};
            method = type.GetMethod("Button", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Button_5);
            args = new Type[]{typeof(System.Boolean), typeof(System.String), typeof(UnityEngine.GUILayoutOption[])};
            method = type.GetMethod("Toggle", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Toggle_6);
            args = new Type[]{};
            method = type.GetMethod("EndHorizontal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, EndHorizontal_7);


        }


        static StackObject* Height_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @height = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = UnityEngine.GUILayout.Height(@height);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Toolbar_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GUILayoutOption[] @options = (UnityEngine.GUILayoutOption[])typeof(UnityEngine.GUILayoutOption[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String[] @texts = (System.String[])typeof(System.String[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @selected = ptr_of_this_method->Value;


            var result_of_this_method = UnityEngine.GUILayout.Toolbar(@selected, @texts, @options);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* HorizontalSlider_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GUILayoutOption[] @options = (UnityEngine.GUILayoutOption[])typeof(UnityEngine.GUILayoutOption[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @rightValue = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @leftValue = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @value = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = UnityEngine.GUILayout.HorizontalSlider(@value, @leftValue, @rightValue, @options);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Width_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @width = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = UnityEngine.GUILayout.Width(@width);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* BeginVertical_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GUILayoutOption[] @options = (UnityEngine.GUILayoutOption[])typeof(UnityEngine.GUILayoutOption[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            UnityEngine.GUILayout.BeginVertical(@options);

            return __ret;
        }

        static StackObject* Button_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GUILayoutOption[] @options = (UnityEngine.GUILayoutOption[])typeof(UnityEngine.GUILayoutOption[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @text = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = UnityEngine.GUILayout.Button(@text, @options);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* Toggle_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GUILayoutOption[] @options = (UnityEngine.GUILayoutOption[])typeof(UnityEngine.GUILayoutOption[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @text = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Boolean @value = ptr_of_this_method->Value == 1;


            var result_of_this_method = UnityEngine.GUILayout.Toggle(@value, @text, @options);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* EndHorizontal_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            UnityEngine.GUILayout.EndHorizontal();

            return __ret;
        }



    }
}
