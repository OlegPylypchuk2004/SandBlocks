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

            // Зменшуємо яскравість кольору (RGB), але залишаємо alpha = 1
            Color originalColor = _block.Color; // Наприклад, Color.white або інший базовий колір
            Color darkenedColor = new Color(
                originalColor.r * darkness,
                originalColor.g * darkness,
                originalColor.b * darkness,
                1f // Альфа завжди максимальна
            );

            _spriteRenderer.color = darkenedColor;
        }
    }
}