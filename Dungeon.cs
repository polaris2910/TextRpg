namespace TextRpg_Comment
{
    // 던전 정보 클래스
    public class Dungeon
    {
        public string Name { get; }        // 던전 이름
        public int RecommendDef { get; }   // 추천 방어력
        public int BaseReward { get; }     // 기본 보상 골드

        // 생성자
        public Dungeon(string name, int recommendDef, int baseReward)
        {
            Name = name;
            RecommendDef = recommendDef;
            BaseReward = baseReward;
        }
    }
}