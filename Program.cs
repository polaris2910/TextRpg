using System;
using System.Collections.Generic;
using System.Linq;
using TextRpg;
using TextRpgPlayerAtk;
using TextRpg_Comment;

namespace TextRpg_Comment
{
    class Program
    {
        // [전역 변수] 플레이어 정보, 게임 데이터 저장
        private static Character player;
        private static Item[] itemDb;
        private static Dungeon[] dungeonDb;
        private static List<Monster> monsterTypes;
        private static Random random = new Random();

        // [현재 층수/체크포인트 저장] 던전 진행 상황 관리용
        static int currentFloor = 1;
        static int checkpointFloor = 1;

        // [프로그램 진입점]
        // 프로그램 실행 시 가장 먼저 호출되어, 전체 게임 진행의 시작점이 됨
        static void Main(string[] args)
        {
            ShowIntro();                // 인트로(시작 화면)를 먼저 보여주고, 엔터 입력 대기
            string name = InputName();  // 이름 입력(유효성 체크, 빈값 시 반복 입력)
            string job = SelectJob();   // 직업 메뉴 출력, 선택 반복
            SetData(name, job);         // 캐릭터 생성 및 게임 데이터(아이템, 몬스터 등) 초기화

            Console.Clear();
            Console.WriteLine($"모험가 {name}({job})님의 여정이 지금 시작됩니다!");
            Console.WriteLine("엔터를 눌러 계속합니다...");
            Console.ReadLine();

            DisplayMainUI();            // 마을에서의 메인 UI를 반복 진입(게임의 핵심 루프)
        }

        // [인트로 화면 출력]
        // Main에서 가장 먼저 호출. 사용자에게 게임 세계관 소개 후 엔터 입력을 대기함
        static void ShowIntro()
        {
            Console.Clear();
            Console.WriteLine("===========================================");
            Console.WriteLine("   어둠이 드리운 대륙, 끝나지 않은 전쟁...");
            Console.WriteLine("   오직 한 사람만이 세계를 구할 수 있다.");
            Console.WriteLine("   전설의 용사가 되어 여정을 시작하세요.");
            Console.WriteLine("===========================================");
            Console.WriteLine("\n엔터를 눌러 계속...");
            Console.ReadLine(); // 입력이 될 때까지 대기, 이후 다음 코드로 이동
        }

