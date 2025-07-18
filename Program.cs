using System;
using System.Collections.Generic;
using System.Linq;
using TextRpg;
using TextRpgPlayerAtk;
using TextRpg_Comment;

namespace TextRpg_Comment
{
    // 메인 프로그램 클래스
    class Program
    {
        // 플레이어 및 데이터베이스(아이템, 던전, 몬스터), 랜덤 객체
        private static Character player;
        private static Item[] itemDb;
        private static Dungeon[] dungeonDb;
        private static List<Monster> monsterTypes;
        private static Random random = new Random();

        // 현재 층수와 체크포인트 관리
        static int currentFloor = 1;
        static int checkpointFloor = 1;

        // 프로그램 시작 지점
        static void Main(string[] args)
        {
            ShowIntro();
            string name = InputName();
            string job = SelectJob();
            SetData(name, job);
            Console.Clear();
            Console.WriteLine($"모험가 {name}({job})님의 여정이 지금 시작됩니다!");
            Console.WriteLine("엔터를 눌러 계속합니다...");
            Console.ReadLine();
            DisplayMainUI();
        }

        // 시작 인트로 안내
        static void ShowIntro()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("   어둠이 드리운 대륙, 끝나지 않은 전쟁...");
            Console.WriteLine("   오직 한 사람만이 세계를 구할 수 있다.");
            Console.WriteLine("   전설의 용사가 되어 여정을 시작하세요.");
            Console.WriteLine("===========================================");
            Console.WriteLine("\n엔터를 눌러 계속...");
            Console.ReadLine();
        }

