using System;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent {

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject currentKitchenObject;

    public static event EventHandler OnDropSomething;

    public static void ResetStaticData()
    {
        OnDropSomething = null;
    }

    public virtual void Interact(Player player)
    {
        Debug.LogError("Interact in BaseCounter");
    }

    public virtual void InteractAlternate()
    {
        Debug.LogError("Interact alternate in BaseCounter");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public KitchenObject GetCurrentKitchenObject()
    {
        return currentKitchenObject;
    }

    public void SetCurrentKitchenObject(KitchenObject kitchenObject)
    {
        currentKitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnDropSomething?.Invoke(this, EventArgs.Empty);
        }

    }

    public void ClearCurrentKitchenObject()
    {
        currentKitchenObject = null;
    }

    public bool HasCurrentKitchenObject()
    {
        return (currentKitchenObject != null);
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
