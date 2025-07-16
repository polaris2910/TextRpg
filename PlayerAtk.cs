using System;
using TextRpg_Comment;

namespace TextRpgPlayerAtk
{
    // 플레이어의 공격 처리 담당 클래스
    
    internal class PlayerAtk
    {
        private Random _random = new Random();
        // 단일 공격 실행: 플레이어가 몬스터를 공격
        public void Attack(Character player, Monster target)
        {

            // 플레이어의 기본 공격력 (예: player.Atk)
            int baseDamage = player.Atk;

            // 15% 확률로 치명타 발생
            bool isCriticalHit = _random.Next(1, 101) <= 15; // 1부터 100 사이의 난수 생성

            if (isCriticalHit)
            {
                // 치명타 시 160% 데미지
                int criticalDamage = (int)(baseDamage * 1.6);
                target.TakeDamage(criticalDamage);
                Console.WriteLine($"{player.Name}의 맹렬한 공격! 치명타! {target.Name}에게 {criticalDamage}의 피해를 입혔습니다!");
            }
            else
            {
                target.TakeDamage(baseDamage);
                Console.WriteLine($"{player.Name}가 {target.Name}에게 {baseDamage}의 피해를 입혔습니다.");
            }

            Console.WriteLine($"{target.Name}의 남은 체력: {target.Hp}");

            if (target.Hp <= 0)
            {
                Console.WriteLine($"{target.Name} 처치!");
            }
            Console.WriteLine("\nEnter를 눌러 다음 턴으로...");
            Console.ReadLine();
        }
    }
}