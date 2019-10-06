using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Game.Code;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class WordList : MonoBehaviour
    {
        public TextAsset textAsset;
        private string[] _words;
        private readonly Dictionary<WordRequirement, List<Tuple<char, bool>>> _requirementCache = new Dictionary<WordRequirement, List<Tuple<char, bool>>>();
        private List<string> _matches = new List<string>();

        private void Awake()
        {
            _words = textAsset.text.Split(new[] {Environment.NewLine}, StringSplitOptions.None).Where(w => w.Length > 2).ToArray();
        }

        public string GetRandomMatchingWord(string pattern, string originalWord, params WordRequirement[] requirements)
        {
            _requirementCache.Clear();
            foreach (var requirement in requirements)
            {
                _requirementCache.Add(requirement, new List<Tuple<char, bool>>());
            }

            var regex = new Regex($"^{pattern}$", RegexOptions.Compiled);
            _matches = _words.Where(word => regex.IsMatch(word) && word != originalWord).ToList();
            _matches.Shuffle();
            return _matches.FirstOrDefault(match => requirements.All(r => CheckRequirement(match, r)));
        }

        private bool CheckRequirement(string word, WordRequirement requirement)
        {
            var testedCharacter = word[requirement.Position];
            var cache = _requirementCache[requirement].FirstOrDefault(t => t.Item1 == testedCharacter);
            if (cache != null)
            {
                return cache.Item2;
            }

            if (_words.Contains(requirement.Word.Replace('.', testedCharacter)))
            {
                _requirementCache[requirement].Add(new Tuple<char, bool>(testedCharacter, true));
                return true;
            }
            else
            {
                _requirementCache[requirement].Add(new Tuple<char, bool>(testedCharacter, false));
                return false;
            }
        }
    }
}