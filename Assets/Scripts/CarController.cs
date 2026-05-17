using UnityEngine;

public class CarController : MonoBehaviour
{
    public Renderer[] carBodies;
    public Color[] carColors;

    public AudioSource engineAudio;
    public Animator carAnimator;

    private int colorIndex = 0;
    private bool turntableOn = false;

    public float rotationSpeed = 20f;

    void Update()
    {
        if (turntableOn)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void ChangeCarColor()
    {
        if (carBodies.Length == 0 || carColors.Length == 0) return;

        colorIndex = (colorIndex + 1) % carColors.Length;

        foreach (Renderer body in carBodies)
        {
            body.material.color = carColors[colorIndex];
        }
    }

    public void ToggleEngine()
    {
        if (engineAudio == null) return;

        if (engineAudio.isPlaying)
            engineAudio.Stop();
        else
            engineAudio.Play();
    }

    public void ToggleTurntable()
    {
        turntableOn = !turntableOn;
    }

    public void OpenDoor()
    {
        if (carAnimator != null)
        {
            carAnimator.SetTrigger("OpenDoor");
        }
    }

    public void ResetCar()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        turntableOn = false;

        if (engineAudio != null)
            engineAudio.Stop();
    }
}