using Game.Scripts;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    public GameManager gameManager;
    public HelpPanel helpPanel;
    public AudioManager audioManager;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ShowHelp()
    {
        audioManager.PlaySelectSound();
        helpPanel.Show();
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        audioManager.PlaySelectSound();
        if (gameObject.activeSelf)
            Hide();
        else
            Show();
    }

    public void ResetCamera()
    {
        audioManager.PlaySelectSound();
        gameManager.ResetCamera();
        Hide();
    }

    public void ResetGame()
    {
        audioManager.PlaySelectSound();
        gameManager.ResetCamera();
        gameManager.ResetGame();
        Hide();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}