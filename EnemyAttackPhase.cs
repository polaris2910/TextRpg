using System;
using System.Collections.Generic;
using TextRpg_Comment;

namespace TextRpg
{
    // 몬스터 턴 전체 처리 담당 클래스
    internal class EnemyAttackPhase
    {
        // 몬스터 전체가 차례로 플레이어에게 공격
        public static void EnemyAtkPhase(Character player, List<Monster> currentMonsters)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            Console.WriteLine("몬스터의 턴입니다.\n");

            foreach (Monster monster in currentMonsters)
            {
                if (monster.Hp <= 0) continue; // 죽은 몬스터는 스킵

                // 플레이어 총 방어력(장비 포함)
                int totalDef = player.Def + player.ExtraDef;
                double damageReductionPercent = Math.Min(totalDef, 80); // 최대 80%까지 감산
                double actualDamage = monster.Atk * (100 - damageReductionPercent) / 100.0;
                int finalDamage = (int)Math.Round(actualDamage);
                if (finalDamage < 0) finalDamage = 0;

                // 회피 판정 (성공시 데미지 0)
                if (Dodge.DodgeAtk())
                {
                    Dodge.ShowDodge();
                    finalDamage = 0;
                }

                int originalPlayerHp = player.Hp;
                player.TakeDamage(finalDamage);

                // 공격 결과 출력
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격!");
                Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {finalDamage}]\n");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {originalPlayerHp}/{player.MaxHp} -> {player.Hp}/{player.MaxHp}\n");

                // 플레이어 사망 시 안내 및 턴 종료
                if (player.Hp <= 0)
                {
                    Console.WriteLine($"{player.Name}이(가) 쓰러졌습니다!");
                    Console.Write(">> ");
                    Console.ReadLine();
                    return;
                }

                // 다음 몬스터 전까지 엔터 대기
                Console.Write(">> ");
                Console.ReadLine();
            }
        }
    }
}