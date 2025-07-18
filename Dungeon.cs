namespace TextRpg_Comment
{
    // [던전 정보 클래스]
    // 각 던전의 이름, 추천 방어력, 클리어 보상(골드)을 저장하는 데이터 객체
    // 게임 실행 시 SetData() 등에서 여러 개 생성되어 배열/리스트로 관리함
    // 던전 입장/출력/보상 계산 등에서 사용
    public class Dungeon
    {
        public string Name { get; }        // [던전 이름] (ex: "쉬운 던전", "어려운 던전")
        public int RecommendDef { get; }   // [추천 방어력] - UI 및 도전 난이도 안내 용
        public int BaseReward { get; }     // [기본 골드 보상] - 클리어 시 지급

        // [생성자]
        // Dungeon 객체 생성 시 각 정보를 세팅함
        // Main → SetData → new Dungeon(...)로 세팅되어 배열/리스트로 저장됨
        public Dungeon(string name, int recommendDef, int baseReward)
        {
            Name = name;
            RecommendDef = recommendDef;
            BaseReward = baseReward;
        }
    }

    // [회피 기능 정적 클래스]
    // 플레이어가 몬스터의 공격을 확률적으로 회피할 수 있는 기능 구현
    public static class Dodge
    {
        // [랜덤 객체]
        // 내부에서 회피 확률 판정에 사용
        private static Random random = new Random();

        // [공격 회피 판정 함수]
        // 공격을 받을 때 호출하여, 일정 확률(10%)로 회피 성공/실패를 bool로 반환
        // ex) EnemyAttackPhase 등에서 몬스터 공격 시 이 함수의 결과에 따라 공격 무효화 분기
        public static bool DodgeAtk()
        {
            // [실행 흐름]
            // 호출되면 0~9 중 랜덤값 → 0이 나올 때 true(회피 성공, 10%)
            // (0만 성공, 1~9는 실패)
            return random.Next(0, 10) < 1;
        }

        // [회피 연출 출력]
        // 회피 성공 시 플레이어에게 메시지를 출력
        // DodgeAtk()이 true를 반환했을 때, 실제로 회피 연출/문구가 필요할 때 호출
        public static void ShowDodge()
        {
            // 실제 구현에서는 더 다이나믹하게 변경 가능
            Console.WriteLine("감나빛!"); // "감나빛!"은 유저 취향에 따라 연출 가능
        }
    }
}