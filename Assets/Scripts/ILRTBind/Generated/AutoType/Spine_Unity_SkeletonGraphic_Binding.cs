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
    unsafe class Spine_Unity_SkeletonGraphic_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Spine.Unity.SkeletonGraphic);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("Initialize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Initialize_0);
            args = new Type[]{};
            method = type.GetMethod("get_AnimationState", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_AnimationState_1);

            field = type.GetField("initialSkinName", flag);
            app.RegisterCLRFieldGetter(field, get_initialSkinName_0);
            app.RegisterCLRFieldSetter(field, set_initialSkinName_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_initialSkinName_0, AssignFromStack_initialSkinName_0);
            field = type.GetField("startingAnimation", flag);
            app.RegisterCLRFieldGetter(field, get_startingAnimation_1);
            app.RegisterCLRFieldSetter(field, set_startingAnimation_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_startingAnimation_1, AssignFromStack_startingAnimation_1);
            field = type.GetField("startingLoop", flag);
            app.RegisterCLRFieldGetter(field, get_startingLoop_2);
            app.RegisterCLRFieldSetter(field, set_startingLoop_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_startingLoop_2, AssignFromStack_startingLoop_2);
            field = type.GetField("initialFlipX", flag);
            app.RegisterCLRFieldGetter(field, get_initialFlipX_3);
            app.RegisterCLRFieldSetter(field, set_initialFlipX_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_initialFlipX_3, AssignFromStack_initialFlipX_3);


        }


        static StackObject* Initialize_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @overwrite = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Spine.Unity.SkeletonGraphic instance_of_this_method = (Spine.Unity.SkeletonGraphic)typeof(Spine.Unity.SkeletonGraphic).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Initialize(@overwrite);

            return __ret;
        }

        static StackObject* get_AnimationState_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Spine.Unity.SkeletonGraphic instance_of_this_method = (Spine.Unity.SkeletonGraphic)typeof(Spine.Unity.SkeletonGraphic).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.AnimationState;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_initialSkinName_0(ref object o)
        {
            return ((Spine.Unity.SkeletonGraphic)o).initialSkinName;
        }

        static StackObject* CopyToStack_initialSkinName_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Spine.Unity.SkeletonGraphic)o).initialSkinName;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_initialSkinName_0(ref object o, object v)
        {
            ((Spine.Unity.SkeletonGraphic)o).initialSkinName = (System.String)v;
        }

        static StackObject* AssignFromStack_initialSkinName_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @initialSkinName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Spine.Unity.SkeletonGraphic)o).initialSkinName = @initialSkinName;
            return ptr_of_this_method;
        }

        static object get_startingAnimation_1(ref object o)
        {
            return ((Spine.Unity.SkeletonGraphic)o).startingAnimation;
        }

        static StackObject* CopyToStack_startingAnimation_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Spine.Unity.SkeletonGraphic)o).startingAnimation;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_startingAnimation_1(ref object o, object v)
        {
            ((Spine.Unity.SkeletonGraphic)o).startingAnimation = (System.String)v;
        }

        static StackObject* AssignFromStack_startingAnimation_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @startingAnimation = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Spine.Unity.SkeletonGraphic)o).startingAnimation = @startingAnimation;
            return ptr_of_this_method;
        }

        static object get_startingLoop_2(ref object o)
        {
            return ((Spine.Unity.SkeletonGraphic)o).startingLoop;
        }

        static StackObject* CopyToStack_startingLoop_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Spine.Unity.SkeletonGraphic)o).startingLoop;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_startingLoop_2(ref object o, object v)
        {
            ((Spine.Unity.SkeletonGraphic)o).startingLoop = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_startingLoop_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @startingLoop = ptr_of_this_method->Value == 1;
            ((Spine.Unity.SkeletonGraphic)o).startingLoop = @startingLoop;
            return ptr_of_this_method;
        }

        static object get_initialFlipX_3(ref object o)
        {
            return ((Spine.Unity.SkeletonGraphic)o).initialFlipX;
        }

        static StackObject* CopyToStack_initialFlipX_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Spine.Unity.SkeletonGraphic)o).initialFlipX;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_initialFlipX_3(ref object o, object v)
        {
            ((Spine.Unity.SkeletonGraphic)o).initialFlipX = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_initialFlipX_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @initialFlipX = ptr_of_this_method->Value == 1;
            ((Spine.Unity.SkeletonGraphic)o).initialFlipX = @initialFlipX;
            return ptr_of_this_method;
        }



    }
}
