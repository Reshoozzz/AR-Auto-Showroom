using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARVehiclePlacement : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;

    [Header("Vehicle Prefab")]
    public GameObject vehiclePrefab;

    [Header("Placement Settings")]
    public bool allowMultiplePlacement = false;

    private GameObject spawnedVehicle;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        // Keyboard reset test in Unity Editor
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetVehicle();
        }

        // Check if user touched the screen
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // Only detect first touch
        if (touch.phase != TouchPhase.Began)
            return;

        // Ignore touches on UI
        if (IsPointerOverUI(touch))
            return;

        // Prevent placing multiple vehicles
        if (spawnedVehicle != null && !allowMultiplePlacement)
            return;

        // Raycast against detected AR planes
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Make vehicle face the user
            Quaternion rotation = Quaternion.Euler(
                0,
                Camera.main.transform.eulerAngles.y,
                0
            );

            // Spawn vehicle
            spawnedVehicle = Instantiate(
                vehiclePrefab,
                hitPose.position,
                rotation
            );
        }
    }

    // Check if touch is over UI
    private bool IsPointerOverUI(Touch touch)
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    // Reset / remove vehicle
    public void ResetVehicle()
    {
        if (spawnedVehicle != null)
        {
            Destroy(spawnedVehicle);
            spawnedVehicle = null;
        }
    }
}