using System;
using UnityEngine;

namespace Gameplay
{
    public class Block : MonoBehaviour
    {
        [field: SerializeField, Min(0)] public int ColorIndex { get; private set; }

        [SerializeField] private LayerMask _cellLayerMask;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Color _color;
        private bool _isShine;

        public event Action<Vector2> PositionChanged;
        public event Action<Color> ColorChanged;

        public Vector2 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;

                PositionChanged?.Invoke(Position);
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;

                if (!IsShine)
                {
                    _spriteRenderer.color = _color;
                }

                ColorChanged?.Invoke(Color);
            }
        }

        public int SpriteOrderInLayer
        {
            get
            {
                return _spriteRenderer.sortingOrder;
            }
            set
            {
                _spriteRenderer.sortingOrder = value;
            }
        }

        public bool IsShine
        {
            get
            {
                return _isShine;
            }
            set
            {
                _isShine = value;

                if (_spriteRenderer == null)
                {
                    return;
                }

                if (_isShine)
                {
                    _spriteRenderer.color = Color.white;
                }
                else
                {
                    _spriteRenderer.color = Color;
                }
            }
        }

        public Cell GetCellUnder()
        {
            Ray2D ray = new Ray2D(transform.position, Vector2.zero);
            RaycastHit2D raycastHit = Physics2D.Raycast(ray.origin, ray.direction, 1f, _cellLayerMask);

            if (raycastHit.collider == null)
            {
                return null;
            }
            else
            {
                if (raycastHit.collider.TryGetComponent(out Cell cell))
                {
                    return cell;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}