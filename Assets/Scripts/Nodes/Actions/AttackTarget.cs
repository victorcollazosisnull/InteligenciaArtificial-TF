// FIX: ahora notifica al NPCAnimator para activar la animacion de ataque.
// Activa IsAttacking=true al entrar, false al salir o perder al enemigo.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class AttackTarget : ActionNode
{
    public float attackCooldown = 1.5f;
    private float _timer;
    private NPCAnimator _npcAnimator;

    public override void OnAwake()
    {
        base.OnAwake();
        _npcAnimator = this.gameObject.GetComponentInChildren<NPCAnimator>();
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
        else if (_Blackboard != null && _Blackboard.currentTarget != null)
            enemy = _Blackboard.currentTarget;

        if (enemy == null) return TaskStatus.Failure;

        HealthBase targetHealth = enemy.GetComponent<HealthBase>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            if (_AIEyeNPC != null) _AIEyeNPC.Enemy = null;
            if (_Blackboard != null) _Blackboard.currentTarget = null;
            if (_AIVehicleLand != null) _AIVehicleLand.StopMoving();
            return TaskStatus.Failure;
        }
        //if (_AIVehicleLand != null && enemy != null) //
        //    _AIVehicleLand.MoveTo(enemy.position); //
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _AICombatLand.Attack();
            _timer = attackCooldown;
        }

        return TaskStatus.Running;
    }
}