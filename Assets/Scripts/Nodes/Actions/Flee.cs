// Flee.cs
// Mueve al NPC en la direccion opuesta a su enemigo.
// Usado por: Duende (huye del Orco), Pato y Unicornio (huyen de amenazas).
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class Flee : ActionNode
{
    public float fleeDistance = 15f;   // hasta donde huye

    private AICharacterActionLand _land;

    public override void OnAwake()
    {
        base.OnAwake();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null || _AIEyeNPC == null) return TaskStatus.Failure;
        if (_AIEyeNPC.Enemy == null)            return TaskStatus.Failure;

        // Calcula punto opuesto al enemigo
        Vector3 fleeDir = (transform.position - _AIEyeNPC.Enemy.position).normalized;
        Vector3 fleePos = transform.position + fleeDir * fleeDistance;

        _land.MoveTo(fleePos);

        // Termina cuando llega al punto de huida
        if (_land.HasReachedDestination()) return TaskStatus.Success;
        return TaskStatus.Running;
    }
}
