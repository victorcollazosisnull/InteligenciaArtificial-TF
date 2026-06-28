// AttackTarget.cs
// Ranged: dispara mientras se mueve (Avatar Mask maneja la animacion arriba).
// Melee: se queda quieto y golpea.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class AttackTarget : ActionNode
{
    public float attackCooldown = 1.5f;
    private float _timer;
    private NPCAnimator _npcAnimator;
    private bool _isRanged;

    public override void OnAwake()
    {
        base.OnAwake();
        _npcAnimator = gameObject.GetComponentInChildren<NPCAnimator>();
    }

    public override void OnStart()
    {
        _timer = 0f;
        _isRanged = false;

        Transform enemy = GetCurrentEnemy();
        if (enemy != null)
        {
            HealthBase th = enemy.GetComponent<HealthBase>();
            if (th != null && th.typeAgent != TypeAgent.Orco)
                _isRanged = true;
        }

        if (_npcAnimator != null)
        {
            if (_isRanged)
                _npcAnimator.TriggerRangedAttack(); // activa IsRanged=true
            else
                _npcAnimator.TriggerMeleeAttack();  // activa IsRanged=false
        }
    }

    public override void OnEnd()
    {
        if (_npcAnimator != null) _npcAnimator.StopAttack();
    }

    public override TaskStatus OnUpdate()
    {
        if (_AICombatLand == null) return TaskStatus.Failure;

        Transform enemy = GetCurrentEnemy();
        if (enemy == null) return TaskStatus.Failure;

        HealthBase targetHealth = enemy.GetComponent<HealthBase>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            if (_AIEyeNPC != null) _AIEyeNPC.Enemy = null;
            if (_Blackboard != null) _Blackboard.enemyTarget = null;
            if (_AIVehicleLand != null) _AIVehicleLand.StopMoving();
            return TaskStatus.Failure;
        }

        // Ranged: sigue moviéndose hacia la presa mientras dispara
        // El Avatar Mask maneja la animacion de disparo en la parte superior
        if (_isRanged && _AIVehicleLand != null && enemy != null)
            _AIVehicleLand.MoveTo(enemy.position);

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _AICombatLand.Attack();
            _timer = attackCooldown;
        }

        return TaskStatus.Running;
    }

    private Transform GetCurrentEnemy()
    {
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            return _AIEyeNPC.Enemy;
        if (_Blackboard != null && _Blackboard.enemyTarget != null)
            return _Blackboard.enemyTarget;
        return null;
    }
}