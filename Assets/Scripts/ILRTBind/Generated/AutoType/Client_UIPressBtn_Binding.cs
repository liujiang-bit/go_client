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
    unsafe class Client_UIPressBtn_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.UIPressBtn);
            args = new Type[]{};
            method = type.GetMethod("RemoveAllPressClick", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveAllPressClick_0);
            args = new Type[]{typeof(System.Single), typeof(System.Boolean)};
            method = type.GetMethod("Register", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Register_1);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("AddPressClick", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddPressClick_2);

            field = type.GetField("OnPointUpCallback", flag);
            app.RegisterCLRFieldGetter(field, get_OnPointUpCallback_0);
            app.RegisterCLRFieldSetter(field, set_OnPointUpCallback_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnPointUpCallback_0, AssignFromStack_OnPointUpCallback_0);


        }


        static StackObject* RemoveAllPressClick_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.UIPressBtn instance_of_this_method = (Client.UIPressBtn)typeof(Client.UIPressBtn).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveAllPressClick();

            return __ret;
        }

        static StackObject* Register_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isBgActived = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @times = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.UIPressBtn instance_of_this_method = (Client.UIPressBtn)typeof(Client.UIPressBtn).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Register(@times, @isBgActived);

            return __ret;
        }

        static StackObject* AddPressClick_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.UIPressBtn instance_of_this_method = (Client.UIPressBtn)typeof(Client.UIPressBtn).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddPressClick(@callback);

            return __ret;
        }


        static object get_OnPointUpCallback_0(ref object o)
        {
            return ((Client.UIPressBtn)o).OnPointUpCallback;
        }

        static StackObject* CopyToStack_OnPointUpCallback_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.UIPressBtn)o).OnPointUpCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnPointUpCallback_0(ref object o, object v)
        {
            ((Client.UIPressBtn)o).OnPointUpCallback = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnPointUpCallback_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnPointUpCallback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.UIPressBtn)o).OnPointUpCallback = @OnPointUpCallback;
            return ptr_of_this_method;
        }



    }
}
