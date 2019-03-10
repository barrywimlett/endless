namespace Endless
{
    public static class IsNullOrZeroExtension
    {
        public static bool IsNullOrZero(this int? x)
        {
            bool isNullOrZero = false;
            if (x.HasValue == false)
            {
                isNullOrZero = true;
            }
            else if (x.Value == 0)
            {
                isNullOrZero = true;
            }
            return isNullOrZero;

        }

    }
}