        // 이름 입력
        static string InputName()
        {
            Console.Clear();
            Console.Write("당신의 이름을 입력해주세요: ");
            string name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("이름은 필수입니다. 다시 입력해주세요: ");
                name = Console.ReadLine();
            }
            return name;
        }

        // 직업 선택
        static string SelectJob()
        {
            Console.Clear();
            Console.WriteLine("직업을 선택하세요:");
            Console.WriteLine("1. 전사");
            Console.WriteLine("2. 마법사");
            Console.WriteLine("3. 궁수");
            Console.WriteLine("4. 도적");
            Console.WriteLine("5. 백수");
            Console.Write(">> ");
            int choice = CheckInput(1, 5);
            switch (choice)
            {
                case 1: return "전사";
                case 2: return "마법사";
                case 3: return "궁수";
                case 4: return "도적";
                case 5: return "백수";
                default: return "백수";
            }
        }

        // 게임 데이터 초기화(캐릭터, 아이템, 던전, 몬스터)
        static void SetData(string name, string job)
        {
            player = new Character(name, job);

            itemDb = new Item[]
            {
                new Item("수련자의 갑옷", ItemType.Armor, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
                new Item("무쇠갑옷", ItemType.Armor, 9, "무쇠로 만들어진 튼튼한 갑옷입니다.", 2000),
                new Item("스파르타의 갑옷", ItemType.Armor, 100, "전설의 갑옷입니다.", 3500),
                new Item("낡은 검", ItemType.Weapon, 2, "낡은 검입니다.", 600),
                new Item("청동 도끼", ItemType.Weapon, 5, "사용감 있는 도끼입니다.", 1500),
                new Item("스파르타의 창", ItemType.Weapon, 100, "전설의 창입니다.", 2500),
                new Item("작은 포션", ItemType.potion, 30, "작은 회복 포션입니다.", 600),
                new Item("중간 포션", ItemType.potion, 70, "중간 크기 포션입니다.", 1200),
                new Item("큰 포션", ItemType.potion, 100, "대용량 회복 포션입니다.", 2000)
            };

            dungeonDb = new Dungeon[]
            {
                new Dungeon("쉬운 던전", 5, 1000),
                new Dungeon("일반 던전", 11, 1700),
                new Dungeon("어려운 던전", 17, 2500)
            };

            monsterTypes = new List<Monster>
            {
                new Monster("미니언", 2, 15, 5 ,0),
                new Monster("공허충", 3, 10, 9 , 0),
                new Monster("대포미니언", 5, 25, 8, 0),
                new Monster("전령", 10, 100, 10, 0),
                new Monster("파이어 드래곤", 15, 150, 15, 0),
                new Monster("황금 고블린" , 7, 40,6,100000 )
            };
        }

        // 숫자 입력 유효성 체크
        static int CheckInput(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out result) && result >= min && result <= max)
                    return result;
                Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
            }
        }

        // 마을 메인 UI 루프
        static void DisplayMainUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== 마을 ====");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine("0. 게임 종료");
                Console.Write("\n원하는 행동을 선택하세요: ");
                int input = CheckInput(0, 5);
                switch (input)
                {
                    case 1: DisplayStatUI(); break;
                    case 2: DisplayInventoryUI(); break;
                    case 3: DisplayShopUI(); break;
                    case 4: DungeonMenuUI(); break;
                    case 5: DisplayRestUI(); break;
                    case 0:
                        Console.WriteLine("게임을 종료합니다.");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        // 상태 보기 화면
        static void DisplayStatUI()
        {
            Console.Clear();
            Console.WriteLine("==== 캐릭터 상태 ====");
            player.DisplayCharacterInfo();
            Console.WriteLine("\n엔터를 누르세요...");
            Console.ReadLine();
        }

        // 인벤토리 화면
        static void DisplayInventoryUI()
        {
            Console.Clear();
            Console.WriteLine("==== 인벤토리 ====");
            player.DisplayInventory(false);
            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            int input = CheckInput(0, 1);
            if (input == 1)
                DisplayEquipUI();
        }

        // 장착 관리 UI
        static void DisplayEquipUI()
        {
            Console.Clear();
            Console.WriteLine("==== 장착 관리 ====");
            player.DisplayInventory(true);
            Console.WriteLine("\n0. 나가기");
            Console.Write(">> ");
            int input = CheckInput(0, player.InventoryCount);
            if (input == 0) return;
            Item item = player.GetInventoryItem(input - 1);
            if (item != null && item.IsEquippable)
                player.EquipItem(item);
            else if (item != null)
            {
                Console.WriteLine("포션은 장착할 수 없습니다.");
                Console.ReadLine();
            }
            DisplayEquipUI();
        }

        // 상점 화면
        static void DisplayShopUI()
        {
            Console.Clear();
            Console.WriteLine("==== 상점 ====");
            Console.WriteLine($"보유 골드: {player.Gold} G\n");
            for (int i = 0; i < itemDb.Length; i++)
            {
                Item item = itemDb[i];
                string priceStr = (item.Type == ItemType.potion || !player.HasItem(item)) ? $"{item.Price} G" : "구매완료";
                Console.WriteLine($"{i + 1}. {item.ItemInfoText()}  |  {priceStr}");
            }
            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            int sel = CheckInput(0, 2);
            if (sel == 1) DisplayBuyUI();
            else if (sel == 2) DisplaySellUI();
        }

        // 아이템 구매 UI
        static void DisplayBuyUI()
        {
            Console.Clear();
            Console.WriteLine($"==== 아이템 구매 ====\n보유 골드: {player.Gold} G\n");
            for (int i = 0; i < itemDb.Length; i++)
            {
                Item item = itemDb[i];
                string priceStr = player.HasItem(item) && item.Type != ItemType.potion ? "구매완료" : $"{item.Price} G";
                Console.WriteLine($"{i + 1}. {item.ItemInfoText()} | {priceStr}");
            }
            Console.WriteLine("\n0. 나가기\n>> ");
            int input = CheckInput(0, itemDb.Length);
            if (input == 0) return;
            Item selected = itemDb[input - 1];
            if (selected.Type != ItemType.potion && player.HasItem(selected))
                Console.WriteLine("이미 구매한 아이템입니다.");
            else if (player.SpendGold(selected.Price))
            {
                player.BuyItem(selected);
                Console.WriteLine("구입 완료!");
            }
            else
                Console.WriteLine("골드가 부족합니다.");
            Console.WriteLine("엔터를 눌러 계속...");
            Console.ReadLine();
            DisplayBuyUI();
        }

        // 아이템 판매 UI
        static void DisplaySellUI()
        {
            Console.Clear();
            Console.WriteLine("==== 아이템 판매 ====");
            if (player.InventoryCount == 0)
            {
                Console.WriteLine("보유 중인 아이템이 없습니다.");
                Console.WriteLine("엔터를 누르세요...");
                Console.ReadLine();
                return;
            }
            for (int i = 0; i < player.InventoryCount; i++)
            {
                Item item = player.GetInventoryItem(i);
                int sellPrice = (int)(item.Price * 0.85);
                string equipped = player.IsEquipped(item) ? "[E] " : "";
                Console.WriteLine($"{i + 1}. {equipped}{item.ItemInfoText()} | {sellPrice} G");
            }
            Console.WriteLine("0. 나가기\n>> ");
            int input = CheckInput(0, player.InventoryCount);
            if (input == 0) return;
            Item sellItem = player.GetInventoryItem(input - 1);
            if (player.IsEquipped(sellItem))
                player.EquipItem(sellItem);
            int sellGold = (int)(sellItem.Price * 0.85);
            player.AddGold(sellGold);
            player.RemoveItem(sellItem);
            Console.WriteLine($"{sellItem.Name}을 판매 완료 (+{sellGold} G)");
            Console.WriteLine("엔터를 누르세요...");
            Console.ReadLine();
            DisplaySellUI();
        }

        // 휴식 기능 UI (골드로 체력 회복)
        static void DisplayRestUI()
        {
            const int restPrice = 500;
            Console.Clear();
            Console.WriteLine($"휴식 비용: {restPrice} G");
            Console.WriteLine($"현재 골드: {player.Gold} G");
            Console.WriteLine("체력을 모두 회복하시겠습니까?");
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.Write(">> ");
            int input = CheckInput(0, 1);
            if (input == 1)
            {
                if (player.SpendGold(restPrice))
                {
                    player.Heal();
                    Console.WriteLine("체력이 회복되었습니다.");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                }
                Console.WriteLine("엔터를 누르세요...");
                Console.ReadLine();
            }
        }

        // 던전 입장 방법 선택 UI
        static void DungeonMenuUI()
        {
            Console.Clear();
            Console.WriteLine("던전 입장 방식을 선택하세요:");
            Console.WriteLine("1. 1층부터");
            if (checkpointFloor >= 5)
                Console.WriteLine($"2. 체크포인트({checkpointFloor}층)부터 시작");
            int maxOption = checkpointFloor >= 5 ? 2 : 1;
            int input = CheckInput(1, maxOption);
            currentFloor = (input == 1) ? 1 : checkpointFloor;
            EnterDungeonUI();
        }

        // 던전 연속 진행 : 유형준 담당
        static void EnterDungeonUI()
        {
            while (true) // 던전 층 반복
            {
                Console.Clear();
                Console.WriteLine($"== {currentFloor}층 도전 ==");
                List<Monster> monsters = CreateFloorMonsters(currentFloor);
                Console.WriteLine("\n몬스터 등장!");

                foreach (var m in monsters)
                Console.WriteLine(m.Info()); // 생성된 몬스터들 정보 화면에 출력
                Console.WriteLine("\n1. 전투 시작");
                Console.WriteLine("2. 마을로 돌아가기");
                Console.Write(">> ");
                int input = CheckInput(1, 2);

                if (input == 2) return; // 2 입력 시 마을 복귀

                int beforeHp = player.Hp; // 전투 전 플레이어의 체력 저장
                int beforeGold = player.Gold; // 전투 전 플레이어의 골드 저장
                bool win = StartBattle(monsters); // 전투 시작 로직 호출

                if (win) // 승리하였을 경우
                {
                    int afterHp = player.Hp; // 전투 후 플레이어의 최종 체력 저장
                    int goldRewardForFloor = currentFloor * 500; // 층에 비례해 골드 보상 계산 (현재 층 x 500)
                    player.AddGold(goldRewardForFloor); // 계산된 골드 보상 플레이어의 골드에 추가
                    int currentGoldAfterReward = player.Gold;
                    bool continueNext = DisplayDungeonResult($"[{currentFloor}층] 던전", beforeHp, afterHp, beforeGold, currentGoldAfterReward, true);
                    // DisplayDungeonResult 호출 + 승리 결과 출력

                    if (continueNext) // DisplayDungeonResult에서 반환받은 값이 true일 경우
                        currentFloor++; // 현재 층에 1 추가

                    else // false일 경우 마을로 복귀
                        break;
                }
                else // 패배 시
                {
                    player.HalveHp(); // 플레이어의 체력을 1로 설정함
                    Console.WriteLine("전투 실패! 체력이 [1]로 줄어듭니다...");
                    Console.ReadLine();
                    DisplayDungeonResult($"[{currentFloor}층] 던전", beforeHp, player.Hp, beforeGold, player.Gold, false); // 패배 결과 출력
                    break;
                }
            }
        }

        // 전투 루프(턴제)
        static bool StartBattle(List<Monster> monsters)
        {
            while (true)
            {
                Console.Clear();
                var living = monsters.Where(m => m.Hp > 0).ToList();
                if (!living.Any()) return true;
                if (player.Hp <= 0) return false;
                Console.WriteLine("[몬스터]");
                foreach (var m in living)
                    Console.WriteLine(m.Info());
                Console.WriteLine("\n[플레이어]");
                player.DisplayCharacterInfo();
                Console.WriteLine("\n1. 공격");
                Console.WriteLine("2. 포션 사용");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    Console.Clear();
                    Console.WriteLine("공격할 몬스터를 선택하세요:");
                    for (int i = 0; i < living.Count; i++)
                        Console.WriteLine($"{i + 1}. {living[i].Info()}");
                    int sel = CheckInput(1, living.Count);
                    Monster target = living[sel - 1];
                    new PlayerAtk().Attack(player, target);
                    if (player.Hp > 0)
                        EnemyAttackPhase.EnemyAtkPhase(player, monsters);
                }
                else if (input == "2")
                {
                    var potions = player.TakeInventory().Where(i => i.Type == ItemType.potion).ToList();
                    // 플레이어 인벤토리 리스트에서 타입이 포션인 구성요소가 리스트에 있는지 확인하는 코드
                    if (!potions.Any())
                    {
                        Console.WriteLine("사용 가능한 포션이 없습니다.");
                        Console.ReadLine();
                        continue;
                    }
                    Console.WriteLine("사용할 포션을 선택하세요:");
                    for (int i = 0; i < potions.Count; i++)
                        Console.WriteLine($"{i + 1}. {potions[i].ItemInfoText()}");
                    int sel = CheckInput(1, potions.Count);// 선택한 포션 번호 확인
                    var potion = potions[sel - 1];// 코드 번호를 맞추기 위해 -1
                    player.SetHp(player.Hp + potion.Value);// 플레이어 체력 증가
                    player.RemoveItem(potion);// 포션 제거
                    Console.WriteLine($"{potion.Name} 사용 - 체력 +{potion.Value}");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                }
            }
        }

        
        // 포션, 골드 드랍 및 전투 결과 처리 : 유형준 담당
        static bool DisplayDungeonResult(
            string dungeonName, int beforeHp, int afterHp, int beforeGold, int afterGold, bool isWin)
        {
            Console.Clear();
            Console.WriteLine($"{dungeonName} {(isWin ? "클리어!" : "실패!")}");
            Console.WriteLine($"체력: {beforeHp} → {afterHp}");
            Console.WriteLine($"골드: {beforeGold} → {afterGold}");

            if (isWin) // 플레이어의 승리 여부 (true면 승리, false면 패배)
            {
                int goldGained = afterGold - beforeGold; // 획득 골드 계산
                Console.WriteLine($"획득 골드: +{goldGained} G");
                Random rand = new Random(); // 포션 랜덤 드랍을 위한 랜덤 객체 생성
                int potionDropCount = rand.Next(1, 4); // 랜덤값 범위 설정 (1~3개)
                if (potionDropCount > 0)
                {
                    Console.WriteLine("획득한 아이템:");
                    for (int i = 0; i < potionDropCount; i++) // 랜덤 값만큼 반복 = 포션 드랍 (반복 시 무작위 포션 종류가 선택되어 드랍)
                    {
                        var potionsInDb = itemDb.Where(item => item.Type == ItemType.potion).ToList(); // itemDb에서 모든 포션 종류를 가져옴

                        if (potionsInDb.Any())
                        {
                            Item droppedPotion = potionsInDb[rand.Next(potionsInDb.Count)]; // 무작위로 하나의 포션 선택
                            player.AddItemToInventory(droppedPotion); // 선택된 포션을 인벤토리에 추가
                            Console.WriteLine($"- {droppedPotion.Name}");
                        }
                    }
                }
                else // 랜덤 값이 0일 경우
                {
                    Console.WriteLine("획득한 아이템: 없음");
                }


                int expReward = 100 + (currentFloor - 1) * 20;
                player.GainExp(expReward);
                Console.WriteLine($"경험치 +{expReward}");
                if ((currentFloor % 5) == 0) // 5층 단위로 체크포인트 저장
                {
                    checkpointFloor = currentFloor;
                    Console.WriteLine($"체크포인트에 도달했습니다! (층: {checkpointFloor})");
                }

                Console.WriteLine("\n1. 다음 층");
                Console.WriteLine("0. 마을로 귀환\n>> ");
                int sel = CheckInput(0, 1); // 0 또는 1을 입력받음
                return sel == 1; // sel이 1일 경우 1 == 1 은 true 이므로 EnterDungeonUI의 continueNext 변수에 true 반환. 0일 경우 false 반환
            }
            else // 패배 시
            {
                Console.WriteLine("골드나 아이템은 드랍되지 않습니다.");
                Console.WriteLine("\n0. 마을로 귀환\n>> ");
                int sel = CheckInput(0, 0); // 0만 입력 받도록 설정
                return false; // false를 반환 = 마을로 귀환
            }
        }

        
        // 몬스터 랜덤 생성 및 특정 층부터 특정 몬스터 출현 : 유형준 담당
        static List<Monster> CreateFloorMonsters(int floor)
        {
            Random rand = new Random(); // 랜덤 값 생성 (몬스터의 수 및 종류에 사용)
            List<Monster> list = new List<Monster>(); // 해당 층에서 스폰될 몬스터 객체들을 담을 빈 리스트 생성
            int count = rand.Next(2, 5); // 랜덤 값 범위 설정
            List<Monster> availableMonsters = new List<Monster>(); // 해당 층에서 등장 가능한 몬스터의 종류를 담을 리스트 생성

            foreach (var monster in monsterTypes) // 특정 몬스터들의 소환 조건 확인
            {
                if (monster.Name == "전령" && floor < 6) //
                    continue;
                if (monster.Name == "파이어 드래곤" && floor < 11)
                    continue;
                //서승우 담당(황금고블린만)
                if (monster.Name == "황금 고블린")
                {
                    if (random.Next(0, 100) != 0)//미리 입력한 렌덤 함수작동해 1/100 계산
                        continue;// 100~0중 0이 아닐경우 다시-> 100분의 1확률로 고블린 등장
                    Console.ForegroundColor = ConsoleColor.Yellow;//글씨 색 변경
                    Console.WriteLine("황★금★고★블★린");//continue가 작동하지 않아 황금고블린이 등장시 문구 출력
                    Console.ResetColor();//색 돌려놓기
                }
                availableMonsters.Add(monster); // 소환 가능 몬스터 리스트에 추가
            }
            if (!availableMonsters.Any()) // 소환 가능 몬스터가 없을 경우 미니언, 공허충, 대포미니언 소환
            {
                availableMonsters.AddRange(monsterTypes.Where(m => m.Name == "미니언" || m.Name == "공허충" || m.Name == "대포미니언"));
            }
            for (int i = 0; i < count; i++) // 랜덤 값만큼 랜덤 종류의 몬스터를 스폰시킴
            {
                Monster baseM = availableMonsters[rand.Next(availableMonsters.Count)];

                // 현재 층에 비례해 몬스터의 공격력과 체력을 업스케일링
                int scaledHp = baseM.Hp + (floor - 1) * 3;
                int scaledAtk = baseM.Atk + (floor - 1) / 2;
                list.Add(new Monster(baseM.Name, baseM.Level, scaledHp, scaledAtk, baseM.RewardGold));
            }
            return list;
        }
    }
}