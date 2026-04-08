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
    unsafe class Client_LevelDetailCamera_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.LevelDetailCamera);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("AddLodChange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddLodChange_0);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("RemoveLodChange", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveLodChange_1);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("GetLodLevelByDxf", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetLodLevelByDxf_2);
            args = new Type[]{};
            method = type.GetMethod("GetCurrentLodLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCurrentLodLevel_3);

            field = type.GetField("instance", flag);
            app.RegisterCLRFieldGetter(field, get_instance_0);
            app.RegisterCLRFieldSetter(field, set_instance_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_instance_0, AssignFromStack_instance_0);


        }


        static StackObject* AddLodChange_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @lodChange = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.LevelDetailCamera instance_of_this_method = (Client.LevelDetailCamera)typeof(Client.LevelDetailCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddLodChange(@lodChange);

            return __ret;
        }

        static StackObject* RemoveLodChange_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @lodChange = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.LevelDetailCamera instance_of_this_method = (Client.LevelDetailCamera)typeof(Client.LevelDetailCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveLodChange(@lodChange);

            return __ret;
        }

        static StackObject* GetLodLevelByDxf_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @dxf = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.LevelDetailCamera instance_of_this_method = (Client.LevelDetailCamera)typeof(Client.LevelDetailCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetLodLevelByDxf(@dxf);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetCurrentLodLevel_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.LevelDetailCamera instance_of_this_method = (Client.LevelDetailCamera)typeof(Client.LevelDetailCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetCurrentLodLevel();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static object get_instance_0(ref object o)
        {
            return Client.LevelDetailCamera.instance;
        }

        static StackObject* CopyToStack_instance_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.LevelDetailCamera.instance;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_instance_0(ref object o, object v)
        {
            Client.LevelDetailCamera.instance = (Client.LevelDetailCamera)v;
        }

        static StackObject* AssignFromStack_instance_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Client.LevelDetailCamera @instance = (Client.LevelDetailCamera)typeof(Client.LevelDetailCamera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Client.LevelDetailCamera.instance = @instance;
            return ptr_of_this_method;
        }



    }
}
