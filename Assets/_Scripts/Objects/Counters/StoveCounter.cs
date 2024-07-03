using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    public enum State {
        Idle,
        Frying,
        Burning,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float cookingTimer;
    private FryingRecipeSO currentFryingRecipeSO;
    private BurningRecipeSO currentBurningRecipeSO;
    private State state;

    public event EventHandler<IHasProgress.OnProgressBarChangedEventArgs> OnProgressBarChanged;
    public event EventHandler<OnStoveRunningEventArgs> OnStoveRunning;
    public class OnStoveRunningEventArgs : EventArgs {
        public State state;
    }


    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                {
                    progressBarPercentage = cookingTimer / currentFryingRecipeSO.fryingTimerMax
                });

                cookingTimer += Time.deltaTime;
                if (cookingTimer >= currentFryingRecipeSO.fryingTimerMax)
                {
                    GetCurrentKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);
                    currentBurningRecipeSO = GetBurningRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
                    cookingTimer = 0f;
                    state = State.Burning;

                    OnStoveRunning?.Invoke(this, new OnStoveRunningEventArgs
                    {
                        state = state
                    });
                }
                break;
            case State.Burning:
                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                {
                    progressBarPercentage = cookingTimer / currentBurningRecipeSO.burningTimerMax
                });

                cookingTimer += Time.deltaTime;
                if (cookingTimer >= currentBurningRecipeSO.burningTimerMax)
                {
                    GetCurrentKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(currentBurningRecipeSO.output, this);
                    cookingTimer = 0f;
                    state = State.Idle;

                    OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                    {
                        progressBarPercentage = 0f
                    });

                    OnStoveRunning?.Invoke(this, new OnStoveRunningEventArgs
                    {
                        state = state
                    });
                }
                break;
        }

    }

    public override void Interact(Player player)
    {
        if (!HasCurrentKitchenObject())
        {
            // if counter does have KO
            if (player.HasCurrentKitchenObject())
            {
                // if player has KO
                if (HasFryingRecipe(player.GetCurrentKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetCurrentKitchenObject().SetKitchentObjectParent(this);
                    currentFryingRecipeSO = GetFryingRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
                    cookingTimer = 0f;
                    state = State.Frying;

                    OnStoveRunning?.Invoke(this, new OnStoveRunningEventArgs
                    {
                        state = state
                    });
                }
            }
        }
        else
        {
            // if counter has KO
            if (!player.HasCurrentKitchenObject())
            {
                GetCurrentKitchenObject().SetKitchentObjectParent(player);
                SetCookingStateIdle();
            }
            if (player.GetCurrentKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetCurrentKitchenObject().GetKitchenObjectSO()))
                {
                    GetCurrentKitchenObject().DestroySelf();
                    SetCookingStateIdle();
                }
            }
        }
    }

    private FryingRecipeSO GetFryingRecipeFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    private bool HasFryingRecipe(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeFromInput(kitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private void SetCookingStateIdle()
    {
        currentFryingRecipeSO = null;
        cookingTimer = 0f;
        state = State.Idle;

        OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
        {
            progressBarPercentage = 0f
        });

        OnStoveRunning?.Invoke(this, new OnStoveRunningEventArgs
        {
            state = state
        });
    }
}