        // [이름 입력 함수]
        // 사용자가 이름을 올바르게 입력할 때까지(입력값 체크) 반복 실행됨
        static string InputName()
        {
            Console.Clear();
            Console.Write("당신의 이름을 입력해주세요: ");
            string name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name)) // 빈 값이면 계속 재입력
            {
                Console.Write("이름은 필수입니다. 다시 입력해주세요: ");
                name = Console.ReadLine();
            }
            return name;
        }

        // [직업 선택 함수]
        // 사용자가 잘못된 값을 입력하면 입력 받을 때까지 반복, 올바른 번호가 입력되면 해당 직업 문자열 반환
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
            int choice = CheckInput(1, 5); // 1~5 사이의 값이 입력될 때까지 반복

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

        // [게임 전체 데이터 초기화]
        // Main에서 캐릭터 정보와 게임 데이터를 전부 세팅함
        static void SetData(string name, string job)
        {
            player = new Character(name, job);

            // [아이템 데이터베이스 초기화]
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

            // [던전 데이터베이스 초기화]
            dungeonDb = new Dungeon[]
            {
                new Dungeon("쉬운 던전", 5, 1000),
                new Dungeon("일반 던전", 11, 1700),
                new Dungeon("어려운 던전", 17, 2500)
            };

            // [몬스터 데이터베이스 초기화]
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

        // [메뉴에서 숫자 입력 보정]
        // 입력값이 올바른 범위(예: 1~5)인지 체크, 아닐 시 반복해서 받음
        static int CheckInput(int min, int max)
        {
            int result;
            while (true) // 원하는 범위 값을 받을 때까지 무한 반복
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out result) && result >= min && result <= max)
                    return result; // 원하는 값이면 그대로 함수 종료

                Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
            }
        }

        // [마을 메인 UI]
        // 프로그램의 "핵심 루프". Main에서 처음 호출되며, 종료가 선택되기 전까지 반복 실행됨
        static void DisplayMainUI()
        {
            while (true) // 유저가 종료 선택(Environment.Exit)할 때까지 반복
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

                // 유저 선택에 따라 기능별로 하위 함수 실행(분기)
                switch (input)
                {
                    case 1: DisplayStatUI(); break;
                    case 2: DisplayInventoryUI(); break;
                    case 3: DisplayShopUI(); break;
                    case 4: DungeonMenuUI(); break;
                    case 5: DisplayRestUI(); break;
                    case 0:
                        Console.WriteLine("게임을 종료합니다.");
                        Environment.Exit(0); // 프로그램 완전 종료
                        break;
                }
            }
        }

        // [상태 보기 UI]
        // "상태 보기" 메뉴 선택 시 호출, 캐릭터 정보 출력
        static void DisplayStatUI()
        {
            Console.Clear();
            Console.WriteLine("==== 캐릭터 상태 ====");
            player.DisplayCharacterInfo(); // 플레이어의 모든 주요 정보를 보여줌
            Console.WriteLine("\n엔터를 누르세요...");
            Console.ReadLine(); // 엔터 입력 시 다시 MainUI로 복귀
        }

        // [인벤토리 UI]
        // "인벤토리" 메뉴 선택 시 호출, 인벤토리 정보와 장착 관리로 이동 분기
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
                DisplayEquipUI(); // 1 선택 시 장착 관리 진입
            // 나가기는 함수 끝나면서 MainUI로 복귀
        }

        // [장착(Equip) UI]
        // 인벤토리에서 "장착 관리" 선택 시 진입, 장비 장착/해제 관리
        static void DisplayEquipUI()
        {
            Console.Clear();
            Console.WriteLine("==== 장착 관리 ====");
            player.DisplayInventory(true); // 인덱스와 함께 모든 아이템 출력
            Console.WriteLine("\n0. 나가기");
            Console.Write(">> ");
            int input = CheckInput(0, player.InventoryCount);
            if (input == 0) return; // 0 입력시 장착UI 종료, 인벤토리로 복귀

            Item item = player.GetInventoryItem(input - 1);
            if (item != null && item.IsEquippable)
                player.EquipItem(item); // 장비류인 경우 장착/해제 처리
            else if (item != null)
            {
                Console.WriteLine("포션은 장착할 수 없습니다.");
                Console.ReadLine();
            }

            DisplayEquipUI(); // 본 화면을 재진입(장착 후 변화 확인)
        }

        // [상점 UI]
        // 메뉴에서 "상점"을 선택할 때마다 호출, 구매/판매 메뉴 진입
        static void DisplayShopUI()
        {
            Console.Clear();
            Console.WriteLine("==== 상점 ====");
            Console.WriteLine($"보유 골드: {player.Gold} G\n");

            // 모든 아이템 정보와 구매 여부 표시
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

            if (sel == 1) DisplayBuyUI(); // 구매 진입
            else if (sel == 2) DisplaySellUI(); // 판매 진입
        }

        // [아이템 구매 UI]
        // 구매 메뉴 진입 시 실행. 각 아이템별 구매 처리 및 골드 차감, 소지품 추가
        static void DisplayBuyUI()
        {
            Console.Clear();
            Console.WriteLine($"==== 아이템 구매 ====\n보유 골드: {player.Gold} G\n");

            // 아이템별로 구매 가능 정보 표시
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
            DisplayBuyUI(); // 본 구매 화면 재진입(반복 구매 용)
        }

        // [아이템 판매 UI]
        // 인벤토리의 아이템을 판매할 수 있으며, 판매 시 골드 증가/아이템 삭제됨
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
                player.EquipItem(sellItem); // 장비 해제(판매 전)
            int sellGold = (int)(sellItem.Price * 0.85);
            player.AddGold(sellGold);
            player.RemoveItem(sellItem);

            Console.WriteLine($"{sellItem.Name}을 판매 완료 (+{sellGold} G)");
            Console.WriteLine("엔터를 누르세요...");
            Console.ReadLine();
            DisplaySellUI(); // 반복 판매를 위해 화면 재진입
        }

        // [휴식 UI]
        // "휴식하기" 메뉴 선택 시, 골드를 소모하여 체력 회복 가능
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
                    player.Heal(); // 체력 완전 회복
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

        // [던전 입장 메뉴]
        // "던전 입장" 선택 시, 1층부터/체크포인트 부터 중 선택 메뉴
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
            EnterDungeonUI(); // 던전(연속 층 루프)로 진입
        }

        // [던전 연속 도전]
        // 한 번 입장시 계속 층을 오르며, 매 층마다 전투, 결과, 반복/마을 복귀 여부를 처리함
        static void EnterDungeonUI()
        {
            while (true) // 던전에서 계속 탐험 혹은 마을 복귀까지
            {
                Console.Clear();
                Console.WriteLine($"== {currentFloor}층 도전 ==");

                List<Monster> monsters = CreateFloorMonsters(currentFloor); // 해당 층 몬스터 생성(랜덤)
                Console.WriteLine("\n몬스터 등장!");
                foreach (var m in monsters)
                    Console.WriteLine(m.Info());

                Console.WriteLine("\n1. 전투 시작");
                Console.WriteLine("2. 마을로 돌아가기");
                Console.Write(">> ");
                int input = CheckInput(1, 2);
                if (input == 2) return; // 마을로 복귀, 종료

                int beforeHp = player.Hp;
                int beforeGold = player.Gold;

                bool win = StartBattle(monsters); // 실제 전투(최대 반복)

                if (win)
                {
                    int afterHp = player.Hp;
                    int goldRewardForFloor = currentFloor * 500;
                    player.AddGold(goldRewardForFloor);

                    int currentGoldAfterReward = player.Gold;

                    // 결과 및 다음 층/복귀 분기
                    bool continueNext = DisplayDungeonResult(
                        $"[{currentFloor}층] 던전", beforeHp, afterHp, beforeGold, currentGoldAfterReward, true);
                    if (continueNext)
                        currentFloor++; // 다음 층 진행
                    else
                        break; // 마을 복귀
                }
                else // 전투 패배
                {
                    player.HalveHp(); // 체력 1/2 차감
                    Console.WriteLine("전투 실패! 체력이 절반으로 줄어듭니다...");
                    Console.ReadLine();
                    DisplayDungeonResult(
                        $"[{currentFloor}층] 던전", beforeHp, player.Hp, beforeGold, player.Gold, false);
                    break;
                }
            }
        }

        // [전투 루프]
        // 한 층의 전투는 플레이어와 몬스터가 번갈아 가며 반복. 모두 죽거나 플레이어가 죽으면 종료
        static bool StartBattle(List<Monster> monsters)
        {
            while (true)
            {
                Console.Clear();
                var living = monsters.Where(m => m.Hp > 0).ToList();
                if (!living.Any()) return true; // 몬스터 전멸=승리
                if (player.Hp <= 0) return false; // 플레이어 사망=패배

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

                    // 공격 후, 살아 있으면 몬스터 전체 턴 실행
                    if (player.Hp > 0)
                        EnemyAttackPhase.EnemyAtkPhase(player, monsters);
                }
                else if (input == "2")
                {
                    var potions = player.TakeInventory().Where(i => i.Type == ItemType.potion).ToList();
                    //플레이어 인벤토리 리스트에서 타입이 포션인 구성요소가 리스트에 있는지 확인하는 코드
                    if (!potions.Any())//포션 타입이 없을 경우
                    {
                        Console.WriteLine("사용 가능한 포션이 없습니다.");
                        Console.ReadLine();
                        continue;
                    }

                    Console.WriteLine("사용할 포션을 선택하세요:");
                    for (int i = 0; i < potions.Count; i++)
                        Console.WriteLine($"{i + 1}. {potions[i].ItemInfoText()}");

                    int sel = CheckInput(1, potions.Count);//선택한 포션 번호 확인
                    var potion = potions[sel - 1];//코드 번호를 맞추기 위해 -1
                    player.SetHp(player.Hp + potion.Value);//플레이어 체력 증가
                    player.RemoveItem(potion);//포션 제거
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

        // [던전 결과/보상 출력 및 분기]
        // 전투가 끝난 뒤, 결과/경험치/아이템 출력, 다음 선택(진행 or 마을로 귀환)
        static bool DisplayDungeonResult(
            string dungeonName, int beforeHp, int afterHp, int beforeGold, int afterGold, bool isWin)
        {
            Console.Clear();
            Console.WriteLine($"{dungeonName} {(isWin ? "클리어!" : "실패!")}");

            // 체력/골드 변화 출력
            Console.WriteLine($"체력: {beforeHp} → {afterHp}");
            Console.WriteLine($"골드: {beforeGold} → {afterGold}");

            if (isWin)
            {
                int goldGained = afterGold - beforeGold;
                Console.WriteLine($"획득 골드: +{goldGained} G");

                // 층마다 1~3포션을 랜덤 드랍 (중복 획득 가능)
                Random rand = new Random();
                int potionDropCount = rand.Next(1, 4);
                if (potionDropCount > 0)
                {
                    Console.WriteLine("획득한 아이템:");
                    for (int i = 0; i < potionDropCount; i++)
                    {
                        var potionsInDb = itemDb.Where(item => item.Type == ItemType.potion).ToList();
                        if (potionsInDb.Any())
                        {
                            Item droppedPotion = potionsInDb[rand.Next(potionsInDb.Count)];
                            player.AddItemToInventory(droppedPotion);
                            Console.WriteLine($"- {droppedPotion.Name}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("획득한 아이템: 없음");
                }

                // 경험치 보상(층수에 비례해 증가)
                int expReward = 100 + (currentFloor - 1) * 20;
                player.GainExp(expReward);
                Console.WriteLine($"경험치 +{expReward}");

                // 5층마다 체크포인트 갱신
                if ((currentFloor % 5) == 0)
                {
                    checkpointFloor = currentFloor;
                    Console.WriteLine($"체크포인트에 도달했습니다! (층: {checkpointFloor})");
                }

                Console.WriteLine("\n1. 다음 층");
                Console.WriteLine("0. 마을로 귀환\n>> ");
                int sel = CheckInput(0, 1);
                return sel == 1; // true면 다음 층, false면 마을 복귀
            }
            else // 전투 패배 시
            {
                Console.WriteLine("골드나 아이템은 드랍되지 않습니다.");
                Console.WriteLine("\n0. 마을로 귀환\n>> ");
                int sel = CheckInput(0, 0);
                return false; // 항상 마을로 복귀 (false)
            }
        }

        // [현재 층의 몬스터 무작위 생성]
        // 난이도별 출현 제한 및 랜덤 스폰(2~4마리)
        static List<Monster> CreateFloorMonsters(int floor)
        {
            Random rand = new Random();
            List<Monster> list = new List<Monster>();
            int count = rand.Next(2, 5);

            List<Monster> availableMonsters = new List<Monster>();

            foreach (var monster in monsterTypes)
            {
                if (monster.Name == "전령" && floor < 6)
                    continue;
                if (monster.Name == "파이어 드래곤" && floor < 11)
                    continue;
                if (monster.Name == "황금 고블린")
                {
                    if (random.Next(0, 100) != 0)//미리 입력한 렌덤 함수작동해 1/100 계산
                        continue; // 100~0중 0이 아닐경우 다시-> 100분의 1확률로 고블린 등장
                    Console.ForegroundColor = ConsoleColor.Yellow;//글씨 색 변경
                    Console.WriteLine("황★금★고★블★린");//continue가 작동하지 않아 황금고블린이 등장시 문구 출력
                }
                availableMonsters.Add(monster);
            }

            // 등장 가능 몬스터가 없을 경우, 기본 몬스터만이라도 출현
            if (!availableMonsters.Any())
            {
                availableMonsters.AddRange(monsterTypes.Where(m => m.Name == "미니언" || m.Name == "공허충" || m.Name == "대포미니언"));
            }

            for (int i = 0; i < count; i++)
            {
                Monster baseM = availableMonsters[rand.Next(availableMonsters.Count)];
                int scaledHp = baseM.Hp + (floor - 1) * 3;
                int scaledAtk = baseM.Atk + (floor - 1) / 2;
                list.Add(new Monster(baseM.Name, baseM.Level, scaledHp, scaledAtk, baseM.RewardGold));
            }
            return list;
        }
    }
}