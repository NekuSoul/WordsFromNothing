using UnityEngine;

namespace Game.Scripts
{
    public class CharacterTile : MonoBehaviour
    {
        public TextMesh textMesh;
        public Vector2 Position { get; set; }
        
        public char Character
        {
            get => textMesh.text.ToLower()[0];
            set => textMesh.text = value.ToString().ToUpper();
        }
    }
}