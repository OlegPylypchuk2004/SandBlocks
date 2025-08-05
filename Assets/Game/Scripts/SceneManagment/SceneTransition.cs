using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace SceneManagment
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private RectMask2D _rectMask;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

        private SceneLoader _sceneLoader;

        public event Action Appeared;
        public event Action Disappeared;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            _sceneLoader.LoadStarted += OnSceneLoadStarted;
            _sceneLoader.LoadCompleted += OnSceneLoadCompleted;

            Disappear();
        }

        private void OnDestroy()
        {
            _sceneLoader.LoadStarted -= OnSceneLoadStarted;
            _sceneLoader.LoadCompleted -= OnSceneLoadCompleted;
        }

        private void OnSceneLoadStarted()
        {
            Appear();
        }

        private void OnSceneLoadCompleted()
        {
            Disappear();
        }

        private Sequence Appear()
        {
            if (_eventSystem != null)
            {
                _eventSystem.gameObject.SetActive(false);
            }

            Sequence sequence = DOTween.Sequence();
            sequence.SetLink(gameObject);

            sequence.AppendCallback(() =>
            {
                _rectMask.gameObject.SetActive(true);
                _verticalLayoutGroup.spacing = 2250f;
            });

            sequence.Append(DOTween.To(() => _verticalLayoutGroup.spacing, x => _verticalLayoutGroup.spacing = x, 0f, 0.5f)
                .SetEase(Ease.OutQuad));

            sequence.AppendCallback(() =>
            {
                Appeared?.Invoke();
            });

            return sequence;
        }

        private Sequence Disappear()
        {
            if (_eventSystem != null)
            {
                _eventSystem.gameObject.SetActive(false);
            }

            Sequence sequence = DOTween.Sequence();
            sequence.SetLink(gameObject);

            sequence.AppendCallback(() =>
            {
                _rectMask.gameObject.SetActive(true);
                _verticalLayoutGroup.spacing = 0f;
            });

            sequence.Append(DOTween.To(() => _verticalLayoutGroup.spacing, x => _verticalLayoutGroup.spacing = x, 2250f, 0.5f)
                .SetEase(Ease.InQuad));

            sequence.AppendInterval(0.25f);

            sequence.Append(_verticalLayoutGroup.transform.DORotate(new Vector3(0f, 0f, -22.5f), 0.25f)
                .SetEase(Ease.OutQuad));

            sequence.AppendCallback(() =>
            {
                _rectMask.gameObject.SetActive(false);

                if (_eventSystem != null)
                {
                    _eventSystem.gameObject.SetActive(true);
                }
            });

            sequence.AppendCallback(() =>
            {
                Disappeared?.Invoke();
            });

            return sequence;
        }
    }
}