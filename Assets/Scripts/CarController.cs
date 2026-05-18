using UnityEngine;

public class CarController : MonoBehaviour
{
    public Renderer[] carBodies;
    public Color[] carColors;

    public Renderer[] wheels;
    public Color[] wheelColors;

    public AudioSource engineAudio;
    public Animator carAnimator;
    public Animator hoodAnimator;

    public BackgroundMusicManager musicManager;

    private int colorIndex = 0;
    private int wheelIndex = 0;
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

    public void ChangeWheelColor()
    {
        if (wheels.Length == 0 || wheelColors.Length == 0) return;

        wheelIndex = (wheelIndex + 1) % wheelColors.Length;

        foreach (Renderer wheel in wheels)
        {
            wheel.material.color = wheelColors[wheelIndex];
        }
    }

    public void ToggleEngine()
    {
        if (engineAudio == null) return;

        if (engineAudio.isPlaying)
        {
            engineAudio.Stop();

            if (musicManager != null)
                musicManager.PlayMusic();
        }
        else
        {
            engineAudio.Play();

            if (musicManager != null)
                musicManager.StopMusic();
        }
    }

    public void ToggleTurntable()
    {
        turntableOn = !turntableOn;
    }

    public void OpenHood()
    {
        if (hoodAnimator != null)
            hoodAnimator.SetTrigger("OpenHood");
    }

    public void OpenDoors()
    {
        if (carAnimator != null)
            carAnimator.SetTrigger("OpenDoors");
    }

    public void OpenLeftDoor()
    {
        if (carAnimator != null)
            carAnimator.SetTrigger("OpenLeftDoor");
    }

    public void OpenRightDoor()
    {
        if (carAnimator != null)
            carAnimator.SetTrigger("OpenRightDoor");
    }

    public void ResetCar()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        turntableOn = false;

        if (engineAudio != null)
            engineAudio.Stop();

        if (musicManager != null)
            musicManager.PlayMusic();

        colorIndex = 0;
        if (carBodies.Length > 0 && carColors.Length > 0)
        {
            foreach (Renderer body in carBodies)
                body.material.color = carColors[0];
        }

        wheelIndex = 0;
        if (wheels.Length > 0 && wheelColors.Length > 0)
        {
            foreach (Renderer wheel in wheels)
                wheel.material.color = wheelColors[0];
        }

        if (carAnimator != null)
        {
            carAnimator.ResetTrigger("OpenDoors");
            carAnimator.ResetTrigger("OpenLeftDoor");
            carAnimator.ResetTrigger("OpenRightDoor");
            carAnimator.Play("Idle", 0, 0f);
        }

        if (hoodAnimator != null)
        {
            hoodAnimator.ResetTrigger("OpenHood");
            hoodAnimator.Play("Idle", 0, 0f);
        }
    }
}