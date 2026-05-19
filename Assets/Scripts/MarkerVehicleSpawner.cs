using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MarkerVehicleSpawner : MonoBehaviour
{
    public GameObject carPrefab1;  // Drag the Car_Sportage from HIERARCHY (not project)
    public GameObject carPrefab2;  // Drag the Honda Civic from HIERARCHY (not project)

    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        // Hide cars at start
        if (carPrefab1 != null) carPrefab1.SetActive(false);
        if (carPrefab2 != null) carPrefab2.SetActive(false);
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
            ShowCar(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            ShowCar(trackedImage);
        }
    }

    void ShowCar(ARTrackedImage trackedImage)
    {
        string markerName = trackedImage.referenceImage.name;

        GameObject carToShow = null;
        if (markerName == "MER") carToShow = carPrefab1;
        else if (markerName == "BMW") carToShow = carPrefab2;

        if (carToShow == null) return;

        // Position the car at the marker, flat on ground
        carToShow.transform.position = trackedImage.transform.position;
        carToShow.transform.rotation = Quaternion.Euler(0, trackedImage.transform.eulerAngles.y, 0);
        carToShow.SetActive(true);
    }
}