using TextRpgPlayerAtk;

namespace TextRpg_Comment
{
    class Program
    {
        private static Character player;
        private static Item[] itemDb;
        private static Dungeon[] dungeonDb;
        private static List<Monster> monsterTypes;

        static int currentFloor = 1;
        static int checkpointFloor = 1; // 5의 배수 저장

        static void Main(string[] args)
        {
            SetData();
            DisplayMainUI();
        }

        static void SetData()
        {
            player = new Character(1, "Chad", "전사", 10, 5, 100, 1000);

            itemDb = new Item[]
            {
                new Item("수련자의 갑옷", ItemType.Armor, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
                new Item("무쇠갑옷", ItemType.Armor, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
                new Item("스파르타의 갑옷", ItemType.Armor, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
                new Item("낡은 검", ItemType.Weapon, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
                new Item("청동 도끼", ItemType.Weapon, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
                new Item("스파르타의 창", ItemType.Weapon, 7,"스파르타의 전사들이 사용했다는 전설의 창입니다. ",2500)
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
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 4);

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
                    DungeonMenuUI(); // 🔸 정의 안 돼 있음 !
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
                        player.EquipItem(targetItem);
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

            // 장착 중이면 해제
            if (player.IsEquipped(sellItem))
            {
                player.EquipItem(sellItem); // 해제
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
                    break;
                }

                // 전투 시작!
                StartBattle(monsters);

                // 전투 승리한 것으로 간주하고 다음 층 이동
                currentFloor++;

                if ((currentFloor - 1) % 5 == 0)
                {
                    checkpointFloor = currentFloor - 1;
                    Console.WriteLine($"\n축하합니다! {checkpointFloor}층 체크포인트에 도달했습니다!");
                    Console.WriteLine("1. 마을로 귀환   2. 계속 탐험");
                    Console.Write(">> ");
                    int sel = CheckInput(1, 2);
                    if (sel == 1)
                    {
                        Console.WriteLine("마을로 돌아갑니다!");
                        break;
                    }
                }
            }
        }

        static void DisplayDungeonResult(string dungeonName, int beforeHp, int afterHp, int beforeGold, int afterGold)
        {
            Console.WriteLine();
            Console.WriteLine("던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{dungeonName}을(를) 클리어 하였습니다.");
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

        static void DisplayDungeonResultFail(string dungeonName, int beforeHp, int afterHp, int beforeGold, int afterGold)
        {
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

        static void StartBattle(List<Monster> monstersInBattle)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            foreach (var monster in monstersInBattle)
            {
                Console.WriteLine(monster.Info());
            }

            Console.WriteLine("\n[내 정보]");
            player.DisplayCharacterInfo();

            Console.WriteLine("\n1. 공격");
            Console.Write(">> ");
            string input = Console.ReadLine();
            if (input == "1")
            {
                Console.Clear();
                Console.WriteLine("\n[공격할 몬스터를 선택하세요]");
                for (int i = 0; i < monstersInBattle.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {monstersInBattle[i].Info()}");
                }
                
                int selected = CheckInput(1, monstersInBattle.Count);
                Monster target = monstersInBattle[selected - 1];
                PlayerAtk PlayerAtk = new PlayerAtk();
                PlayerAtk.Attack(player, target, monstersInBattle);
            }
            else
            {
                Console.WriteLine("\n잘못된 입력입니다");
            }
        }

        static List<Monster> CreateFloorMonsters(int floor)
        {
            Random rand = new Random();
            int count = rand.Next(1, 5); // 1~4 마리

            List<Monster> monsters = new List<Monster>();
            for (int i = 0; i < count; i++)
            {
                int index = rand.Next(monsterTypes.Count);
                Monster baseM = monsterTypes[index];

                // 층수 기반 보정
                int scaledHp = baseM.Hp + (floor - 1) * 3;
                int scaledAtk = baseM.Atk + (floor - 1) / 2;

                monsters.Add(new Monster(baseM.Name, baseM.Level, scaledHp, scaledAtk));
            }
            return monsters;
        }
    }
}
