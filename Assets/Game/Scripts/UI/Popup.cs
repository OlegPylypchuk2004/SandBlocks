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
            });

            _currentSequence.Append(_blurBackground.Appear());

            _currentSequence.Append(_canvasGroup.DOFade(1f, 0.25f)
                .From(0f)
                .SetEase(Ease.OutQuad));

            _currentSequence.Append(_panel.Appear());

            _currentSequence.OnComplete(() =>
            {
                _canvasGroup.interactable = true;

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

                UnsubscribeFromEvents();
            });

            _currentSequence.Append(_canvasGroup.DOFade(0f, 0.25f)
                .SetEase(Ease.InQuad));

            _currentSequence.Append(_blurBackground.Disappear());

            _currentSequence.OnComplete(() =>
            {
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
    }
}