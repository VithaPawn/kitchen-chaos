using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent {
    public Transform GetKitchenObjectFollowTransform();

    public KitchenObject GetCurrentKitchenObject();

    public void SetCurrentKitchenObject(KitchenObject kitchenObject);

    public void ClearCurrentKitchenObject();

    public bool HasCurrentKitchenObject();

    public NetworkObject GetNetworkObject();
}
