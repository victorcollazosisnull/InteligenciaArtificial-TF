using UnityEngine;

public static class FuzzyMembership
{
    public static float Triangle(float x, float a, float b, float c)
    {
        if (x <= a || x >= c)
            return 0f;

        if (x == b)
            return 1f;

        if (x < b)
            return (x - a) / (b - a);

        return (c - x) / (c - b);
    }


    public static float Trapezoid(
    float x,
    float a,
    float b,
    float c,
    float d)
    {
        if (x < a || x > d)
            return 0f;

        if (x >= b && x <= c)
            return 1f;

        if (x < b)
            return (x - a) / (b - a);

        return (d - x) / (d - c);
    }
}