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
    unsafe class Skyunion_IAudioService_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Skyunion.IAudioService);
            args = new Type[]{typeof(Skyunion.AudioHandler), typeof(System.Single)};
            method = type.GetMethod("SetHandlerVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetHandlerVolume_0);
            args = new Type[]{typeof(Skyunion.AudioHandler), typeof(System.Single), typeof(System.Single), typeof(System.Boolean)};
            method = type.GetMethod("FadeHandlerVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FadeHandlerVolume_1);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.GameObject), typeof(System.Action<Skyunion.AudioHandler>)};
            method = type.GetMethod("PlayOneShot3D", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayOneShot3D_2);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.GameObject), typeof(System.Action<Skyunion.AudioHandler>)};
            method = type.GetMethod("PlayLoop3D", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayLoop3D_3);
            args = new Type[]{typeof(Skyunion.AudioHandler)};
            method = type.GetMethod("StopByHandler", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopByHandler_4);
            args = new Type[]{typeof(Skyunion.AudioHandler)};
            method = type.GetMethod("OnHandlerDestroy", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnHandlerDestroy_5);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("PlayBgm", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayBgm_6);
            args = new Type[]{typeof(System.String), typeof(System.Action<Skyunion.AudioHandler>)};
            method = type.GetMethod("PlayOneShot", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayOneShot_7);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("StopByName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopByName_8);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("SetSfxVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetSfxVolume_9);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("SetMusicVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetMusicVolume_10);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetEnvSoundMaxPlayCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetEnvSoundMaxPlayCount_11);
            args = new Type[]{};
            method = type.GetMethod("GetCurBgmName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCurBgmName_12);
            args = new Type[]{};
            method = type.GetMethod("GetSfxVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetSfxVolume_13);
            args = new Type[]{};
            method = type.GetMethod("GetMusicVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetMusicVolume_14);


        }


        static StackObject* SetHandlerVolume_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @volume = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.AudioHandler @handler = (Skyunion.AudioHandler)typeof(Skyunion.AudioHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetHandlerVolume(@handler, @volume);

            return __ret;
        }

        static StackObject* FadeHandlerVolume_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @destroyWhenDone = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @speed = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @toVolume = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.AudioHandler @handler = (Skyunion.AudioHandler)typeof(Skyunion.AudioHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.FadeHandlerVolume(@handler, @toVolume, @speed, @destroyWhenDone);

            return __ret;
        }

        static StackObject* PlayOneShot3D_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<Skyunion.AudioHandler> @action = (System.Action<Skyunion.AudioHandler>)typeof(System.Action<Skyunion.AudioHandler>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @obj = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayOneShot3D(@name, @obj, @action);

            return __ret;
        }

        static StackObject* PlayLoop3D_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<Skyunion.AudioHandler> @action = (System.Action<Skyunion.AudioHandler>)typeof(System.Action<Skyunion.AudioHandler>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @obj = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayLoop3D(@name, @obj, @action);

            return __ret;
        }

        static StackObject* StopByHandler_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.AudioHandler @handler = (Skyunion.AudioHandler)typeof(Skyunion.AudioHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StopByHandler(@handler);

            return __ret;
        }

        static StackObject* OnHandlerDestroy_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.AudioHandler @handler = (Skyunion.AudioHandler)typeof(Skyunion.AudioHandler).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnHandlerDestroy(@handler);

            return __ret;
        }

        static StackObject* PlayBgm_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayBgm(@name);

            return __ret;
        }

        static StackObject* PlayOneShot_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<Skyunion.AudioHandler> @action = (System.Action<Skyunion.AudioHandler>)typeof(System.Action<Skyunion.AudioHandler>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayOneShot(@name, @action);

            return __ret;
        }

        static StackObject* StopByName_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StopByName(@name);

            return __ret;
        }

        static StackObject* SetSfxVolume_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @v = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetSfxVolume(@v);

            return __ret;
        }

        static StackObject* SetMusicVolume_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @v = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetMusicVolume(@v);

            return __ret;
        }

        static StackObject* SetEnvSoundMaxPlayCount_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @maxCount = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetEnvSoundMaxPlayCount(@maxCount);

            return __ret;
        }

        static StackObject* GetCurBgmName_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetCurBgmName();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetSfxVolume_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetSfxVolume();

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetMusicVolume_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Skyunion.IAudioService instance_of_this_method = (Skyunion.IAudioService)typeof(Skyunion.IAudioService).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetMusicVolume();

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }



    }
}
