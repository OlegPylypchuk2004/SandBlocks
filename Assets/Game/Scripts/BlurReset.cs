using LeTai.Asset.TranslucentImage;
using UnityEngine;

public class BlurReset : MonoBehaviour
{
    [SerializeField] private TranslucentImageSource _translucentImageSource;

    private void OnDestroy()
    {
        _translucentImageSource.BlurConfig.Strength = 0f;
    }
}