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

    private NetworkVariable<float> cookingTimer;
    private FryingRecipeSO currentFryingRecipeSO;
    private BurningRecipeSO currentBurningRecipeSO;
    private State state;

    public event Action<float>  OnProgressBarChanged;
    public event Action<State> OnStoveRunning;

    private void Start()
    {
        state = State.Idle;
    }

    public override void OnNetworkSpawn()
    {
        cookingTimer.OnValueChanged += CookingTime_OnValueChanged;
    }

    private void CookingTime_OnValueChanged(float previousValue, float newValue)
    {
        //error here
        float fryingTimerMax = currentFryingRecipeSO != null ? currentFryingRecipeSO.fryingTimerMax : 1f;
        OnProgressBarChanged?.Invoke(cookingTimer.Value / fryingTimerMax);
    }

    private void Update()
    {
        if (!IsServer) return;

        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                cookingTimer.Value += Time.deltaTime;
                if (cookingTimer.Value >= currentFryingRecipeSO.fryingTimerMax)
                {
                    KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
                    KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);
                    currentBurningRecipeSO = GetBurningRecipeFromInput(GetCurrentKitchenObject().GetKitchenObjectSO());
                    cookingTimer.Value = 0f;
                    state = State.Burning;

                    OnStoveRunning?.Invoke(state);
                }
                break;
            case State.Burning:
                cookingTimer.Value += Time.deltaTime;
                if (cookingTimer.Value >= currentBurningRecipeSO.burningTimerMax)
                {
                    KitchenObject.DestroyKitchenObject(GetCurrentKitchenObject());
                    KitchenObject.SpawnKitchenObject(currentBurningRecipeSO.output, this);
                    cookingTimer.Value = 0f;
                    state = State.Idle;

                    OnStoveRunning?.Invoke(state);
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

    [ServerRpc (RequireOwnership = false)]
    private void StartFryingKitchenObjectServerRpc(int kitchenObjectIndex)
    {
        StartFryingKitchenObjectClientRpc(kitchenObjectIndex);
    }

    [ClientRpc]
    private void StartFryingKitchenObjectClientRpc(int kitchenObjectIndex)
    {
        KitchenObjectSO currentKitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);
        currentFryingRecipeSO = GetFryingRecipeFromInput(currentKitchenObjectSO);
        cookingTimer = 0f;
        state = State.Frying;
        OnStoveRunning?.Invoke(state);
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

        OnProgressBarChanged?.Invoke(0f);

        OnStoveRunning?.Invoke(state);
    }
}
