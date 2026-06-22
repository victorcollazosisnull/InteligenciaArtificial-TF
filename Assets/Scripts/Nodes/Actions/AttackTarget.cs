// AttackTarget.cs — usa _AICombatLand para atacar
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class AttackTarget : ActionNode
{
    public float attackCooldown = 1.5f;
    private float _timer;

    public override void OnStart() => _timer = 0f;

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

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _AICombatLand.Attack();
            _timer = attackCooldown;
        }
        return TaskStatus.Running;
    }
}
