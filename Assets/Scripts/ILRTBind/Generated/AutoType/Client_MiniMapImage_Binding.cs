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
    unsafe class Client_MiniMapImage_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.MiniMapImage);
            args = new Type[]{typeof(UnityEngine.Vector2[])};
            method = type.GetMethod("SetUvs", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetUvs_0);
            args = new Type[]{typeof(UnityEngine.Vector2[])};
            method = type.GetMethod("SetPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetPos_1);

            field = type.GetField("lineWidth", flag);
            app.RegisterCLRFieldGetter(field, get_lineWidth_0);
            app.RegisterCLRFieldSetter(field, set_lineWidth_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_lineWidth_0, AssignFromStack_lineWidth_0);


        }


        static StackObject* SetUvs_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2[] @uvs = (UnityEngine.Vector2[])typeof(UnityEngine.Vector2[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.MiniMapImage instance_of_this_method = (Client.MiniMapImage)typeof(Client.MiniMapImage).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetUvs(@uvs);

            return __ret;
        }

        static StackObject* SetPos_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2[] @pos = (UnityEngine.Vector2[])typeof(UnityEngine.Vector2[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.MiniMapImage instance_of_this_method = (Client.MiniMapImage)typeof(Client.MiniMapImage).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetPos(@pos);

            return __ret;
        }


        static object get_lineWidth_0(ref object o)
        {
            return ((Client.MiniMapImage)o).lineWidth;
        }

        static StackObject* CopyToStack_lineWidth_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.MiniMapImage)o).lineWidth;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_lineWidth_0(ref object o, object v)
        {
            ((Client.MiniMapImage)o).lineWidth = (System.Single)v;
        }

        static StackObject* AssignFromStack_lineWidth_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @lineWidth = *(float*)&ptr_of_this_method->Value;
            ((Client.MiniMapImage)o).lineWidth = @lineWidth;
            return ptr_of_this_method;
        }



    }
}
