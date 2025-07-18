﻿using System;
using TextRpg_Comment;

namespace TextRpgPlayerAtk
{

    // 플레이어의 공격 처리용 클래스
    // StartBattle에서 공격 선택 시 매번 객체를 만들어 Attack()을 호출함.


    internal class PlayerAtk
    {
        private Random _random = new Random(); // 랜덤(치명타 판정)

        // 플레이어가 몬스터를 공격하는 메소드
        public void Attack(Character player, Monster target)
        {

            int baseDamage = player.Atk;

            // 15% 확률로 치명타 판정
            bool isCriticalHit = _random.Next(1, 101) <= 15;

            if (isCriticalHit)
            {
                // 치명타: 데미지 1.6배
                int criticalDamage = (int)(baseDamage * 1.6);
                target.TakeDamage(criticalDamage);
                Console.WriteLine($"{player.Name}의 맹렬한 공격! 치명타! {target.Name}에게 {criticalDamage}의 피해를 입혔습니다!");
            }
            else
            {
                target.TakeDamage(baseDamage);
                Console.WriteLine($"{player.Name}가 {target.Name}에게 {baseDamage}의 피해를 입혔습니다.");
            }

            // 몬스터 남은 체력 출력
            Console.WriteLine($"{target.Name}의 남은 체력: {target.Hp}");


            // 몬스터 처치/보상 출력

            // [5] 몬스터가 사망(HP <= 0) 시 "처치!" 및 보상 메시지 출력 분기
            // RewardGold>0이면 보상 메세지, 없으면 그냥 "처치!"-> 황금고블린을 위한 코드

            if (target.Hp <= 0)
            {
                if (target.RewardGold <= 0)
                    Console.WriteLine($"{target.Name} 처치!");
                else
                    Console.WriteLine($"{target.Name} 처치!{target.RewardGold}G 획득!");
            }


            Console.WriteLine("\nEnter를 눌러 다음 턴으로...");
            Console.ReadLine();
        }
    }
}