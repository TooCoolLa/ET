using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeStart_AddComponent: AEvent<EventType.SceneChangeStart>
    {
        protected override async ETTask Run(Scene scene, EventType.SceneChangeStart args)
        {
            Scene currentScene = scene.CurrentScene();
            ResourcesLoaderComponent resourcesLoaderComponent = currentScene.GetComponent<ResourcesLoaderComponent>();
            // 加载场景资源
            await resourcesLoaderComponent.LoadSceneAsync(currentScene.Name,LoadSceneMode.Single);
            // 切换到map场景

            await SceneManager.LoadSceneAsync(currentScene.Name);
			

            currentScene.AddComponent<OperaComponent>();
            
            
        }
    }
}