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

    
    // 플레이어가 몬스터의 공격을 확률적으로 회피할 수 있는 기능 구현
    public static class Dodge
    {
       
        // 내부에서 회피 확률 판정에 사용
        private static Random random = new Random();

        
        // 공격을 받을 때 호출하여, 일정 확률(10%)로 회피 성공/실패를 bool로 반환
       //몬스터의 공격과 플레이어의 공격에 동시 사용
        public static bool DodgeAtk()
        {
            
            // 호출되면 0~9 중 랜덤값 → 0이 나올 때 true(회피 성공, 10%)
            // (0만 성공, 1~9는 실패)
            return random.Next(0, 10) < 1;
        }

       
        // 회피 성공 시 플레이어에게 메시지를 출력
        // DodgeAtk()이 true를 반환했을 때, 실제로 회피 연출/문구가 필요할 때 호출
        public static void ShowDodge()
        {
           
            Console.WriteLine("감나빛!");
        }
    }
}