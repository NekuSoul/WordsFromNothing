using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class WordList : MonoBehaviour
    {
        public TextAsset textAsset;
        private string[] _words;

        private void Awake()
        {
            _words = textAsset.text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
        }

        public string GetRandomMatchingWord(string pattern)
        {
            var regex = new Regex($"^{pattern}$", RegexOptions.Compiled);
            var matches = _words.Where(word => regex.IsMatch(word)).ToList();
            return matches.Count == 0 ? null : matches[Random.Range(0, matches.Count)];
        }
    }
}