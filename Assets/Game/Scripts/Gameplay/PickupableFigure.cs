using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PickupableFigure : MonoBehaviour
    {
        [SerializeField] private Block[] _blocks;
        [SerializeField] private GameObject[] _outlines;
        [SerializeField] private GameObject _shine;

        [field: SerializeField] public float MaxXPosition { get; private set; }
        [field: SerializeField] public float MaxYPosition { get; private set; }

        private Color[] _colors;
        private Tween _shineTween;

        public Block[] Blocks => _blocks;

        public Color[] Colors
        {
            get
            {
                return _colors;
            }
            set
            {
                _colors = value;

                foreach (Block block in _blocks)
                {
                    int colorIndex = Mathf.Clamp(block.ColorIndex, 0, _colors.Length - 1);
                    block.Color = _colors[colorIndex];
                }
            }
        }

        private void Awake()
        {
            _shine.transform.localPosition = Vector2.one * 10f;
            _shine.gameObject.SetActive(false);
        }

        public void Pickup()
        {
            foreach (Block block in _blocks)
            {
                block.SpriteOrderInLayer = 100;

                foreach (GameObject outline in _outlines)
                {
                    outline.SetActive(false);
                }
            }

            _shineTween?.Kill();
        }

        public void Drop()
        {
            foreach (Block block in _blocks)
            {
                block.SpriteOrderInLayer = 0;
            }

            foreach (GameObject outline in _outlines)
            {
                outline.SetActive(true);
            }
        }

        public bool IsCanPutBlocks()
        {
            Cell[] cellsUnderBlocks = GetCellsUnderBlocks();

            if (cellsUnderBlocks.Length == 0 || cellsUnderBlocks.Length < _blocks.Length)
            {
                return false;
            }

            foreach (Cell cellUnderBlock in cellsUnderBlocks)
            {
                if (cellUnderBlock == null || cellUnderBlock.IsFilled)
                {
                    return false;
                }
            }

            return true;
        }

        public Cell[] GetCellsUnderBlocks()
        {
            List<Cell> cells = new List<Cell>();

            foreach (Block block in _blocks)
            {
                Cell cellUnder = block.GetCellUnder();

                if (cellUnder != null)
                {
                    cells.Add(cellUnder);
                }
            }

            return cells.ToArray();
        }

        public Tween Shine()
        {
            if (_shine == null)
            {
                return null;
            }

            _shineTween?.Kill();

            _shine.gameObject.SetActive(true);

            _shineTween = _shine.transform.DOLocalMove(Vector2.one * -10f, 0.5f)
                .From(Vector2.one * 10f)
                .SetDelay(0.125f)
                .SetEase(Ease.OutQuad)
                .SetLink(gameObject)
                .OnKill(() =>
                {
                    _shine.gameObject.SetActive(false);
                });

            return _shineTween;
        }
    }
}