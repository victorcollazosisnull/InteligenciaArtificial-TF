using UnityEngine;

public class AICharacterActionLandHada : AICharacterActionLand
{
    [Header("Hada - Configuración de Acciones")]
    public float protectionRange = 3f;
    public float fleeDistance = 15f;

    private HealthIALandHada healthHada;

    public override void LoadComponent()
    {
        base.LoadComponent();
        // Conectamos las acciones con su sistema de salud mágico
        healthHada = GetComponent<HealthIALandHada>();
    }

    /// <summary>
    /// Intenta proteger al aliado. Si está lo suficientemente cerca, detiene el movimiento y lanza el escudo.
    /// </summary>
    /// <returns>True si logró protegerlo, False si aún está lejos.</returns>
    public bool ProtegerAliado(HealthBase ally)
    {
        if (ally == null || healthHada == null) return false;

        float distance = Vector3.Distance(transform.position, ally.transform.position);

        if (distance <= protectionRange)
        {
            StopMoving();
            //healthHada.CastShieldOn(ally);
            Debug.Log($"[Hada] Protegiendo a {ally.name} con un escudo mágico!");
            return true;
        }

        return false;
    }
}
   
