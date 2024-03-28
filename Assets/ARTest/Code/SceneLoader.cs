using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public enum SceneName
    {
        Bootstrap,
        CoreScene
    }

    public class SceneLoader : MonoBehaviour
    {
        private SceneName _sceneActive;

        public SceneName SceneActive => _sceneActive;

        public void Awake()
        {
            Init();
        }

        private void Init()
        {
            SceneManager.sceneLoaded += OnLoadComplete;
            LoadScene(SceneName.CoreScene);
        }

        public void LoadScene(SceneName sceneToLoad)
        {
            Loading(sceneToLoad);
        }

        private async void Loading(SceneName sceneToLoad)
        {
            var operation = LoadSceneLocal(sceneToLoad);
            await operation;
        }

        private Task<AsyncOperation> LoadSceneLocal(SceneName sceneToLoad)
        {
            return Task.FromResult(SceneManager.LoadSceneAsync(sceneToLoad.ToString()));
        }

        private void OnLoadComplete(Scene scene, LoadSceneMode loadSceneMode)
        {
            Enum.TryParse(scene.name, out _sceneActive);

            switch (_sceneActive)
            {
                case SceneName.Bootstrap:
                    break;
                case SceneName.CoreScene:
                    break;
            }
        }
    }
}