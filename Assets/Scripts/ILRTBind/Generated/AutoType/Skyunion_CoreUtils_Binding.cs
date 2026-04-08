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
    unsafe class Skyunion_CoreUtils_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.CoreUtils);
            args = new Type[]{};
            method = type.GetMethod("ClearCore", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearCore_0);
            args = new Type[]{};
            method = type.GetMethod("get_dataService", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_dataService_1);
            args = new Type[]{};
            method = type.GetMethod("get_logService", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_logService_2);
            args = new Type[]{};
            method = type.GetMethod("get_audioService", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_audioService_3);
            args = new Type[]{};
            method = type.GetMethod("get_assetService", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_assetService_4);
            args = new Type[]{};
            method = type.GetMethod("get_uiManager", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_uiManager_5);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetGraphicLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetGraphicLevel_6);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("GetFileMd5", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetFileMd5_7);
            args = new Type[]{typeof(System.String), typeof(System.String), typeof(System.Action), typeof(System.Action<System.Int64>), typeof(System.Action<System.Int64, System.Int64, System.String>)};
            method = type.GetMethod("unZipFileAsync", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, unZipFileAsync_8);
            args = new Type[]{};
            method = type.GetMethod("RestarGame", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RestarGame_9);
            args = new Type[]{};
            method = type.GetMethod("get_inputManager", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_inputManager_10);
            args = new Type[]{};
            method = type.GetMethod("GetGraphicLevel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetGraphicLevel_11);
            args = new Type[]{};
            method = type.GetMethod("getScreenScale", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, getScreenScale_12);


        }


        static StackObject* ClearCore_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            Skyunion.CoreUtils.ClearCore();

            return __ret;
        }

        static StackObject* get_dataService_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.dataService;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_logService_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.logService;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_audioService_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.audioService;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_assetService_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.assetService;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_uiManager_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.uiManager;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetGraphicLevel_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @level = ptr_of_this_method->Value;


            Skyunion.CoreUtils.SetGraphicLevel(@level);

            return __ret;
        }

        static StackObject* GetFileMd5_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @path = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Skyunion.CoreUtils.GetFileMd5(@path);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* unZipFileAsync_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int64, System.Int64, System.String> @onProgress = (System.Action<System.Int64, System.Int64, System.String>)typeof(System.Action<System.Int64, System.Int64, System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.Int64> @onTotalFileCount = (System.Action<System.Int64>)typeof(System.Action<System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action @onError = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @fileDir = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.String @zipFile = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Skyunion.CoreUtils.unZipFileAsync(@zipFile, @fileDir, @onError, @onTotalFileCount, @onProgress);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* RestarGame_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            Skyunion.CoreUtils.RestarGame();

            return __ret;
        }

        static StackObject* get_inputManager_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.inputManager;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetGraphicLevel_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.GetGraphicLevel();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* getScreenScale_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Skyunion.CoreUtils.getScreenScale();

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }



    }
}
