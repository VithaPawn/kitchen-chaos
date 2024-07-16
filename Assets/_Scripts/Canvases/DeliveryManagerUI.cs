using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
    [SerializeField] DeliveryManager deliveryManager;
    [SerializeField] Transform deliveryMenu;
    [SerializeField] Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnDeliveryMenuUpdated += DeliveryManager_OnDeliveryMenuUpdated;
    }

    private void DeliveryManager_OnDeliveryMenuUpdated(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in deliveryMenu)
        {
            if (child != recipeTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (RecipeSO recipeSO in deliveryManager.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, deliveryMenu);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeContents(recipeSO);
            recipeTransform.gameObject.SetActive(true);
        }
    }
}
