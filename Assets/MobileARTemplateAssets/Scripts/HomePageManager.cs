using UnityEngine;

public class HomePageManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject mainGameUI;

    public void StartApp()
    {
        homePanel.SetActive(false);
        mainGameUI.SetActive(true);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}