using UnityEngine;

namespace ET.Client
{
    [UIEvent(UIType.UILobby)]
    public class UILobbyEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer)
        {
            await ETTask.CompletedTask;
           var gameobj= await uiComponent.DomainScene().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(UIType.UILobby);
           GameObject bundleGameObject = gameobj;
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.GetLayer((int)uiLayer));
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UILobby, gameObject);

            ui.AddComponent<UILobbyComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            //ResourcesComponent.Instance.UnloadBundle(UIType.UILobby.StringToAB());
        }
    }
}