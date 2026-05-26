// AttackTarget.cs
// FIX: cuando el objetivo muere limpia el Blackboard y retorna Failure
// para que el NPC no se quede pegado al cadáver y vuelva a patrullar.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class AttackTarget : ActionNode
{
    public float attackCooldown = 1.5f;
    private float _timer;

    private AICharacterCombatLand _combat;
    private AICharacterActionLand _land;

    public override void OnAwake()
    {
        base.OnAwake();
        _combat = GetComponent<AICharacterCombatLand>();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override void OnStart()
    {
        _timer = 0f; // ataca de inmediato al entrar
    }

    public override TaskStatus OnUpdate()
    {
        if (_combat == null) return TaskStatus.Failure;

        // Busca el enemigo por las dos vías
        Transform enemy = null;
        if (_AIEyeNPC != null && _AIEyeNPC.Enemy != null)
            enemy = _AIEyeNPC.Enemy;
        else if (_Blackboard != null && _Blackboard.currentTarget != null)
            enemy = _Blackboard.currentTarget;

        if (enemy == null) return TaskStatus.Failure;

        // FIX: verifica que el objetivo no esté muerto
        HealthBase targetHealth = enemy.GetComponent<HealthBase>();
        if (targetHealth != null && targetHealth.IsDead)
        {
            // Limpia todo y deja de atacar
            if (_AIEyeNPC != null) _AIEyeNPC.Enemy = null;
            if (_Blackboard != null) _Blackboard.currentTarget = null;
            if (_land != null) _land.StopMoving();
            Debug.Log($"[{gameObject.name}] objetivo eliminado, vuelve a patrullar.");
            return TaskStatus.Failure;  // Failure ? Selector reevalúa ? Patrol
        }

        // Ataca según el cooldown
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _combat.Attack();
            _timer = attackCooldown;
        }

        return TaskStatus.Running; // sigue atacando mientras el enemigo esté vivo
    }
}
