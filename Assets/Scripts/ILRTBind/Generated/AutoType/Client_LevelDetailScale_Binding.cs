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
    unsafe class Client_LevelDetailScale_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.LevelDetailScale);

            field = type.GetField("IsExplore", flag);
            app.RegisterCLRFieldGetter(field, get_IsExplore_0);
            app.RegisterCLRFieldSetter(field, set_IsExplore_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsExplore_0, AssignFromStack_IsExplore_0);


        }



        static object get_IsExplore_0(ref object o)
        {
            return ((Client.LevelDetailScale)o).IsExplore;
        }

        static StackObject* CopyToStack_IsExplore_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.LevelDetailScale)o).IsExplore;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsExplore_0(ref object o, object v)
        {
            ((Client.LevelDetailScale)o).IsExplore = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsExplore_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsExplore = ptr_of_this_method->Value == 1;
            ((Client.LevelDetailScale)o).IsExplore = @IsExplore;
            return ptr_of_this_method;
        }



    }
}
