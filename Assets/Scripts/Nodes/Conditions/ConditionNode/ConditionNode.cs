// ConditionNode.cs — base para todas las condiciones
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ConditionNode : Conditional
{
    protected AIEyeNPC _AIEyeNPC;
    protected Blackboard _Blackboard;
    protected HealthBase _HealthBase;

    public override void OnAwake()
    {
        base.OnAwake();
        _AIEyeNPC = GetComponent<AIEyeNPC>();
        _Blackboard = GetComponent<Blackboard>();
        _HealthBase = GetComponent<HealthBase>();
    }
}