// AttackTarget.cs
// FIX: retorna Running en loop mientras el enemigo este visible,
// no Success, para que el Sequence no termine y el Selector no reinicie.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class AttackTarget : ActionNode
{
    public float attackCooldown = 1.5f;
    private float _timer;

    private AICharacterCombatLand _combat;

    public override void OnAwake()
    {
        base.OnAwake();
        _combat = GetComponent<AICharacterCombatLand>();
    }

    public override void OnStart()
    {
        _timer = 0f;  // ataca de inmediato al entrar
    }

    public override TaskStatus OnUpdate()
    {
        if (_combat == null) return TaskStatus.Failure;

        // Busca el enemigo por las dos vias
        Transform enemy = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.currentTarget != null)
            enemy = _Blackboard.currentTarget;

        // Si perdió al enemigo, termina el Sequence para que el Selector evalúe de nuevo
        if (enemy == null) return TaskStatus.Failure;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _combat.Attack();
            _timer = attackCooldown;
        }

        // Running = sigue en este nodo mientras tenga enemigo
        return TaskStatus.Running;
    }
}
