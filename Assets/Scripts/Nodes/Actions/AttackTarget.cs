// AttackTarget.cs
// FIX: si el enemigo sale del rango de ataque, retorna Failure
// para que el BT vuelva a MoveToTarget y lo persiga.
// Evita que el Orco ataque al aire cuando el Duende huye.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class AttackTarget : ActionNode
{
    public float attackCooldown = 1.5f;
    public float attackRange = 2.5f;  // debe coincidir con el rango del NPC
    private float _timer;
    private NPCAnimator _npcAnimator;

    public override void OnAwake()
    {
        base.OnAwake();
        _npcAnimator = gameObject.GetComponentInChildren<NPCAnimator>();
    }

    public override void OnStart()
    {
        _timer = 0f;
        if (_npcAnimator != null) _npcAnimator.TriggerAttack();
    }

    public override void OnEnd()
    {
        if (_npcAnimator != null) _npcAnimator.StopAttack();
    }

    public override TaskStatus OnUpdate()
    {
        if (_AICombatLand == null) return TaskStatus.Failure;

        Transform enemy = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.enemyTarget != null)
            enemy = _Blackboard.enemyTarget;

        if (enemy == null) return TaskStatus.Failure;

        // Verifica que el objetivo no esté muerto
        HealthBase targetHealth = enemy.GetComponent<HealthBase>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            if (_AIEyeNPC != null) _AIEyeNPC.Enemy = null;
            if (_Blackboard != null) _Blackboard.enemyTarget = null;
            if (_AIVehicleLand != null) _AIVehicleLand.StopMoving();
            return TaskStatus.Failure;
        }

        // FIX: si el enemigo salió del rango, deja de atacar
        // El Selector interno volverá a MoveToTarget para perseguirlo
        //float dist = Vector3.Distance(transform.position, enemy.position); ATAQUE RANGO TEST
        //if (dist > attackRange)
        //{
        //    if (_npcAnimator != null) _npcAnimator.StopAttack();
        //    return TaskStatus.Failure;
        //}

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _AICombatLand.Attack();
            _timer = attackCooldown;
        }

        return TaskStatus.Running;
    }
}