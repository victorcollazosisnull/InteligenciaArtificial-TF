using UnityEngine;

public class FuzzyUnicornSystem
{
    public float Evaluate(float healthPercent, float enemyHealthPercent, float distance, float nearbyAllies)
    {
        float saludBaja  = FuzzyMembership.Trapezoid(healthPercent, 0, 0, 20, 40);
        float saludMedia = FuzzyMembership.Triangle(healthPercent, 20, 50, 80);
        float saludAlta  = FuzzyMembership.Trapezoid(healthPercent, 60, 80, 100, 100);

        float cerca = FuzzyMembership.Trapezoid(distance, 0, 0, 5, 12);
        float medio = FuzzyMembership.Triangle(distance, 5, 15, 25);
        float lejos = FuzzyMembership.Trapezoid(distance, 18, 30, 100, 100);

        float solo  = FuzzyMembership.Trapezoid(nearbyAllies, 0, 0, 0, 2);
        float grupo = FuzzyMembership.Trapezoid(nearbyAllies, 2, 4, 10, 10);

        float huir = Mathf.Min(saludBaja, Mathf.Max(cerca, medio));

        float cautela = Mathf.Max(saludMedia, lejos);

        float atacar = saludAlta;
        atacar = Mathf.Max(atacar, Mathf.Min(saludAlta, Mathf.Max(cerca, medio)));
        atacar *= (1f - saludBaja);

        float atacarFinal = Mathf.Clamp01(atacar * (1f + grupo * 0.4f));
        float huirFinal   = Mathf.Clamp01(huir * (1f + solo * 0.3f));

        float suma = huirFinal + cautela + atacarFinal;
        if (suma <= 0.001f) return 50f;

        return ((huirFinal * 20f) + (cautela * 50f) + (atacarFinal * 80f)) / suma;
    }
}
