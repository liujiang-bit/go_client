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
    unsafe class Client_FSM_WorkerFMS_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.FSM.WorkerFMS);
            args = new Type[]{typeof(UnityEngine.Vector2)};
            method = type.GetMethod("RunToCitizen", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RunToCitizen_0);
            args = new Type[]{};
            method = type.GetMethod("Clear", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Clear_1);
            args = new Type[]{};
            method = type.GetMethod("IsEndStep", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsEndStep_2);
            args = new Type[]{typeof(global::People)};
            method = type.GetMethod("set_Owner", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_Owner_3);
            args = new Type[]{};
            method = type.GetMethod("IsWorking", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsWorking_4);
            args = new Type[]{typeof(System.Collections.Generic.List<UnityEngine.Vector2>), typeof(System.Collections.Generic.List<UnityEngine.Vector2>), typeof(System.Int64), typeof(System.Int64), typeof(System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE>), typeof(System.Action<System.Int64, System.Int64>)};
            method = type.GetMethod("GoGetWaterToFireBuild", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GoGetWaterToFireBuild_5);
            args = new Type[]{};
            method = type.GetMethod("WalkAround", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, WalkAround_6);
            args = new Type[]{typeof(UnityEngine.Vector2)};
            method = type.GetMethod("SetBorn", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetBorn_7);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("IsWorkForBuild", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsWorkForBuild_8);
            args = new Type[]{typeof(System.Collections.Generic.List<UnityEngine.Vector2>), typeof(System.Int64)};
            method = type.GetMethod("GoOnAroundBuild", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GoOnAroundBuild_9);
            args = new Type[]{typeof(System.Collections.Generic.List<UnityEngine.Vector2>), typeof(System.Collections.Generic.List<UnityEngine.Vector2>), typeof(System.Int64), typeof(System.Int64), typeof(System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE>)};
            method = type.GetMethod("GoGetResToBuild", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GoGetResToBuild_10);
            args = new Type[]{};
            method = type.GetMethod("GoOnWalkAround", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GoOnWalkAround_11);
            args = new Type[]{typeof(System.Int64), typeof(System.Int64)};
            method = type.GetMethod("IsWorkResBuildMove", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsWorkResBuildMove_12);
            args = new Type[]{typeof(System.Int64), typeof(System.Int64)};
            method = type.GetMethod("IsWorkFireBuildMove", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsWorkFireBuildMove_13);
            args = new Type[]{};
            method = type.GetMethod("CheckWaklAroundEndPoint", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CheckWaklAroundEndPoint_14);

            field = type.GetField("Finder", flag);
            app.RegisterCLRFieldGetter(field, get_Finder_0);
            app.RegisterCLRFieldSetter(field, set_Finder_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Finder_0, AssignFromStack_Finder_0);
            field = type.GetField("buildType", flag);
            app.RegisterCLRFieldGetter(field, get_buildType_1);
            app.RegisterCLRFieldSetter(field, set_buildType_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_buildType_1, AssignFromStack_buildType_1);
            field = type.GetField("buildIndex", flag);
            app.RegisterCLRFieldGetter(field, get_buildIndex_2);
            app.RegisterCLRFieldSetter(field, set_buildIndex_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_buildIndex_2, AssignFromStack_buildIndex_2);
            field = type.GetField("index", flag);
            app.RegisterCLRFieldGetter(field, get_index_3);
            app.RegisterCLRFieldSetter(field, set_index_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_index_3, AssignFromStack_index_3);
            field = type.GetField("ResBuildIndex", flag);
            app.RegisterCLRFieldGetter(field, get_ResBuildIndex_4);
            app.RegisterCLRFieldSetter(field, set_ResBuildIndex_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_ResBuildIndex_4, AssignFromStack_ResBuildIndex_4);


        }


        static StackObject* RunToCitizen_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @endPos = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @endPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @endPos = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RunToCitizen(@endPos);

            return __ret;
        }

        static StackObject* Clear_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Clear();

            return __ret;
        }

        static StackObject* IsEndStep_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsEndStep();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* set_Owner_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::People @value = (global::People)typeof(global::People).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Owner = value;

            return __ret;
        }

        static StackObject* IsWorking_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsWorking();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GoGetWaterToFireBuild_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 7);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int64, System.Int64> @fireCountCall = (System.Action<System.Int64, System.Int64>)typeof(System.Action<System.Int64, System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE> @resourceTypes = (System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE>)typeof(System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @resBuildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Int64 @buildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Collections.Generic.List<UnityEngine.Vector2> @buildOutSide = (System.Collections.Generic.List<UnityEngine.Vector2>)typeof(System.Collections.Generic.List<UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            System.Collections.Generic.List<UnityEngine.Vector2> @resBuildOutSide = (System.Collections.Generic.List<UnityEngine.Vector2>)typeof(System.Collections.Generic.List<UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.GoGetWaterToFireBuild(@resBuildOutSide, @buildOutSide, @buildID, @resBuildID, @resourceTypes, @fireCountCall);

            return __ret;
        }

        static StackObject* WalkAround_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.WalkAround();

            return __ret;
        }

        static StackObject* SetBorn_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector2 @born = new UnityEngine.Vector2();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.ParseValue(ref @born, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @born = (UnityEngine.Vector2)typeof(UnityEngine.Vector2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetBorn(@born);

            return __ret;
        }

        static StackObject* IsWorkForBuild_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @buildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsWorkForBuild(@buildID);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GoOnAroundBuild_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @buildIndex = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<UnityEngine.Vector2> @buildOutSide = (System.Collections.Generic.List<UnityEngine.Vector2>)typeof(System.Collections.Generic.List<UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.GoOnAroundBuild(@buildOutSide, @buildIndex);

            return __ret;
        }

        static StackObject* GoGetResToBuild_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 6);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE> @resourceTypes = (System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE>)typeof(System.Collections.Generic.List<global::People.ENMU_CARRY_RESOURCE_TYPE>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64 @resBuildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int64 @buildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Collections.Generic.List<UnityEngine.Vector2> @buildOutSide = (System.Collections.Generic.List<UnityEngine.Vector2>)typeof(System.Collections.Generic.List<UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.Collections.Generic.List<UnityEngine.Vector2> @resBuildOutSide = (System.Collections.Generic.List<UnityEngine.Vector2>)typeof(System.Collections.Generic.List<UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.GoGetResToBuild(@resBuildOutSide, @buildOutSide, @buildID, @resBuildID, @resourceTypes);

            return __ret;
        }

        static StackObject* GoOnWalkAround_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.GoOnWalkAround();

            return __ret;
        }

        static StackObject* IsWorkResBuildMove_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @buildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64 @StbuildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsWorkResBuildMove(@StbuildID, @buildID);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* IsWorkFireBuildMove_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @buildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64 @StbuildID = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.IsWorkFireBuildMove(@StbuildID, @buildID);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* CheckWaklAroundEndPoint_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.FSM.WorkerFMS instance_of_this_method = (Client.FSM.WorkerFMS)typeof(Client.FSM.WorkerFMS).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CheckWaklAroundEndPoint();

            return __ret;
        }


        static object get_Finder_0(ref object o)
        {
            return ((Client.FSM.WorkerFMS)o).Finder;
        }

        static StackObject* CopyToStack_Finder_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FSM.WorkerFMS)o).Finder;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Finder_0(ref object o, object v)
        {
            ((Client.FSM.WorkerFMS)o).Finder = (global::TownSearch)v;
        }

        static StackObject* AssignFromStack_Finder_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TownSearch @Finder = (global::TownSearch)typeof(global::TownSearch).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.FSM.WorkerFMS)o).Finder = @Finder;
            return ptr_of_this_method;
        }

        static object get_buildType_1(ref object o)
        {
            return ((Client.FSM.WorkerFMS)o).buildType;
        }

        static StackObject* CopyToStack_buildType_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FSM.WorkerFMS)o).buildType;
            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_buildType_1(ref object o, object v)
        {
            ((Client.FSM.WorkerFMS)o).buildType = (System.Int64)v;
        }

        static StackObject* AssignFromStack_buildType_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64 @buildType = *(long*)&ptr_of_this_method->Value;
            ((Client.FSM.WorkerFMS)o).buildType = @buildType;
            return ptr_of_this_method;
        }

        static object get_buildIndex_2(ref object o)
        {
            return ((Client.FSM.WorkerFMS)o).buildIndex;
        }

        static StackObject* CopyToStack_buildIndex_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FSM.WorkerFMS)o).buildIndex;
            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_buildIndex_2(ref object o, object v)
        {
            ((Client.FSM.WorkerFMS)o).buildIndex = (System.Int64)v;
        }

        static StackObject* AssignFromStack_buildIndex_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64 @buildIndex = *(long*)&ptr_of_this_method->Value;
            ((Client.FSM.WorkerFMS)o).buildIndex = @buildIndex;
            return ptr_of_this_method;
        }

        static object get_index_3(ref object o)
        {
            return ((Client.FSM.WorkerFMS)o).index;
        }

        static StackObject* CopyToStack_index_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FSM.WorkerFMS)o).index;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_index_3(ref object o, object v)
        {
            ((Client.FSM.WorkerFMS)o).index = (System.Int32)v;
        }

        static StackObject* AssignFromStack_index_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @index = ptr_of_this_method->Value;
            ((Client.FSM.WorkerFMS)o).index = @index;
            return ptr_of_this_method;
        }

        static object get_ResBuildIndex_4(ref object o)
        {
            return ((Client.FSM.WorkerFMS)o).ResBuildIndex;
        }

        static StackObject* CopyToStack_ResBuildIndex_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.FSM.WorkerFMS)o).ResBuildIndex;
            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_ResBuildIndex_4(ref object o, object v)
        {
            ((Client.FSM.WorkerFMS)o).ResBuildIndex = (System.Int64)v;
        }

        static StackObject* AssignFromStack_ResBuildIndex_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int64 @ResBuildIndex = *(long*)&ptr_of_this_method->Value;
            ((Client.FSM.WorkerFMS)o).ResBuildIndex = @ResBuildIndex;
            return ptr_of_this_method;
        }



    }
}
