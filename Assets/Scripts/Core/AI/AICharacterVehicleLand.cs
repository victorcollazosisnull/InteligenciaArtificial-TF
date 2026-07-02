// AICharacterVehicleLand.cs
// Rama de MOVIMIENTO terrestre.
// Hereda de AICharacterVehicle (tiene NavMesh y MoveTo).
// Base para NPCs terrestres: Orco, Duende, Cazador, Pato, Unicornio.
using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLand : AICharacterVehicle
{
    [Header("Patrol Settings")]
    public float patrolRadius = 10f;

    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public virtual bool Flee(Transform enemy, float fleeDistance, float sampleRadius)
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
    public void GenerateNewPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            MoveTo(hit.position);
    }
}
