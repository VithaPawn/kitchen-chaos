using UnityEngine;

public class ClearCounter : MonoBehaviour {

    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;
    [SerializeField]
    private Transform counterTopPoint;
    [SerializeField]
    private ClearCounter targetClearCounter;
    [SerializeField]
    private bool testing;

    private KitchenObject currentKitchenObject;

    private void Update()
    {
        if (testing && Input.GetKeyDown(KeyCode.R))
        {
            if (currentKitchenObject != null)
            {
                currentKitchenObject.SetParentClearCounter(targetClearCounter);
            }
        }
    }
    public void Interact()
    {
        if (currentKitchenObject == null)
        {
            // Set kitchen prefab on the top of clear counter
            Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            KitchenObjectTransform.localPosition = Vector3.zero;

            // Assign kitchen object and clear counter for each other
            currentKitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();
            currentKitchenObject.SetParentClearCounter(this);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public KitchenObject GetCurrentKitchenObject()
    {
        return currentKitchenObject;
    }

    public void SetCurrentKitchenObject(KitchenObject kitchenObject)
    {
        currentKitchenObject = kitchenObject;

    }

    public void ClearCurrentKitchenObject()
    {
        currentKitchenObject = null;
    }

    public bool HasCurrentKitchenObject()
    {
        return (currentKitchenObject != null);
    }

}
