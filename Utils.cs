namespace TextRpg_Comment
{
    // [공통 유틸 함수 클래스]
    // 다양한 곳에서 반복적으로 사용할 수 있는 보조 기능을 제공
    public static class Utils
    {
        // [최소~최대 범위로 값 제한(Clamp) 함수]
        // 인자: value(제한할 값), min(최소), max(최대)
        // 호출 코드에서 어떤 값을 min~max 사이로 강제하고 싶을 때 씀
        // 예시: 실제로 캐릭터의 Hp, Mp, 경험치 등이 음수나 최대치를 넘지 않도록 제한할 때 사용
        public static int Clamp(int value, int min, int max)
        {
            // [실행 흐름] 호출하면 먼저 value < min 인지 체크
            if (value < min)
                return min;        // [최소 미만이면] 항상 min 반환

            // [실행 흐름] 그 다음 value > max 인지 체크
            if (value > max)
                return max;        // [최대 초과면] 항상 max 반환

            // [실행 흐름] 둘 다 아니면 원래 값 그대로 반환
            return value;
        }
    }
}