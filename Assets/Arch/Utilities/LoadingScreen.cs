using System.Collections;
using UnityEngine;

namespace Assets.Arch.Utilities
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Show()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
        }

        public void Hide(float time) => StartCoroutine(FadeIn(time));

        private IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= 0.03f;
                yield return new WaitForSeconds(time / 60f);
            }

            gameObject.SetActive(false);
        }
    }
}