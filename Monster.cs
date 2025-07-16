namespace TextRpg_Comment
{
    // 몬스터 정보 클래스
    public class Monster
    {
        public string Name { get; }    // 몬스터 이름
        public int Level { get; }      // 레벨
        public int Hp { get; set; }    // 현재 체력
        public int Atk { get; }        // 공격력

        // 생성자
        public Monster(string name, int level, int hp, int atk)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
        }

        // 정보 출력용 텍스트
        public string Info()
        {
            return $"Lv.{Level} {Name}  HP {Hp}";
        }
    }
}