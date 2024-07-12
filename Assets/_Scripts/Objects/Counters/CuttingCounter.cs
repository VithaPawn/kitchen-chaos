using System;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;

    public event Action<float> OnProgressBarChanged;
    public event Action OnCut;
    public static event Action<BaseCounter> OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public override void Interact(Player player)
    {
        // if counter does have KO
        if (!HasCurrentKitchenObject())
        {
            // if player has KO
            if (player.HasCurrentKitchenObject() && !(player.GetCurrentKitchenObject() is PlateKitchenObject))
            {
                KitchenObject currentKitchenObject = player.GetCurrentKitchenObject();
                currentKitchenObject.SetKitchentObjectParent(this);

                // if KO can be cutted, display progress bar
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeFromInput(currentKitchenObject.GetKitchenObjectSO());
                if (cuttingRecipeSO != null)
                {
                    TriggerProgressBarChangedEventServerRpc((float)currentKitchenObject.GetCuttingProgress() / cuttingRecipeSO.cuttingProgressMax);
                }
            }
        }
        // if counter has KO
        else
        {
            if (!player.HasCurrentKitchenObject())
            {
                GetCurrentKitchenObject().SetKitchentObjectParent(player);

                // hide progress bar after player grabbed KO from cutting counter
                TriggerProgressBarChangedEventServerRpc(0f);
            }
            else if (player.GetCurrentKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetCurrentKitchenObject().GetKitchenObjectSO()))
                {
                    KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
                }
            }
        }
    }

    [ServerRpc (RequireOwnership = false)]
    private void TriggerProgressBarChangedEventServerRpc(float progress)
    {
        TriggerProgressBarChangedEventClientRpc(progress);
    }

    [ClientRpc]
    private void TriggerProgressBarChangedEventClientRpc(float progress)
    {
        OnProgressBarChanged?.Invoke(progress);
    }

    public override void InteractAlternate()
    {
        if (HasCurrentKitchenObject())
        {

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
            if (cuttingRecipeSO != null)
            {
                // update cuttingProgress in KO
                int updatedCuttingProgress = GetCurrentKitchenObject().GetCuttingProgress() + 1;
                UpdateCuttingProgressServerRpc(updatedCuttingProgress);

                // destroy current KO and replace new KO when cutting progress is completed
                CompleteCuttingProgressServerRpc(updatedCuttingProgress);
            }

        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateCuttingProgressServerRpc(int cuttingProgress)
    {
        UpdateCuttingProgressClientRpc(cuttingProgress);
    }

    [ClientRpc]
    private void UpdateCuttingProgressClientRpc(int cuttingProgress)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
        GetCurrentKitchenObject().SetCuttingProgress(cuttingProgress);
        // update cuttingProgress UI
        OnCut?.Invoke();
        OnAnyCut?.Invoke(this);
        OnProgressBarChanged?.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CompleteCuttingProgressServerRpc(int cuttingProgress)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
        {
            // Destroy current kitchen object 
            KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
            KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
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
