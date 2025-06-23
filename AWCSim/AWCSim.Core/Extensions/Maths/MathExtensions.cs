namespace AWCSim.Core.Extensions.Maths;

public static class MathExtensions
{
    public static bool IsPowerOf(int value, int baseValue) => Math.Log(value, baseValue) % 1 == 0;
}
