using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    private int cuttingProgress;

    private void Awake()
    {
        cuttingProgress = 0;
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public IKitchenObjectParent GetKitchenObjectParennt()
    {
        return kitchenObjectParent;
    }

    public void SetKitchentObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearCurrentKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if (!kitchenObjectParent.HasCurrentKitchenObject())
        {
            kitchenObjectParent.SetCurrentKitchenObject(this);
            transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
            transform.localPosition = Vector3.zero;
        }
    }

    public void DestroySelf()
    {
        GetKitchenObjectParennt().ClearCurrentKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        // Get kitchen object prefab
        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        KitchenObject kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();

        // Set kitchen object (position, parent)
        kitchenObject.SetKitchentObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

    public int GetCuttingProgress()
    {
        return cuttingProgress;
    }

    public void SetCuttingProgress(int cuttingProcess)
    {
        this.cuttingProgress = cuttingProcess;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
