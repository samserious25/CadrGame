using System;
using System.Collections;
using Assets.Arch.Services;
using UnityEngine.SceneManagement;

namespace Assets.Arch.Utilities
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void Load(string nextSceneName, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(nextSceneName, onLoaded));

        private IEnumerator LoadScene(string nextSceneName, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextSceneName)
            {
                onLoaded?.Invoke();
                yield break;
            }

            var waitNextScene = SceneManager.LoadSceneAsync(nextSceneName);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}