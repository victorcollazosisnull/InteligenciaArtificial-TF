// MoveToTarget.cs
// FIX: retorna Running hasta llegar AL rango de ataque, no hasta llegar exactamente.
// Asi el Sequence de ataque no termina y AttackTarget puede ejecutarse.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class MoveToTarget : ActionNode
{
    public float stopDistance = 2.0f;   // distancia a la que para de moverse

    private AICharacterActionLand _land;

    public override void OnAwake()
    {
        base.OnAwake();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null) return TaskStatus.Failure;

        // Prioridad 1: usa currentTarget del Blackboard
        Transform target = null;
        if (_Blackboard != null && _Blackboard.currentTarget != null)
            target = _Blackboard.currentTarget;
        // Prioridad 2: usa Enemy de AIEyeNPC directamente
        else if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            target = _AIEyeNPC.Enemy;

        if (target == null)
        {
            _land.StopMoving();
            return TaskStatus.Failure;
        }

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= stopDistance)
        {
            _land.StopMoving();
            return TaskStatus.Success;   // llegó, AttackTarget puede ejecutarse
        }

        _land.MoveTo(target.position);
        return TaskStatus.Running;
    }
}
