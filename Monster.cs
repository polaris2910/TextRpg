namespace TextRpg_Comment
{
    // 몬스터 정보(이름/레벨/체력/공격력/보상) 관리
    public class Monster
    {
        public string Name { get; }           // 몬스터 이름
        public int Level { get; }             // 레벨
        public int Hp { get; set; }           // 현재 체력
        public int Atk { get; }               // 공격력
        public int RewardGold { get; set; }   // 처치 보상 골드

        // 생성자: 몬스터 속성 할당
        public Monster(string name, int level, int hp, int atk, int rewardGold)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
            RewardGold = rewardGold;
        }

        // 몬스터 정보 출력 (상태창/전투 UI 등)
        public string Info()
        {
            return $"Lv.{Level} {Name}  HP {Hp}";
        }

        // 데미지 입고 체력 감소(0 미만이면 0으로)
        public void TakeDamage(int damage)
        {
            Hp -= damage;
            if (Hp < 0)
            {
                Hp = 0;
            }
        }
    }
}