namespace Game.Code
{
    public struct WordRequirement
    {
        public int Position;
        public string Word;

        public WordRequirement(int position, string word)
        {
            Position = position;
            Word = word;
        }
    }
}