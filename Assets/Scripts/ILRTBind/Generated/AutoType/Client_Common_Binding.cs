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
    unsafe class Client_Common_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.Common);
            args = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3)};
            method = type.GetMethod("GetAngle360", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetAngle360_0);
            args = new Type[]{};
            method = type.GetMethod("Update", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Update_1);
            args = new Type[]{typeof(UnityEngine.Camera), typeof(System.Single), typeof(System.Single), typeof(System.String)};
            method = type.GetMethod("IsInViewPort2DS", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsInViewPort2DS_2);
            args = new Type[]{typeof(UnityEngine.Camera), typeof(System.Single), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("IsInViewPort2D", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsInViewPort2D_3);
            args = new Type[]{typeof(UnityEngine.Camera), typeof(UnityEngine.Vector3), typeof(System.Single)};
            method = type.GetMethod("IsInViewPort", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsInViewPort_4);
            args = new Type[]{};
            method = type.GetMethod("GetLodDistance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetLodDistance_5);
            args = new Type[]{typeof(UnityEngine.Plane).MakeByRefType(), typeof(UnityEngine.Ray), typeof(UnityEngine.Vector3).MakeByRefType()};
            method = type.GetMethod("GetRayPlaneIntersection", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetRayPlaneIntersection_6);
            args = new Type[]{typeof(UnityEngine.Camera), typeof(UnityEngine.Plane)};
            method = type.GetMethod("GetCameraCornors", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCameraCornors_7);

            field = type.GetField("DATA_DELIMITER_LEVEL_0", flag);
            app.RegisterCLRFieldGetter(field, get_DATA_DELIMITER_LEVEL_0_0);
            app.RegisterCLRFieldSetter(field, set_DATA_DELIMITER_LEVEL_0_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_DATA_DELIMITER_LEVEL_0_0, AssignFromStack_DATA_DELIMITER_LEVEL_0_0);
            field = type.GetField("DATA_DELIMITER_LEVEL_1", flag);
            app.RegisterCLRFieldGetter(field, get_DATA_DELIMITER_LEVEL_1_1);
            app.RegisterCLRFieldSetter(field, set_DATA_DELIMITER_LEVEL_1_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_DATA_DELIMITER_LEVEL_1_1, AssignFromStack_DATA_DELIMITER_LEVEL_1_1);
            field = type.GetField("DATA_DELIMITER_LEVEL_2", flag);
            app.RegisterCLRFieldGetter(field, get_DATA_DELIMITER_LEVEL_2_2);
            app.RegisterCLRFieldSetter(field, set_DATA_DELIMITER_LEVEL_2_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_DATA_DELIMITER_LEVEL_2_2, AssignFromStack_DATA_DELIMITER_LEVEL_2_2);
            field = type.GetField("DATA_DELIMITER_LEVEL_3", flag);
            app.RegisterCLRFieldGetter(field, get_DATA_DELIMITER_LEVEL_3_3);
            app.RegisterCLRFieldSetter(field, set_DATA_DELIMITER_LEVEL_3_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_DATA_DELIMITER_LEVEL_3_3, AssignFromStack_DATA_DELIMITER_LEVEL_3_3);


        }


        static StackObject* GetAngle360_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @to_ = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @to_, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @to_ = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @from_ = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @from_, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @from_ = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = Client.Common.GetAngle360(@from_, @to_);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Update_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            Client.Common.Update();

            return __ret;
        }

        static StackObject* IsInViewPort2DS_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Camera @camera = (UnityEngine.Camera)typeof(UnityEngine.Camera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.Common.IsInViewPort2DS(@camera, @x, @y, @name);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsInViewPort2D_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @border = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Camera @camera = (UnityEngine.Camera)typeof(UnityEngine.Camera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.Common.IsInViewPort2D(@camera, @x, @y, @border);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsInViewPort_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @border = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @pos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Camera @camera = (UnityEngine.Camera)typeof(UnityEngine.Camera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.Common.IsInViewPort(@camera, @pos, @border);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetLodDistance_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.Common.GetLodDistance();

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetRayPlaneIntersection_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @intersection = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @intersection, __intp, ptr_of_this_method, __mStack, false);
            } else {
                ptr_of_this_method = ILIntepreter.GetObjectAndResolveReference(ptr_of_this_method);
                @intersection = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Ray @ray = (UnityEngine.Ray)typeof(UnityEngine.Ray).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Plane @plane = (UnityEngine.Plane)typeof(UnityEngine.Plane).CheckCLRTypes(__intp.RetriveObject(ptr_of_this_method, __mStack));


            var result_of_this_method = Client.Common.GetRayPlaneIntersection(ref @plane, @ray, out @intersection);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                        ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.WriteBackValue(__domain, ptr_of_this_method, __mStack, ref intersection);
                } else {
                        object ___obj = @intersection;
                        if (___dst->ObjectType >= ObjectTypes.Object)
                        {
                            if (___obj is CrossBindingAdaptorType)
                                ___obj = ((CrossBindingAdaptorType)___obj).ILInstance;
                            __mStack[___dst->Value] = ___obj;
                        }
                        else
                        {
                            ILIntepreter.UnboxObject(___dst, ___obj, __mStack, __domain);
                        }
                }
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @intersection;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @intersection);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @intersection;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @intersection);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as UnityEngine.Vector3[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @intersection;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            __intp.FreeStackValueType(ptr_of_this_method);
            __intp.Free(ptr_of_this_method);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            switch(ptr_of_this_method->ObjectType)
            {
                case ObjectTypes.StackObjectReference:
                    {
                        var ___dst = ILIntepreter.ResolveReference(ptr_of_this_method);
                        object ___obj = @plane;
                        if (___dst->ObjectType >= ObjectTypes.Object)
                        {
                            if (___obj is CrossBindingAdaptorType)
                                ___obj = ((CrossBindingAdaptorType)___obj).ILInstance;
                            __mStack[___dst->Value] = ___obj;
                        }
                        else
                        {
                            ILIntepreter.UnboxObject(___dst, ___obj, __mStack, __domain);
                        }
                    }
                    break;
                case ObjectTypes.FieldReference:
                    {
                        var ___obj = __mStack[ptr_of_this_method->Value];
                        if(___obj is ILTypeInstance)
                        {
                            ((ILTypeInstance)___obj)[ptr_of_this_method->ValueLow] = @plane;
                        }
                        else
                        {
                            var ___type = __domain.GetType(___obj.GetType()) as CLRType;
                            ___type.SetFieldValue(ptr_of_this_method->ValueLow, ref ___obj, @plane);
                        }
                    }
                    break;
                case ObjectTypes.StaticFieldReference:
                    {
                        var ___type = __domain.GetType(ptr_of_this_method->Value);
                        if(___type is ILType)
                        {
                            ((ILType)___type).StaticInstance[ptr_of_this_method->ValueLow] = @plane;
                        }
                        else
                        {
                            ((CLRType)___type).SetStaticFieldValue(ptr_of_this_method->ValueLow, @plane);
                        }
                    }
                    break;
                 case ObjectTypes.ArrayReference:
                    {
                        var instance_of_arrayReference = __mStack[ptr_of_this_method->Value] as UnityEngine.Plane[];
                        instance_of_arrayReference[ptr_of_this_method->ValueLow] = @plane;
                    }
                    break;
            }

            __intp.Free(ptr_of_this_method);
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetCameraCornors_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Plane @plane = (UnityEngine.Plane)typeof(UnityEngine.Plane).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Camera @camera = (UnityEngine.Camera)typeof(UnityEngine.Camera).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.Common.GetCameraCornors(@camera, @plane);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_DATA_DELIMITER_LEVEL_0_0(ref object o)
        {
            return Client.Common.DATA_DELIMITER_LEVEL_0;
        }

        static StackObject* CopyToStack_DATA_DELIMITER_LEVEL_0_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.Common.DATA_DELIMITER_LEVEL_0;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DATA_DELIMITER_LEVEL_0_0(ref object o, object v)
        {
            Client.Common.DATA_DELIMITER_LEVEL_0 = (System.Char[])v;
        }

        static StackObject* AssignFromStack_DATA_DELIMITER_LEVEL_0_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Char[] @DATA_DELIMITER_LEVEL_0 = (System.Char[])typeof(System.Char[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Client.Common.DATA_DELIMITER_LEVEL_0 = @DATA_DELIMITER_LEVEL_0;
            return ptr_of_this_method;
        }

        static object get_DATA_DELIMITER_LEVEL_1_1(ref object o)
        {
            return Client.Common.DATA_DELIMITER_LEVEL_1;
        }

        static StackObject* CopyToStack_DATA_DELIMITER_LEVEL_1_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.Common.DATA_DELIMITER_LEVEL_1;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DATA_DELIMITER_LEVEL_1_1(ref object o, object v)
        {
            Client.Common.DATA_DELIMITER_LEVEL_1 = (System.Char[])v;
        }

        static StackObject* AssignFromStack_DATA_DELIMITER_LEVEL_1_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Char[] @DATA_DELIMITER_LEVEL_1 = (System.Char[])typeof(System.Char[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Client.Common.DATA_DELIMITER_LEVEL_1 = @DATA_DELIMITER_LEVEL_1;
            return ptr_of_this_method;
        }

        static object get_DATA_DELIMITER_LEVEL_2_2(ref object o)
        {
            return Client.Common.DATA_DELIMITER_LEVEL_2;
        }

        static StackObject* CopyToStack_DATA_DELIMITER_LEVEL_2_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.Common.DATA_DELIMITER_LEVEL_2;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DATA_DELIMITER_LEVEL_2_2(ref object o, object v)
        {
            Client.Common.DATA_DELIMITER_LEVEL_2 = (System.Char[])v;
        }

        static StackObject* AssignFromStack_DATA_DELIMITER_LEVEL_2_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Char[] @DATA_DELIMITER_LEVEL_2 = (System.Char[])typeof(System.Char[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Client.Common.DATA_DELIMITER_LEVEL_2 = @DATA_DELIMITER_LEVEL_2;
            return ptr_of_this_method;
        }

        static object get_DATA_DELIMITER_LEVEL_3_3(ref object o)
        {
            return Client.Common.DATA_DELIMITER_LEVEL_3;
        }

        static StackObject* CopyToStack_DATA_DELIMITER_LEVEL_3_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = Client.Common.DATA_DELIMITER_LEVEL_3;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DATA_DELIMITER_LEVEL_3_3(ref object o, object v)
        {
            Client.Common.DATA_DELIMITER_LEVEL_3 = (System.Char[])v;
        }

        static StackObject* AssignFromStack_DATA_DELIMITER_LEVEL_3_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Char[] @DATA_DELIMITER_LEVEL_3 = (System.Char[])typeof(System.Char[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            Client.Common.DATA_DELIMITER_LEVEL_3 = @DATA_DELIMITER_LEVEL_3;
            return ptr_of_this_method;
        }



    }
}
