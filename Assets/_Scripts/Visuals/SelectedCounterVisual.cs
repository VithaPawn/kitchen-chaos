using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    [Header("Selected Event")]
    [SerializeField] private CounterEventSO selectedCounterChanged;

    // Start is called before the first frame update
    private void OnEnable()
    {
        selectedCounterChanged.OnCounterRequested += SelectedCounterChanged_OnCounterRequested;
    }

    private void OnDisable()
    {
        selectedCounterChanged.OnCounterRequested -= SelectedCounterChanged_OnCounterRequested;
    }

    private void SelectedCounterChanged_OnCounterRequested(BaseCounter counter)
    {
        if (counter == baseCounter)
        {
            foreach (var visualGameObject in visualGameObjectArray)
            {
                visualGameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var visualGameObject in visualGameObjectArray)
            {
                visualGameObject.SetActive(false);
            }
        }
    }

}
