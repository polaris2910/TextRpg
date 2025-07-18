namespace TextRpg_Comment
{
    // 공통 유틸리티 클래스
    public static class Utils
    {
        // 값이 min과 max 사이에서 벗어나지 않도록 강제(범위 제한)
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}