// AICharacterVehicleLandUnicorn.cs
// FIX: hereda de AICharacterVehicleLand (no de AICharacterActionLand).
// MoveTo(), StopMoving(), HasReachedDestination() y agent
// vienen de AICharacterVehicle (via VehicleLand).
using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLandUnicorn : AICharacterVehicleLand
{
    public float _defaultSpeed;

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (agent != null)      // OK: agent viene de AICharacterVehicle
            _defaultSpeed = agent.speed;
    }

    public bool Flee(Transform enemy, float fleeDistance, float sampleRadius)
    {
        if (enemy == null) return false;

        Vector3 dir = (transform.position - enemy.position).normalized;
        Vector3 desiredPosition = transform.position + dir * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredPosition, out hit, sampleRadius, NavMesh.AllAreas))
        {
            ResetSpeed();
            MoveTo(hit.position);   // OK: viene de AICharacterVehicle
            return true;
        }
        return false;
    }

    public void MoveCautiously(Vector3 destination, float cautiousSpeed = 1.5f)
    {
        if (agent != null)
            agent.speed = cautiousSpeed;
        MoveTo(destination);    // OK: viene de AICharacterVehicle
    }

    public void ResetSpeed()
    {
        if (agent != null)
            agent.speed = _defaultSpeed;
    }
}
