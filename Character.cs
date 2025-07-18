using System;
using System.Collections.Generic;
using System.Linq;

namespace TextRpg_Comment
{
    // [캐릭터 클래스]
    // 플레이어 캐릭터의 모든 상태, 성장, 인벤토리/장비, 자원 등을 통합 관리함
    public class Character
    {
        // [기본 속성] 이름/직업 등
        public string Name { get; }
        public string Job { get; }

        // [레벨/경험치 속성] 성장에 사용됨
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int ExpToNextLevel { get; private set; }

        // [공격력/방어력]
        public int Atk { get; private set; }
        public int Def { get; private set; }

        // [장비로 인한 추가 수치]
        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        // [체력]
        public int Hp { get; private set; }
        public int MaxHp { get; private set; }

        // [보유 골드]
        public int Gold { get; private set; }

        // [인벤토리 & 장착목록]
        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        // [인벤토리 개수] 읽기 전용
        public int InventoryCount => Inventory.Count;

        // [생성자] 
        // 생성시 이름과 직업에 따라 능력치/체력/시작골드가 다르게 설정됨
        // 프로그램 시작(Main)→SetData에서 호출됨
        public Character(string name, string job)
        {
            Name = name;
            Job = job;
            Level = 1;
            Exp = 0;
            ExpToNextLevel = 100;

            ExtraAtk = 0;
            ExtraDef = 0;

            // [직업별 능력치 분기]
            // 각 직업에 맞는 시작 능력치/체력/골드 할당
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
                    Gold = 10000;
                    break;
                default: // 예외/디폴트
                    Atk = 10;
                    Def = 5;
                    MaxHp = 100;
                    Gold = 10000;
                    break;
            }

            Hp = MaxHp; // 시작시 체력은 풀로 설정
        }

        // [상태 보기] 
        // 마을 UI의 '상태 보기'에서 호출, 캐릭터 모든 정보를 터미널에 출력
        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"[이름] {Name}   [직업] {Job}");
            Console.WriteLine($"[레벨] {Level}   [경험치] {Exp} / {ExpToNextLevel}");
            Console.WriteLine($"[체력] {Hp} / {MaxHp}");
            Console.WriteLine($"[공격력] {Atk + ExtraAtk}  (+{ExtraAtk})");
            Console.WriteLine($"[방어력] {Def + ExtraDef}  (+{ExtraDef})");
            Console.WriteLine($"[보유 골드] {Gold} G");
        }

        // [경험치 획득]
        // 던전 보상 등에서 호출, 경험치 누적/레벨업까지 자동 처리
        public void GainExp(int amount)
        {
            Exp += amount;
            // [반복] 현재 경험치가 요구량 이상일 때마다 계속 레벨업
            while (Exp >= ExpToNextLevel)
            {
                Exp -= ExpToNextLevel; // 초과 경험치는 차감
                LevelUp(); // 내부적으로 레벨+1 및 능력치 상승
            }
        }

        // [레벨업 처리]
        // GainExp에서 호출, 1레벨씩 올리고 능력치/다음경험치 요구량 갱신 + 안내창 출력
        private void LevelUp()
        {
            Level++;
            Atk += 1;           // 공격력/방어력 각 1 증가(매 레벨)
            Def += 1;
            ExpToNextLevel = 100 + (Level - 1) * 30; // 레벨 오를수록 필요 경험치 증가

            // [실행] 레벨업 메시지 출력(즉시 안내)
            Console.WriteLine($"\n레벨업! Lv.{Level}이 되었습니다!");
            Console.WriteLine("  → 공격력 +1");
            Console.WriteLine("  → 방어력 +1");
        }

        // [체력 설정] (직접 변경X, 항상 Clamp로 제한)
        // 구조적으로 Hp변경은 반드시 SetHp 통해 0~MaxHp 보장
        public void SetHp(int value)
        {
            // Utils.Clamp로 안전하게 0~MaxHp 이외 값이 안 들어가게 함
            Hp = Utils.Clamp(value, 0, MaxHp);
        }

        // [체력 완전 회복]
        // ex) 휴식, 포션 등에서 바로 호출
        public void Heal()
        {
            Hp = MaxHp;
        }

        // [데미지 입기]
        // 전투 등에서 호출, Clamp까지 한 번에 적용
        public void TakeDamage(int damage)
        {
            SetHp(Hp - damage);
        }

        // [체력 절반 만들기] (패배 페널티)
        // Hp가 1보다 작아지는 걸 방지(최소 1 체력 보장)
        public void HalveHp()
        {
            SetHp(Math.Max(1, Hp / 2));
        }

        // [골드 증가]
        // 보상 등에서 호출, 그냥 정수값 누적
        public void AddGold(int amount)
        {
            Gold += amount;
        }

        // [골드 소모, true/false 반환]
        // 아이템 구매, 휴식 등에서 호출, 부족하면 false 반환
        public bool SpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true; // [성공] 차감
            }
            return false; // [실패] 소지금 부족
        }

        // [아이템 구매 시 추가] (골드 차감은 외부에서!)
        public void BuyItem(Item item)
        {
            Inventory.Add(item);
        }

        // [드랍/이벤트 등 소모 없이 인벤토리 추가]
        public void AddItemToInventory(Item item)
        {
            Inventory.Add(item);
        }

        // [인벤토리 아이템 제거]
        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        // [소지 여부 확인]
        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        // [인벤토리 리스트 반환] (복사 아님, 직접 리스트)
        public List<Item> TakeInventory()
        {
            return Inventory;
        }

        // [인벤토리에서 특정 인덱스 아이템 반환]
        public Item GetInventoryItem(int idx)
        {
            if (idx < 0 || idx >= InventoryCount) return null;
            return Inventory[idx];
        }

        // [인벤토리 목록 출력] 
        // showIndex: true면 앞에 번호, false면 번호 생략
        // 인벤토리/장착관리 등에서 사용
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

        // [장비 장착/해제]
        // 장비 메뉴에서 호출, 현재 착용여부/같은 타입 중복 체크 후 처리
        public void EquipItem(Item item)
        {
            if (!item.IsEquippable) return;

            // [이미 장착 → 해제]
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == ItemType.Weapon)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
                return;
            }

            // [같은 타입 기존 장비가 있으면 먼저 해제]
            Item already = EquipList.FirstOrDefault(i => i.Type == item.Type);
            if (already != null)
            {
                EquipList.Remove(already);
                if (already.Type == ItemType.Weapon)
                    ExtraAtk -= already.Value;
                else
                    ExtraDef -= already.Value;
            }
            // [새 장비 장착]
            EquipList.Add(item);
            if (item.Type == ItemType.Weapon)
                ExtraAtk += item.Value;
            else
                ExtraDef += item.Value;
        }

        // [장착여부 반환]
        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }
    }
}