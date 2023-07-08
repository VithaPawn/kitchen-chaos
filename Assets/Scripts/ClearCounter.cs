using UnityEngine;

public class ClearCounter : MonoBehaviour {

    [SerializeField]
    private KitchenObjectSO kitchenObjectSO;
    [SerializeField]
    private Transform counterTopPoint;
    public void Interact()
    {
        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
        KitchenObjectTransform.localPosition = Vector3.zero;

        Debug.Log(kitchenObjectSO.objectName);
    }

}
