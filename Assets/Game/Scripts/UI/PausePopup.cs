using DG.Tweening;
using SceneManagment;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class PausePopup : Popup
    {
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public override Sequence Appear()
        {
            return base.Appear()
                .SetUpdate(true);
        }

        public override Sequence Disappear()
        {
            return base.Disappear()
                .SetUpdate(true);
        }

        protected override void SubscribeOnEvents()
        {
            base.SubscribeOnEvents();

            _yesButton.onClick.AddListener(OnYesButtonClicked);
            _noButton.onClick.AddListener(OnNoButtonClicked);
        }

        protected override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();

            _yesButton.onClick.RemoveListener(OnYesButtonClicked);
            _noButton.onClick.RemoveListener(OnNoButtonClicked);
        }

        protected override void AppearAnimationStarted()
        {
            base.AppearAnimationStarted();

            Time.timeScale = 0f;
        }

        protected override void DisappearAnimationCompleted()
        {
            base.DisappearAnimationCompleted();

            Time.timeScale = 1f;
        }

        private void OnYesButtonClicked()
        {
            Time.timeScale = 1f;

            _sceneLoader.Load(_sceneLoader.CurrentSceneIndex);
        }

        private void OnNoButtonClicked()
        {
            Disappear();
        }
    }
}