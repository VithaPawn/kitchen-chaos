
using UnityEngine;

[CreateAssetMenu(fileName = "FryingRecipeSO", menuName = "Scriptable Objects/Recipes/Frying Recipe")]
public class FryingRecipeSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryingTimerMax;
}
