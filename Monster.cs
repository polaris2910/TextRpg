namespace TextRpg_Comment
{
    // [몬스터 정보 클래스]
    // 각종 몬스터의 이름, 능력치, 상태(Hp), 보상 등을 관리하는 데이터 객체
    // 던전 진입, 전투, 보상 산출 등 여러 곳에서 Monster 인스턴스가 사용됨
    public class Monster
    {
        // [몬스터 고유 정보]
        public string Name { get; }        // (예: "미니언")
        public int Level { get; }          // 몬스터 레벨(난이도 표기용)
        public int Hp { get; set; }        // 현재 체력 (전투 중 실시간 변동)
        public int Atk { get; }            // 공격력 (턴마다 플레이어에게 입히는 피해량)

        public int RewardGold { get; set; } // 처치 시 획득 가능한 골드

        // [생성자]
        // Monster 객체를 만들 때, 이름/레벨/초기Hp/Atk/보상골드를 지정함
        // EnterDungeonUI → CreateFloorMonsters 함수에서 매번 여러 개 생성!
        public Monster(string name, int level, int hp, int atk, int rewardGold)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
            RewardGold = rewardGold;
        }

        // [몬스터 정보 출력]
        // UI·전투 등에서 몬스터 리스트 출력 시 사용됨
        // ex) "Lv.3 공허충  HP 25"
        public string Info()
        {
            return $"Lv.{Level} {Name}  HP {Hp}";
        }

        // [피해 입기 함수]
        // 플레이어, 마법 등에서 공격 받을 때 호출됨. damage만큼 체력 깎임
        // StartBattle 등에 턴마다 호출, 체력 0 미만이면 0으로 고정
        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
            {
                Hp = 0;  // 0 이하로 내려가지 않도록!
            }
        }
    }
}