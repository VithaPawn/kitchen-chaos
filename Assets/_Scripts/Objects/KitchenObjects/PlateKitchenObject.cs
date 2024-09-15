using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    [SerializeField]
    private List<KitchenObjectSO> validKitchenObjectSOArray;

    private List<KitchenObjectSO> kitchenObjectSOArray;

    public event Action<KitchenObjectSO> OnAddIgredient;

    private void Start()
    {
        kitchenObjectSOArray = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOArray.Contains(kitchenObjectSO)) return false;
        if (kitchenObjectSOArray.Contains(kitchenObjectSO)) return false;
        AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetIndexOfKitchenObjectSO(kitchenObjectSO));
        return true;
    }

    [ServerRpc (RequireOwnership = false)]
    private void AddIngredientServerRpc(int index)
    {
        AddIngredientClientRpc(index);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(int index)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(index);
        kitchenObjectSOArray.Add(kitchenObjectSO);
        OnAddIgredient?.Invoke(kitchenObjectSO);
    }

    public List<KitchenObjectSO> GetKitchenObjectSOArray()
    {
        return kitchenObjectSOArray;
    }

}
