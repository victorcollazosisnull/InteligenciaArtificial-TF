// MoveToTarget.cs
// FIX: verifica que el objetivo siga vivo antes de moverse hacia él.
// Si el objetivo está muerto, limpia el Blackboard y retorna Failure
// para que el NPC deje de perseguir el cadáver y vuelva a patrullar.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class MoveToTarget : ActionNode
{
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

        // Busca el target por las dos vías
        Transform target = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            target = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.currentTarget != null)
            target = _Blackboard.currentTarget;

        if (target == null)
        {
            _land.StopMoving();
            return TaskStatus.Failure;
        }

        // FIX: verifica que el objetivo no esté muerto
        HealthBase targetHealth = target.GetComponent<HealthBase>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            // Limpia el target para no seguir al cadáver
            if (_Blackboard != null) _Blackboard.currentTarget = null;
            if (_AIEyeNPC != null) _AIEyeNPC.Enemy = null;
            _land.StopMoving();
            Debug.Log($"[{gameObject.name}] objetivo muerto, deja de perseguir.");
            return TaskStatus.Failure;
        }

        float dist = Vector3.Distance(transform.position, target.position);
        if (dist <= stopDistance)
        {
            _land.StopMoving();
            return TaskStatus.Success;
        }

        _land.MoveTo(target.position);
        return TaskStatus.Running;
    }
}
