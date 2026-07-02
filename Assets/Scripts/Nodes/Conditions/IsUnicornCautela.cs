using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Condition/Unicornio")]
public class IsUnicornCautela : ConditionNode
{
    private AICharacterActionLandUnicorn _actionUnicorn;

    public override void OnAwake()
    {
        base.OnAwake();
        _actionUnicorn = GetComponent<AICharacterActionLandUnicorn>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_actionUnicorn == null) return TaskStatus.Failure;
        return _actionUnicorn.GetEstadoActual() == AICharacterActionLandUnicorn.Estado.Cautela
            ? TaskStatus.Success : TaskStatus.Failure;
    }
}
