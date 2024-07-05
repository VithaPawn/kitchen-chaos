using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "Scriptable Objects/Recipes/Recipe")]
public class RecipeSO : ScriptableObject {
    public string recipeName;
    public List<KitchenObjectSO> kitchenObjectSOArray;
}
