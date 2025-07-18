using System;
using System.Collections.Generic;
using TextRpg_Comment;

namespace TextRpg
{
    // [몬스터 턴 전체 처리용 클래스]
    // 전투에서 플레이어 공격이 끝나면, 살아있는 모든 몬스터가 차례대로 플레이어를 공격하는 과정을 담당
    internal class EnemyAttackPhase
    {
        // [몬스터 전체 공격 턴 실행 함수]
        // StartBattle(전투루프)에서 플레이어 턴 직후 호출.
        // currentMonsters : 현재 살아있는 몬스터 리스트
        // player : 공격받는 플레이어 객체(상태정보/방어력 등 모두 포함)
        public static void EnemyAtkPhase(Character player, List<Monster> currentMonsters)
        {
            // [실행 흐름] 
            // 1. 화면 클리어, 몬스터 턴 안내 메시지 출력
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            Console.WriteLine("몬스터의 턴입니다.\n");

            // [2. 각 몬스터별로 반복] (죽은 몬스터는 제외)
            foreach (Monster monster in currentMonsters)
            {
                // [조건: 몬스터가 죽었으면 패스]
                if (monster.Hp <= 0) continue;

                // [3. 플레이어 전체 방어력 계산]
                int totalDef = player.Def + player.ExtraDef; // 장비 포함 총 방어력
                double damageReductionPercent = Math.Min(totalDef, 80); // 방어력에 의한 피해 감소(최대 80% 제한)

                // [실제 데미지 계산]
                // 피해량 = 몬스터 공격력 × (100 - 피해감소비율) / 100
                double actualDamage = monster.Atk * (100 - damageReductionPercent) / 100.0;
                int finalDamage = (int)Math.Round(actualDamage); // 소수점은 반올림

                // [4. 음수 데미지 방지]
                if (finalDamage < 0) finalDamage = 0;

                // [5. 회피 판정] (ex: 10% 확률, Dodge에서 결정)
                if (Dodge.DodgeAtk())
                {
                    Dodge.ShowDodge(); // 회피 성공 메시지 출력
                    finalDamage = 0;   // 피해 무효(0)
                }

                // [6. 실제 플레이어 체력 적용 이전 기록]
                int originalPlayerHp = player.Hp;

                // [7. 데미지만큼 플레이어 체력 감소]
                player.TakeDamage(finalDamage);

                // [8. 이번 몬스터의 공격 결과 출력]
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격!");
                Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {finalDamage}]\n");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {originalPlayerHp}/{player.MaxHp} -> {player.Hp}/{player.MaxHp}\n");

                // [9. 사망 판정] 
                // 이번 공격으로 HP≤0이면 "플레이어 사망" 메시지 출력 후 바로 턴 종료(더 공격 X)
                if (player.Hp <= 0)
                {
                    Console.WriteLine($"{player.Name}이(가) 쓰러졌습니다!");
                    Console.Write(">> ");
                    Console.ReadLine();
                    return; // 함수 즉시 종료
                }

                // [10. 한 몬스터 공격 끝날 때마다 엔터 대기]
                // 실사용 시, 플레이어가 Enter 입력해야 다음 몬스터 공격이 진행됨
                Console.Write(">> ");
                Console.ReadLine();
            }
        }
    }
}