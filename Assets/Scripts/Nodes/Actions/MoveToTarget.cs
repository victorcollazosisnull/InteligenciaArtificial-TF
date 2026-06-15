using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// NUEVO: Enum para decidir a quién seguir
public enum TargetFaction
{
    Enemy,
    Ally
}

[TaskCategory("BosqueEncantado/Move")]
public class MoveToTarget : ActionNode
{
    public TargetFaction targetToFollow = TargetFaction.Enemy; // Opcion en el editor

    public float stopDistance = 2.0f;

    private AICharacterActionLand _land;

    public override void OnAwake()
    {
        base.OnAwake();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null) return TaskStatus.Failure;

        Transform target = null;

        // NUEVO: Selecciona el objetivo dependiendo de la configuracion del nodo
        if (targetToFollow == TargetFaction.Enemy)
        {
            if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
                target = _AIEyeNPC.Enemy;
            else if (_Blackboard != null && _Blackboard.enemyTarget != null)
                target = _Blackboard.enemyTarget;
        }
        else if (targetToFollow == TargetFaction.Ally)
        {
            if (_AIEyeNPC != null && _AIEyeNPC.Allied != null)
                target = _AIEyeNPC.Allied;
            else if (_Blackboard != null && _Blackboard.allyTarget != null)
                target = _Blackboard.allyTarget;
        }

        if (target == null)
        {
            _land.StopMoving();
            return TaskStatus.Failure;
        }

        // Verifica que el objetivo no esté muerto (Aplica para aliados también)
        HealthBase targetHealth = target.GetComponent<HealthBase>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            if (targetToFollow == TargetFaction.Enemy)
            {
                if (_Blackboard != null) _Blackboard.enemyTarget = null;
                if (_AIEyeNPC != null) _AIEyeNPC.Enemy = null;
            }
            else
            {
                if (_Blackboard != null) _Blackboard.allyTarget = null;
                if (_AIEyeNPC != null) _AIEyeNPC.Allied = null;
            }

            _land.StopMoving();
            Debug.Log($"[{gameObject.name}] objetivo muerto, cancelando seguimiento.");
            return TaskStatus.Failure;
        }

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= stopDistance)
        {
            _land.StopMoving();
            return TaskStatus.Success;
        }

        _land.MoveTo(target.position);
        return TaskStatus.Running; // FIX IMPORTANTE: Debe ser Running mientras se mueve
    }
}