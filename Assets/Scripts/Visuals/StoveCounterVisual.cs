using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    // Start is called before the first frame update
    void Start()
    {
        stoveCounter.OnStoveRunning += StoveCounter_OnStoveRunning;
    }

    private void StoveCounter_OnStoveRunning(object sender, StoveCounter.OnStoveRunningEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Burning;
        foreach (var visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(showVisual);
        }
    }
}
