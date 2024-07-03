using System;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    private const float PLATES_SPAWN_TIMER_MAX = 4f;
    private const int PLATES_SPAWN_AMOUNT_MAX = 4;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float plateSpawnTimer;
    private int platesAmount;

    public event EventHandler OnPlateAdded;
    public event EventHandler OnPlateRemoved;

    private void Awake()
    {
        plateSpawnTimer = 0f;
        platesAmount = 0;
    }
    private void Update()
    {
        if (GameHandler.Instance.IsGamePlaying() && platesAmount < PLATES_SPAWN_AMOUNT_MAX)
        {
            plateSpawnTimer += Time.deltaTime;
            if (plateSpawnTimer >= PLATES_SPAWN_TIMER_MAX)
            {
                OnPlateAdded?.Invoke(this, EventArgs.Empty);
                platesAmount++;
                plateSpawnTimer = 0f;
            }

        }
    }
    public override void Interact(Player player)
    {
        if (platesAmount > 0)
        {
            // if counter has plate(s)
            if (!player.HasCurrentKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                platesAmount--;
            }
        }
    }
}
