using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    public Text closeButtonText;
    public AudioManager audioManager;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        audioManager.PlaySelectSound();
        closeButtonText.text = "Close";
        gameObject.SetActive(false);
    }
}
