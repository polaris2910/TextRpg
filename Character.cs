using System;
using System.Collections.Generic;
using System.Linq;

namespace TextRpg_Comment
{
    public class Character
    {
        // 기본 캐릭터 설정
        public string Name { get; }
        public string Job { get; }

        // 성장 관련 속성
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int ExpToNextLevel { get; private set; }

        // 능력치
        public int Atk { get; private set; }
        public int Def { get; private set; }

        // 장비로 인한 증가량
        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        // 체력 관련
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }

        // 보유 자원
        public int Gold { get; private set; }

        // 인벤토리 및 장비 목록
        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public int InventoryCount => Inventory.Count;

        // 캐릭터 생성자 - 직업에 따라 초기 설정 분기
        public Character(string name, string job)
        {
            Name = name;
            Job = job;
            Level = 1;
            Exp = 0;
            ExpToNextLevel = 100;

            ExtraAtk = 0;
            ExtraDef = 0;

            switch (job)
            {
                case "전사":
                    Atk = 13;
                    Def = 7;
                    MaxHp = 100;
                    Gold = 10000;
                    break;
                case "마법사":
                    Atk = 17;
                    Def = 3;
                    MaxHp = 80;
                    Gold = 10000;
                    break;
                case "궁수":
                    Atk = 16;
                    Def = 4;
                    MaxHp = 80;
                    Gold = 10000;
                    break;
                case "도적":
                    Atk = 14;
                    Def = 6;
                    MaxHp = 90;
                    Gold = 10000;
                    break;
                case "백수":
                    Atk = 9;
                    Def = 9;
                    MaxHp = 150;
                    Gold = 3000;
                    break;
                default:
                    Atk = 10;
                    Def = 5;
                    MaxHp = 100;
                    Gold = 1000;
                    break;
            }

            Hp = MaxHp;
        }

        // 캐릭터 정보 출력 (상태 보기용)
        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"[이름] {Name}   [직업] {Job}");
            Console.WriteLine($"[레벨] {Level}   [경험치] {Exp} / {ExpToNextLevel}");
            Console.WriteLine($"[체력] {Hp} / {MaxHp}");
            Console.WriteLine($"[공격력] {Atk + ExtraAtk}  (+{ExtraAtk})");
            Console.WriteLine($"[방어력] {Def + ExtraDef}  (+{ExtraDef})");
            Console.WriteLine($"[보유 골드] {Gold} G");
        }

        // 경험치 획득 및 레벨업 처리
        public void GainExp(int amount)
        {
            Exp += amount;
            while (Exp >= ExpToNextLevel)
            {
                Exp -= ExpToNextLevel;
                LevelUp();
            }
        }

        // 레벨업 후 능력치 증가
        private void LevelUp()
        {
            Level++;
            Atk += 1;
            Def += 1;
            ExpToNextLevel = 100 + (Level - 1) * 30;

            Console.WriteLine($"\n레벨업! Lv.{Level}이 되었습니다!");
            Console.WriteLine("  → 공격력 +1");
            Console.WriteLine("  → 방어력 +1");
        }

        // 체력 설정 및 관리
        public void SetHp(int value)
        {
            Hp = Utils.Clamp(value, 0, MaxHp);
        }

        public void Heal()
        {
            Hp = MaxHp;
        }

        public void TakeDamage(int damage)
        {
            SetHp(Hp - damage);
        }

        public void HalveHp()
        {
            SetHp(Math.Max(1, Hp / 2));
        }

        // 골드 조작
        public void AddGold(int amount)
        {
            Gold += amount;
        }

        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        // 아이템 구매 및 인벤토리 관리
        public void BuyItem(Item item)
        {
            SpendGold(item.Price);
            Inventory.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        public List<Item> TakeInventory()
        {
            return Inventory;
        }

        public Item GetInventoryItem(int idx)
        {
            if (idx < 0 || idx >= InventoryCount) return null;
            return Inventory[idx];
        }

        // 인벤토리 출력
        public void DisplayInventory(bool showIndex)
        {
            if (InventoryCount == 0)
            {
                Console.WriteLine("인벤토리에 아이템이 없습니다.");
                return;
            }

            for (int i = 0; i < Inventory.Count; i++)
            {
                Item item = Inventory[i];
                string indexStr = showIndex ? $"{i + 1}. " : "";
                string equippedStr = IsEquipped(item) ? "[E] " : "";
                Console.WriteLine($"{indexStr}{equippedStr}{item.ItemInfoText()}");
            }
        }

        // 장비 장착 및 해제 관리
        public void EquipItem(Item item)
        {
            if (!item.IsEquippable) return;

            // 기존 같은 타입 장비 해제
            Item already = EquipList.FirstOrDefault(i => i.Type == item.Type);
            if (already != null)
            {
                EquipList.Remove(already);
                if (already.Type == ItemType.Weapon)
                    ExtraAtk -= already.Value;
                else
                    ExtraDef -= already.Value;
            }

            // 장착 해제
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == ItemType.Weapon)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
            }
            else // 장착
            {
                EquipList.Add(item);
                if (item.Type == ItemType.Weapon)
                    ExtraAtk += item.Value;
                else
                    ExtraDef += item.Value;
            }
        }

        // 장착 여부 확인
        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }
    }
}
