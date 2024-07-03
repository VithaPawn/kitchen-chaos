using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    [SerializeField]
    private List<KitchenObjectSO> validKitchenObjectSOArray;

    private List<KitchenObjectSO> kitchenObjectSOArray;

    public event EventHandler<OnAddIgredientEventArgs> OnAddIgredient;
    public class OnAddIgredientEventArgs : EventArgs {
        public KitchenObjectSO KitchenObjectSO;
    }

    private void Start()
    {
        kitchenObjectSOArray = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOArray.Contains(kitchenObjectSO)) return false;
        if (kitchenObjectSOArray.Contains(kitchenObjectSO)) return false;
        kitchenObjectSOArray.Add(kitchenObjectSO);
        OnAddIgredient?.Invoke(this, new OnAddIgredientEventArgs
        {
            KitchenObjectSO = kitchenObjectSO
        });
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOArray()
    {
        return kitchenObjectSOArray;
    }

}
