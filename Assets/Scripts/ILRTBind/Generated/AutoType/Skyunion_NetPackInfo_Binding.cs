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
    unsafe class Skyunion_NetPackInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.NetPackInfo);

            field = type.GetField("packet_size", flag);
            app.RegisterCLRFieldGetter(field, get_packet_size_0);
            app.RegisterCLRFieldSetter(field, set_packet_size_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_packet_size_0, AssignFromStack_packet_size_0);
            field = type.GetField("content", flag);
            app.RegisterCLRFieldGetter(field, get_content_1);
            app.RegisterCLRFieldSetter(field, set_content_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_content_1, AssignFromStack_content_1);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_packet_size_0(ref object o)
        {
            return ((Skyunion.NetPackInfo)o).packet_size;
        }

        static StackObject* CopyToStack_packet_size_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.NetPackInfo)o).packet_size;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_packet_size_0(ref object o, object v)
        {
            ((Skyunion.NetPackInfo)o).packet_size = (System.Int32)v;
        }

        static StackObject* AssignFromStack_packet_size_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @packet_size = ptr_of_this_method->Value;
            ((Skyunion.NetPackInfo)o).packet_size = @packet_size;
            return ptr_of_this_method;
        }

        static object get_content_1(ref object o)
        {
            return ((Skyunion.NetPackInfo)o).content;
        }

        static StackObject* CopyToStack_content_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.NetPackInfo)o).content;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_content_1(ref object o, object v)
        {
            ((Skyunion.NetPackInfo)o).content = (System.IO.MemoryStream)v;
        }

        static StackObject* AssignFromStack_content_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.IO.MemoryStream @content = (System.IO.MemoryStream)typeof(System.IO.MemoryStream).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.NetPackInfo)o).content = @content;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Skyunion.NetPackInfo();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
