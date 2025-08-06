using DG.Tweening;
using UnityEngine;

namespace Environment
{
    public class CloudsAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject[] _chunks;
        [SerializeField] private float _speed;

        private float _minXPosition;
        private float _maxXPosition;

        private void Awake()
        {
            float spacing = 0f;

            if (_chunks.Length > 1)
            {
                spacing = _chunks[1].transform.localPosition.x - _chunks[0].transform.localPosition.x;
            }

            _minXPosition = _chunks[0].transform.localPosition.x;
            _maxXPosition = _chunks[_chunks.Length - 1].transform.localPosition.x + spacing;
        }

        private void Start()
        {
            foreach (GameObject chunk in _chunks)
            {
                MoveChunk(chunk);
            }
        }

        private Tween MoveChunk(GameObject chunk)
        {
            return chunk.transform.DOLocalMoveX(_maxXPosition, _speed)
                .SetEase(Ease.Linear)
                .SetSpeedBased()
                .SetLink(gameObject)
                .OnKill(() =>
                {
                    chunk.transform.position = new Vector2(_minXPosition, chunk.transform.position.y);

                    MoveChunk(chunk);
                });
        }
    }
}