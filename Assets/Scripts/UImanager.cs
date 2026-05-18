using UnityEngine;

public class CarUIPanelManager : MonoBehaviour
{
    public GameObject sportagePanel;
    public GameObject hondaPanel;

    public void ShowSportageUI()
    {
        sportagePanel.SetActive(true);
        hondaPanel.SetActive(false);
    }

    public void ShowHondaUI()
    {
        sportagePanel.SetActive(false);
        hondaPanel.SetActive(true);
    }
}