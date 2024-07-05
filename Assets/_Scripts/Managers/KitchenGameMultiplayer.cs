using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour {
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private KitchenObjectListSO kitchenObjectListSO;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one KitchenGameMultiplayer Instance at the same time!");
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetIndexOfKitchenObjectSO(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc (RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        // Spawn kitchen objects 
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);

        ////Set KitchenObjectParent
        if (kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetowrkObject))
        {
            IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetowrkObject.GetComponent<IKitchenObjectParent>();
            KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

            kitchenObject.SetKitchentObjectParent(kitchenObjectParent);
        }

    }

    public int GetIndexOfKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        return kitchenObjectListSO.GetKitchenObjectList().IndexOf(kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSOFromIndex(int index)
    {
        return kitchenObjectListSO.GetKitchenObjectList()[index];
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {

        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc (RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectReference)
    {
        kitchenObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        if (kitchenObjectNetworkObject.TryGetComponent(out KitchenObject kitchenObject))
        {
            kitchenObject.DestroySelf();
        }
    }
}
