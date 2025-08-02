using UnityEngine;

namespace CustomLayoutGroup
{
    public class WorldSpaceGridLayoutGroup : MonoBehaviour
    {
        [SerializeField, Min(0f)] private Vector2 _spacing;
        [SerializeField, Min(0f)] private Vector2 _cellSize;
        [SerializeField] private bool _isAutoUpdate;

        [field: SerializeField, Min(1)] public int Columns { get; set; }

        private void OnValidate()
        {
            if (_isAutoUpdate)
            {
                UpdateLayout();
            }
        }

        public void UpdateLayout()
        {
            Transform[] childrenTransforms = GetChildrenTransforms();
            int totalItems = childrenTransforms.Length;

            if (Columns <= 0 || totalItems <= 0)
            {
                return;
            }

            int rows = Mathf.CeilToInt((float)totalItems / Columns);

            float stepX = _cellSize.x + _spacing.x;
            float stepY = _cellSize.y + _spacing.y;

            float totalWidth = (Columns - 1) * stepX;
            float totalHeight = (rows - 1) * stepY;

            float startX = -totalWidth / 2f;
            float startY = totalHeight / 2f;

            for (int i = 0; i < totalItems; i++)
            {
                int row = i / Columns;
                int column = i % Columns;

                float x = startX + column * stepX;
                float y = startY - row * stepY;

                childrenTransforms[i].localPosition = new Vector3(x, y, 0f);
            }
        }

        private Transform[] GetChildrenTransforms()
        {
            Transform[] childrenTransforms = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                childrenTransforms[i] = transform.GetChild(i);
            }

            return childrenTransforms;
        }
    }
}