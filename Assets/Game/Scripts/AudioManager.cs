using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip errorSound;
    public AudioClip selectSound;
    public AudioClip successSound;
    public AudioClip winSound;

    public void PlayErrorSound()
    {
        audioSource.PlayOneShot(errorSound);
    }
    
    public void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }
    
    public void PlaySuccessSound()
    {
        audioSource.PlayOneShot(successSound);
    }
    
    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
}
