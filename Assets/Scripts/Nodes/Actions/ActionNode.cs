// ActionNode.cs
// Clase base de todos los nodos de accion y condicion del proyecto.
// Hereda de Action (BehaviorDesigner) y agrega referencias
// a los componentes comunes que todo nodo puede necesitar.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ActionNode : Action
{
    protected AIEyeNPC          _AIEyeNPC;
    protected AICharacterActions _AICharacterAction;
    protected Blackboard         _Blackboard;

    public override void OnAwake()
    {
        base.OnAwake();
        _AIEyeNPC          = GetComponent<AIEyeNPC>();
        _AICharacterAction = GetComponent<AICharacterActions>();
        _Blackboard        = GetComponent<Blackboard>();
    }
}
