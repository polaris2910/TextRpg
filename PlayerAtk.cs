using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRpg; // EnemyAttackPhase 접근을 위해 필요
using TextRpg_Comment; // Character, Monster 클래스 접근을 위해 필요

namespace TextRpgPlayerAtk
{
    internal class PlayerAtk
    {
        
        public void Attack(Character player, Monster target) // battleMonsters 매개변수 제거
        {
            Console.Clear();
            int attackPower = player.Atk + player.ExtraAtk; // 플레이어의 최종 공격력 계산
            int damage = attackPower;
            if (damage < 0) damage = 0; // 데미지가 음수가 되지 않도록 방지

            target.Hp = Math.Max(target.Hp - damage, 0); // 몬스터의 체력 감소 (0 미만으로 내려가지 않도록)



            // 공격 결과 메시지 출력
            Console.WriteLine($"{player.Name}이(가) {target.Name}을(를) 공격했습니다! (피해량: {damage})");
            Console.WriteLine($"{target.Name}의 남은 체력: {target.Hp}");
            if (target.Hp == 0)
            {
                Console.WriteLine($"{target.Name} 처치!");
            }

            Console.WriteLine("\nEnter를 눌러 다음 턴으로...");
            Console.ReadLine();

            
        }
    }
}
