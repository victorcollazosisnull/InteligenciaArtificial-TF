// Flee.cs
// FIX: usa enemyTarget en vez de currentTarget como fallback.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Move")]
public class Flee : ActionNode
{
    public float fleeDistance = 15f;
    public float sampleRadius = 5f;

    private Vector3 _fleeTarget;
    private bool _fleeTargetSet;

    public override void OnStart()
    {
        _fleeTargetSet = false;

        // FIX: usa enemyTarget en vez de currentTarget
        Transform enemy = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.enemyTarget != null)
            enemy = _Blackboard.enemyTarget;

        if (enemy == null || _AIVehicleLand == null) return;

        Vector3 dir = (transform.position - enemy.position).normalized;
        Vector3 desiredPosition = transform.position + dir * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(desiredPosition, out hit, sampleRadius, NavMesh.AllAreas))
        {
            _fleeTarget = hit.position;
            _fleeTargetSet = true;
            _AIVehicleLand.MoveTo(_fleeTarget);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (_AIVehicleLand == null || !_fleeTargetSet) return TaskStatus.Failure;
        if (_AIVehicleLand.HasReachedDestination()) return TaskStatus.Failure;
        return TaskStatus.Running;
    }
}
