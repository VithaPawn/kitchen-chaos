using UnityEngine;

[CreateAssetMenu(fileName = "BurningRecipeSO", menuName = "Scriptable Objects/Recipes/Burning Recipe")]
public class BurningRecipeSO : ScriptableObject {
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float burningTimerMax;
}
