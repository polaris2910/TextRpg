namespace TextRpg_Comment
{
    // 아이템(장비/포션 등) 데이터 관리 클래스
    public class Item
    {
        // 무기 또는 방어구일 때만 장착 가능
        public bool IsEquippable => Type == ItemType.Weapon || Type == ItemType.Armor;

        // 아이템 기본 속성 (읽기 전용)
        public string Name { get; }       // 이름
        public ItemType Type { get; }     // 종류(enum)
        public int Value { get; }         // 효과 수치(공격력/방어력/회복량 등)
        public string Desc { get; }       // 한 줄 설명
        public int Price { get; }         // 가격

        // 종류에 따라 출력용 텍스트 (공격력/방어력/회복량 등)
        public string DisplayTypeText =>
            Type == ItemType.Weapon ? "공격력" :
            Type == ItemType.Armor ? "방어력" :
            Type == ItemType.potion ? "회복량" : "기타";

        // 생성자: 모든 속성 할당
        public Item(string name, ItemType type, int value, string desc, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }

        // 출력용 정보(인벤토리/상점 등)
        public string ItemInfoText()
        {
            return $"{Name}  |  {DisplayTypeText} +{Value}  |  {Desc}";
        }
    }
}