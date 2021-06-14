using UnityEngine;

namespace CTE
{
    public struct Vector2Decimal
    {
        /* field */
        public decimal x;
        public decimal y;

        /* oeprator */
        public static explicit operator Vector2(Vector2Decimal vector2Decimal) =>
            new Vector2((float)vector2Decimal.x, (float)vector2Decimal.y);
    }
}