using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Template.Scripts
{
    public class FadePanelController : PersistentSingleton<FadePanelController>
    {
        public Image targetImage;
        
        [Space(10)]
        public Color fadeColor = Color.black;
        public Color brightColor = Color.clear;
        
        [Space(10)]
        public float fadeDuration = 1f;
        public float brightDuration = 1f;
        
        [Space(10)]
        public float delayTime = 1f;

        private IEnumerator  fadeSequence;

        protected override void Initialize()
        {
            base.Initialize();
            InitPanel();
        }

        private void InitPanel()
        {
            targetImage.raycastTarget = false;
            targetImage.color = brightColor;
        }

        public void StartFadeSequence()
        {
            if (fadeSequence != null)
                StopCoroutine(fadeSequence);

            fadeSequence = FadeSequence();
            StartCoroutine(fadeSequence);
        }
        
        private IEnumerator FadeSequence()
        {
            targetImage.raycastTarget = true;
            yield return StartCoroutine(TransitionColor(fadeColor, fadeDuration));
            yield return new WaitForSeconds(delayTime);
            yield return StartCoroutine(TransitionColor(brightColor, brightDuration));
            targetImage.raycastTarget = false;
        }

        private IEnumerator TransitionColor(Color targetColor, float duration)
        {
            Color startColor = targetImage.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                targetImage.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetImage.color = targetColor;
        }
    }
}