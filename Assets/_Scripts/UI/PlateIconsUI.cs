using UnityEngine;

public class PlateIconsUI : MonoBehaviour {
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        plateKitchenObject.OnAddIgredient += PlateKitchenObject_OnAddIgredient;
    }

    private void PlateKitchenObject_OnAddIgredient(KitchenObjectSO kitchenObjectSO)
    {
        Transform iconTransform = Instantiate(iconTemplate, transform);
        iconTransform.GetComponent<PlateIconsSingleUI>().SetPlateIcon(kitchenObjectSO);
        iconTransform.gameObject.SetActive(true);
    }
}
