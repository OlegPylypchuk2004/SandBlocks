using DG.Tweening;
using TMPro;
using UnityEngine;
using VContainer;

namespace ScoreSystem
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private float _animationDuration;
        [SerializeField] private Ease _animationEase;
        [SerializeField] private TextMeshProUGUI _textMesh;

        private ScoreCounter _scoreCounter;
        private int _currentScore;
        private Tween _currentTween;

        [Inject]
        private void Construct(ScoreCounter scoreCounter)
        {
            _scoreCounter = scoreCounter;
        }

        private void OnEnable()
        {
            _scoreCounter.ScoreChanged += OnScoreChanged;
        }

        private void OnDisable()
        {
            _scoreCounter.ScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            _currentTween?.Kill();

            _currentTween = DOTween.To(() => _currentScore, x => _currentScore = x, score, _animationDuration)
                .SetEase(_animationEase)
                .SetLink(gameObject)
                .OnUpdate(() =>
                {
                    _textMesh.text = _currentScore.ToString("D6");
                });
        }
    }
}