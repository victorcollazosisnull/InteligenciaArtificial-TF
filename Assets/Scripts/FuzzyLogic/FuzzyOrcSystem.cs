using System.Collections.Generic;
using UnityEngine;

public class FuzzyOrcSystem
{
    public float Evaluate(
        float hunger,
        float healthPercent,
        float distance)
    {
        List<FuzzyRule> rules = new();

        //----------------------------------
        // HAMBRE
        //----------------------------------

        float saciado =
            FuzzyMembership.Trapezoid(
                hunger,
                0, 0, 25, 50);

        float hambriento =
            FuzzyMembership.Triangle(
                hunger,
                25, 50, 75);

        float famelico =
            FuzzyMembership.Trapezoid(
                hunger,
                50, 75, 100, 100);

        //----------------------------------
        // SALUD
        //----------------------------------

        float critica =
            FuzzyMembership.Trapezoid(
                healthPercent,
                0, 0, 20, 40);

        //----------------------------------
        // REGLAS DIFUSAS
        //----------------------------------

        // Orco calmado
        rules.Add(new FuzzyRule(
            saciado,
            20));

        // Orco irritado
        rules.Add(new FuzzyRule(
            hambriento,
            50));

        // Orco furioso
        rules.Add(new FuzzyRule(
            famelico,
            90));

        // Orco defensivo cuando está herido
        rules.Add(new FuzzyRule(
            critica,
            70));

        //----------------------------------
        // SUGENO
        //----------------------------------

        float numerator = 0f;
        float denominator = 0f;

        foreach (var rule in rules)
        {
            numerator += rule.Strength * rule.Output;
            denominator += rule.Strength;
        }

        if (denominator == 0f)
            return 0f;

        return numerator / denominator;
    }
}