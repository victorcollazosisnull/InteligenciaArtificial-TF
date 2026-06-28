// AttackTarget.cs
// Detecta si el enemigo es Orco (melee) o presa (ranged) y activa
// la animacion correcta. Si es ranged, detiene el movimiento para disparar.
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
            HealthBase targetHealth = enemy.GetComponent<HealthBase>();
            if (targetHealth != null && targetHealth.typeAgent != TypeAgent.Orco)
                _isRanged = true;
        }

        if (_npcAnimator != null)
        {
            if (_isRanged)
            {
                _npcAnimator.TriggerRangedAttack();
                // Detiene el movimiento para disparar quieto
                if (_AIVehicleLand != null) _AIVehicleLand.StopMoving();
            }
            else
            {
                _npcAnimator.TriggerMeleeAttack();
            }
        }
        else
        {
            // Fallback para NPCs sin WeaponSwitcher (Orco, Duende)
            if (_npcAnimator != null) _npcAnimator.TriggerAttack();
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

        // Si es ranged, mantiene al NPC quieto mientras ataca
        if (_isRanged && _AIVehicleLand != null)
            _AIVehicleLand.StopMoving();

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