// AICharacterVehicleLandHada.cs
// FIX: hereda de AICharacterVehicleLand (no de AICharacterActionLand).
// Es un script de MOVIMIENTO del Hada, no de acciones.
// StopMoving() y MoveTo() vienen de AICharacterVehicle.
using UnityEngine;

public class AICharacterVehicleLandHada : AICharacterVehicleLand
{
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
            Debug.Log($"[Hada] Protegiendo a {ally.name} con un escudo m�gico!");
            return true;
        }
        return false;
    }
}
