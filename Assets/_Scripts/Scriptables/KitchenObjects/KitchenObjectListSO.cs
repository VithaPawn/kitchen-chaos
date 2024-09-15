using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KitchenObjectListSO", menuName = "Scriptable Objects/Kitchen Objects/Kitchen Object List")]
public class KitchenObjectListSO : ScriptableObject {
    [SerializeField] private List<KitchenObjectSO> kitchenObjectList;

    public List<KitchenObjectSO> GetKitchenObjectList() {  return kitchenObjectList; }
}
