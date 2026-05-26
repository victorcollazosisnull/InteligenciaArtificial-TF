using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("BosqueEncantado/Move")]
public class Flee : ActionNode
{
    public float fleeDistance = 15f;
    public float sampleRadius = 5f;

    private AICharacterActionLand _land;

    private Vector3 _fleeTarget;
    private bool _fleeTargetSet;

    public override void OnAwake()
    {
        base.OnAwake();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override void OnStart()
    {
        _fleeTargetSet = false;

        Transform enemy = null;

        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.currentTarget != null)
            enemy = _Blackboard.currentTarget;

        if (enemy == null) return;

        Vector3 dir = (transform.position - enemy.position).normalized;

        Vector3 desiredPosition =
            transform.position + dir * fleeDistance;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(desiredPosition,
                                   out hit,
                                   sampleRadius,
                                   NavMesh.AllAreas))
        {
            _fleeTarget = hit.position;
            _fleeTargetSet = true;

            _land.MoveTo(_fleeTarget);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null || !_fleeTargetSet)
            return TaskStatus.Failure;

        if (_land.HasReachedDestination())
            return TaskStatus.Success;

        return TaskStatus.Running;
    }
}