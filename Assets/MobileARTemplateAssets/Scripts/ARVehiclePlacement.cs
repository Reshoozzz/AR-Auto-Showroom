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

    [Header("Scale Settings")]
    public float scaleSpeed = 0.001f;
    public float minimumScale = 0.3f;
    public float maximumScale = 2f;

    private GameObject spawnedVehicle;
    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 previousTouchPosition;
    private bool isRotating = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetVehicle();
        }

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

        if (spawnedVehicle != null && Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PreviousPosition = touch1.position - touch1.deltaPosition;
            Vector2 touch2PreviousPosition = touch2.position - touch2.deltaPosition;

            float previousDistance = Vector2.Distance(touch1PreviousPosition, touch2PreviousPosition);
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            float distanceDifference = currentDistance - previousDistance;

            Vector3 currentScale = spawnedVehicle.transform.localScale;
            Vector3 newScale = currentScale + Vector3.one * distanceDifference * scaleSpeed;

            float clampedScale = Mathf.Clamp(newScale.x, minimumScale, maximumScale);

            spawnedVehicle.transform.localScale = new Vector3(
                clampedScale,
                clampedScale,
                clampedScale
            );
        }

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if (IsPointerOverUI(touch))
            return;

        if (spawnedVehicle != null && !allowMultiplePlacement)
            return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

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