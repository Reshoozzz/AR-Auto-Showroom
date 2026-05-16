using UnityEngine;

public class CarController : MonoBehaviour
{
    public Renderer carBody;
    public Renderer[] wheels;
    public AudioSource engineAudio;

    public Color[] carColors;
    public Material[] wheelMaterials;

    private int colorIndex = 0;
    private int wheelIndex = 0;
    private bool engineOn = false;
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
        if (carBody == null || carColors.Length == 0) return;

        colorIndex = (colorIndex + 1) % carColors.Length;
        carBody.material.color = carColors[colorIndex];
    }

    public void ChangeWheelColor()
    {
        if (wheels.Length == 0 || wheelMaterials.Length == 0) return;

        wheelIndex = (wheelIndex + 1) % wheelMaterials.Length;

        foreach (Renderer wheel in wheels)
        {
            wheel.material = wheelMaterials[wheelIndex];
        }
    }

    public void ToggleEngine()
    {
        if (engineAudio == null) return;

        engineOn = !engineOn;

        if (engineOn)
            engineAudio.Play();
        else
            engineAudio.Stop();
    }

    public void ToggleTurntable()
    {
        turntableOn = !turntableOn;
    }

    public void ResetCar()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        turntableOn = false;

        if (engineAudio != null)
            engineAudio.Stop();

        engineOn = false;
    }
}