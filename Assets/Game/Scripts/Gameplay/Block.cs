using UnityEngine;

namespace Gameplay
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private LayerMask _cellLayerMask;

        public Cell GetCellUnder()
        {
            Ray2D ray = new Ray2D(transform.position, Vector2.zero);
            RaycastHit2D raycastHit = Physics2D.Raycast(ray.origin, ray.direction);

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