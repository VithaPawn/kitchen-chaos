using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeContents(RecipeSO recipeSO)
    {
        recipeName.text = recipeSO.recipeName;
        foreach (Transform child in iconContainer)
        {
            if (child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOArray)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
