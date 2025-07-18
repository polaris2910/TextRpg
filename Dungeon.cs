namespace TextRpg_Comment
{
    // 던전 정보 클래스
    public class Dungeon
    {
        public string Name { get; }            // 던전 이름
        public int RecommendDef { get; }       // 추천 방어력
        public int BaseReward { get; }         // 기본 골드 보상

        public Dungeon(string name, int recommendDef, int baseReward)
        {
            Name = name;
            RecommendDef = recommendDef;
            BaseReward = baseReward;
        }
    }

    // 플레이어 회피 관련 기능 (정적 클래스)
    public static class Dodge
    {
        private static Random random = new Random();

        // 10% 확률로 true(회피), false(피격) 판정
        public static bool DodgeAtk()
        {
            return random.Next(0, 10) < 1;
        }

        // 회피 성공 시 출력
        public static void ShowDodge()
        {
            Console.WriteLine("감나빛!");
        }
    }
}