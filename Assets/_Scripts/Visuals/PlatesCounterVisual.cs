using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {

    private const float PLATE_HEIGHT = 0.1f;

    [SerializeField]
    private PlatesCounter platesCounter;
    [SerializeField]
    private Transform plateVisualPrefab;
    [SerializeField]
    private Transform topPointCounter;

    private List<GameObject> plateGameObjectsArray;

    private void Awake()
    {
        plateGameObjectsArray = new List<GameObject>();
    }
    private void Start()
    {
        platesCounter.OnPlateAdded += PlatesCounter_OnPlateAdded;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject removedGameObject = plateGameObjectsArray[plateGameObjectsArray.Count - 1];
        plateGameObjectsArray.Remove(removedGameObject);
        Destroy(removedGameObject);
    }

    private void PlatesCounter_OnPlateAdded(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, topPointCounter);
        plateVisualTransform.localPosition = new Vector3(0, plateGameObjectsArray.Count * PLATE_HEIGHT, 0);

        plateGameObjectsArray.Add(plateVisualTransform.gameObject);
    }
}
