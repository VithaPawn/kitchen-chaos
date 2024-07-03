using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;

    public event EventHandler<IHasProgress.OnProgressBarChangedEventArgs> OnProgressBarChanged;
    public event EventHandler OnCut;
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public override void Interact(Player player)
    {
        if (!HasCurrentKitchenObject())
        {
            // if counter does have KO
            if (player.HasCurrentKitchenObject() && !(player.GetCurrentKitchenObject() is PlateKitchenObject))
            {
                // if player has KO
                player.GetCurrentKitchenObject().SetKitchentObjectParent(this);

                // if KO can be cutted, display progress bar
                CuttingRecipeSO outputKitchenObjectSO = GetCuttingRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
                if (outputKitchenObjectSO != null)
                {
                    OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                    {
                        progressBarPercentage = (float)GetCurrentKitchenObject().GetCuttingProgress() / outputKitchenObjectSO.cuttingProgressMax
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

                // hide progress bar after player grabbed KO from cutting counter
                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                {
                    progressBarPercentage = 0f
                });
            }
            else if (player.GetCurrentKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetCurrentKitchenObject().GetKitchenObjectSO()))
                {
                    GetCurrentKitchenObject().DestroySelf();
                }
            }
        }
    }

    public override void InteractAlternate()
    {
        if (HasCurrentKitchenObject())
        {

            CuttingRecipeSO outputKitchenObjectSO = GetCuttingRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
            if (outputKitchenObjectSO != null)
            {
                // update cuttingProgress in KO
                int cuttingProgress = GetCurrentKitchenObject().GetCuttingProgress();
                GetCurrentKitchenObject().SetCuttingProgress(++cuttingProgress);

                // update cuttingProgress UI
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
                OnProgressBarChanged?.Invoke(this, new IHasProgress.OnProgressBarChangedEventArgs
                {
                    progressBarPercentage = (float)GetCurrentKitchenObject().GetCuttingProgress() / outputKitchenObjectSO.cuttingProgressMax
                });

                // destroy current KO and replace new KO when cutting progress is completed
                if (GetCurrentKitchenObject().GetCuttingProgress() >= outputKitchenObjectSO.cuttingProgressMax)
                {
                    // Destroy current kitchen object 
                    GetCurrentKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(outputKitchenObjectSO.output, this);
                }
            }

        }
    }

    private CuttingRecipeSO GetCuttingRecipeFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

}
