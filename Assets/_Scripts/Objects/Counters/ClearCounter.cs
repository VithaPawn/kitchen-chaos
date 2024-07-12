public class ClearCounter : BaseCounter {

    public override void Interact(Player player)
    {
        if (!HasCurrentKitchenObject())
        {
            // if counter does have KO
            if (player.HasCurrentKitchenObject())
            {
                // if player has KO
                player.GetCurrentKitchenObject().SetKitchentObjectParent(this);
            }
        }
        else
        {
            // if counter has KO
            if (!player.HasCurrentKitchenObject())
            {
                // if player is not grabbing KO
                GetCurrentKitchenObject().SetKitchentObjectParent(player);
            }
            else
            {
                // if player is grabbing a plate
                if (player.GetCurrentKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetCurrentKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
                    }
                }
                else if (GetCurrentKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(player.GetCurrentKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(player.GetCurrentKitchenObject());
                    }
                }
            }
        }
    }

}
