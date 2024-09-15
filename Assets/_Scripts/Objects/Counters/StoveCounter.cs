using System;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    public enum State {
        Idle,
        Frying,
        Burning,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private NetworkVariable<float> cookingTimer = new NetworkVariable<float>(0);
    private FryingRecipeSO currentFryingRecipeSO;
    private BurningRecipeSO currentBurningRecipeSO;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);

    public event Action<float> OnProgressBarChanged;
    public event Action<State> OnStoveRunning;

    public override void OnNetworkSpawn()
    {
        cookingTimer.OnValueChanged += CookingTime_OnValueChanged;
    }

    private void CookingTime_OnValueChanged(float previousValue, float newValue)
    {
        if (state.Value == State.Frying)
        {
            float fryingTimerMax = currentFryingRecipeSO != null ? currentFryingRecipeSO.fryingTimerMax : 1f;
            OnProgressBarChanged?.Invoke(cookingTimer.Value / fryingTimerMax);
        }
        else if (state.Value == State.Burning)
        {
            float burningTimerMax = currentBurningRecipeSO != null ? currentBurningRecipeSO.burningTimerMax : 1f;
            OnProgressBarChanged?.Invoke(cookingTimer.Value / burningTimerMax);
        }
    }

    private void Update()
    {
        if (!IsServer) return;

        switch (state.Value)
        {
            case State.Idle:
                break;
            case State.Frying:
                cookingTimer.Value += Time.deltaTime;
                if (cookingTimer.Value >= currentFryingRecipeSO.fryingTimerMax)
                {
                    KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
                    KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);
                    cookingTimer.Value = 0f;
                    state.Value = State.Burning;
                    TriggerStoveRunningEventClientRpc(state.Value);
                    SetBurningRecipeSOClientRpc();
                }
                break;
            case State.Burning:
                cookingTimer.Value += Time.deltaTime;
                if (cookingTimer.Value >= currentBurningRecipeSO.burningTimerMax)
                {
                    KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
                    KitchenObject.SpawnKitchenObject(currentBurningRecipeSO.output, this);
                    cookingTimer.Value = 0f;
                    state.Value = State.Idle;
                    TriggerStoveRunningEventClientRpc(state.Value);
                }
                break;
        }

    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc()
    {
        currentBurningRecipeSO = currentBurningRecipeSO = GetBurningRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
    }

    [ClientRpc]
    private void TriggerStoveRunningEventClientRpc(State state)
    {
        OnStoveRunning?.Invoke(state);
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
                    KitchenObject currentKitchenObject = player.GetCurrentKitchenObject();
                    currentKitchenObject.SetKitchentObjectParent(this);
                    StartFryingKitchenObjectServerRpc(KitchenGameMultiplayer.Instance.GetIndexOfKitchenObjectSO(currentKitchenObject.GetKitchenObjectSO()));
                }
            }
        }
        else
        {
            // if counter has KO
            if (!player.HasCurrentKitchenObject())
            {
                GetCurrentKitchenObject().SetKitchentObjectParent(player);
                ResetCookingStateServerRpc();
            }
            else if (player.GetCurrentKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(GetCurrentKitchenObject().GetKitchenObjectSO()))
                {
                    ResetCookingStateServerRpc();
                    KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartFryingKitchenObjectServerRpc(int kitchenObjectIndex)
    {
        state.Value = State.Frying;
        cookingTimer.Value = 0f;
        TriggerStoveRunningEventClientRpc(state.Value);
        SetFryingRecipeSOClientRpc(kitchenObjectIndex);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectIndex)
    {
        KitchenObjectSO currentKitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);
        currentFryingRecipeSO = GetFryingRecipeFromInput(currentKitchenObjectSO);
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

    [ServerRpc(RequireOwnership = false)]
    private void ResetCookingStateServerRpc()
    {
        cookingTimer.Value = 0f;
        state.Value = State.Idle;
        TriggerStoveRunningEventClientRpc(state.Value);
        ResetCookingStateClientRpc();
    }

    [ClientRpc]
    private void ResetCookingStateClientRpc()
    {
        currentFryingRecipeSO = null;
        currentBurningRecipeSO = null;
        OnProgressBarChanged?.Invoke(0f);
    }
}
