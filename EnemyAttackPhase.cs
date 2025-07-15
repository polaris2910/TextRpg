using System;
using System.Collections.Generic;
using System.Linq;
using TextRpg_Comment; // Character, Monster, Item, ItemType, Utils 클래스에 접근하기 위해 필요

namespace TextRpg
{
    internal class EnemyAttackPhase
    {
        
        
        public static void EnemyAtkPhase(Character player, List<Monster> currentMonsters)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            Console.WriteLine("몬스터의 턴입니다.\n");

            // 모든 몬스터에 대해 순서대로 공격을 시도함
            foreach (Monster monster in currentMonsters)
            {
                // 몬스터가 죽었을 경우 다음 몬스터로 넘어감 
                if (monster.Hp <= 0) continue;

                // 플레이어의 최종 방어력 계산 (기본 방어력 + 장착 아이템으로 인한 추가 방어력)
                int totalDef = player.Def + player.ExtraDef;

                // 몬스터 공격력에 플레이어 방어력을 적용하여 최종 데미지를 계산합니다.
                // 방어력 1당 1%의 데미지 감소 효과를 가지며, 최대 80%까지 감소시킬 수 있습니다.

                double damageReductionPercent = Math.Min(totalDef, 80); // 방어력은 최대 80% 감소 효과를 가짐

                double actualDamage = monster.Atk * (100 - damageReductionPercent) / 100.0;

                int finalDamage = (int)Math.Round(actualDamage); // 최종 데미지를 반올림하여 정수로 변경

                if (finalDamage < 0) finalDamage = 0; // 데미지가 음수가 되는 것을 방지

                int originalPlayerHp = player.Hp; // 플레이어의 공격 전 체력을 저장
                player.TakeDamage(finalDamage); // 플레이어에게 데미지를 적용

                // 몬스터의 공격 정보를 출력
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} 의 공격!");
                Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {finalDamage}]\n");

                // 플레이어의 체력 변화를 출력
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {originalPlayerHp}/{player.MaxHp} -> {player.Hp}/{player.MaxHp}\n"); // 현재 체력과 최대 체력 출력

                // 플레이어가 몬스터 공격으로 사망한 경우를 처리
                if (player.Hp <= 0)
                {
                    Console.WriteLine($"{player.Name}이(가) 쓰러졌습니다!");
                    Console.Write(">> ");
                    Console.ReadLine(); // 사용자 입력 대기
                    return; // 플레이어 사망 시 몬스터 턴을 종료하고 전투 종료
                }

                Console.Write(">> ");
                Console.ReadLine(); // 각 몬스터 공격 후 사용자 입력 대기
            }
        }
    }
}
