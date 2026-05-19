using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageManager : MonoBehaviour
{
    public void LoadMarkerless()
    {
        SceneManager.LoadScene("Resha");
    }

    public void LoadMarkerBased()
    {
        SceneManager.LoadScene("joe");
    }
}