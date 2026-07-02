// AICharacterVehicleLandCazador.cs
// Componente de MOVIMIENTO del Cazador.
// Se adjunta al mismo GameObject que AICharacterActionLandCazador.
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AICharacterVehicleLandCazador : AICharacterVehicleLand
{
    public override void LoadComponent()
    {
        base.LoadComponent();
    }
    public override bool Flee(Transform enemy, float fleeDistance, float sampleRadius)
    {
        if (enemy == null) return false;

        Vector3 dir = (transform.position - enemy.position).normalized;
        Vector3 desiredPosition = transform.position + dir * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredPosition, out hit, sampleRadius, NavMesh.AllAreas))
        {
            MoveTo(hit.position);
            return true;
        }
        return false;
    }
}
