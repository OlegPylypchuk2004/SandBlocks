using UnityEngine;

namespace Gameplay
{
    public class BlockSpriteDark : MonoBehaviour
    {
        [SerializeField] private float noiseScale;
        [SerializeField] private float darknessStrength;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Block _block;

        private void Start()
        {
            UpdateDisplay();
        }

        private void OnEnable()
        {
            _block.PositionChanged += OnPositionChanged;
        }

        private void OnDisable()
        {
            _block.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged(Vector2 position)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            float noise = Mathf.PerlinNoise(transform.position.x * noiseScale, transform.position.y * noiseScale);
            float darkness = 1f - noise * darknessStrength;

            Color color = _spriteRenderer.color;
            color.a = Mathf.Clamp01(darkness);

            _spriteRenderer.color = color;
        }
    }
}