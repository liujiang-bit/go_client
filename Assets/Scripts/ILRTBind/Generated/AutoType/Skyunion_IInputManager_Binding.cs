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
    unsafe class Skyunion_IInputManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.IInputManager);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("AddTouch2DEvent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddTouch2DEvent_0);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("RemoveTouch2DEvent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveTouch2DEvent_1);
            args = new Type[]{};
            method = type.GetMethod("GetTouchCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetTouchCount_2);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32, System.String, System.String>), typeof(System.Action<System.Int32, System.Int32, System.String, System.String>), typeof(System.Action<System.Int32, System.Int32, System.String, System.String>), typeof(System.Action<System.Int32, System.Int32, System.String, System.String>)};
            method = type.GetMethod("SetTouch3DEvent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetTouch3DEvent_3);
            args = new Type[]{typeof(System.Int32), typeof(System.Int32), typeof(System.Int32).MakeByRefType()};
            method = type.GetMethod("RayCashHit3D", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RayCashHit3D_4);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("RemoveTouchEvent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RemoveTouchEvent_5);
            args = new Type[]{typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>), typeof(System.Action<System.Int32, System.Int32>)};
            method = type.GetMethod("AddTouchEvent", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddTouchEvent_6);


        }


        static StackObject* AddTouch2DEvent_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @eventOnToucheEnd = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Int32, System.Int32> @eventOnToucheMove = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<System.Int32, System.Int32> @eventOnToucheBegin = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IInputManager instance_of_this_method = (Skyunion.IInputManager)typeof(Skyunion.IInputManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddTouch2DEvent(@eventOnToucheBegin, @eventOnToucheMove, @eventOnToucheEnd);

            return __ret;
        }

        static StackObject* RemoveTouch2DEvent_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @eventOnToucheEnd = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Int32, System.Int32> @eventOnToucheMove = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<System.Int32, System.Int32> @eventOnToucheBegin = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IInputManager instance_of_this_method = (Skyunion.IInputManager)typeof(Skyunion.IInputManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveTouch2DEvent(@eventOnToucheBegin, @eventOnToucheMove, @eventOnToucheEnd);

            return __ret;
        }

        static StackObject* GetTouchCount_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IInputManager instance_of_this_method = (Skyunion.IInputManager)typeof(Skyunion.IInputManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetTouchCount();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* SetTouch3DEvent_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32, System.String, System.String> @eventOnTouche3DReleaseOutside = (System.Action<System.Int32, System.Int32, System.String, System.String>)typeof(System.Action<System.Int32, System.Int32, System.String, System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Int32, System.Int32, System.String, System.String> @eventOnTouche3DEnd = (System.Action<System.Int32, System.Int32, System.String, System.String>)typeof(System.Action<System.Int32, System.Int32, System.String, System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<System.Int32, System.Int32, System.String, System.String> @eventOnTouche3D = (System.Action<System.Int32, System.Int32, System.String, System.String>)typeof(System.Action<System.Int32, System.Int32, System.String, System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Action<System.Int32, System.Int32, System.String, System.String> @eventOnTouche3DBegin = (System.Action<System.Int32, System.Int32, System.String, System.String>)typeof(System.Action<System.Int32, System.Int32, System.String, System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Skyunion.IInputManager instance_of_this_method = (Skyunion.IInputManager)typeof(Skyunion.IInputManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetTouch3DEvent(@eventOnTouche3DBegin, @eventOnTouche3D, @eventOnTouche3DEnd, @eventOnTouche3DReleaseOutside);

            return __ret;
        }

        static StackObject* RayCashHit3D_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @nIndex = __intp.RetriveInt32(ptr_of_this_method, __mStack);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @y = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @x = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IInputManager instance_of_this_method = (Skyunion.IInputManager)typeof(Skyunion.IInputManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));

            var result_of_this_method = instance_of_this_method.RayCashHit3D(@x, @y, out @nIndex);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        ___dst->ObjectType = ObjectTypes.Integer;
                        ___dst->Value = @nIndex;
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @nIndex;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @nIndex);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @nIndex;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @nIndex);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as System.Int32[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @nIndex;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            __intp.Free(ptr_of_this_method);
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* RemoveTouchEvent_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @eventOnToucheEnd = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Int32, System.Int32> @eventOnToucheMove = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<System.Int32, System.Int32> @eventOnToucheBegin = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IInputManager instance_of_this_method = (Skyunion.IInputManager)typeof(Skyunion.IInputManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RemoveTouchEvent(@eventOnToucheBegin, @eventOnToucheMove, @eventOnToucheEnd);

            return __ret;
        }

        static StackObject* AddTouchEvent_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.Int32> @eventOnToucheEnd = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Int32, System.Int32> @eventOnToucheMove = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<System.Int32, System.Int32> @eventOnToucheBegin = (System.Action<System.Int32, System.Int32>)typeof(System.Action<System.Int32, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IInputManager instance_of_this_method = (Skyunion.IInputManager)typeof(Skyunion.IInputManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddTouchEvent(@eventOnToucheBegin, @eventOnToucheMove, @eventOnToucheEnd);

            return __ret;
        }



    }
}
