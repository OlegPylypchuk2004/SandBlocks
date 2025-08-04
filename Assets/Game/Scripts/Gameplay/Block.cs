using System;
using UnityEngine;

namespace Gameplay
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private LayerMask _cellLayerMask;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public event Action<Vector2> PositionChanged;

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

        public void ApplyColor(Color color)
        {
            _spriteRenderer.color = color;
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