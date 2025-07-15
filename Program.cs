using System;
using System.Collections.Generic;
using System.Linq; // LINQ 확장 메서드 (예: .Where(), .All()) 사용을 위해 필요합니다.
using TextRpgPlayerAtk; // PlayerAtk 클래스 접근을 위해 필요합니다.
using TextRpg; // EnemyAttackPhase 접근을 위해 필요합니다.

namespace TextRpg_Comment
{
    class Program
    {
        private static Character player;
        private static Item[] itemDb;
        private static Dungeon[] dungeonDb;
        private static List<Monster> monsterTypes;

        static int currentFloor = 1;
        static int checkpointFloor = 1; // 5의 배수 층을 체크포인트로 저장

        static void Main(string[] args)
        {
            SetData(); // 게임 데이터 초기화
            DisplayMainUI(); // 메인 UI 표시
        }

        // 게임에 필요한 기본 데이터를 설정합니다.
        static void SetData()
        {
            player = new Character(1, "Chad", "전사", 10, 5, 100, 10000); // 플레이어 캐릭터 생성

            // 아이템 데이터베이스 초기화
            itemDb = new Item[]
            {
                new Item("수련자의 갑옷", ItemType.Armor, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
                new Item("무쇠갑옷", ItemType.Armor, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
                new Item("스파르타의 갑옷", ItemType.Armor, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
                new Item("낡은 검", ItemType.Weapon, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
                new Item("청동 도끼", ItemType.Weapon, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
                new Item("스파르타의 창", ItemType.Weapon, 7,"스파르타의 전사들이 사용했다는 전설의 창입니다. ",2500)
            };

            // 던전 데이터베이스 초기화
            dungeonDb = new Dungeon[]
            {
                new Dungeon("쉬운 던전", 5, 1000),
                new Dungeon("일반 던전", 11, 1700),
                new Dungeon("어려운 던전", 17, 2500)
            };

            // 몬스터 타입 목록 초기화
            monsterTypes = new List<Monster>()
            {
                new Monster ("미니언", 2, 15, 5),
                new Monster ("공허충", 3, 10, 9),
                new Monster ("대포미니언", 5, 25, 8)
            };
        }

        // 메인 UI를 표시하고 사용자 입력을 처리합니다.
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
            Console.WriteLine("5. 휴식하기"); // 휴식하기 옵션 추가
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 5); // 입력 범위 1-5로 변경

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
                case 5: // 휴식하기 선택 시
                    DisplayRestUI();
                    break;
            }
        }

        // 캐릭터 상태 정보를 표시합니다.
        static void DisplayStatUI()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            player.DisplayCharacterInfo(); // 캐릭터 정보 출력
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 0); // 0만 유효한 입력
            if (result == 0)
                DisplayMainUI(); // 메인 UI로 돌아가기
        }

