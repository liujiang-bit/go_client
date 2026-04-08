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
    unsafe class Client_LightManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.LightManager);
            args = new Type[]{typeof(UnityEngine.Color)};
            method = type.GetMethod("SetCameraFillBaseColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetCameraFillBaseColor_0);
            args = new Type[]{};
            method = type.GetMethod("get_isNight", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isNight_1);
            args = new Type[]{typeof(UnityEngine.Color), typeof(UnityEngine.Color), typeof(System.Single), typeof(UnityEngine.Color), typeof(UnityEngine.Color), typeof(System.Single), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("UpdateLighting", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateLighting_2);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetIsNight", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetIsNight_3);
            args = new Type[]{typeof(UnityEngine.Color), typeof(UnityEngine.Color), typeof(System.Single), typeof(System.Single), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("UpdateThunderLighting", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateThunderLighting_4);


        }


        static StackObject* SetCameraFillBaseColor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @value = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.LightManager instance_of_this_method = (Client.LightManager)typeof(Client.LightManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetCameraFillBaseColor(@value);

            return __ret;
        }

        static StackObject* get_isNight_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Client.LightManager instance_of_this_method = (Client.LightManager)typeof(Client.LightManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isNight;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* UpdateLighting_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 9);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @passed_time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @update_time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @new_direction_intensity = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Color @new_direction_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.Color @new_ambient_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            System.Single @org_direction_intensity = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            UnityEngine.Color @org_direction_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 8);
            UnityEngine.Color @org_ambient_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 9);
            Client.LightManager instance_of_this_method = (Client.LightManager)typeof(Client.LightManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateLighting(@org_ambient_color, @org_direction_color, @org_direction_intensity, @new_ambient_color, @new_direction_color, @new_direction_intensity, @update_time, @passed_time);

            return __ret;
        }

        static StackObject* SetIsNight_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @is_night = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.LightManager instance_of_this_method = (Client.LightManager)typeof(Client.LightManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetIsNight(@is_night);

            return __ret;
        }

        static StackObject* UpdateThunderLighting_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 7);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @passed_time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @fade_out_time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @fade_in_time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.Single @new_direction_intensity = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            UnityEngine.Color @new_direction_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            UnityEngine.Color @new_ambient_color = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            Client.LightManager instance_of_this_method = (Client.LightManager)typeof(Client.LightManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateThunderLighting(@new_ambient_color, @new_direction_color, @new_direction_intensity, @fade_in_time, @fade_out_time, @passed_time);

            return __ret;
        }



    }
}
