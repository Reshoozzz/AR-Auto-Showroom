using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARVehiclePlacement : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("Vehicle Prefab")]
    public GameObject vehiclePrefab;

    [Header("Placement Settings")]
    public bool allowMultiplePlacement = false;
    public bool hidePlanesAfterPlacement = true;

    [Header("Rotation Settings")]
    public float rotationSpeed = 0.2f;
    public float rotationSmoothness = 10f;

    [Header("Scale Settings")]
    public float scaleSpeed = 0.001f;
    public float minimumScale = 0.3f;
    public float maximumScale = 2f;
    public float scaleSmoothness = 10f;

    private GameObject spawnedVehicle;
    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector2 previousTouchPosition;
    private bool isRotating = false;

    private float targetYRotation;
    private Vector3 targetScale;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetVehicle();
        }

        if (spawnedVehicle != null)
        {
            SmoothTransformUpdates();
            HandleOneFingerRotation();
            HandlePinchScaling();
        }

        HandlePlacementOrRepositioning();
    }

    private void HandlePlacementOrRepositioning()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if (IsPointerOverUI(touch))
            return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            Quaternion rotation = Quaternion.Euler(
                0,
                Camera.main.transform.eulerAngles.y,
                0
            );

            if (spawnedVehicle == null)
            {
                spawnedVehicle = Instantiate(vehiclePrefab, hitPose.position, rotation);

                targetYRotation = spawnedVehicle.transform.eulerAngles.y;
                targetScale = spawnedVehicle.transform.localScale;

                if (hidePlanesAfterPlacement)
                {
                    SetPlaneVisibility(false);
                }
            }
            else
            {
                spawnedVehicle.transform.SetPositionAndRotation(hitPose.position, rotation);
                targetYRotation = spawnedVehicle.transform.eulerAngles.y;
            }
        }
    }

    private void HandleOneFingerRotation()
    {
        if (Input.touchCount != 1)
            return;

        Touch rotateTouch = Input.GetTouch(0);

        if (IsPointerOverUI(rotateTouch))
            return;

        if (rotateTouch.phase == TouchPhase.Began)
        {
            previousTouchPosition = rotateTouch.position;
            isRotating = true;
        }
        else if (rotateTouch.phase == TouchPhase.Moved && isRotating)
        {
            float deltaX = rotateTouch.position.x - previousTouchPosition.x;
            targetYRotation -= deltaX * rotationSpeed;
            previousTouchPosition = rotateTouch.position;
        }
        else if (rotateTouch.phase == TouchPhase.Ended || rotateTouch.phase == TouchPhase.Canceled)
        {
            isRotating = false;
        }
    }

    private void HandlePinchScaling()
    {
        if (Input.touchCount != 2)
            return;

        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        if (IsPointerOverUI(touch1) || IsPointerOverUI(touch2))
            return;

        Vector2 touch1PreviousPosition = touch1.position - touch1.deltaPosition;
        Vector2 touch2PreviousPosition = touch2.position - touch2.deltaPosition;

        float previousDistance = Vector2.Distance(touch1PreviousPosition, touch2PreviousPosition);
        float currentDistance = Vector2.Distance(touch1.position, touch2.position);

        float distanceDifference = currentDistance - previousDistance;

        Vector3 newScale = targetScale + Vector3.one * distanceDifference * scaleSpeed;

        float clampedScale = Mathf.Clamp(newScale.x, minimumScale, maximumScale);

        targetScale = new Vector3(clampedScale, clampedScale, clampedScale);
    }

    private void SmoothTransformUpdates()
    {
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0);

        spawnedVehicle.transform.rotation = Quaternion.Lerp(
            spawnedVehicle.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmoothness
        );

        spawnedVehicle.transform.localScale = Vector3.Lerp(
            spawnedVehicle.transform.localScale,
            targetScale,
            Time.deltaTime * scaleSmoothness
        );
    }

    private void SetPlaneVisibility(bool isVisible)
    {
        if (planeManager == null)
            return;

        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(isVisible);
        }

        planeManager.enabled = isVisible;
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
            SetPlaneVisibility(true);
        }
    }
}