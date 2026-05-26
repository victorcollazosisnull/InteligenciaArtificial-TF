// MoveToLastKnownPosition.cs
// Mueve al NPC a la ultima posicion conocida de su objetivo.
// Usado cuando el enemigo desaparece del campo de vision.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class MoveToLastKnownPosition : ActionNode
{
    private AICharacterActionLand _land;

    public override void OnAwake()
    {
        base.OnAwake();
        _land = GetComponent<AICharacterActionLand>();
    }

    public override void OnStart()
    {
        if (_Blackboard != null && _Blackboard.hasLastKnownPosition)
            _land.MoveTo(_Blackboard.lastKnownPosition);
    }

    public override TaskStatus OnUpdate()
    {
        if (_land == null || _Blackboard == null)             return TaskStatus.Failure;
        if (!_Blackboard.hasLastKnownPosition)                return TaskStatus.Failure;
        if (_land.HasReachedDestination())
        {
            _Blackboard.hasLastKnownPosition = false;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
