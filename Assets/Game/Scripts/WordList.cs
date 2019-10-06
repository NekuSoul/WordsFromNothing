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
        private readonly Dictionary<WordRequirement, List<char>> _requirementCache = new Dictionary<WordRequirement, List<char>>();
        private List<string> _matches = new List<string>();

        private void Awake()
        {
            _words = textAsset.text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
        }

        public string GetRandomMatchingWord(string pattern, string originalWord, params WordRequirement[] requirements)
        {
            _requirementCache.Clear();
            foreach (var requirement in requirements)
            {
                _requirementCache.Add(requirement, new List<char>());
            }

            var regex = new Regex($"^{pattern}$", RegexOptions.Compiled);
            _matches = _words.Where(word => regex.IsMatch(word) && word != originalWord).ToList();
            _matches.Shuffle();
            return _matches.FirstOrDefault(match => requirements.All(r => CheckRequirement(match, r)));
        }

        private bool CheckRequirement(string word, WordRequirement requirement)
        {
            var testedCharacter = word[requirement.Position];
            if (_requirementCache[requirement].Contains(testedCharacter))
                return false;
            _requirementCache[requirement].Add(testedCharacter);
            return _words.Contains(requirement.Word.Replace('.', testedCharacter));
        }
    }
}