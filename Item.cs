namespace TextRpg_Comment
{
    public class Item
    {
        public bool IsEquippable => Type == ItemType.Weapon || Type == ItemType.Armor;
        public string Name { get; }
        public ItemType Type { get; }
        public int Value { get; }
        public string Desc { get; }
        public int Price { get; }

        public string DisplayTypeText =>
            Type == ItemType.Weapon ? "공격력" :
            Type == ItemType.Armor ? "방어력" :
            Type == ItemType.potion ? "회복량" :
            "기본값";
            

        public Item(string name, ItemType type, int value, string desc, int price)
        {
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
        }

        public string ItemInfoText()
        {
            return $"{Name}  |  {DisplayTypeText} +{Value}  |  {Desc}";
        }
    }
}
