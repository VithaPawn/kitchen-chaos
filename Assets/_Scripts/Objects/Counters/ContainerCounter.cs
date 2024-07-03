using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {

    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnGrabbedKitchenObject;

    public override void Interact(Player player)
    {
        if (!player.HasCurrentKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            // Fire grab kitchen object event for animator
            OnGrabbedKitchenObject?.Invoke(this, EventArgs.Empty);
        }
    }

}
