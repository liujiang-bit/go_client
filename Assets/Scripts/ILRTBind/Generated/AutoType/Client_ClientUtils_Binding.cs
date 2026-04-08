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
    unsafe class Client_ClientUtils_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Client.ClientUtils);
            args = new Type[]{};
            method = type.GetMethod("get_hudManager", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_hudManager_0);
            args = new Type[]{typeof(UnityEngine.UI.PolygonImage), typeof(System.String), typeof(System.Boolean), typeof(System.Action)};
            method = type.GetMethod("LoadSprite", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LoadSprite_1);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("FormatComma", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatComma_2);
            args = new Type[]{};
            method = type.GetMethod("ClearCore", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearCore_3);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.String)};
            method = type.GetMethod("FindDeepChild", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FindDeepChild_4);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("CurrencyFormat", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CurrencyFormat_5);
            args = new Type[]{};
            method = type.GetMethod("get_lightingManager", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_lightingManager_6);
            args = new Type[]{};
            method = type.GetMethod("get_mapManager", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mapManager_7);
            args = new Type[]{};
            method = type.GetMethod("get_weatherMgr", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_weatherMgr_8);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.Vector3), typeof(System.Action<UnityEngine.GameObject>)};
            method = type.GetMethod("AddEffect", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddEffect_9);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("NumberSymbol", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, NumberSymbol_10);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.Transform), typeof(System.Action<UnityEngine.GameObject>), typeof(System.Boolean), typeof(System.Single)};
            method = type.GetMethod("UIAddEffect", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UIAddEffect_11);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.String)};
            method = type.GetMethod("FindDeepMulChild", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FindDeepMulChild_12);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.Transform)};
            method = type.GetMethod("GetEffect", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetEffect_13);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FormatCountDown", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatCountDown_14);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Collections.Generic.List<System.String>), typeof(System.Action<System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>>)};
            method = type.GetMethod("PreLoadRes", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PreLoadRes_15);
            args = new Type[]{};
            method = type.GetMethod("get_lodManager", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_lodManager_16);
            args = new Type[]{typeof(UnityEngine.SpriteRenderer), typeof(System.String), typeof(System.Action)};
            method = type.GetMethod("LoadSprite", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LoadSprite_17);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FormatTimeTroop", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatTimeTroop_18);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("StringToHtmlColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StringToHtmlColor_19);
            args = new Type[]{typeof(UnityEngine.UI.PolygonImage), typeof(System.String)};
            method = type.GetMethod("ImageSetColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ImageSetColor_20);
            args = new Type[]{typeof(UnityEngine.SpriteRenderer), typeof(System.String)};
            method = type.GetMethod("ImageSetColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ImageSetColor_21);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FormatTimeCityBuff", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatTimeCityBuff_22);
            args = new Type[]{typeof(System.String), typeof(System.Collections.Generic.List<System.Int32>)};
            method = type.GetMethod("SafeFormat", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SafeFormat_23);
            args = new Type[]{typeof(System.Object), typeof(System.String)};
            method = type.GetMethod("Print", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Print_24);
            args = new Type[]{typeof(System.DateTime), typeof(System.DateTime)};
            method = type.GetMethod("TimeSpanDays", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TimeSpanDays_25);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Collections.Generic.List<System.String>), typeof(System.Action<System.Collections.Generic.Dictionary<System.String, Skyunion.IAsset>>)};
            method = type.GetMethod("PreLoadRes", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PreLoadRes_26);
            args = new Type[]{typeof(Spine.Unity.SkeletonGraphic), typeof(System.String), typeof(System.String), typeof(System.Boolean)};
            method = type.GetMethod("PlaySpine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlaySpine_27);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FormatTimeSplit", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatTimeSplit_28);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("UIReLayout", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UIReLayout_29);
            args = new Type[]{typeof(UnityEngine.UI.LanguageText)};
            method = type.GetMethod("TranslatorSDK", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TranslatorSDK_30);
            args = new Type[]{typeof(UnityEngine.UI.ContentSizeFitter)};
            method = type.GetMethod("GetPreferredSize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetPreferredSize_31);
            args = new Type[]{typeof(global::GameButton)};
            method = type.GetMethod("UIReLayout", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UIReLayout_32);
            args = new Type[]{typeof(UnityEngine.Animator), typeof(System.String)};
            method = type.GetMethod("PlayUIAnimation", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayUIAnimation_33);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("FormatTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatTime_34);
            args = new Type[]{typeof(UnityEngine.UI.Text), typeof(System.String)};
            method = type.GetMethod("TextSetColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TextSetColor_35);
            args = new Type[]{typeof(System.Single), typeof(UnityEngine.UI.Slider)};
            method = type.GetMethod("SetPro", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetPro_36);
            args = new Type[]{typeof(UnityEngine.UI.Text), typeof(UnityEngine.Color)};
            method = type.GetMethod("TextSetColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TextSetColor_37);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("ClearRichText", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearRichText_38);
            args = new Type[]{typeof(Spine.Unity.SkeletonGraphic), typeof(System.String), typeof(System.Action)};
            method = type.GetMethod("LoadSpine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LoadSpine_39);
            args = new Type[]{typeof(UnityEngine.Transform)};
            method = type.GetMethod("DestroyAllChild", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DestroyAllChild_40);
            args = new Type[]{typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("FormatTimeUpgrad", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatTimeUpgrad_41);
            args = new Type[]{typeof(Spine.Unity.SkeletonAnimation), typeof(System.String), typeof(System.Action)};
            method = type.GetMethod("LoadSpine", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, LoadSpine_42);
            args = new Type[]{typeof(UnityEngine.Animator), typeof(System.Int32)};
            method = type.GetMethod("GetAnimationLength", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetAnimationLength_43);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("FormatF2", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatF2_44);
            args = new Type[]{typeof(UnityEngine.Animator), typeof(System.String)};
            method = type.GetMethod("GetAnimationLength", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetAnimationLength_45);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("SubStringGetTotalIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SubStringGetTotalIndex_46);
            args = new Type[]{typeof(System.String), typeof(System.Int32), typeof(System.Int32)};
            method = type.GetMethod("SubStringUTF8", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SubStringUTF8_47);
            args = new Type[]{typeof(UnityEngine.UI.LanguageText), typeof(System.String)};
            method = type.GetMethod("FormatBeyondText", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatBeyondText_48);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("HexRGBToColor", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, HexRGBToColor_49);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(UnityEngine.UI.PolygonImage)};
            if (genericMethods.TryGetValue("ShowChild", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(void), typeof(UnityEngine.Transform), typeof(System.Int32)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ShowChild_50);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("PerctangleToDecimal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PerctangleToDecimal_51);
            args = new Type[]{typeof(System.Decimal), typeof(System.Int32)};
            method = type.GetMethod("FormatDecimal", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, FormatDecimal_52);


        }


        static StackObject* get_hudManager_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.ClientUtils.hudManager;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* LoadSprite_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @Callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @isNativeSize = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @assetname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.UI.PolygonImage @image = (UnityEngine.UI.PolygonImage)typeof(UnityEngine.UI.PolygonImage).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.LoadSprite(@image, @assetname, @isNativeSize, @Callback);

            return __ret;
        }

        static StackObject* FormatComma_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @num = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatComma(@num);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ClearCore_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            Client.ClientUtils.ClearCore();

            return __ret;
        }

        static StackObject* FindDeepChild_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_childName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_target = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.FindDeepChild(@_target, @_childName);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* CurrencyFormat_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @number = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.CurrencyFormat(@number);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_lightingManager_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.ClientUtils.lightingManager;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_mapManager_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.ClientUtils.mapManager;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_weatherMgr_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.ClientUtils.weatherMgr;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AddEffect_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<UnityEngine.GameObject> @callBack = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Vector3 @postion = new UnityEngine.Vector3();
            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.ParseValue(ref @postion, __intp, ptr_of_this_method, __mStack, true);
            } else {
                @postion = (UnityEngine.Vector3)typeof(UnityEngine.Vector3).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
                __intp.Free(ptr_of_this_method);
            }

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @effectName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.AddEffect(@effectName, @postion, @callBack);

            return __ret;
        }

        static StackObject* NumberSymbol_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @n = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.NumberSymbol(@n);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* UIAddEffect_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @removeTime = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @autoremove = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<UnityEngine.GameObject> @callBack = (System.Action<UnityEngine.GameObject>)typeof(System.Action<UnityEngine.GameObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.Transform @targetPart = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.String @effectName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.UIAddEffect(@effectName, @targetPart, @callBack, @autoremove, @removeTime);

            return __ret;
        }

        static StackObject* FindDeepMulChild_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @_childName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @_target = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.FindDeepMulChild(@_target, @_childName);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetEffect_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @targetPart = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @effectName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.GetEffect(@effectName, @targetPart);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* FormatCountDown_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @num = ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatCountDown(@num);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* PreLoadRes_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>> @loadFinishCallback = (System.Action<System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>>)typeof(System.Action<System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<System.String> @prefabNames = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.GameObject @viewObj = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.PreLoadRes(@viewObj, @prefabNames, @loadFinishCallback);

            return __ret;
        }

        static StackObject* get_lodManager_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = Client.ClientUtils.lodManager;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* LoadSprite_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @Callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @assetname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.SpriteRenderer @spriteRenderer = (UnityEngine.SpriteRenderer)typeof(UnityEngine.SpriteRenderer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.LoadSprite(@spriteRenderer, @assetname, @Callback);

            return __ret;
        }

        static StackObject* FormatTimeTroop_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @num = ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatTimeTroop(@num);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* StringToHtmlColor_19(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @rgb = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.StringToHtmlColor(@rgb);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ImageSetColor_20(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @rgb = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.PolygonImage @image = (UnityEngine.UI.PolygonImage)typeof(UnityEngine.UI.PolygonImage).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.ImageSetColor(@image, @rgb);

            return __ret;
        }

        static StackObject* ImageSetColor_21(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @rgb = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.SpriteRenderer @image = (UnityEngine.SpriteRenderer)typeof(UnityEngine.SpriteRenderer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.ImageSetColor(@image, @rgb);

            return __ret;
        }

        static StackObject* FormatTimeCityBuff_22(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @num = ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatTimeCityBuff(@num);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SafeFormat_23(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<System.Int32> @arr = (System.Collections.Generic.List<System.Int32>)typeof(System.Collections.Generic.List<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.SafeFormat(@str, @arr);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Print_24(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Object @obj = (System.Object)typeof(System.Object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.Print(@obj, @name);

            return __ret;
        }

        static StackObject* TimeSpanDays_25(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.DateTime @arrival = (System.DateTime)typeof(System.DateTime).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.DateTime @departure = (System.DateTime)typeof(System.DateTime).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.TimeSpanDays(@departure, @arrival);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* PreLoadRes_26(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Collections.Generic.Dictionary<System.String, Skyunion.IAsset>> @loadFinishCallback = (System.Action<System.Collections.Generic.Dictionary<System.String, Skyunion.IAsset>>)typeof(System.Action<System.Collections.Generic.Dictionary<System.String, Skyunion.IAsset>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Collections.Generic.List<System.String> @prefabNames = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.GameObject @viewObj = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.PreLoadRes(@viewObj, @prefabNames, @loadFinishCallback);

            return __ret;
        }

        static StackObject* PlaySpine_27(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isLoop = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @amname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @initSkinName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            Spine.Unity.SkeletonGraphic @graphic = (Spine.Unity.SkeletonGraphic)typeof(Spine.Unity.SkeletonGraphic).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.PlaySpine(@graphic, @initSkinName, @amname, @isLoop);

            return __ret;
        }

        static StackObject* FormatTimeSplit_28(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @num = ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatTimeSplit(@num);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* UIReLayout_29(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @btn = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.UIReLayout(@btn);

            return __ret;
        }

        static StackObject* TranslatorSDK_30(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.LanguageText @text = (UnityEngine.UI.LanguageText)typeof(UnityEngine.UI.LanguageText).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.TranslatorSDK(@text);

            return __ret;
        }

        static StackObject* GetPreferredSize_31(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.ContentSizeFitter @obj = (UnityEngine.UI.ContentSizeFitter)typeof(UnityEngine.UI.ContentSizeFitter).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.GetPreferredSize(@obj);

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector2_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* UIReLayout_32(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::GameButton @btn = (global::GameButton)typeof(global::GameButton).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.UIReLayout(@btn);

            return __ret;
        }

        static StackObject* PlayUIAnimation_33(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @strName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Animator @animator = (UnityEngine.Animator)typeof(UnityEngine.Animator).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.PlayUIAnimation(@animator, @strName);

            return __ret;
        }

        static StackObject* FormatTime_34(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @num = ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatTime(@num);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* TextSetColor_35(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @rgb = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Text @text = (UnityEngine.UI.Text)typeof(UnityEngine.UI.Text).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.TextSetColor(@text, @rgb);

            return __ret;
        }

        static StackObject* SetPro_36(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.UI.Slider @slider = (UnityEngine.UI.Slider)typeof(UnityEngine.UI.Slider).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @value = *(float*)&ptr_of_this_method->Value;


            Client.ClientUtils.SetPro(@value, @slider);

            return __ret;
        }

        static StackObject* TextSetColor_37(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Color @c = (UnityEngine.Color)typeof(UnityEngine.Color).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.Text @text = (UnityEngine.UI.Text)typeof(UnityEngine.UI.Text).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.TextSetColor(@text, @c);

            return __ret;
        }

        static StackObject* ClearRichText_38(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @txt = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.ClearRichText(@txt);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* LoadSpine_39(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @Callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @assetname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Spine.Unity.SkeletonGraphic @graphic = (Spine.Unity.SkeletonGraphic)typeof(Spine.Unity.SkeletonGraphic).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.LoadSpine(@graphic, @assetname, @Callback);

            return __ret;
        }

        static StackObject* DestroyAllChild_40(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.Transform @ransform = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.DestroyAllChild(@ransform);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* FormatTimeUpgrad_41(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isFull = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @num = ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatTimeUpgrad(@num, @isFull);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* LoadSpine_42(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @Callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @assetname = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Spine.Unity.SkeletonAnimation @skeleton = (Spine.Unity.SkeletonAnimation)typeof(Spine.Unity.SkeletonAnimation).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.LoadSpine(@skeleton, @assetname, @Callback);

            return __ret;
        }

        static StackObject* GetAnimationLength_43(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Animator @animator = (UnityEngine.Animator)typeof(UnityEngine.Animator).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.GetAnimationLength(@animator, @index);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* FormatF2_44(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;


            var result_of_this_method = Client.ClientUtils.FormatF2(@value);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetAnimationLength_45(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @animationName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Animator @animator = (UnityEngine.Animator)typeof(UnityEngine.Animator).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.GetAnimationLength(@animator, @animationName);

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* SubStringGetTotalIndex_46(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.SubStringGetTotalIndex(@str);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* SubStringUTF8_47(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @endIndex = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @startIndex = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.SubStringUTF8(@str, @startIndex, @endIndex);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* FormatBeyondText_48(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @content = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.UI.LanguageText @targetText = (UnityEngine.UI.LanguageText)typeof(UnityEngine.UI.LanguageText).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.FormatBeyondText(@targetText, @content);

            return __ret;
        }

        static StackObject* HexRGBToColor_49(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @hex = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.HexRGBToColor(@hex);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ShowChild_50(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @nCount = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.Transform @node = (UnityEngine.Transform)typeof(UnityEngine.Transform).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Client.ClientUtils.ShowChild<UnityEngine.UI.PolygonImage>(@node, @nCount);

            return __ret;
        }

        static StackObject* PerctangleToDecimal_51(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @perc = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.PerctangleToDecimal(@perc);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* FormatDecimal_52(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @dotafer = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Decimal @data = (System.Decimal)typeof(System.Decimal).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Client.ClientUtils.FormatDecimal(@data, @dotafer);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
