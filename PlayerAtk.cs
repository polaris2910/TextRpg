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
        
            public void Attack(Character player, Monster monster , List<Monster> battleMonsters)
            {
                int attackPower = player.Atk + player.ExtraAtk;
                int damage = attackPower;
                if (damage < 0) damage = 0;

                monster.Hp = Math.Max(monster.Hp - damage, 0);

                Console.WriteLine($"{player.Name}이(가) {monster.Name}을(를) 공격했습니다! (피해량: {damage})");
                Console.WriteLine($"{monster.Name}의 남은 체력: {monster.Hp}");
            if (monster.Hp == 0)
            {
                Console.WriteLine($"{monster.Name} 처치!");
                

            }
            EnemyAttackPhase.EnemyAtkPhase(player, battleMonsters);
        }
        
    }
}
