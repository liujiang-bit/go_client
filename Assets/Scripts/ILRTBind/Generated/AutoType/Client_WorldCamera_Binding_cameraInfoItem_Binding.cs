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
    unsafe class Client_WorldCamera_Binding_cameraInfoItem_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.WorldCamera.cameraInfoItem);

            field = type.GetField("dist", flag);
            app.RegisterCLRFieldGetter(field, get_dist_0);
            app.RegisterCLRFieldSetter(field, set_dist_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_dist_0, AssignFromStack_dist_0);
            field = type.GetField("dxf", flag);
            app.RegisterCLRFieldGetter(field, get_dxf_1);
            app.RegisterCLRFieldSetter(field, set_dxf_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_dxf_1, AssignFromStack_dxf_1);
            field = type.GetField("forward", flag);
            app.RegisterCLRFieldGetter(field, get_forward_2);
            app.RegisterCLRFieldSetter(field, set_forward_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_forward_2, AssignFromStack_forward_2);
            field = type.GetField("fov", flag);
            app.RegisterCLRFieldGetter(field, get_fov_3);
            app.RegisterCLRFieldSetter(field, set_fov_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_fov_3, AssignFromStack_fov_3);
            field = type.GetField("Id", flag);
            app.RegisterCLRFieldGetter(field, get_Id_4);
            app.RegisterCLRFieldSetter(field, set_Id_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_Id_4, AssignFromStack_Id_4);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_dist_0(ref object o)
        {
            return ((Client.WorldCamera.cameraInfoItem)o).dist;
        }

        static StackObject* CopyToStack_dist_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera.cameraInfoItem)o).dist;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_dist_0(ref object o, object v)
        {
            ((Client.WorldCamera.cameraInfoItem)o).dist = (System.Single)v;
        }

        static StackObject* AssignFromStack_dist_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @dist = *(float*)&ptr_of_this_method->Value;
            ((Client.WorldCamera.cameraInfoItem)o).dist = @dist;
            return ptr_of_this_method;
        }

        static object get_dxf_1(ref object o)
        {
            return ((Client.WorldCamera.cameraInfoItem)o).dxf;
        }

        static StackObject* CopyToStack_dxf_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera.cameraInfoItem)o).dxf;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_dxf_1(ref object o, object v)
        {
            ((Client.WorldCamera.cameraInfoItem)o).dxf = (System.Single)v;
        }

        static StackObject* AssignFromStack_dxf_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @dxf = *(float*)&ptr_of_this_method->Value;
            ((Client.WorldCamera.cameraInfoItem)o).dxf = @dxf;
            return ptr_of_this_method;
        }

        static object get_forward_2(ref object o)
        {
            return ((Client.WorldCamera.cameraInfoItem)o).forward;
        }

        static StackObject* CopyToStack_forward_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera.cameraInfoItem)o).forward;
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static void set_forward_2(ref object o, object v)
        {
            ((Client.WorldCamera.cameraInfoItem)o).forward = (UnityEngine.Vector3)v;
        }

        static StackObject* AssignFromStack_forward_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Vector3 @forward = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @forward, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @forward = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }
            ((Client.WorldCamera.cameraInfoItem)o).forward = @forward;
            return ptr_of_this_method;
        }

        static object get_fov_3(ref object o)
        {
            return ((Client.WorldCamera.cameraInfoItem)o).fov;
        }

        static StackObject* CopyToStack_fov_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera.cameraInfoItem)o).fov;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_fov_3(ref object o, object v)
        {
            ((Client.WorldCamera.cameraInfoItem)o).fov = (System.Single)v;
        }

        static StackObject* AssignFromStack_fov_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @fov = *(float*)&ptr_of_this_method->Value;
            ((Client.WorldCamera.cameraInfoItem)o).fov = @fov;
            return ptr_of_this_method;
        }

        static object get_Id_4(ref object o)
        {
            return ((Client.WorldCamera.cameraInfoItem)o).Id;
        }

        static StackObject* CopyToStack_Id_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.WorldCamera.cameraInfoItem)o).Id;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Id_4(ref object o, object v)
        {
            ((Client.WorldCamera.cameraInfoItem)o).Id = (System.String)v;
        }

        static StackObject* AssignFromStack_Id_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @Id = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.WorldCamera.cameraInfoItem)o).Id = @Id;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new Client.WorldCamera.cameraInfoItem();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
