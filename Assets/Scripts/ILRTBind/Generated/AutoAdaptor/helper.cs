
using System;

namespace Skyunion
{
    class ILRuntimeHelper
    {
        public static void Init(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            if (app == null)
            {
                // should log error
                return;
            }

			// adaptor register 
                        
			app.RegisterCrossBindingAdaptor(new Sproto.ProtocolBaseAdaptor());            
			app.RegisterCrossBindingAdaptor(new Sproto.SprotoTypeBaseAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.UIPopValueAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.BehaviourBinderAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.GameViewAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.MonoLikeEntityAdaptor());            
			app.RegisterCrossBindingAdaptor(new Skyunion.ViewBinderAdaptor());

			// delegate register 
						
			app.DelegateManager.RegisterMethodDelegate<Client.HUDUI>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Int32,System.Int32>();
			
			app.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
			
			app.DelegateManager.RegisterMethodDelegate<Skyunion.AudioHandler>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Boolean>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Int64>();
			
			app.DelegateManager.RegisterMethodDelegate<Skyunion.IAsset>();
			
			app.DelegateManager.RegisterMethodDelegate<System.String>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Single>();
			
			app.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector3>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Single>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Single,System.Single>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Single,System.Single,System.Single>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Int32,System.Int32,System.String,System.String>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Boolean,Client.HUDUI>();
			
			app.DelegateManager.RegisterMethodDelegate<Skyunion.NetEvent,System.Int32>();
			
			app.DelegateManager.RegisterMethodDelegate<UnityEngine.Transform>();
			
			app.DelegateManager.RegisterMethodDelegate<System.IO.MemoryStream>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Boolean>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Int32>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Int32,System.Int32>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Int32,UnityEngine.Transform>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Object,UnityEngine.Transform>();
			
			app.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
			
			app.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2>();

			app.DelegateManager.RegisterMethodDelegate<System.Int32>();
			
			app.DelegateManager.RegisterMethodDelegate<Client.PageView.ListItem>();
			
			app.DelegateManager.RegisterMethodDelegate<Client.ScrollView.ScrollItem>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Int32,UnityEngine.Vector3>();


			// delegate convertor
            
        }
    }
}