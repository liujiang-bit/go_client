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
    unsafe class Client_FunctionFormula_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.FunctionFormula);

            field = type.GetField("heightPercent", flag);
            app.RegisterCLRFieldGetter(field, get_heightPercent_0);
            app.RegisterCLRFieldSetter(field, set_heightPercent_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_heightPercent_0, AssignFromStack_heightPercent_0);
            field = type.GetField("lengthPercent", flag);
            app.RegisterCLRFieldGetter(field, get_lengthPercent_1);
            app.RegisterCLRFieldSetter(field, set_lengthPercent_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_lengthPercent_1, AssignFromStack_lengthPercent_1);
            field = type.GetField("FormulaWidth", flag);
            app.RegisterCLRFieldGetter(field, get_FormulaWidth_2);
            app.RegisterCLRFieldSetter(field, set_FormulaWidth_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_FormulaWidth_2, AssignFromStack_FormulaWidth_2);
            field = type.GetField("FormulaColor", flag);
            app.RegisterCLRFieldGetter(field, get_FormulaColor_3);
            app.RegisterCLRFieldSetter(field, set_FormulaColor_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_FormulaColor_3, AssignFromStack_FormulaColor_3);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_heightPercent_0(ref object o)
        {
            return ((Client.FunctionFormula)o).heightPercent;
        }

        static StackObject* CopyToStack_heightPercent_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FunctionFormula)o).heightPercent;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_heightPercent_0(ref object o, object v)
        {
            ((Client.FunctionFormula)o).heightPercent = (System.Single)v;
        }

        static StackObject* AssignFromStack_heightPercent_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @heightPercent = *(float*)&ptr_of_this_method->Value;
            ((Client.FunctionFormula)o).heightPercent = @heightPercent;
            return ptr_of_this_method;
        }

        static object get_lengthPercent_1(ref object o)
        {
            return ((Client.FunctionFormula)o).lengthPercent;
        }

        static StackObject* CopyToStack_lengthPercent_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FunctionFormula)o).lengthPercent;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_lengthPercent_1(ref object o, object v)
        {
            ((Client.FunctionFormula)o).lengthPercent = (System.Single)v;
        }

        static StackObject* AssignFromStack_lengthPercent_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @lengthPercent = *(float*)&ptr_of_this_method->Value;
            ((Client.FunctionFormula)o).lengthPercent = @lengthPercent;
            return ptr_of_this_method;
        }

        static object get_FormulaWidth_2(ref object o)
        {
            return ((Client.FunctionFormula)o).FormulaWidth;
        }

        static StackObject* CopyToStack_FormulaWidth_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FunctionFormula)o).FormulaWidth;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_FormulaWidth_2(ref object o, object v)
        {
            ((Client.FunctionFormula)o).FormulaWidth = (System.Single)v;
        }

        static StackObject* AssignFromStack_FormulaWidth_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @FormulaWidth = *(float*)&ptr_of_this_method->Value;
            ((Client.FunctionFormula)o).FormulaWidth = @FormulaWidth;
            return ptr_of_this_method;
        }

        static object get_FormulaColor_3(ref object o)
        {
            return ((Client.FunctionFormula)o).FormulaColor;
        }

        static StackObject* CopyToStack_FormulaColor_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FunctionFormula)o).FormulaColor;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_FormulaColor_3(ref object o, object v)
        {
            ((Client.FunctionFormula)o).FormulaColor = (UnityEngine.Color)v;
        }

        static StackObject* AssignFromStack_FormulaColor_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Color @FormulaColor = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.FunctionFormula)o).FormulaColor = @FormulaColor;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Client.FunctionFormula();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
