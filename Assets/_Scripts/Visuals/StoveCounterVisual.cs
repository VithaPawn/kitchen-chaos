using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    // Start is called before the first frame update
    void Start()
    {
        stoveCounter.OnStoveRunning += StoveCounter_OnStoveRunning;
    }

    private void StoveCounter_OnStoveRunning(StoveCounter.State state)
    {
        bool showVisual = state == StoveCounter.State.Frying || state == StoveCounter.State.Burning;
        foreach (var visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(showVisual);
        }
    }
}
