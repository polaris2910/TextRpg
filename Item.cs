namespace TextRpg_Comment
{
    // 아이템 데이터 클래스
    public class Item
    {
        // 아이템 종류 판별 속성
        public bool IsEquippable => Type == ItemType.Weapon || Type == ItemType.Armor;

        public string Name { get; }
        public ItemType Type { get; }
        public int Value { get; }    // 공격력/방어력/회복량
        public string Desc { get; }  // 설명
        public int Price { get; }    // 가격

        // 아이템 타입별 표시 텍스트
        public string DisplayTypeText =>
            Type == ItemType.Weapon ? "공격력" :
            Type == ItemType.Armor ? "방어력" :
            Type == ItemType.potion ? "회복량" : "기타";

        // 생성자
        public Item(string name, ItemType type, int value, string desc, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }

        // 정보 텍스트 반환
        public string ItemInfoText()
        {
            return $"{Name}  |  {DisplayTypeText} +{Value}  |  {Desc}";
        }
    }
}