using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class MarkerVehicleSpawner : MonoBehaviour
{
    public GameObject carPrefab1;
    public GameObject carPrefab2;

    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> spawnedVehicles = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            SpawnOrUpdate(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            SpawnOrUpdate(trackedImage);
        }
    }

    void SpawnOrUpdate(ARTrackedImage trackedImage)
    {
        string markerName = trackedImage.referenceImage.name;

        GameObject prefabToSpawn = null;

        if (markerName == "marker1")
            prefabToSpawn = carPrefab1;
        else if (markerName == "marker2")
            prefabToSpawn = carPrefab2;

        if (prefabToSpawn == null)
            return;

        if (!spawnedVehicles.ContainsKey(markerName))
        {
            GameObject vehicle = Instantiate(prefabToSpawn);
            spawnedVehicles.Add(markerName, vehicle);
        }

        GameObject spawnedVehicle = spawnedVehicles[markerName];

        spawnedVehicle.transform.position = trackedImage.transform.position;
        spawnedVehicle.transform.rotation = trackedImage.transform.rotation;
        spawnedVehicle.transform.localScale = Vector3.one * 0.2f;
    }
}