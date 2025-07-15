using System;
using System.Collections.Generic;
using System.Linq;
using TextRpgPlayerAtk;
using TextRpg;

namespace TextRpg_Comment
{
    class Program
    {
        private static Character player;
        private static Item[] itemDb;
        private static Dungeon[] dungeonDb;
        private static List<Monster> monsterTypes;

        static int currentFloor = 1;
        static int checkpointFloor = 1;

        static void Main(string[] args)
        {
            SetData();
            DisplayMainUI();
        }

        static void SetData()
        {
            player = new Character(1, "Chad", "전사", 10, 5, 100, 10000);

            itemDb = new Item[]
            {
                new Item("수련자의 갑옷", ItemType.Armor, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
                new Item("무쇠갑옷", ItemType.Armor, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
                new Item("스파르타의 갑옷", ItemType.Armor, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
                new Item("낡은 검", ItemType.Weapon, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
                new Item("청동 도끼", ItemType.Weapon, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
                new Item("스파르타의 창", ItemType.Weapon, 70,"스파르타의 전사들이 사용했다는 전설의 창입니다. ",2500),
                new Item("작은 포션", ItemType.potion, 30,"목을 축일 정도로 담긴 포션 입니다. ",600),
                new Item("중간 포션", ItemType.potion, 70,"시원하게 들이킬 수 있는 정도의 포션입니다. ",1200),
                new Item("큰 포션", ItemType.potion, 100,"모든것이 좋아질듯한 양의 포션입니다.",2000),

            };

            dungeonDb = new Dungeon[]
            {
                new Dungeon("쉬운 던전", 5, 1000),
                new Dungeon("일반 던전", 11, 1700),
                new Dungeon("어려운 던전", 17, 2500)
            };

            monsterTypes = new List<Monster>()
            {
                new Monster ("미니언", 2, 15, 5),
                new Monster ("공허충", 3, 10, 9),
                new Monster ("대포미니언", 5, 25, 8)
            };
        }

        static void DisplayMainUI()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 5);

            switch (result)
            {
                case 1:
                    DisplayStatUI();
                    break;
                case 2:
                    DisplayInventoryUI();
                    break;
                case 3:
                    DisplayShopUI();
                    break;
                case 4:
                    DungeonMenuUI();
                    break;
                case 5:
                    DisplayRestUI();
                    break;
            }
        }

        static void DisplayStatUI()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            player.DisplayCharacterInfo();
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 0);
            if (result == 0)
                DisplayMainUI();
        }

        static void DisplayInventoryUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            player.DisplayInventory(false);
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    DisplayEquipUI();
                    break;
            }
        }

