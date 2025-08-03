using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "ColorConfig", menuName = "Configs/Color")]
    public class ColorConfig : ScriptableObject
    {
        [field: SerializeField] public Color Color { get; private set; }
    }
}