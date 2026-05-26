// AttackTarget.cs
// Llama al metodo Attack() del NPC segun el cooldown configurado.
// El NPC concreto (Orco, Duende, Cazador) define como ataca.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class AttackTarget : ActionNode
{
    public float attackCooldown = 1.5f;
    private float timer;

    private AICharacterCombatLand _combat;

    public override void OnAwake()
    {
        base.OnAwake();
        _combat = GetComponent<AICharacterCombatLand>();
    }

    public override void OnStart()
    {
        timer = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        if (_combat == null || _AIEyeNPC == null) return TaskStatus.Failure;
        if (_AIEyeNPC.Enemy == null)              return TaskStatus.Failure;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            _combat.Attack();
            timer = attackCooldown;
        }
        return TaskStatus.Running;
    }
}
