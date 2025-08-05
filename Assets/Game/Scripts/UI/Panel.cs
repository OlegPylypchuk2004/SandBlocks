using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] private Vector2 _minSize;
        [SerializeField] private Vector2 _normalSize;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Sequence _currentSequence;

        private void Awake()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0f;
        }

        public Sequence Appear()
        {
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence();
            _currentSequence.SetLink(gameObject);

            _currentSequence.AppendCallback(() =>
            {
                gameObject.SetActive(true);

                _rectTransform.sizeDelta = _minSize;
                _canvasGroup.interactable = false;
                _canvasGroup.alpha = 0f;
            });

            _currentSequence.Append(_image.DOFade(1f, 0.25f)
                .From(0f)
                .SetEase(Ease.Linear));

            _currentSequence.Append(_rectTransform.DOSizeDelta(new Vector2(_minSize.x, _normalSize.y), 0.25f)
                .SetEase(Ease.OutQuad));

            _currentSequence.AppendInterval(0.0625f);

            _currentSequence.Append(_rectTransform.DOSizeDelta(_normalSize, 0.25f)
                .SetEase(Ease.OutQuad));

            _currentSequence.Append(_canvasGroup.DOFade(1f, 0.25f)
                .SetEase(Ease.OutQuad));

            _currentSequence.AppendCallback(() =>
            {
                _canvasGroup.interactable = true;
            });

            return _currentSequence;
        }

        public Sequence Disappear()
        {
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence();
            _currentSequence.SetLink(gameObject);

            _currentSequence.AppendCallback(() =>
            {
                gameObject.SetActive(true);

                _canvasGroup.interactable = false;
            });

            _currentSequence.Append(_canvasGroup.DOFade(0f, 0.25f)
                .SetEase(Ease.InQuad));

            _currentSequence.Append(_rectTransform.DOSizeDelta(new Vector2(_normalSize.x, _minSize.y), 0.25f)
                .SetEase(Ease.InQuad));

            _currentSequence.AppendInterval(0.0625f);

            _currentSequence.Append(_rectTransform.DOSizeDelta(_minSize, 0.25f)
                .SetEase(Ease.InQuad));

            _currentSequence.Append(_image.DOFade(0f, 0.25f)
                .SetEase(Ease.Linear));

            return _currentSequence;
        }
    }
}