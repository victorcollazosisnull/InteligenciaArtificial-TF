using UnityEngine;

public class AICharacterActionLandHada : AICharacterActionLand
{
    [Header("Hada - Configuraci�n")]
    public float protectionRange = 3f;
    public float fleeDistance = 15f;
    public float shieldAmount = 30f;
    public float healAmount = 25f;

    private HealthIALandHada healthHada;

    public override void LoadComponent()
    {
        base.LoadComponent();
        healthHada = GetComponent<HealthIALandHada>();
    }

    public bool CastShieldOn(HealthBase ally)
    {
        if (ally == null || healthHada == null) return false;

        float distance = Vector3.Distance(transform.position, ally.transform.position);
        if (distance > protectionRange) return false;

        ally.AddShield(shieldAmount);
        Debug.Log($"[Hada] Escudo m�gico lanzado sobre {ally.name}!");
        return true;
    }

    public bool HealAlly(HealthBase ally)
    {
        if (ally == null || healthHada == null) return false;

        float distance = Vector3.Distance(transform.position, ally.transform.position);
        if (distance > protectionRange) return false;

        ally.Heal(healAmount);
        Debug.Log($"[Hada] Curaci�n aplicada a {ally.name}!");
        return true;
    }

    public bool ProtegerAliado(HealthBase ally)
    {
        if (ally == null || healthHada == null) return false;

        float distance = Vector3.Distance(transform.position, ally.transform.position);
        if (distance <= protectionRange)
        {
            ally.AddShield(shieldAmount);
            Debug.Log($"[Hada] Protegiendo a {ally.name} con un escudo m�gico!");
            return true;
        }

        return false;
    }
}
