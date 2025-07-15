﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TextRpg_Comment
{
    public class Character
    {
        public int Level { get; private set; }
        public int Atk { get; private set; }
        public int Def { get; private set; }
        public string Name { get; }
        public string Job { get; }
        public int Hp { get; set; }
        public int MaxHp { get; private set; }
        public int Gold { get; private set; }

        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public int InventoryCount => Inventory.Count;

        private int dungeonClearCount = 0;

        public Character(int level, string name, string job, int atk, int def, int hp, int gold)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            MaxHp = hp;
            Gold = gold;
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"체력 : {Hp} / {MaxHp}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        public void DisplayInventory(bool showIdx)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];
                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";
                Console.WriteLine($"- {displayIdx}{displayEquipped} {targetItem.ItemInfoText()}");
            }
        }

        // 타입별 1개만 장착
        public void EquipItem(Item item)
        {
            // 이미 장착 중이면 해제
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == ItemType.Weapon)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
                return;
            }

            // 같은 타입 이미 장착 중이면 해제
            Item alreadyEquipped = EquipList.FirstOrDefault(i => i.Type == item.Type);
            if (alreadyEquipped != null)
            {
                EquipList.Remove(alreadyEquipped);
                if (alreadyEquipped.Type == ItemType.Weapon)
                    ExtraAtk -= alreadyEquipped.Value;
                else
                    ExtraDef -= alreadyEquipped.Value;
            }

            // 새로 장착
            EquipList.Add(item);
            if (item.Type == ItemType.Weapon)
                ExtraAtk += item.Value;
            else
                ExtraDef += item.Value;
        }

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }

        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.Add(item);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        public Item GetInventoryItem(int idx)
        {
            if (idx < 0 || idx >= Inventory.Count) return null;
            return Inventory[idx];
        }

        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        public void Heal()
        {
            Hp = MaxHp;
        }

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

        // 던전 클리어/레벨업
        public void AddDungeonClear()
        {
            dungeonClearCount++;
            int requiredClear = Level; // Lv1→2:1, Lv2→3:2, Lv3→4:3, Lv4→5:4

            if (dungeonClearCount >= requiredClear && Level < 5)
            {
                dungeonClearCount = 0;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Level++;
            Atk += 1; // int로 관리, float로 하고 싶으면 0.5f
            Def += 1;
            Console.WriteLine($"레벨업! Lv.{Level}이 되었습니다. 공격력 +1, 방어력 +1");
        }
    }
}
