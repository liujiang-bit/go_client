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
    unsafe class Skyunion_UIViewInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.UIViewInfo);

            field = type.GetField("layer", flag);
            app.RegisterCLRFieldGetter(field, get_layer_0);
            app.RegisterCLRFieldSetter(field, set_layer_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_layer_0, AssignFromStack_layer_0);

            args = new Type[]{typeof(Skyunion.UIViewType), typeof(Skyunion.UILayer), typeof(Skyunion.UIAddMode), typeof(Skyunion.UICloseMode)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_layer_0(ref object o)
        {
            return ((Skyunion.UIViewInfo)o).layer;
        }

        static StackObject* CopyToStack_layer_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.UIViewInfo)o).layer;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_layer_0(ref object o, object v)
        {
            ((Skyunion.UIViewInfo)o).layer = (Skyunion.UILayer)v;
        }

        static StackObject* AssignFromStack_layer_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Skyunion.UILayer @layer = (Skyunion.UILayer)typeof(Skyunion.UILayer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.UIViewInfo)o).layer = @layer;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.UICloseMode @_closeMode = (Skyunion.UICloseMode)typeof(Skyunion.UICloseMode).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.UIAddMode @_addMode = (Skyunion.UIAddMode)typeof(Skyunion.UIAddMode).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Skyunion.UILayer @_layer = (Skyunion.UILayer)typeof(Skyunion.UILayer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.UIViewType @_viewType = (Skyunion.UIViewType)typeof(Skyunion.UIViewType).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new Skyunion.UIViewInfo(@_viewType, @_layer, @_addMode, @_closeMode);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
