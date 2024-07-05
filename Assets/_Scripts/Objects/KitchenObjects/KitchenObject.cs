using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    private FollowTransform followTransform;
    private IKitchenObjectParent kitchenObjectParent;
    private int cuttingProgress;

    private void Awake()
    {
        cuttingProgress = 0;
        followTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void SetKitchentObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        SetKitchentObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc (RequireOwnership = false)]
    private void SetKitchentObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchentObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchentObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        if (kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetowrkObject))
        {
            IKitchenObjectParent newKitchenObjectParent = kitchenObjectParentNetowrkObject.GetComponent<IKitchenObjectParent>();

            if (kitchenObjectParent != null)
            {
                kitchenObjectParent.ClearCurrentKitchenObject();
            }

            kitchenObjectParent = newKitchenObjectParent;

            if (!newKitchenObjectParent.HasCurrentKitchenObject())
            {
                newKitchenObjectParent.SetCurrentKitchenObject(this);
                followTransform.SetTargetTransform(newKitchenObjectParent.GetKitchenObjectFollowTransform());
            }
        }
    }

    public void DestroySelf()
    {
        ClearKitchenObjectOnParentServerRpc();
        Destroy(gameObject);
    }

    [ServerRpc (RequireOwnership = false)] 
    private void ClearKitchenObjectOnParentServerRpc()
    {
        ClearKitchenObjectOnParentClientRpc();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc()
    {
        GetKitchenObjectParent().ClearCurrentKitchenObject();
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
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
