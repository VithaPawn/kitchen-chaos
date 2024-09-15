using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipeSO", menuName = "Scriptable Objects/Recipes/Cutting Recipe")]
public class CuttingRecipeSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
