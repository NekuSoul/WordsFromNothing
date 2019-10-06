using Game.Scripts;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    public GameManager gameManager;
    public HelpPanel helpPanel;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ShowHelp()
    {
        helpPanel.Show();
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (gameObject.activeSelf)
            Hide();
        else
            Show();
    }

    public void ResetCamera()
    {
        gameManager.ResetCamera();
        Hide();
    }

    public void ResetGame()
    {
        gameManager.ResetGame();
        Hide();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}