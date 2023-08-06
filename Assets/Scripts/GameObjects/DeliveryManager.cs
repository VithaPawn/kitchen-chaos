using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeMenuSO recipeMenuSO;

    private float spawnRecipeTimerMax = 4f;
    private int recipesAmountMax = 4;
    private float spawnRecipeTimer;
    private int deliveredRecipesAmount;

    private List<RecipeSO> waitingRecipeSOList;

    public event EventHandler OnDeliveryMenuUpdated;
    public event EventHandler OnDeliverySuccess;
    public event EventHandler OnDeliveryFail;

    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>();
        if (Instance != null)
        {
            Debug.LogError("There is more than one Delivery Manager instance");
        }
        Instance = this;
    }

    private void Update()
    {
        if (!GameHandler.Instance.IsGamePlaying()) return;
        if (waitingRecipeSOList.Count < recipesAmountMax)
        {
            spawnRecipeTimer += Time.deltaTime;
            if (spawnRecipeTimer >= spawnRecipeTimerMax)
            {
                RecipeSO newRecipeSO = recipeMenuSO.recipeSOList[UnityEngine.Random.Range(0, recipeMenuSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(newRecipeSO);
                OnDeliveryMenuUpdated?.Invoke(this, EventArgs.Empty);
                spawnRecipeTimer = 0f;
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            // loop in waiting recipe list
            RecipeSO recipeSO = waitingRecipeSOList[i];
            if (plateKitchenObject.GetKitchenObjectSOArray().Count == recipeSO.kitchenObjectSOArray.Count)
            {
                // check if plate recipe and waiting recipe have the same ammount ingredient or not
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in recipeSO.kitchenObjectSOArray)
                {
                    // loop ingredient in waiting recipe
                    bool IngredientMatched = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOArray())
                    {
                        // loop ingredient in plate recipe
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient matches
                            IngredientMatched = true;
                            break;
                        }
                    }
                    if (!IngredientMatched)
                    {
                        // if ingredient did not matched, they are different recipes
                        plateContentsMatchesRecipe = false;
                        break;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    waitingRecipeSOList.RemoveAt(i);
                    OnDeliveryMenuUpdated?.Invoke(this, EventArgs.Empty);
                    OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
                    deliveredRecipesAmount++;
                    return;
                }
            }
        }
        OnDeliveryFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetDeliveredRecipesAmount()
    {
        return deliveredRecipesAmount;
    }

}
