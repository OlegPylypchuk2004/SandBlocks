using Cysharp.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

namespace SceneManagment
{
    public class SceneLoader
    {
        public int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        public event Action LoadStarted;
        public event Action LoadCompleted;

        public void Load(int index)
        {
            LoadAsync(index)
                .Forget();
        }

        public void Load(string name)
        {
            LoadAsync(name)
                .Forget();
        }

        private async UniTaskVoid LoadAsync(int index)
        {
            LoadStarted?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            await SceneManager.LoadSceneAsync(index)
                .ToUniTask();

            LoadCompleted?.Invoke();
        }

        private async UniTaskVoid LoadAsync(string name)
        {
            LoadStarted?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(1f));

            await SceneManager.LoadSceneAsync(name)
                .ToUniTask();

            LoadCompleted?.Invoke();
        }
    }
}