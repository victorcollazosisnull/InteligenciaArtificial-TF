// AICharacterVehicleLandUnicorn.cs
// FIX: hereda de AICharacterVehicleLand (no de AICharacterActionLand).
// MoveTo(), StopMoving(), HasReachedDestination() y agent
// vienen de AICharacterVehicle (via VehicleLand).
using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLandUnicorn : AICharacterVehicleLand
{
    public float _defaultSpeed;
    public float _cautiousSpeed;

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (agent != null)
        {
            _defaultSpeed = agent.speed;
            _cautiousSpeed = _defaultSpeed / 2f;
        }    
    }

    public override bool Flee(Transform enemy, float fleeDistance, float sampleRadius)
    {
        if (enemy == null) return false;

        Vector3 dir = (transform.position - enemy.position).normalized;
        Vector3 desiredPosition = transform.position + dir * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredPosition, out hit, sampleRadius, NavMesh.AllAreas))
        {
            ResetSpeed();
            MoveTo(hit.position);
            return true;
        }
        return false;
    }

    public void MoveCautiously(Vector3 destination)
    {
        if (agent != null)
            agent.speed = _cautiousSpeed;
        MoveTo(destination);    
    }

    public void ResetSpeed()
    {
        if (agent != null)
            agent.speed = _defaultSpeed;
    }
}