        static void DisplayEquipUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            player.DisplayInventory(true);
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, player.InventoryCount);

            switch (result)
            {
                case 0:
                    DisplayInventoryUI();
                    break;
                default:
                    int itemIdx = result - 1;
                    Item targetItem = player.GetInventoryItem(itemIdx);
                    if (targetItem != null)
                    {
                        if(targetItem.IsEquippable)
                        {
                            player.EquipItem(targetItem);
                        }
                        else
                        {
                            Console.WriteLine("포션 아이템은 장착할 수 없습니다.");
                            Console.ReadLine();
                        }
                    }
                    
                    DisplayEquipUI();
                    break;
                    
            }
        }

        static void DisplayShopUI()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < itemDb.Length; i++)
            {
                Item curItem = itemDb[i];
                string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
                Console.WriteLine($"- {curItem.ItemInfoText()}  |  {displayPrice}");
            }
            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 2);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    DisplayBuyUI();
                    break;
                case 2:
                    DisplaySellUI();
                    break;
            }
        }

        static void DisplayBuyUI()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < itemDb.Length; i++)
            {
                Item curItem = itemDb[i];
                string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
                Console.WriteLine($"- {i + 1} {curItem.ItemInfoText()}  |  {displayPrice}");
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, itemDb.Length);

            switch (result)
            {
                case 0:
                    DisplayShopUI();
                    break;
                default:
                    int itemIdx = result - 1;
                    Item targetItem = itemDb[itemIdx];

                    if (player.HasItem(targetItem))
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }
                    else if (player.Gold >= targetItem.Price)
                    {
                        Console.WriteLine("구매를 완료했습니다.");
                        player.BuyItem(targetItem);
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }
                    DisplayBuyUI();
                    break;
            }
        }

        static void DisplaySellUI()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();

            if (player.InventoryCount == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.");
                Console.WriteLine("Enter를 누르면 상점으로 돌아갑니다.");
                Console.ReadLine();
                DisplayShopUI();
                return;
            }

            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < player.InventoryCount; i++)
            {
                Item item = player.GetInventoryItem(i);
                int sellPrice = (int)(item.Price * 0.85);
                string equipped = player.IsEquipped(item) ? "[E]" : "";
                Console.WriteLine($"- {i + 1} {equipped}{item.ItemInfoText()}  |  {sellPrice} G");
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, player.InventoryCount);

            if (result == 0)
            {
                DisplayShopUI();
                return;
            }

            int idx = result - 1;
            Item sellItem = player.GetInventoryItem(idx);

            if (player.IsEquipped(sellItem))
            {
                player.EquipItem(sellItem);
            }

            int sellPrice2 = (int)(sellItem.Price * 0.85);
            player.AddGold(sellPrice2);
            player.RemoveItem(sellItem);

            Console.WriteLine($"{sellItem.Name}을(를) 판매했습니다! (+{sellPrice2} G)");
            Console.WriteLine("Enter를 누르면 계속합니다.");
            Console.ReadLine();

            DisplaySellUI();
        }

        static void DisplayRestUI()
        {
            const int restPrice = 500;

            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine($"{restPrice} G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)");
            Console.WriteLine();
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    if (player.Gold >= restPrice)
                    {
                        player.SpendGold(restPrice);
                        player.Heal();
                        Console.WriteLine("휴식을 완료했습니다. 체력이 모두 회복되었습니다!");
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                    }
                    Console.WriteLine("Enter 를 눌러주세요.");
                    Console.ReadLine();
                    DisplayMainUI();
                    break;
            }
        }

        static void DungeonMenuUI()
        {
            Console.Clear();
            Console.WriteLine("던전 입장 방식을 선택하세요:");
            Console.WriteLine("1. 1층부터 시작");
            if (checkpointFloor >= 5)
                Console.WriteLine($"2. 체크포인트({checkpointFloor}층)부터 시작");
            Console.Write(">> ");
            int maxOption = checkpointFloor >= 5 ? 2 : 1;
            int input = CheckInput(1, maxOption);
            currentFloor = (input == 1) ? 1 : checkpointFloor;

            EnterDungeonUI();
        }

        static void EnterDungeonUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"[{currentFloor}층] 던전을 시작합니다!");

                List<Monster> monsters = CreateFloorMonsters(currentFloor);

                Console.WriteLine("등장 몬스터:");
                foreach (var m in monsters)
                    Console.WriteLine(m.Info());
                Console.WriteLine();

                player.DisplayCharacterInfo();
                Console.WriteLine();

                Console.WriteLine("1. 전투 시작   2. 던전 중단");
                Console.Write(">> ");
                int act = CheckInput(1, 2);

                if (act == 2)
                {
                    Console.WriteLine("던전 탐험을 중단하고 마을로 돌아갑니다.");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    break;
                }

                int beforeHp = player.Hp;
                int beforeGold = player.Gold;

                bool battleResult = StartBattle(monsters);

                if (battleResult) // 전투 승리 시
                {
                    int afterHp = player.Hp;
                    int afterGold = player.Gold;
                    player.AddDungeonClear(); // 던전 클리어 횟수 증가 및 레벨업 확인

                    // DisplayDungeonResult에서 다음 층으로 갈지, 마을로 귀환할지 결정
                    bool continueDungeon = DisplayDungeonResult($"[{currentFloor}층] 던전", beforeHp, afterHp, beforeGold, afterGold);

                    if (continueDungeon)
                    {
                        currentFloor++; // 다음 층으로 이동
                    }
                    else
                    {
                        break; // 던전 탐험 루프 종료
                    }
                }
                else // 전투 패배 시 (플레이어 사망)
                {
                    int afterHp = player.Hp;
                    int afterGold = player.Gold;
                    DisplayDungeonResultFail($"[{currentFloor}층] 던전", beforeHp, afterHp, beforeGold, afterGold);

                    player.HalveHp();
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    break;
                }
            }
            DisplayMainUI();
        }

        static bool DisplayDungeonResult(string dungeonName, int beforeHp, int afterHp, int beforeGold, int afterGold)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{dungeonName}을(를) 클리어 하였습니다.");
            Console.WriteLine();
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {beforeHp} -> {afterHp}");
            Console.WriteLine($"Gold {beforeGold} G -> {afterGold} G");
            Console.WriteLine();

            // 현재 클리어한 층이 체크포인트인지 확인
            if ((currentFloor % 5) == 0)
            {
                checkpointFloor = currentFloor; // 현재 클리어한 층을 체크포인트로 저장
                Console.WriteLine($"\n축하합니다! {checkpointFloor}층 체크포인트에 도달했습니다!");
            }

            Console.WriteLine("1. 다음 층으로 이동   0. 마을로 귀환");
            Console.Write(">> ");
            int sel = CheckInput(0, 1);

            if (sel == 1)
            {
                return true; // 다음 층으로 이동
            }
            else
            {
                Console.WriteLine("마을로 돌아갑니다!");
                Console.WriteLine("Enter를 눌러 계속...");
                Console.ReadLine();
                return false; // 마을로 귀환
            }
        }

        static void DisplayDungeonResultFail(string dungeonName, int beforeHp, int afterHp, int beforeGold, int afterGold)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("던전 실패");
            Console.WriteLine($"{dungeonName} 도전 실패! 보상 없음.");
            Console.WriteLine();
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {beforeHp} -> {afterHp}");
            Console.WriteLine($"Gold {beforeGold} G -> {afterGold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 0);
            if (result == 0)
                DisplayMainUI();
        }


        
        static bool StartBattle(List<Monster> monstersInBattle)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");

                Console.WriteLine("[몬스터 정보]");
                List<Monster> livingMonsters = monstersInBattle.Where(m => m.Hp > 0).ToList();
                if (!livingMonsters.Any())
                {
                    Console.WriteLine("\n남아있는 몬스터가 없습니다.");
                    Console.WriteLine("\n모든 몬스터를 처치했습니다!");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    return true;
                }

                foreach (var monster in livingMonsters)
                {
                    Console.WriteLine(monster.Info());
                }

                Console.WriteLine("\n[내 정보]");
                player.DisplayCharacterInfo();

                if (player.Hp <= 0)
                {
                    Console.WriteLine("\n플레이어가 쓰러졌습니다!");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    return false;
                }

                Console.WriteLine("\n1. 공격");
                Console.WriteLine("\n2. 포션사용");
                Console.Write(">> ");
                string input = Console.ReadLine();

                switch(input)
                {
                    case "1":
                    Console.Clear();
                    Console.WriteLine("\n[공격할 몬스터를 선택하세요]");

                    livingMonsters = monstersInBattle.Where(m => m.Hp > 0).ToList();
                    for (int i = 0; i < livingMonsters.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {livingMonsters[i].Info()}");
                    }

                    int selected = CheckInput(1, livingMonsters.Count);
                    Monster target = livingMonsters[selected - 1];

                    PlayerAtk playerAttackHandler = new PlayerAtk();
                    playerAttackHandler.Attack(player, target);

                    if (monstersInBattle.All(m => m.Hp <= 0))
                    {
                        continue;
                    }

                    if (player.Hp > 0)
                    {
                        EnemyAttackPhase.EnemyAtkPhase(player, monstersInBattle);
                    }
                    else
                    {
                        continue;
                    }
                    break;
                    case "2":
                    Console.Clear();
                    Console.WriteLine("보유중인 포션");
                    List<Item> potions = player.TakeInventory().Where(i => i.Type == ItemType.potion).ToList();
                        if (potions.Count == 0)
                        {
                            Console.WriteLine(" 사용 가능한 포션이 없습니다.");
                            Console.WriteLine("Enter를 눌러 돌아갑니다...");
                            Console.ReadLine();
                            continue; 
                        }

                        for (int i = 0; i < potions.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {potions[i].ItemInfoText()}");
                        }

                        Console.WriteLine("\n사용할 포션을 선택하세요:");
                        int potiontake = CheckInput(1, potions.Count);
                        Item selectedPotion = potions[potiontake - 1];

                        // 회복 처리
                        player.SetHp(player.Hp + selectedPotion.Value);
                        Console.WriteLine($"{selectedPotion.Name} 사용 체력 +{selectedPotion.Value}");
                        Console.WriteLine($"현재 체력: {player.Hp}");

                        // 포션 제거 (사용되었다면)
                        player.RemoveItem(selectedPotion);

                        Console.WriteLine("\nEnter를 눌러 전투로 돌아갑니다...");
                        Console.ReadLine();
                        break;

                    default:
                    Console.WriteLine("\n잘못된 입력입니다");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    break;
                        


                }
                
            }
        }

        static List<Monster> CreateFloorMonsters(int floor)
        {
            Random rand = new Random();
            int count = rand.Next(1, 5);

            List<Monster> monsters = new List<Monster>();
            for (int i = 0; i < count; i++)
            {
                int index = rand.Next(monsterTypes.Count);
                Monster baseM = monsterTypes[index];

                int scaledHp = baseM.Hp + (floor - 1) * 3;
                int scaledAtk = baseM.Atk + (floor - 1) / 2;

                monsters.Add(new Monster(baseM.Name, baseM.Level, scaledHp, scaledAtk));
            }
            return monsters;
        }

        static int CheckInput(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out result);

                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result;
                }
                Console.WriteLine("잘못된 입력입니다!!!!");
            }
        }
    }
}
