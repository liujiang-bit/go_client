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
    unsafe class Client_RssAniGroup_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Client.RssAniGroup);
            args = new Type[]{};
            method = type.GetMethod("Register", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Register_0);
            args = new Type[]{typeof(UnityEngine.RectTransform)};
            method = type.GetMethod("SetStartPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStartPos_1);
            args = new Type[]{typeof(UnityEngine.RectTransform)};
            method = type.GetMethod("SetEndPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetEndPos_2);
            args = new Type[]{typeof(UnityEngine.Vector3), typeof(UnityEngine.Vector3)};
            method = type.GetMethod("Register", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Register_3);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("SetScale2D", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetScale2D_4);
            args = new Type[]{typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("SetAniTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetAniTime_5);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("SetStartReachCallback", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStartReachCallback_6);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("SetAniDoneCallBack", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetAniDoneCallBack_7);
            args = new Type[]{typeof(UnityEngine.GameObject[])};
            method = type.GetMethod("SetAniGameObjects", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetAniGameObjects_8);
            args = new Type[]{typeof(UnityEngine.Vector3)};
            method = type.GetMethod("SetStartPos", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetStartPos_9);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(UnityEngine.Vector3), typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("FlyItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FlyItem_10);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetDestroy", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDestroy_11);

            field = type.GetField("m_rssAniItems", flag);
            app.RegisterCLRFieldGetter(field, get_m_rssAniItems_0);
            app.RegisterCLRFieldSetter(field, set_m_rssAniItems_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_m_rssAniItems_0, AssignFromStack_m_rssAniItems_0);


        }


        static StackObject* Register_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.RssAniGroup.Register();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetStartPos_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.RectTransform @uiObj = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetStartPos(@uiObj);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetEndPos_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.RectTransform @uiObj = (UnityEngine.RectTransform)typeof(UnityEngine.RectTransform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetEndPos(@uiObj);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Register_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @targetUIPos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @targetUIPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @targetUIPos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @startPos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @startPos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @startPos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }


            var result_of_this_method = Client.RssAniGroup.Register(@startPos, @targetUIPos);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetScale2D_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @end = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @start = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetScale2D(@start, @end);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetAniTime_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @step2Time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @step1Time = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetAniTime(@step1Time, @step2Time);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetStartReachCallback_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callBack = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetStartReachCallback(@callBack);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetAniDoneCallBack_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callBack = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetAniDoneCallBack(@callBack);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetAniGameObjects_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject[] @gameObjs = (UnityEngine.GameObject[])typeof(UnityEngine.GameObject[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetAniGameObjects(@gameObjs);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetStartPos_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Vector3 @pos = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @pos, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @pos = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetStartPos(@pos);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* FlyItem_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @randomDirection = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @count = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.Vector3 @scale = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @scale, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @scale = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.FlyItem(@go, @scale, @count, @randomDirection);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetDestroy_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @destroy = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Client.RssAniGroup instance_of_this_method = (Client.RssAniGroup)typeof(Client.RssAniGroup).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetDestroy(@destroy);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_m_rssAniItems_0(ref object o)
        {
            return ((Client.RssAniGroup)o).m_rssAniItems;
        }

        static StackObject* CopyToStack_m_rssAniItems_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Client.RssAniGroup)o).m_rssAniItems;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_m_rssAniItems_0(ref object o, object v)
        {
            ((Client.RssAniGroup)o).m_rssAniItems = (System.Collections.Generic.List<Client.RssAniItem>)v;
        }

        static StackObject* AssignFromStack_m_rssAniItems_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<Client.RssAniItem> @m_rssAniItems = (System.Collections.Generic.List<Client.RssAniItem>)typeof(System.Collections.Generic.List<Client.RssAniItem>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Client.RssAniGroup)o).m_rssAniItems = @m_rssAniItems;
            return ptr_of_this_method;
        }



    }
}
