// Flee.cs
// FIX: usa ambas fuentes de enemigo (AIEyeNPC y Blackboard).
// Retorna Running mientras huye, Success cuando llega al punto seguro.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class Flee : ActionNode
{
    public float fleeDistance = 15f;

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

        // Busca el enemigo por las dos vias
        Transform enemy = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.currentTarget != null)
            enemy = _Blackboard.currentTarget;

        if (enemy != null)
        {
            Vector3 dir = (transform.position - enemy.position).normalized;
            _fleeTarget = transform.position + dir * fleeDistance;
            _fleeTarget.y = transform.position.y;
            _fleeTargetSet = true;
            _land.MoveTo(_fleeTarget);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null || !_fleeTargetSet) return TaskStatus.Failure;
        if (_land.HasReachedDestination()) return TaskStatus.Success;
        return TaskStatus.Running;
    }
}
