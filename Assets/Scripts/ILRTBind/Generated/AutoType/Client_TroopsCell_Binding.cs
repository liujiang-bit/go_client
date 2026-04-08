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
    unsafe class Client_TroopsCell_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.TroopsCell);

            field = type.GetField("unitId", flag);
            app.RegisterCLRFieldGetter(field, get_unitId_0);
            app.RegisterCLRFieldSetter(field, set_unitId_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_unitId_0, AssignFromStack_unitId_0);
            field = type.GetField("unitCount", flag);
            app.RegisterCLRFieldGetter(field, get_unitCount_1);
            app.RegisterCLRFieldSetter(field, set_unitCount_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_unitCount_1, AssignFromStack_unitCount_1);
            field = type.GetField("unitMaxCount", flag);
            app.RegisterCLRFieldGetter(field, get_unitMaxCount_2);
            app.RegisterCLRFieldSetter(field, set_unitMaxCount_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_unitMaxCount_2, AssignFromStack_unitMaxCount_2);
            field = type.GetField("unitType", flag);
            app.RegisterCLRFieldGetter(field, get_unitType_3);
            app.RegisterCLRFieldSetter(field, set_unitType_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_unitType_3, AssignFromStack_unitType_3);
            field = type.GetField("unitLevel", flag);
            app.RegisterCLRFieldGetter(field, get_unitLevel_4);
            app.RegisterCLRFieldSetter(field, set_unitLevel_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_unitLevel_4, AssignFromStack_unitLevel_4);
            field = type.GetField("unitserverId", flag);
            app.RegisterCLRFieldGetter(field, get_unitserverId_5);
            app.RegisterCLRFieldSetter(field, set_unitserverId_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_unitserverId_5, AssignFromStack_unitserverId_5);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_unitId_0(ref object o)
        {
            return ((Client.TroopsCell)o).unitId;
        }

        static StackObject* CopyToStack_unitId_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsCell)o).unitId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_unitId_0(ref object o, object v)
        {
            ((Client.TroopsCell)o).unitId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_unitId_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @unitId = ptr_of_this_method->Value;
            ((Client.TroopsCell)o).unitId = @unitId;
            return ptr_of_this_method;
        }

        static object get_unitCount_1(ref object o)
        {
            return ((Client.TroopsCell)o).unitCount;
        }

        static StackObject* CopyToStack_unitCount_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsCell)o).unitCount;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_unitCount_1(ref object o, object v)
        {
            ((Client.TroopsCell)o).unitCount = (System.Int32)v;
        }

        static StackObject* AssignFromStack_unitCount_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @unitCount = ptr_of_this_method->Value;
            ((Client.TroopsCell)o).unitCount = @unitCount;
            return ptr_of_this_method;
        }

        static object get_unitMaxCount_2(ref object o)
        {
            return ((Client.TroopsCell)o).unitMaxCount;
        }

        static StackObject* CopyToStack_unitMaxCount_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsCell)o).unitMaxCount;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_unitMaxCount_2(ref object o, object v)
        {
            ((Client.TroopsCell)o).unitMaxCount = (System.Int32)v;
        }

        static StackObject* AssignFromStack_unitMaxCount_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @unitMaxCount = ptr_of_this_method->Value;
            ((Client.TroopsCell)o).unitMaxCount = @unitMaxCount;
            return ptr_of_this_method;
        }

        static object get_unitType_3(ref object o)
        {
            return ((Client.TroopsCell)o).unitType;
        }

        static StackObject* CopyToStack_unitType_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsCell)o).unitType;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_unitType_3(ref object o, object v)
        {
            ((Client.TroopsCell)o).unitType = (System.Int32)v;
        }

        static StackObject* AssignFromStack_unitType_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @unitType = ptr_of_this_method->Value;
            ((Client.TroopsCell)o).unitType = @unitType;
            return ptr_of_this_method;
        }

        static object get_unitLevel_4(ref object o)
        {
            return ((Client.TroopsCell)o).unitLevel;
        }

        static StackObject* CopyToStack_unitLevel_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsCell)o).unitLevel;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_unitLevel_4(ref object o, object v)
        {
            ((Client.TroopsCell)o).unitLevel = (System.Int32)v;
        }

        static StackObject* AssignFromStack_unitLevel_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @unitLevel = ptr_of_this_method->Value;
            ((Client.TroopsCell)o).unitLevel = @unitLevel;
            return ptr_of_this_method;
        }

        static object get_unitserverId_5(ref object o)
        {
            return ((Client.TroopsCell)o).unitserverId;
        }

        static StackObject* CopyToStack_unitserverId_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.TroopsCell)o).unitserverId;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_unitserverId_5(ref object o, object v)
        {
            ((Client.TroopsCell)o).unitserverId = (System.Int32)v;
        }

        static StackObject* AssignFromStack_unitserverId_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @unitserverId = ptr_of_this_method->Value;
            ((Client.TroopsCell)o).unitserverId = @unitserverId;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Client.TroopsCell();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
