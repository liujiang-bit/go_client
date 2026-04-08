using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using UnityEngine;
using System;
using Skyunion;

namespace ILRTBind
{
    public class ILRTBind
    {
        public static unsafe void Init(AppDomain appdomain)
        {
            // ע��������
            ILRuntimeHelper.Init(appdomain);
            // ע��Value Type Binder
            RegisterValueTypeBinder(appdomain);

            // ע��Adaptor
            RegisterCrossBindingAdaptor(appdomain);

            // ע���ض��򷽷�
            RegisterCLRMethodRedirection(appdomain);

            //// ע��ί��
            RegisterDelegates(appdomain);
        }

        private static unsafe void RegisterValueTypeBinder(AppDomain appdomain)
        {
            appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
        }

        private static unsafe void RegisterCrossBindingAdaptor(AppDomain appdomain)
        {
        }

        private static unsafe void RegisterCLRMethodRedirection(AppDomain appdomain)
        {
            //LitJson
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
            CLRBindings.Initialize(appdomain);
            //ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
        }

        private static void RegisterDelegates(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int64, System.Int64>();
            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.IAsset>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int64>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Type, System.Object>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Skyunion.CreateInstance>((act) =>
            {
                return new Skyunion.CreateInstance((type) =>
                {
                    return ((Func<System.Type, System.Object>)act)(type);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Playables.PlayableDirector>();
            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.NetEvent>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.IO.MemoryStream>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.ArraySegment<System.Byte>, Skyunion.NetPackInfo>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Skyunion.ProtocolResolverDelegate>((act) =>
            {
                return new Skyunion.ProtocolResolverDelegate((segmentBytes) =>
                {
                    return ((Func<System.ArraySegment<System.Byte>, Skyunion.NetPackInfo>)act)(segmentBytes);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.Single>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single, System.Single>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.UIInfo>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Skyunion.OnShowUI>((act) =>
            {
                return new Skyunion.OnShowUI((ui) =>
                {
                    ((Action<Skyunion.UIInfo>)act)(ui);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.Dictionary<System.Int64, ILRuntime.Runtime.Intepreter.ILTypeInstance>>();
            appdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, Sproto.SprotoTypeBaseAdaptor.Adaptor>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Int32, System.String, System.String>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.Dictionary<System.String, UnityEngine.GameObject>>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Int32>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Int32>((arg0) =>
                {
                    ((Action<System.Int32>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<Sproto.SprotoTypeBaseAdaptor.Adaptor, System.Int64>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Sproto.SprotoTypeDeserialize.gen_key_func<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>>((act) =>
            {
                return new Sproto.SprotoTypeDeserialize.gen_key_func<System.Int64, Sproto.SprotoTypeBaseAdaptor.Adaptor>((v) =>
                {
                    return ((Func<Sproto.SprotoTypeBaseAdaptor.Adaptor, System.Int64>)act)(v);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>)act)(obj);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.Dictionary<System.String, Skyunion.IAsset>>();

            appdomain.DelegateManager.RegisterDelegateConvertor<Skyunion.OnCloseUI>((act) =>
            {
                return new Skyunion.OnCloseUI((ui) =>
                {
                    ((Action<Skyunion.UIInfo>)act)(ui);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, ILRuntime.Runtime.Intepreter.ILTypeInstance>();

            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.NetEvent, System.Int32>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.AsyncOperation>();

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
                {
                    ((Action<System.Boolean>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single>();
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() =>
                {
                    ((Action)act)();
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<Skyunion.AudioHandler>();

            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int32>>((act) =>
            {
                return new System.Predicate<System.Int32>((obj) =>
                {
                    return ((Func<System.Int32, System.Boolean>)act)(obj);
                });
            });

            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>, System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>>>((act) =>
            {
                return new System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>>((x, y) =>
                {
                    return ((Func<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>, System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>, System.Int32>)act)(x, y);
                });
            });


            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
                {
                    ((Action<System.String>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int64, System.Int64, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int64>>((act) =>
            {
                return new System.Comparison<System.Int64>((x, y) =>
                {
                    return ((Func<System.Int64, System.Int64, System.Int32>)act)(x, y);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<Client.ListView.ListItem, System.String>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Client.ListView.FuncTab.ReturnString>((act) =>
            {
                return new Client.ListView.FuncTab.ReturnString((item) =>
                {
                    return ((Func<Client.ListView.ListItem, System.String>)act)(item);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<Client.ListView.ListItem, System.Single>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Client.ListView.FuncTab.ReturnFloat>((act) =>
            {
                return new Client.ListView.FuncTab.ReturnFloat((item) =>
                {
                    return ((Func<Client.ListView.ListItem, System.Single>)act)(item);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<Sproto.SprotoTypeBaseAdaptor.Adaptor, Sproto.SprotoTypeBaseAdaptor.Adaptor, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<Sproto.SprotoTypeBaseAdaptor.Adaptor>>((act) =>
            {
                return new System.Comparison<Sproto.SprotoTypeBaseAdaptor.Adaptor>((x, y) =>
                {
                    return ((Func<Sproto.SprotoTypeBaseAdaptor.Adaptor, Sproto.SprotoTypeBaseAdaptor.Adaptor, System.Int32>)act)(x, y);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Single>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Single>((arg0) =>
                {
                    ((Action<System.Single>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int32>>((act) =>
            {
                return new System.Comparison<System.Int32>((x, y) =>
                {
                    return ((Func<System.Int32, System.Int32, System.Int32>)act)(x, y);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<Client.MarchLine>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean, Client.HUDUI>();

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Client.UIEventTrigger.EventTriggerCB1>((act) =>
            {
                return new Client.UIEventTrigger.EventTriggerCB1((data) =>
                {
                    ((Action<UnityEngine.EventSystems.PointerEventData>)act)(data);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Client.UIEventTrigger.EventTriggerCB2>((act) =>
            {
                return new Client.UIEventTrigger.EventTriggerCB2((data) =>
                {
                    ((Action<UnityEngine.EventSystems.BaseEventData>)act)(data);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.AxisEventData>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Client.UIEventTrigger.EventTriggerCB3>((act) =>
            {
                return new Client.UIEventTrigger.EventTriggerCB3((data) =>
                {
                    ((Action<UnityEngine.EventSystems.AxisEventData>)act)(data);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<Sproto.SprotoTypeBaseAdaptor.Adaptor>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Single>();
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOGetter<System.Single>>((act) =>
            {
                return new DG.Tweening.Core.DOGetter<System.Single>(() =>
                {
                    return ((Func<System.Single>)act)();
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Single>>((act) =>
            {
                return new DG.Tweening.Core.DOSetter<System.Single>((pNewValue) =>
                {
                    ((Action<System.Single>)act)(pNewValue);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Transform>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.String, System.String, System.String, global::IGGSDKUtils.MsgBoxReturnListener.Listener>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGSDKUtils.ShowMsgBoxListener1>((act) =>
            {
                return new global::IGGSDKUtils.ShowMsgBoxListener1((message, title, ok, cancle, listener) =>
                {
                    ((Action<System.String, System.String, System.String, System.String, global::IGGSDKUtils.MsgBoxReturnListener.Listener>)act)(message, title, ok, cancle, listener);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.String, System.String, global::IGGSDKUtils.MsgBoxReturnListener.Listener>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGSDKUtils.ShowMsgBoxListener2>((act) =>
            {
                return new global::IGGSDKUtils.ShowMsgBoxListener2((message, title, ok, listene) =>
                {
                    ((Action<System.String, System.String, System.String, global::IGGSDKUtils.MsgBoxReturnListener.Listener>)act)(message, title, ok, listene);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGSDKUtils.ShowToastListener>((act) =>
            {
                return new global::IGGSDKUtils.ShowToastListener((message) =>
                {
                    ((Action<System.String>)act)(message);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGPrimaryAppConfig, global::IGGEasternStandardTime>();

            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGSDK.IGGSDKInitFinishedListener.Listener1>((act) =>
            {
                return new global::IGGSDK.IGGSDKInitFinishedListener.Listener1((appConfig, standardTime) =>
                {
                    ((Action<global::IGGPrimaryAppConfig, global::IGGEasternStandardTime>)act)(appConfig, standardTime);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGPrimaryAppConfigBackup, global::IGGEasternStandardTime>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGSDK.IGGSDKInitFinishedListener.Listener2>((act) =>
            {
                return new global::IGGSDK.IGGSDKInitFinishedListener.Listener2((appConfig, standardTime) =>
                {
                    ((Action<global::IGGPrimaryAppConfigBackup, global::IGGEasternStandardTime>)act)(appConfig, standardTime);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGSession>();

            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGLogin.IGGLoginDelegate.Listener>((act) =>
            {
                return new global::IGGLogin.IGGLoginDelegate.Listener((var1) =>
                {
                    ((Action<global::IGGSession>)act)(var1);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, global::IGGSession>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGLogin.IGGLoginListener.Listener>((act) =>
            {
                return new global::IGGLogin.IGGLoginListener.Listener((var1, var2) =>
                {
                    ((Action<global::IGGException, global::IGGSession>)act)(var1, var2);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.List<global::IGGGameItem>>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGPayment.OnItemLoad>((act) =>
            {
                return new global::IGGPayment.OnItemLoad((gameItems) =>
                {
                    ((Action<System.Collections.Generic.List<global::IGGGameItem>>)act)(gameItems);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, global::IGGAgreementSigningStatus>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAgreementSigning.IGGStatusRequestListener.Listener>((act) =>
            {
                return new global::IGGAgreementSigning.IGGStatusRequestListener.Listener((exception, status) =>
                {
                    ((Action<global::IGGException, global::IGGAgreementSigningStatus>)act)(exception, status);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAgreementSigning.IGGSigningListener.Listener>((act) =>
            {
                return new global::IGGAgreementSigning.IGGSigningListener.Listener((exception) =>
                {
                    ((Action<global::IGGException>)act)(exception);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
                {
                    ((Action<UnityEngine.Vector2>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<Client.ListView.ListItem>();
            appdomain.DelegateManager.RegisterMethodDelegate<Client.ProvinceName>();
            appdomain.DelegateManager.RegisterMethodDelegate<Spine.TrackEntry>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Spine.AnimationState.TrackEntryDelegate>((act) =>
            {
                return new Spine.AnimationState.TrackEntryDelegate((trackEntry) =>
                {
                    ((Action<Spine.TrackEntry>)act)(trackEntry);
                });
            });
            
            
            appdomain.DelegateManager.RegisterMethodDelegate<global::ManorItem>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>, System.Boolean>();
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGPayment.OnItemBuy>((act) =>
            {
                return new global::IGGPayment.OnItemBuy((ex, bIsUserCancle) =>
                {
                    ((Action<global::IGGException, System.Boolean>)act)(ex, bIsUserCancle);
                });
            });


            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int64, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int64>>((act) =>
            {
                return new System.Predicate<System.Int64>((obj) =>
                {
                    return ((Func<System.Int64, System.Boolean>)act)(obj);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean, global::IGGInGameReporting.WebRequestReturn>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGInGameReporting.RequestCallback>((act) =>
            {
                return new global::IGGInGameReporting.RequestCallback((successed, retData) =>
                {
                    ((Action<System.Boolean, global::IGGInGameReporting.WebRequestReturn>)act)(successed, retData);
                });
            });            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, System.Collections.Generic.List<global::IGGTranslationSource>>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGTranslator.IGGTranslatorListener.Listener2>((act) =>
            {
                return new global::IGGTranslator.IGGTranslatorListener.Listener2((var1, sources) =>
                {
                    ((Action<global::IGGException, System.Collections.Generic.List<global::IGGTranslationSource>>)act)(var1, sources);
                });
            });            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGTranslationSet>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGTranslator.IGGTranslatorListener.Listener1>((act) =>
            {
                return new global::IGGTranslator.IGGTranslatorListener.Listener1((var1) =>
                {
                    ((Action<global::IGGTranslationSet>)act)(var1);
                });
            });            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, global::IGGUserProfile>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAccountManagementGuideline.IGGAccountManagementGuidelineListener.Listener>((act) =>
            {
                return new global::IGGAccountManagementGuideline.IGGAccountManagementGuidelineListener.Listener((exception, userProfile) =>
                {
                    ((Action<global::IGGException, global::IGGUserProfile>)act)(exception, userProfile);
                });
            });            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, System.String>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGLogin.IGGLoginListener.Listener2>((act) =>
            {
                return new global::IGGLogin.IGGLoginListener.Listener2((var1, iggid) =>
                {
                    ((Action<global::IGGException, System.String>)act)(var1, iggid);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<System.String, IGGSDKConstant.AccountState, IGGSDKConstant.IGGLoginType, global::IGGLogin.CheckStateReturn>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGLogin.CheckStateBox>((act) =>
            {
                return new global::IGGLogin.CheckStateBox((iggid, accountState, loginType, callback) =>
                {
                    ((Action<System.String, IGGSDKConstant.AccountState, IGGSDKConstant.IGGLoginType, global::IGGLogin.CheckStateReturn>)act)(iggid, accountState, loginType, callback);
                });
            });            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, global::IGGAssignedAgreements>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAgreementSigning.IGGAssignedAgreementsRequestListener.Listener>((act) =>
            {
                return new global::IGGAgreementSigning.IGGAssignedAgreementsRequestListener.Listener((exception, agreements) =>
                {
                    ((Action<global::IGGException, global::IGGAssignedAgreements>)act)(exception, agreements);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGException, global::IGGAgreementSignedFile, global::IGGAgreementTerminationAlert>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAgreementTerminationController.IGGAssignedAgreementsRequestForTerminationListener.Listener>((act) =>
            {
                return new global::IGGAgreementTerminationController.IGGAssignedAgreementsRequestForTerminationListener.Listener((exception, signingFile, alert) =>
                {
                    ((Action<global::IGGException, global::IGGAgreementSignedFile, global::IGGAgreementTerminationAlert>)act)(exception, signingFile, alert);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGURLBundle.IGGURLBundleListener.Listener>((act) =>
            {
                return new global::IGGURLBundle.IGGURLBundleListener.Listener((exception, url) =>
                {
                    ((Action<global::IGGException, System.String>)act)(exception, url);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGStarndardAppRating>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAppRating.IGGRequestReviewListener.Listener4>((act) =>
            {
                return new global::IGGAppRating.IGGRequestReviewListener.Listener4((rating) =>
                {
                    ((Action<global::IGGStarndardAppRating>)act)(rating);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::WegamersSDK.WGSDKInitListener.Listener2>((act) =>
            {
                return new global::WegamersSDK.WGSDKInitListener.Listener2((var1) =>
                {
                    ((Action<System.Boolean>)act)(var1);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::WegamersSDK.WGMsgReddotListener.Listener>((act) =>
            {
                return new global::WegamersSDK.WGMsgReddotListener.Listener((var1) =>
                {
                    ((Action<System.Boolean>)act)(var1);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGSDKUtils.MsgBoxReturnListener.Listener>((act) =>
            {
                return new global::IGGSDKUtils.MsgBoxReturnListener.Listener((sure) =>
                {
                    ((Action<System.Boolean>)act)(sure);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGMinimizedAppRating>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAppRating.IGGRequestReviewListener.Listener3>((act) =>
            {
                return new global::IGGAppRating.IGGRequestReviewListener.Listener3((rating) =>
                {
                    ((Action<global::IGGMinimizedAppRating>)act)(rating);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAppRating.IGGRequestReviewListener.Listener2>((act) =>
            {
                return new global::IGGAppRating.IGGRequestReviewListener.Listener2((exception) =>
                {
                    ((Action<global::IGGException>)act)(exception);
                });
            });            appdomain.DelegateManager.RegisterMethodDelegate<global::IGGAppRatingStatus>();            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAppRating.IGGRequestReviewListener.Listener1>((act) =>
            {
                return new global::IGGAppRating.IGGRequestReviewListener.Listener1((status) =>
                {
                    ((Action<global::IGGAppRatingStatus>)act)(status);
                });
            });            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAgreementTerminationController.IGGTerminateRequestListener.Listener>((act) =>
            {
                return new global::IGGAgreementTerminationController.IGGTerminateRequestListener.Listener((exception) =>
                {
                    ((Action<global::IGGException>)act)(exception);
                });
            });            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAppRating.IGGAppRatingResultListener.Listener>((act) =>
            {
                return new global::IGGAppRating.IGGAppRatingResultListener.Listener((exception) =>
                {
                    ((Action<global::IGGException>)act)(exception);
                });
            });            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAppRating.IGGFeedbackResultListener.Listener>((act) =>
            {
                return new global::IGGAppRating.IGGFeedbackResultListener.Listener((exception) =>
                {
                    ((Action<global::IGGException>)act)(exception);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGAppRating.IGGFeedbackWebPageURLResultListener.Listener>((act) =>
            {
                return new global::IGGAppRating.IGGFeedbackWebPageURLResultListener.Listener((url) =>
                {
                    ((Action<System.String>)act)(url);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.String>>((act) =>
            {
                return new System.Predicate<System.String>((obj) =>
                {
                    return ((Func<System.String, System.Boolean>)act)(obj);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Animation>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>>((act) =>
            {
                return new System.Comparison<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>>((x, y) =>
                {
                    return ((Func<System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Collections.Generic.KeyValuePair<System.Int32, System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>, System.Int32>)act)(x, y);
                });
            });			appdomain.DelegateManager.RegisterDelegateConvertor<Client.UIClickListener.EventTriggerCB1>((act) =>
			{
				return new Client.UIClickListener.EventTriggerCB1((data) =>
				{
					((Action<UnityEngine.EventSystems.PointerEventData>)act)(data);
				});
			});
            appdomain.DelegateManager.RegisterMethodDelegate<global::ManorLod3Data>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.Int64, System.Int64, System.String>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean, global::IGGStorageService.WebRequestReturn>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::IGGStorageService.RequestCallback>((act) =>
            {
                return new global::IGGStorageService.RequestCallback((successed, retData) =>
                {
                    ((Action<System.Boolean, global::IGGStorageService.WebRequestReturn>)act)(successed, retData);
                });
            });

            appdomain.DelegateManager.RegisterFunctionDelegate<System.Boolean>();
			appdomain.DelegateManager.RegisterFunctionDelegate<Sproto.SprotoTypeBaseAdaptor.Adaptor, System.Boolean>();
            
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<Sproto.SprotoTypeBaseAdaptor.Adaptor>>((act) =>
            {
                return new System.Predicate<Sproto.SprotoTypeBaseAdaptor.Adaptor>((obj) =>
                {
                    return ((Func<Sproto.SprotoTypeBaseAdaptor.Adaptor, System.Boolean>)act)(obj);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<System.Threading.Tasks.Task>();            appdomain.DelegateManager.RegisterFunctionDelegate<global::UniWebView, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<global::UniWebView.ShouldCloseDelegate>((act) =>
            {
                return new global::UniWebView.ShouldCloseDelegate((webView) =>
                {
                    return ((Func<global::UniWebView, System.Boolean>)act)(webView);
                });
            });

        }
    }
}