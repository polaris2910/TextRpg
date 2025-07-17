namespace TextRpg_Comment
{
    // 몬스터 정보 클래스
    public class Monster
    {
        public string Name { get; }    // 몬스터 이름
        public int Level { get; }      // 레벨
        public int Hp { get; set; }    // 현재 체력
        public int Atk { get; }        // 공격력

        public int RewardGold {  get; set; }

        // 생성자
        public Monster(string name, int level, int hp, int atk, int rewardGold)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
            RewardGold = rewardGold;
        }

        // 정보 출력용 텍스트
        public string Info()
        {
            return $"Lv.{Level} {Name}  HP {Hp}";
        }

        public void TakeDamage(int damage) // 'internal'을 'public'으로 변경하고, 매개변수 이름을 'damage'로 변경 (선택 사항)
        {
            Hp -= damage; // 몬스터의 현재 HP에서 받은 데미지를 뺍니다.
            if (Hp < 0)
            {
                Hp = 0; // HP가 0 미만으로 내려가지 않도록 0으로 설정합니다.
            }
        }

    }
}