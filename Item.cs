namespace TextRpg_Comment
{
    // [아이템 데이터 클래스]
    // 이 클래스는 아이템(장비, 포션 등)의 정보를 저장하고,
    // 인벤토리, 상점, 장착 등 여러 곳에서 객체로 사용됨
    public class Item
    {
        // [장비 장착 가능 여부 속성]
        // true면 무기/방어구, false면 포션(소모품) 등.
        // 인벤토리/장착관리 메뉴 등에서 장착할 수 있나 분기 처리할 때 활용
        public bool IsEquippable => Type == ItemType.Weapon || Type == ItemType.Armor;

        // [아이템 고유 정보]
        public string Name { get; }           // 아이템 이름(유일/구별)
        public ItemType Type { get; }         // 무기/방어구/포션 등 타입
        public int Value { get; }             // 타입에 따라 공격력/방어력/회복량 등 실제 효과값
        public string Desc { get; }           // 한줄 설명(출력용)
        public int Price { get; }             // 상점 판매가(골드 기준)

        // [아이템 타입별 출력용 텍스트]
        // 공격력/방어력/회복량 등 UI, 툴팁 등에 표시됨
        public string DisplayTypeText =>
            Type == ItemType.Weapon ? "공격력" :
            Type == ItemType.Armor ? "방어력" :
            Type == ItemType.potion ? "회복량" : "기타";

        // [생성자]
        // 아이템 데이터 세팅(Indexer 미사용, 이름/타입/값/설명/가격을 모두 지정)
        // SetData에서 아이템 배열 생성, 상점/드랍 등에서 인스턴스로 사용됨
        public Item(string name, ItemType type, int value, string desc, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }

        // [아이템 정보 문자열 반환]
        // 인벤토리, 상점, 드랍, 장착관리 UI 등에서 목록 출력을 위해 사용됨
        public string ItemInfoText()
        {
            // 예) "낡은 검  |  공격력 +2  |  낡은 검입니다."
            return $"{Name}  |  {DisplayTypeText} +{Value}  |  {Desc}";
        }
    }
}