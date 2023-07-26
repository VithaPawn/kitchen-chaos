using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private ClearCounter parentClearCounter;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public ClearCounter GetParentClearCounter()
    {
        return parentClearCounter;
    }

    public void SetParentClearCounter(ClearCounter clearCounter)
    {
        if (parentClearCounter != null)
        {
            parentClearCounter.ClearCurrentKitchenObject();
        }

        parentClearCounter = clearCounter;

        if (!clearCounter.HasCurrentKitchenObject())
        {
            clearCounter.SetCurrentKitchenObject(this);
            transform.parent = clearCounter.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }
    }
}
