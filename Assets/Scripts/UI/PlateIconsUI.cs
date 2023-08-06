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

    private void PlateKitchenObject_OnAddIgredient(object sender, PlateKitchenObject.OnAddIgredientEventArgs e)
    {
        Transform iconTransform = Instantiate(iconTemplate, transform);
        iconTransform.GetComponent<PlateIconsSingleUI>().SetPlateIcon(e.KitchenObjectSO);
        iconTransform.gameObject.SetActive(true);
    }
}
