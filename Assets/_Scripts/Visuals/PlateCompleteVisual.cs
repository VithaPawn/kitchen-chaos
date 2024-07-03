using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private KitchenObjectSO breadKitchenObjectSO;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectArray;

    private void Awake()
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSOGameObjectArray)
        {
            kitchenObjectSO_GameObject.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        plateKitchenObject.OnAddIgredient += PlateKitchenObject_OnAddIgredient;
    }

    private void PlateKitchenObject_OnAddIgredient(object sender, PlateKitchenObject.OnAddIgredientEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSOGameObjectArray)
        {
            if (kitchenObjectSO_GameObject.kitchenObjectSO == e.KitchenObjectSO)
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
        }
    }
}
