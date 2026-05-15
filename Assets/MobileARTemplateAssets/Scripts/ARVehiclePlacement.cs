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

    [Header("Rotation Settings")]
    public float rotationSpeed = 0.2f;

    private GameObject spawnedVehicle;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 previousTouchPosition;
    private bool isRotating = false;

    void Update()
    {
        // Keyboard reset test in Unity Editor
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetVehicle();
        }

        // Rotate vehicle with one finger drag
        if (spawnedVehicle != null && Input.touchCount == 1)
        {
            Touch rotateTouch = Input.GetTouch(0);

            if (rotateTouch.phase == TouchPhase.Began)
            {
                previousTouchPosition = rotateTouch.position;
                isRotating = true;
            }
            else if (rotateTouch.phase == TouchPhase.Moved && isRotating)
            {
                float deltaX = rotateTouch.position.x - previousTouchPosition.x;

                spawnedVehicle.transform.Rotate(
                    0,
                    -deltaX * rotationSpeed,
                    0,
                    Space.World
                );

                previousTouchPosition = rotateTouch.position;
            }
            else if (rotateTouch.phase == TouchPhase.Ended || rotateTouch.phase == TouchPhase.Canceled)
            {
                isRotating = false;
            }
        }

        // Check if user touched the screen
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        // Only place on first touch
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

            spawnedVehicle = Instantiate(
                vehiclePrefab,
                hitPose.position,
                rotation
            );
        }
    }

    private bool IsPointerOverUI(Touch touch)
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    public void ResetVehicle()
    {
        if (spawnedVehicle != null)
        {
            Destroy(spawnedVehicle);
            spawnedVehicle = null;
        }
    }
}