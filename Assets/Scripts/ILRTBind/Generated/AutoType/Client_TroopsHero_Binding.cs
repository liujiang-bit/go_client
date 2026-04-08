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
    unsafe class Client_TroopsHero_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.TroopsHero);

            field = type.GetField("heroId", flag);
            app.RegisterCLRFieldGetter(field, get_heroId_0);
            app.RegisterCLRFieldSetter(field, set_heroId_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_heroId_0, AssignFromStack_heroId_0);
            field = type.GetField("label", flag);
            app.RegisterCLRFieldGetter(field, get_label_1);
            app.RegisterCLRFieldSetter(field, set_label_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_label_1, AssignFromStack_label_1);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_heroId_0(ref object o)
        {
            return ((Client.TroopsHero)o).heroId;
        }

        static StackObject* CopyToStack_heroId_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsHero)o).heroId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_heroId_0(ref object o, object v)
        {
            ((Client.TroopsHero)o).heroId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_heroId_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @heroId = ptr_of_this_method->Value;
            ((Client.TroopsHero)o).heroId = @heroId;
            return ptr_of_this_method;
        }

        static object get_label_1(ref object o)
        {
            return ((Client.TroopsHero)o).label;
        }

        static StackObject* CopyToStack_label_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsHero)o).label;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_label_1(ref object o, object v)
        {
            ((Client.TroopsHero)o).label = (System.String)v;
        }

        static StackObject* AssignFromStack_label_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @label = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.TroopsHero)o).label = @label;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Client.TroopsHero();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
