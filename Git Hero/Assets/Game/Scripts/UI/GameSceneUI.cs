using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Githero.UI
{
    public class GameSceneUI : MonoBehaviour
    {
        private enum AnimationsParameters
        {
            CloseSceneTrigger,
            PlayTrigger
        }

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Canvas overlayCanvas;

        [SerializeField]
        private Text overlayTitle;

        [SerializeField]
        private Text overlaySubtitle;

        [SerializeField]
        private Text overlayTip;

        [SerializeField]
        private Text titleGameplay;

        [SerializeField]
        private Text missScoreGameplay;

        [SerializeField]
        private Text hitScoreGameplay;

        private AnimationsParameters currentAnimationsParameters;

        private int timeToCloseScreen = 5;

        private int countDown = 3;
        private int missCount = 0;
        private int hitCount = 0;

        public void SetGameplayTitle(string title) =>
            titleGameplay.text = title;

        public void NewMiss()
        {
            missCount++;
            missScoreGameplay.text = missCount.ToString();
        }

        public void NewHit()
        {
            hitCount++;
            hitScoreGameplay.text = hitCount.ToString();
        }

        public void SetCountDown() =>
           overlayTitle.text = countDown.ToString();

        public void DecreaseCountDown()
        {
            countDown--;
            overlayTitle.text = countDown.ToString();
        }

        public void ShowOverlayCanvas() =>
            SetActive(overlayCanvas.gameObject, true);

        public void HideOverlayTitle() =>
            SetActive(overlayTitle.gameObject, false);

        public void HideOverlaySubtitle() =>
            SetActive(overlaySubtitle.gameObject, false);

        public void HideOverlayTip() =>
            SetActive(overlayTip.gameObject, false);

        public void HideOverlayCanvas() =>
            SetActive(overlayCanvas.gameObject, false);

        public void StartCloseScene() =>
            StartCoroutine(CloseScene());

        public void StarPlayAnimation() =>
            StartAnimationsParametersTrigger(AnimationsParameters.PlayTrigger);

        public void StarExitAnimation() =>
            StartAnimationsParametersTrigger(AnimationsParameters.CloseSceneTrigger);

        private void SetActive(GameObject gameObject, bool active) =>
            gameObject.SetActive(active);

        private void StartAnimationsParametersTrigger(AnimationsParameters animationsParameters)
        {
            currentAnimationsParameters = animationsParameters;
            animator.SetTrigger(currentAnimationsParameters.ToString());
        }

        private IEnumerator CloseScene()
        {
            yield return new WaitForSeconds(timeToCloseScreen);
            StarExitAnimation();
        }

    }

}