        // 인벤토리 UI를 표시하고 아이템 관리 옵션을 제공합니다.
        static void DisplayInventoryUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            player.DisplayInventory(false); // 인벤토리 아이템 목록 출력 (번호 없이)
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI(); // 메인 UI로 돌아가기
                    break;
                case 1:
                    DisplayEquipUI(); // 장착 관리 UI로 이동
                    break;
            }
        }

        // 아이템 장착/해제 관리 UI를 표시합니다.
        static void DisplayEquipUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            player.DisplayInventory(true); // 인벤토리 아이템 목록 출력 (번호 포함)
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, player.InventoryCount); // 입력 범위 0부터 인벤토리 아이템 개수까지

            switch (result)
            {
                case 0:
                    DisplayInventoryUI(); // 인벤토리 UI로 돌아가기
                    break;
                default:
                    int itemIdx = result - 1; // 선택된 아이템의 인덱스 계산
                    Item targetItem = player.GetInventoryItem(itemIdx); // 선택된 아이템 가져오기
                    if (targetItem != null)
                        player.EquipItem(targetItem); // 아이템 장착/해제
                    DisplayEquipUI(); // 장착 관리 UI 새로고침
                    break;
            }
        }

        // 상점 UI를 표시하고 아이템 구매/판매 옵션을 제공합니다.
        static void DisplayShopUI()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G"); // 현재 보유 골드 표시
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < itemDb.Length; i++)
            {
                Item curItem = itemDb[i];
                // 플레이어가 이미 구매한 아이템인지에 따라 가격 표시를 변경
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
                    DisplayMainUI(); // 메인 UI로 돌아가기
                    break;
                case 1:
                    DisplayBuyUI(); // 아이템 구매 UI로 이동
                    break;
                case 2:
                    DisplaySellUI(); // 아이템 판매 UI로 이동
                    break;
            }
        }

        // 아이템 구매 UI를 표시하고 구매 로직을 처리합니다.
        static void DisplayBuyUI()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G"); // 현재 보유 골드 표시
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < itemDb.Length; i++)
            {
                Item curItem = itemDb[i];
                // 플레이어가 이미 구매한 아이템인지에 따라 가격 표시를 변경
                string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
                Console.WriteLine($"- {i + 1} {curItem.ItemInfoText()}  |  {displayPrice}"); // 아이템 번호 포함하여 출력
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, itemDb.Length); // 입력 범위 0부터 아이템 데이터베이스 길이까지

            switch (result)
            {
                case 0:
                    DisplayShopUI(); // 상점 UI로 돌아가기
                    break;
                default:
                    int itemIdx = result - 1; // 선택된 아이템의 인덱스 계산
                    Item targetItem = itemDb[itemIdx]; // 선택된 아이템 가져오기

                    if (player.HasItem(targetItem))
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }
                    else if (player.Gold >= targetItem.Price)
                    {
                        Console.WriteLine("구매를 완료했습니다.");
                        player.BuyItem(targetItem); // 아이템 구매 처리
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }
                    DisplayBuyUI(); // 아이템 구매 UI 새로고침
                    break;
            }
        }

        // 아이템 판매 UI를 표시하고 판매 로직을 처리합니다.
        static void DisplaySellUI()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G"); // 현재 보유 골드 표시
            Console.WriteLine();

            // 판매할 아이템이 없는 경우
            if (player.InventoryCount == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.");
                Console.WriteLine("Enter를 누르면 상점으로 돌아갑니다.");
                Console.ReadLine();
                DisplayShopUI(); // 상점 UI로 돌아가기
                return;
            }

            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < player.InventoryCount; i++)
            {
                Item item = player.GetInventoryItem(i);
                int sellPrice = (int)(item.Price * 0.85); // 판매 가격 (원가의 85%)
                string equipped = player.IsEquipped(item) ? "[E]" : ""; // 장착 여부 표시
                Console.WriteLine($"- {i + 1} {equipped}{item.ItemInfoText()}  |  {sellPrice} G");
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, player.InventoryCount);

            if (result == 0)
            {
                DisplayShopUI(); // 상점 UI로 돌아가기
                return;
            }

            int idx = result - 1; // 선택된 아이템의 인덱스 계산
            Item sellItem = player.GetInventoryItem(idx); // 판매할 아이템 가져오기

            // 판매할 아이템이 장착 중이면 먼저 해제합니다.
            if (player.IsEquipped(sellItem))
            {
                player.EquipItem(sellItem); // 아이템 해제
            }

            int sellPrice2 = (int)(sellItem.Price * 0.85); // 판매 가격 다시 계산
            player.AddGold(sellPrice2); // 골드 획득
            player.RemoveItem(sellItem); // 인벤토리에서 아이템 제거

            Console.WriteLine($"{sellItem.Name}을(를) 판매했습니다! (+{sellPrice2} G)");
            Console.WriteLine("Enter를 누르면 계속합니다.");
            Console.ReadLine();

            DisplaySellUI(); // 아이템 판매 UI 새로고침
        }

        // 휴식하기 UI를 표시하고 체력 회복 로직을 처리합니다.
        static void DisplayRestUI()
        {
            const int restPrice = 500; // 휴식 비용

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
                    DisplayMainUI(); // 메인 UI로 돌아가기
                    break;
                case 1:
                    if (player.Gold >= restPrice)
                    {
                        player.SpendGold(restPrice); // 골드 소비
                        player.Heal(); // 체력 회복
                        Console.WriteLine("휴식을 완료했습니다. 체력이 모두 회복되었습니다!");
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                    }
                    Console.WriteLine("Enter 를 눌러주세요.");
                    Console.ReadLine();
                    DisplayMainUI(); // 메인 UI로 돌아가기
                    break;
            }
        }

        // 던전 입장 방식을 선택하는 메뉴를 표시합니다.
        static void DungeonMenuUI()
        {
            Console.Clear();
            Console.WriteLine("던전 입장 방식을 선택하세요:");
            Console.WriteLine("1. 1층부터 시작");
            if (checkpointFloor >= 5) // 체크포인트가 5층 이상일 경우 옵션 표시
                Console.WriteLine($"2. 체크포인트({checkpointFloor}층)부터 시작");
            Console.Write(">> ");
            int maxOption = checkpointFloor >= 5 ? 2 : 1; // 최대 선택 옵션 결정
            int input = CheckInput(1, maxOption);
            currentFloor = (input == 1) ? 1 : checkpointFloor; // 선택에 따라 시작 층 설정

            EnterDungeonUI(); // 던전 입장 UI로 이동
        }

        // 던전 탐험 로직을 처리합니다.
        static void EnterDungeonUI()
        {
            while (true) // 던전 탐험 루프
            {
                Console.Clear();
                Console.WriteLine($"[{currentFloor}층] 던전을 시작합니다!");

                List<Monster> monsters = CreateFloorMonsters(currentFloor); // 현재 층의 몬스터 생성

                Console.WriteLine("등장 몬스터:");
                foreach (var m in monsters)
                    Console.WriteLine(m.Info()); // 몬스터 정보 출력
                Console.WriteLine();

                player.DisplayCharacterInfo(); // 플레이어 정보 출력
                Console.WriteLine();

                Console.WriteLine("1. 전투 시작   2. 던전 중단");
                Console.Write(">> ");
                int act = CheckInput(1, 2);

                if (act == 2)
                {
                    Console.WriteLine("던전 탐험을 중단하고 마을로 돌아갑니다.");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    break; // 던전 탐험 루프 종료
                }

                int beforeHp = player.Hp; // 전투 전 플레이어 체력 저장
                int beforeGold = player.Gold; // 전투 전 플레이어 골드 저장

                // 전투 시작!
                bool battleResult = StartBattle(monsters); // 전투 시작 및 결과 받기

                if (battleResult) // 전투 승리 시
                {
                    int afterHp = player.Hp; // 전투 후 플레이어 체력
                    int afterGold = player.Gold; // 전투 후 플레이어 골드
                    DisplayDungeonResult($"[{currentFloor}층] 던전", beforeHp, afterHp, beforeGold, afterGold); // 던전 클리어 결과 표시

                    player.AddDungeonClear(); // 던전 클리어 횟수 증가 및 레벨업 확인
                    currentFloor++; // 다음 층으로 이동

                    if ((currentFloor - 1) % 5 == 0) // 5층마다 체크포인트 설정
                    {
                        checkpointFloor = currentFloor - 1;
                        Console.WriteLine($"\n축하합니다! {checkpointFloor}층 체크포인트에 도달했습니다!");
                        Console.WriteLine("1. 마을로 귀환   2. 계속 탐험");
                        Console.Write(">> ");
                        int sel = CheckInput(1, 2);
                        if (sel == 1)
                        {
                            Console.WriteLine("마을로 돌아갑니다!");
                            Console.WriteLine("Enter를 눌러 계속...");
                            Console.ReadLine();
                            break; // 던전 탐험 루프 종료
                        }
                    }
                }
                else // 전투 패배 시 (플레이어 사망)
                {
                    int afterHp = player.Hp; // 전투 후 플레이어 체력
                    int afterGold = player.Gold; // 전투 후 플레이어 골드 (변화 없음)
                    DisplayDungeonResultFail($"[{currentFloor}층] 던전", beforeHp, afterHp, beforeGold, afterGold); // 던전 실패 결과 표시

                    player.HalveHp(); // 체력 절반으로 감소
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    break; // 던전 탐험 루프 종료
                }
            }
            DisplayMainUI(); // 던전 탐험 종료 후 메인 UI로 돌아가기
        }

        // 던전 클리어 결과를 표시합니다.
        static void DisplayDungeonResult(string dungeonName, int beforeHp, int afterHp, int beforeGold, int afterGold)
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
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 0);
            if (result == 0)
                DisplayMainUI();
        }

        // 던전 실패 결과를 표시합니다.
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


        // 전투 로직을 처리합니다. (턴 기반)
        /// <returns>전투 승리 시 true, 패배 시 false를 반환합니다.</returns>
        static bool StartBattle(List<Monster> monstersInBattle)
        {
            while (true) // 전투 턴 루프
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");

                // 살아있는 몬스터 정보 표시
                Console.WriteLine("[몬스터 정보]");
                List<Monster> livingMonsters = monstersInBattle.Where(m => m.Hp > 0).ToList();
                if (!livingMonsters.Any()) // 모든 몬스터가 죽었으면 전투 승리
                {
                    Console.WriteLine("\n남아있는 몬스터가 없습니다.");
                    Console.WriteLine("\n모든 몬스터를 처치했습니다!");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    return true; // 전투 승리
                }

                foreach (var monster in livingMonsters)
                {
                    Console.WriteLine(monster.Info());
                }

                Console.WriteLine("\n[내 정보]");
                player.DisplayCharacterInfo(); // 플레이어 정보 표시

                if (player.Hp <= 0) // 플레이어 사망 시 전투 패배
                {
                    Console.WriteLine("\n플레이어가 쓰러졌습니다!");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                    return false; // 전투 패배
                }

                Console.WriteLine("\n1. 공격");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Clear();
                    Console.WriteLine("\n[공격할 몬스터를 선택하세요]");

                    // 살아있는 몬스터 목록을 다시 가져와서 선택지를 제공합니다.
                    livingMonsters = monstersInBattle.Where(m => m.Hp > 0).ToList();
                    for (int i = 0; i < livingMonsters.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {livingMonsters[i].Info()}");
                    }

                    int selected = CheckInput(1, livingMonsters.Count);
                    Monster target = livingMonsters[selected - 1]; // 선택된 몬스터

                    PlayerAtk playerAttackHandler = new PlayerAtk();
                    playerAttackHandler.Attack(player, target); // 플레이어 공격 실행

                    // 플레이어 공격 후, 모든 몬스터가 죽었는지 다시 확인
                    if (monstersInBattle.All(m => m.Hp <= 0))
                    {
                        // 모든 몬스터가 죽었으므로 다음 턴으로 넘어가지 않고 바로 전투 승리 처리
                        continue; // while 루프의 시작으로 돌아가 승리 조건 확인
                    }

                    // 플레이어가 살아있고 몬스터가 남아있으면, 몬스터의 턴
                    if (player.Hp > 0)
                    {
                        EnemyAttackPhase.EnemyAtkPhase(player, monstersInBattle); // 몬스터 공격 실행
                    }
                    else
                    {
                        // 몬스터 공격 페이즈에서 플레이어가 사망했을 경우
                        // StartBattle 루프의 다음 반복에서 player.Hp <= 0 조건에 의해 전투 패배 처리됨
                        continue; // while 루프의 시작으로 돌아가 패배 조건 확인
                    }
                }
                else
                {
                    Console.WriteLine("\n잘못된 입력입니다");
                    Console.WriteLine("Enter를 눌러 계속...");
                    Console.ReadLine();
                }
            }
        }

        // 현재 층에 맞는 몬스터들을 생성하여 리스트로 반환합니다.
        static List<Monster> CreateFloorMonsters(int floor)
        {
            Random rand = new Random();
            int count = rand.Next(1, 5); // 1~4 마리 무작위 생성

            List<Monster> monsters = new List<Monster>();
            for (int i = 0; i < count; i++)
            {
                int index = rand.Next(monsterTypes.Count); // 무작위 몬스터 타입 선택
                Monster baseM = monsterTypes[index];

                // 층수 기반으로 몬스터의 체력과 공격력을 보정합니다.
                int scaledHp = baseM.Hp + (floor - 1) * 3;
                int scaledAtk = baseM.Atk + (floor - 1) / 2;

                monsters.Add(new Monster(baseM.Name, baseM.Level, scaledHp, scaledAtk));
            }
            return monsters;
        }

        // 사용자 입력을 검증하여 지정된 범위 내의 정수만 받도록 합니다.
        static int CheckInput(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out result); // 입력이 숫자인지 확인

                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result; // 유효한 범위 내의 숫자면 반환
                }
                Console.WriteLine("잘못된 입력입니다!!!!"); // 잘못된 입력 시 메시지 출력
            }
        }
    }
}
