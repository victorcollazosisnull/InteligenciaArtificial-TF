// AICharacterVehicleLandHada.cs
// FIX: hereda de AICharacterVehicleLand (no de AICharacterActionLand).
// Es un script de MOVIMIENTO del Hada, no de acciones.
// StopMoving() y MoveTo() vienen de AICharacterVehicle.
using UnityEngine;

public class AICharacterVehicleLandHada : AICharacterVehicleLand
{
    [Header("Hada - Configuración de Acciones")]
    public float protectionRange = 3f;
    public float fleeDistance = 15f;

    private HealthIALandHada healthHada;

    public override void LoadComponent()
    {
        base.LoadComponent();
        healthHada = GetComponent<HealthIALandHada>();
    }

    public bool IntentarProtegerAliado(HealthBase ally)
    {
        if (ally == null || healthHada == null) return false;

        float distance = Vector3.Distance(transform.position, ally.transform.position);
        if (distance <= protectionRange)
        {
            StopMoving();  // OK: viene de AICharacterVehicle
            Debug.Log($"[Hada] Protegiendo a {ally.name} con un escudo mágico!");
            return true;
        }
        return false;
    }
}
