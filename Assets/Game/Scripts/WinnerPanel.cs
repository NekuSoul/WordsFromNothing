﻿using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class WinnerPanel : MonoBehaviour
    {
        public Text text;
        public AudioManager audioManager;
        
        private string originalText = string.Empty;
        
        public void Show(int steps)
        {
            originalText = text.text;
            text.text = string.Format(originalText, steps);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            audioManager.PlaySelectSound();
            gameObject.SetActive(false);
        }
    }
}
