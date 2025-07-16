using System;
using TextRpg_Comment;

namespace TextRpgPlayerAtk
{
    // 플레이어의 공격 처리 담당 클래스
    internal class PlayerAtk
    {
        // 단일 공격 실행: 플레이어가 몬스터를 공격
        public void Attack(Character player, Monster target)
        {
            Console.Clear();
            int attackPower = player.Atk + player.ExtraAtk;       // 최종 공격력 계산
            if(Dodge.DodgeAtk())
            {
                Dodge.ShowDodge();
                attackPower = 0;
            }

            int damage = Math.Max(attackPower, 0);                // 음수 방지

            target.Hp = Math.Max(target.Hp - damage, 0);          // 몬스터 체력 감소 (0 하한)

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