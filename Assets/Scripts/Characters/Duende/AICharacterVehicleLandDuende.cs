// AICharacterVehicleLandDuende.cs
// Componente de MOVIMIENTO del Duende.
// Se adjunta al mismo GameObject que AICharacterActionLandDuende.
using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLandDuende : AICharacterVehicleLand
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
