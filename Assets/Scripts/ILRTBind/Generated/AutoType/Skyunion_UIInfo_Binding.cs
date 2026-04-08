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
    unsafe class Skyunion_UIInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Skyunion.UIInfo);

            field = type.GetField("View", flag);
            app.RegisterCLRFieldGetter(field, get_View_0);
            app.RegisterCLRFieldSetter(field, set_View_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_View_0, AssignFromStack_View_0);
            field = type.GetField("uiObj", flag);
            app.RegisterCLRFieldGetter(field, get_uiObj_1);
            app.RegisterCLRFieldSetter(field, set_uiObj_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_uiObj_1, AssignFromStack_uiObj_1);
            field = type.GetField("info", flag);
            app.RegisterCLRFieldGetter(field, get_info_2);
            app.RegisterCLRFieldSetter(field, set_info_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_info_2, AssignFromStack_info_2);

            app.RegisterCLRCreateArrayInstance(type, s => new Skyunion.UIInfo[s]);

            args = new Type[]{typeof(System.String), typeof(System.Type), typeof(Skyunion.UIViewInfo), typeof(Skyunion.EnumMaskStatus), typeof(Skyunion.UIInfo[]), typeof(System.Int32), typeof(System.Int32), typeof(System.Boolean), typeof(System.Boolean)};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_View_0(ref object o)
        {
            return ((Skyunion.UIInfo)o).View;
        }

        static StackObject* CopyToStack_View_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.UIInfo)o).View;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_View_0(ref object o, object v)
        {
            ((Skyunion.UIInfo)o).View = (Skyunion.GameView)v;
        }

        static StackObject* AssignFromStack_View_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Skyunion.GameView @View = (Skyunion.GameView)typeof(Skyunion.GameView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.UIInfo)o).View = @View;
            return ptr_of_this_method;
        }

        static object get_uiObj_1(ref object o)
        {
            return ((Skyunion.UIInfo)o).uiObj;
        }

        static StackObject* CopyToStack_uiObj_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.UIInfo)o).uiObj;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_uiObj_1(ref object o, object v)
        {
            ((Skyunion.UIInfo)o).uiObj = (UnityEngine.GameObject)v;
        }

        static StackObject* AssignFromStack_uiObj_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.GameObject @uiObj = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.UIInfo)o).uiObj = @uiObj;
            return ptr_of_this_method;
        }

        static object get_info_2(ref object o)
        {
            return ((Skyunion.UIInfo)o).info;
        }

        static StackObject* CopyToStack_info_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Skyunion.UIInfo)o).info;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_info_2(ref object o, object v)
        {
            ((Skyunion.UIInfo)o).info = (Skyunion.UIViewInfo)v;
        }

        static StackObject* AssignFromStack_info_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Skyunion.UIViewInfo @info = (Skyunion.UIViewInfo)typeof(Skyunion.UIViewInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Skyunion.UIInfo)o).info = @info;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 9);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @canAndroidBack = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isCheckPop = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @group = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int32 @uiId = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Skyunion.UIInfo[] @uiInfos = (Skyunion.UIInfo[])typeof(Skyunion.UIInfo[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            Skyunion.EnumMaskStatus @maskStatus = (Skyunion.EnumMaskStatus)typeof(Skyunion.EnumMaskStatus).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            Skyunion.UIViewInfo @info = (Skyunion.UIViewInfo)typeof(Skyunion.UIViewInfo).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 8);
            System.Type @viewClass = (System.Type)typeof(System.Type).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 9);
            System.String @assetName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = new Skyunion.UIInfo(@assetName, @viewClass, @info, @maskStatus, @uiInfos, @uiId, @group, @isCheckPop, @canAndroidBack);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
