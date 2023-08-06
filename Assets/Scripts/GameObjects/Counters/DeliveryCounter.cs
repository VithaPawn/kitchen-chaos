using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one Delivery Counter at the same time.");
        }
        else
        {
            Instance = this;
        }
    }
    public override void Interact(Player player)
    {
        if (player.HasCurrentKitchenObject())
        {
            if (player.GetCurrentKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetCurrentKitchenObject().DestroySelf();
            }
        }

    }
}
