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
    unsafe class Client_PolyLineChart_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.PolyLineChart);

            field = type.GetField("Formulas", flag);
            app.RegisterCLRFieldGetter(field, get_Formulas_0);
            app.RegisterCLRFieldSetter(field, set_Formulas_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Formulas_0, AssignFromStack_Formulas_0);


        }



        static object get_Formulas_0(ref object o)
        {
            return ((Client.PolyLineChart)o).Formulas;
        }

        static StackObject* CopyToStack_Formulas_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.PolyLineChart)o).Formulas;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Formulas_0(ref object o, object v)
        {
            ((Client.PolyLineChart)o).Formulas = (System.Collections.Generic.List<Client.FunctionFormula>)v;
        }

        static StackObject* AssignFromStack_Formulas_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<Client.FunctionFormula> @Formulas = (System.Collections.Generic.List<Client.FunctionFormula>)typeof(System.Collections.Generic.List<Client.FunctionFormula>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.PolyLineChart)o).Formulas = @Formulas;
            return ptr_of_this_method;
        }



    }
}
