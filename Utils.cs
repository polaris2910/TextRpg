namespace TextRpg_Comment
{
    // 공통 유틸 함수
    public static class Utils
    {
        // 최소~최대 구간 클램프 함수
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}