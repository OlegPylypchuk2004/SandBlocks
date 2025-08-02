using UnityEngine;

namespace Gameplay
{
    public class FigureSpawnPoint : MonoBehaviour
    {
        private PickupableFigure _pickupableFigure;

        public PickupableFigure Figure
        {
            get
            {
                return _pickupableFigure;
            }
            set
            {
                _pickupableFigure = value;

                if (_pickupableFigure == null)
                {
                    return;
                }

                _pickupableFigure.transform.SetParent(transform);
                _pickupableFigure.transform.localPosition = Vector3.zero;

                PreviousFigure = _pickupableFigure;
            }
        }

        public PickupableFigure PreviousFigure { get; private set; }
    }
}