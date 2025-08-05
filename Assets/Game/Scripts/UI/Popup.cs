using DG.Tweening;
using UnityEngine;

namespace UI
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] private BlurBackground _blurBackground;
        [SerializeField] private Panel _panel;
        [SerializeField] private CanvasGroup _canvasGroup;


        protected Sequence _currentSequence;

        public virtual Sequence Appear()
        {
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence();
            _currentSequence.SetLink(gameObject);

            _currentSequence.AppendCallback(() =>
            {
                gameObject.SetActive(true);
                _canvasGroup.interactable = false;

                AppearAnimationStarted();
            });

            _currentSequence.Append(_blurBackground.Appear());

            _currentSequence.Append(_canvasGroup.DOFade(1f, 0.25f)
                .From(0f)
                .SetEase(Ease.OutQuad));

            _currentSequence.Append(_panel.Appear());

            _currentSequence.AppendCallback(() =>
            {
                _canvasGroup.interactable = true;

                AppearAnimationCompleted();
                SubscribeOnEvents();
            });

            return _currentSequence;
        }

        public virtual Sequence Disappear()
        {
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence();
            _currentSequence.SetLink(gameObject);

            _currentSequence.AppendCallback(() =>
            {
                _canvasGroup.interactable = false;

                DisappearAnimationStarted();
                UnsubscribeFromEvents();
            });

            //_currentSequence.Append(_panel.Disappear());

            _currentSequence.Append(_canvasGroup.DOFade(0f, 0.25f)
                .SetEase(Ease.InQuad));

            _currentSequence.Append(_blurBackground.Disappear());

            _currentSequence.AppendCallback(() =>
            {
                DisappearAnimationCompleted();

                gameObject.SetActive(false);
            });

            return _currentSequence;
        }

        protected virtual void SubscribeOnEvents()
        {

        }

        protected virtual void UnsubscribeFromEvents()
        {

        }

        protected virtual void AppearAnimationStarted()
        {

        }

        protected virtual void AppearAnimationCompleted()
        {

        }

        protected virtual void DisappearAnimationStarted()
        {

        }

        protected virtual void DisappearAnimationCompleted()
        {

        }

    }
}