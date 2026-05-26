// ActionNode.cs
// Clase base de todos los nodos. 
// FIX: cachea HealthBase directamente para no depender del orden de Awake().
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ActionNode : Action
{
    protected AIEyeNPC _AIEyeNPC;
    protected AICharacterActions _AICharacterAction;
    protected Blackboard _Blackboard;
    protected HealthBase _HealthBase;


    public override void OnAwake()
    {
        base.OnAwake();
        _AIEyeNPC = GetComponent<AIEyeNPC>();
        _AICharacterAction = GetComponent<AICharacterActions>();
        _Blackboard = GetComponent<Blackboard>(); 
        _HealthBase = GetComponent<HealthBase>();      
    }
}
