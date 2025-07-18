using System;
using System.Collections.Generic;
using TextRpg_Comment;

namespace TextRpg
{
    // 회피 제외 나머지 유형준 담당
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
            // 현재 소환된 모든 몬스터의 수만큼 반복함 = 모든 몬스터가 공격을 실행함
            {
                if (monster.Hp <= 0) continue; // 단, 죽은 몬스터는 스킵 처리
              
                int totalDef = player.Def + player.ExtraDef;
                // 플레이어의 전체 방어력 계산

                double damageReductionPercent = Math.Min(totalDef, 80);
                double actualDamage = monster.Atk * (100 - damageReductionPercent) / 100.0;
                // 몬스터의 공격력은 방어력에 비례해 최대 80%까지 감산

                int finalDamage = (int)Math.Round(actualDamage);
                // 감산한 공격력은 최종 데미지에 저장

                if (finalDamage < 0) finalDamage = 0;
                // 최종 데미지의 최소값은 0으로 설정 (음수로 내려가지 않음)

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