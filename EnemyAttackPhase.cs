using System;
using System.Collections.Generic;
using TextRpg_Comment;

namespace TextRpg
{
    // 몬스터 턴 전체 처리 담당 클래스
    internal class EnemyAttackPhase
    {
        // 몬스터 전체가 차례대로 플레이어를 공격
        public static void EnemyAtkPhase(Character player, List<Monster> currentMonsters)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            Console.WriteLine("몬스터의 턴입니다.\n");

            foreach (Monster monster in currentMonsters)
            {
                if (monster.Hp <= 0) continue;    // 살아있는 몬스터만 공격 진행

                int totalDef = player.Def + player.ExtraDef; // 전체 방어력
                double damageReductionPercent = Math.Min(totalDef, 80); // 방어력 80% 제한
                double actualDamage = monster.Atk * (100 - damageReductionPercent) / 100.0;
                int finalDamage = (int)Math.Round(actualDamage);         // 반올림

                if (finalDamage < 0) finalDamage = 0; // 음수 방지

                int originalPlayerHp = player.Hp;
                player.TakeDamage(finalDamage);       // 플레이어에 피해 적용

                Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격!");
                Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {finalDamage}]\n");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {originalPlayerHp}/{player.MaxHp} -> {player.Hp}/{player.MaxHp}\n");

                if (player.Hp <= 0)
                {
                    Console.WriteLine($"{player.Name}이(가) 쓰러졌습니다!");
                    Console.Write(">> ");
                    Console.ReadLine();
                    return; // 사망 시 턴 종료
                }

                Console.Write(">> ");
                Console.ReadLine(); // 한 몬스터 공격 시마다 대기
            }
        }
    }
}