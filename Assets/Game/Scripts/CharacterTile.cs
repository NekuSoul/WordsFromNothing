using System;
using UnityEngine;

namespace Game.Scripts
{
    public class CharacterTile : MonoBehaviour
    {
        public TextMesh textMesh;
        public Animator animator;
        public Vector2 Position { get; set; }
        
        public char Character
        {
            get => textMesh.text.ToLower()[0];
            set => textMesh.text = value.ToString().ToUpper();
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayChangeCharacterAnimation()
        {
            animator.Play("CharacterTileChangeAnimation");
        }
    }
}