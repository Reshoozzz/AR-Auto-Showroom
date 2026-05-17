using UnityEngine;
using UnityEngine.EventSystems;

public class CarTouchControls : MonoBehaviour
{
    public float rotateSpeed = 0.2f;
    public float scaleSpeed = 0.005f;
    public float minScale = 0.5f;
    public float maxScale = 2f;

    

    private float previousDistance;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.Rotate(0, -touch.deltaPosition.x * rotateSpeed, 0);
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                previousDistance = currentDistance;
            }

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float difference = currentDistance - previousDistance;

                float newScale = transform.localScale.x + difference * scaleSpeed;
                newScale = Mathf.Clamp(newScale, minScale, maxScale);

                transform.localScale = Vector3.one * newScale;

                previousDistance = currentDistance;
            }
        }
    }
}