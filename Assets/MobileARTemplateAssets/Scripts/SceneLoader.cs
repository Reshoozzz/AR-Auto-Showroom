using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Abdo");
    }

    public void LoadMarkerless()
    {
        SceneManager.LoadScene("Resha");
    }

    public void LoadMarkerBased()
    {
        SceneManager.LoadScene("joe");
    }
}