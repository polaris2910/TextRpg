namespace TextRpg_Comment
{
    public class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Hp { get; private set; }
        public int Atk { get; }

        public Monster(string name, int level, int hp, int atk)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
        }

        public string Info()
        {
            return $"Lv.{Level} {Name}  HP {Hp}";
        }
    }

}
