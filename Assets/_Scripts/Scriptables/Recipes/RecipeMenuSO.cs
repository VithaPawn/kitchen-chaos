using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeMenuSO", menuName = "Scriptable Objects/Recipes/Recipe Menu")]
public class RecipeMenuSO : ScriptableObject {
    public List<RecipeSO> recipeSOList;
}
