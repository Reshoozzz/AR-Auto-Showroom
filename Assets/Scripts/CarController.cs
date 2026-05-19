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

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        originalScale = transform.localScale;
    }

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
            if (body != null)
                body.material.color = carColors[colorIndex];
        }
    }

    public void ChangeWheelColor()
    {
        if (wheels.Length == 0 || wheelColors.Length == 0) return;

        wheelIndex = (wheelIndex + 1) % wheelColors.Length;

        foreach (Renderer wheel in wheels)
        {
            if (wheel != null)
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
        TrySetTrigger(hoodAnimator, "OpenHood");
        TrySetTrigger(hoodAnimator, "OpenHooc");
        TrySetTrigger(carAnimator, "OpenHood");
        TrySetTrigger(carAnimator, "OpenHooc");
    }

    public void OpenDoors()
    {
        TrySetTrigger(carAnimator, "OpenDoors");
        TrySetTrigger(carAnimator, "OpenDoor");
    }

    public void OpenLeftDoor()
    {
        TrySetTrigger(carAnimator, "OpenLeftDoor");
        TrySetTrigger(carAnimator, "OpenLeft");
        TrySetTrigger(carAnimator, "OpenDoor");
    }

    public void OpenRightDoor()
    {
        TrySetTrigger(carAnimator, "OpenRightDoor");
        TrySetTrigger(carAnimator, "OpenRight");
        TrySetTrigger(carAnimator, "OpenDoor");
    }

    public void ResetCar()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        transform.localScale = originalScale;

        turntableOn = false;

        if (engineAudio != null)
        {
            engineAudio.Stop();
            engineAudio.time = 0f;
        }

        if (musicManager != null)
            musicManager.PlayMusic();

        colorIndex = 0;
        if (carBodies.Length > 0 && carColors.Length > 0)
        {
            foreach (Renderer body in carBodies)
            {
                if (body != null)
                    body.material.color = carColors[0];
            }
        }

        wheelIndex = 0;
        if (wheels.Length > 0 && wheelColors.Length > 0)
        {
            foreach (Renderer wheel in wheels)
            {
                if (wheel != null)
                    wheel.material.color = wheelColors[0];
            }
        }

        ResetAnimator(carAnimator);
        ResetAnimator(hoodAnimator);
    }

    private void ResetAnimator(Animator animator)
    {
        if (animator == null) return;

        TryResetTrigger(animator, "OpenDoors");
        TryResetTrigger(animator, "OpenDoor");
        TryResetTrigger(animator, "OpenLeftDoor");
        TryResetTrigger(animator, "OpenRightDoor");
        TryResetTrigger(animator, "OpenLeft");
        TryResetTrigger(animator, "OpenRight");
        TryResetTrigger(animator, "OpenHood");
        TryResetTrigger(animator, "OpenHooc");

        animator.Rebind();
        animator.Update(0f);

        if (HasState(animator, "Idle"))
            animator.Play("Idle", 0, 0f);
    }

    private void TrySetTrigger(Animator animator, string triggerName)
    {
        if (animator == null) return;

        if (HasParameter(animator, triggerName, AnimatorControllerParameterType.Trigger))
            animator.SetTrigger(triggerName);
    }

    private void TryResetTrigger(Animator animator, string triggerName)
    {
        if (animator == null) return;

        if (HasParameter(animator, triggerName, AnimatorControllerParameterType.Trigger))
            animator.ResetTrigger(triggerName);
    }

    private bool HasParameter(Animator animator, string parameterName, AnimatorControllerParameterType type)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == parameterName && parameter.type == type)
                return true;
        }

        return false;
    }

    private bool HasState(Animator animator, string stateName)
    {
        return animator.HasState(0, Animator.StringToHash(stateName));
    }
}