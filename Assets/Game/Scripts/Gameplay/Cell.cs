using UnityEngine;

namespace Gameplay
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private GameObject _filledView;

        public bool IsFilled
        {
            get
            {
                return _filledView.activeInHierarchy;
            }
            set
            {
                _filledView.SetActive(value);
            }
        }
    }
}