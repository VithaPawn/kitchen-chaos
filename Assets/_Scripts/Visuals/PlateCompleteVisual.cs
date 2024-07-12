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

    private void PlateKitchenObject_OnAddIgredient(KitchenObjectSO kitchenObjectSO)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSOGameObjectArray)
        {
            if (kitchenObjectSO_GameObject.kitchenObjectSO == kitchenObjectSO)
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
        }
    }
}
