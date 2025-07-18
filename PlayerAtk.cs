using System;
using TextRpg_Comment;

namespace TextRpgPlayerAtk
{
    // [플레이어의 공격 처리 전용 클래스]
    // 턴제 전투에서 플레이어가 몬스터 한 마리를 공격하는 로직을 담당. 
    // StartBattle에서 공격 선택 시 매번 객체를 만들어 Attack()을 호출함.

    internal class PlayerAtk
    {
        private Random _random = new Random(); // [랜덤객체] 치명타 판정 등에 사용

        // [플레이어가 단일 몬스터를 공격하는 함수]
        // StartBattle(전투루프)에서 플레이어가 공격을 선택했을 때 호출
        public void Attack(Character player, Monster target)
        {
            // [1] 플레이어의 기본 공격력 산출(버프/장비 등은 이미 캐릭터 Atk에 반영됨)
            int baseDamage = player.Atk;

            // [2] 치명타 여부 판정
            // [실행 흐름] 공격 때마다 15% 확률. Next(1,101)은 1~100 중 1~15이면 치명타, 16~100이면 일반공격
            bool isCriticalHit = _random.Next(1, 101) <= 15;

            if (isCriticalHit)
            {
                // [3-1] 치명타 성공 시: 데미지 1.6배. 소수점 버림(int 캐스팅)
                int criticalDamage = (int)(baseDamage * 1.6);
                target.TakeDamage(criticalDamage); // [실행] 몬스터의 체력을 치명타만큼 감소

                // [실행 흐름] 결과 및 치명타 메시지를 먼저 출력(당장 표시)
                Console.WriteLine($"{player.Name}의 맹렬한 공격! 치명타! {target.Name}에게 {criticalDamage}의 피해를 입혔습니다!");
            }
            else
            {
                // [3-2] 일반공격
                target.TakeDamage(baseDamage); // 몬스터 HP baseDamage만큼 감소

                // [실행 흐름] 일반 공격 메시지 출력
                Console.WriteLine($"{player.Name}가 {target.Name}에게 {baseDamage}의 피해를 입혔습니다.");
            }

            // [4] 항상 공격 후 몬스터의 남은 HP 표시(UI)
            Console.WriteLine($"{target.Name}의 남은 체력: {target.Hp}");

            // [5] 몬스터가 사망(HP <= 0) 시 "처치!" 및 보상 메시지 출력 분기
            // RewardGold>0이면 보상 메세지, 없으면 그냥 "처치!"
            if (target.Hp <= 0)
            {
                if (target.RewardGold <= 0)
                {
                    Console.WriteLine($"{target.Name} 처치!");
                }
                else
                {
                    Console.WriteLine($"{target.Name} 처치!{target.RewardGold}G 획득!");
                }
            }

            // [6] 전투 연출상 '다음 턴'을 위한 엔터 대기
            // [실제 흐름] 엔터를 입력하면 몬스터 턴 등 다음 코드로 이동
            Console.WriteLine("\nEnter를 눌러 다음 턴으로...");
            Console.ReadLine();
        }
    }
}