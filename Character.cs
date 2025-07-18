using System;
using System.Collections.Generic;
using System.Linq;

namespace TextRpg_Comment
{
    // 캐릭터 클래스: 플레이어의 속성, 성장, 인벤토리, 장비, 자원 관리
    public class Character
    {
        // 기본 정보 및 성장 관련
        public string Name { get; }
        public string Job { get; }

        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int ExpToNextLevel { get; private set; }

        // 능력치
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        // 체력, 골드
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }
        public int Gold { get; private set; }

        // 인벤토리, 장착 장비
        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        // 인벤토리 개수
        public int InventoryCount => Inventory.Count;

        // 캐릭터 생성자: 직업별 초기 스탯, 자원 설정
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
                    Atk = 13; Def = 7; MaxHp = 100; Gold = 10000; break;
                case "마법사":
                    Atk = 17; Def = 3; MaxHp = 80; Gold = 10000; break;
                case "궁수":
                    Atk = 16; Def = 4; MaxHp = 80; Gold = 10000; break;
                case "도적":
                    Atk = 14; Def = 6; MaxHp = 90; Gold = 10000; break;
                case "백수":
                    Atk = 9; Def = 9; MaxHp = 150; Gold = 10000; break;
                default:
                    Atk = 10; Def = 5; MaxHp = 100; Gold = 10000; break;
            }
            Hp = MaxHp;
        }

        // ------------------------------------------
        // [캐릭터 상태 출력]
        // 담당자: 김인빈
        // → 현재 이름, 직업, 레벨, 경험치/레벨업 필요 경험치, 능력치 등을 보여줌
        // ------------------------------------------
        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"[이름] {Name}   [직업] {Job}");
            Console.WriteLine($"[레벨] {Level}   [경험치] {Exp} / {ExpToNextLevel}");
            Console.WriteLine($"[체력] {Hp} / {MaxHp}");
            Console.WriteLine($"[공격력] {Atk + ExtraAtk}  (+{ExtraAtk})");
            Console.WriteLine($"[방어력] {Def + ExtraDef}  (+{ExtraDef})");
            Console.WriteLine($"[보유 골드] {Gold} G");
        }

        // ------------------------------------------
        // [경험치 획득 및 레벨업 처리]
        // 담당자: 김인빈
        // → 경험치 누적, 레벨업 판정 후 능력치 증강과 알림 기능 담당
        // ------------------------------------------
        public void GainExp(int amount)
        {
            Exp += amount;
            while (Exp >= ExpToNextLevel)
            {
                Exp -= ExpToNextLevel;
                LevelUp();
            }
        }

        // ------------------------------------------
        // [레벨업 세부 처리]
        // 담당자: 김인빈
        // → 실질적으로 캐릭터 레벨/능력치 증가 및 레벨업 메시지 출력
        // ------------------------------------------
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

        // 체력 설정
        public void SetHp(int value)
        {
            Hp = Utils.Clamp(value, 0, MaxHp);
        }

        // 체력 완전 회복
        public void Heal()
        {
            Hp = MaxHp;
        }

        // 데미지 입기
        public void TakeDamage(int damage)
        {
            SetHp(Hp - damage);
        }

        // 체력 절반(패배 시)
        public void HalveHp()
        {
            SetHp(Math.Max(1, Hp / 2));
        }

        // 골드 증가
        public void AddGold(int amount)
        {
            Gold += amount;
        }

        // 골드 소모(성공 시 true)
        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        // 아이템 구매(인벤토리에 추가)
        public void BuyItem(Item item)
        {
            Inventory.Add(item);
        }

        // 인벤토리에 아이템 추가(무료 드랍 등)
        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);
        }

        // 인벤토리에서 아이템 제거
        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        // 아이템 소유 여부 확인
        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        // 인벤토리 전체 반환
        public List<Item> TakeInventory()
        {
            return Inventory;
        }

        // 인덱스로 아이템 반환
        public Item GetInventoryItem(int idx)
        {
            if (idx < 0 || idx >= InventoryCount) return null;
            return Inventory[idx];
        }

        // 인벤토리 목록 출력(번호/장착표시 옵션)
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

        // 장비 장착/해제 처리 (중복 장비 교체도 관리)
        public void EquipItem(Item item)
        {
            if (!item.IsEquippable) return;
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == ItemType.Weapon) ExtraAtk -= item.Value;
                else ExtraDef -= item.Value;
                return;
            }
            Item already = EquipList.FirstOrDefault(i => i.Type == item.Type);
            if (already != null)
            {
                EquipList.Remove(already);
                if (already.Type == ItemType.Weapon) ExtraAtk -= already.Value;
                else ExtraDef -= already.Value;
            }
            EquipList.Add(item);
            if (item.Type == ItemType.Weapon) ExtraAtk += item.Value;
            else ExtraDef += item.Value;
        }

        // 장착 여부 확인
        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }
    }
}