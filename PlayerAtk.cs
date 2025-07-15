using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRpg;
using TextRpg_Comment;

namespace TextRpgPlayerAtk
{
    internal class PlayerAtk
    {
        
            public void Attack(Character player, Monster target, List<Monster> battleMonsters)
            {
                Console.Clear();
                int attackPower = player.Atk + player.ExtraAtk;
                int damage = attackPower;
                if (damage < 0) damage = 0;

                target.Hp = Math.Max(target.Hp - damage, 0);

                Console.WriteLine($"{player.Name}이(가) {target.Name}을(를) 공격했습니다! (피해량: {damage})");
                Console.WriteLine($"{target.Name}의 남은 체력: {target.Hp}");
            if (target.Hp == 0)
            {
                Console.WriteLine($"{target.Name} 처치!");
             
            }

            Console.WriteLine("\nEnter를 눌러 적 턴으로...");
            Console.ReadLine(); // 
            EnemyAttackPhase.EnemyAtkPhase();
        }
        
    }
}
