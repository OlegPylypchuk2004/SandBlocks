using TMPro;
using UnityEngine;
using VContainer;

namespace ScoreSystem
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMesh;

        private ScoreCounter _scoreCounter;

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
            _textMesh.text = $"{score}";
        }
    }
}