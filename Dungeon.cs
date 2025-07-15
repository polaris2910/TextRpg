namespace TextRpg_Comment
{
    public class Dungeon
    {
        public string Name { get; }
        public int RecommendDef { get; }
        public int BaseReward { get; }

        public Dungeon(string name, int recommendDef, int baseReward)
        {
            Name = name;
            RecommendDef = recommendDef;
            BaseReward = baseReward;
        }
    }
}
