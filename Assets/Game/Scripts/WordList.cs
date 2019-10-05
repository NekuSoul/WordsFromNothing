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

        public string GetRandomMatchingWord(string pattern, params Tuple<int, string>[] requirements)
        {
            var regex = new Regex($"^{pattern}$", RegexOptions.Compiled);
            var matches = _words.Where(word => regex.IsMatch(word)).ToList();
            matches = matches.Where(match => requirements.All(r => CheckRequirement(match, r))).ToList();
            return matches.Count == 0 ? null : matches[Random.Range(0, matches.Count)];
        }

        private bool CheckRequirement(string word, Tuple<int, string> requirement)
        {
            var target = requirement.Item2
                .Remove(requirement.Item1, 1)
                .Insert(requirement.Item1, word[requirement.Item1].ToString());
            return _words.Contains(target);
        }
    }
}