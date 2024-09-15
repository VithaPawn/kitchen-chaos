using System;
using Unity.Netcode;

public class TrashCounter : BaseCounter {

    public static event Action<BaseCounter> OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasCurrentKitchenObject())
        {
            //Destroy kitchen object
            KitchenObject.DestroyKitchenObject(player.GetCurrentKitchenObject());
            //Trigger throwing trash event
            IKitchenObjectParent playerKitchenObjectParent = player.GetComponent<IKitchenObjectParent>();
            OnThrowTrashServerRpc(playerKitchenObjectParent.GetNetworkObject());
        }
    }

    [ServerRpc (RequireOwnership = false)]
    private void OnThrowTrashServerRpc(NetworkObjectReference player)
    {
        OnThrowTrashClientRpc(player);
    }

    [ClientRpc]
    private void OnThrowTrashClientRpc(NetworkObjectReference playerReference)
    {
        playerReference.TryGet(out NetworkObject playerNetworkObject);
        if (playerNetworkObject.TryGetComponent(out Player player))
        {
            OnAnyObjectTrashed?.Invoke(this);
        }

    }
}
