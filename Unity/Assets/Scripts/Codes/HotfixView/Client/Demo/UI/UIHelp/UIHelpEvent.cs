using System;
using UnityEngine;

namespace ET.Client
{
	[UIEvent(UIType.UIHelp)]
    public class UIHelpEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
	        try
	        {
		        //string name = $"Assets/Bundles/UI/{UIType.UIHelp}.prefab";
		        var  obj= await uiComponent.DomainScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(UIType.UIHelp);
		        GameObject bundleGameObject = obj;
		        GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.GetLayer((int)uiLayer));
		        UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIHelp, gameObject);

				ui.AddComponent<UIHelpComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}

        public override void OnRemove(UIComponent uiComponent)
        {
        }
    }
}