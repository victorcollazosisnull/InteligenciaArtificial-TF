using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLandHada : AICharacterVehicleLand
{
    private float _defaultSpeed;

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (agent != null)
            _defaultSpeed = agent.speed;
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
