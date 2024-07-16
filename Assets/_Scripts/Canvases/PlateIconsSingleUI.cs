using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour {
    [SerializeField] private Image plateIcon;
    public void SetPlateIcon(KitchenObjectSO kitchenObjectSO)
    {
        plateIcon.sprite = kitchenObjectSO.sprite;
    }
}
