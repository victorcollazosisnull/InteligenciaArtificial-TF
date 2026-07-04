// ActionNode.cs
// REFACTOR: ahora cachea AICharacterVehicleLand para movimiento
// y AICharacterCombatLand para ataque — separados como en el diagrama del profe.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ActionNode : Action
{
    protected AIEyeNPC _AIEyeNPC;
    protected AICharacterAction _AICharacterAction;   // rama acciones
    protected AICharacterVehicleLand _AIVehicleLand;      // rama movimiento
    protected AICharacterCombatLand _AICombatLand;       // ataque
    protected Blackboard _Blackboard;
    protected HealthBase _HealthBase;

    public override void OnAwake()
    {
        base.OnAwake();
        _AIEyeNPC = GetComponent<AIEyeNPC>();
        _AICharacterAction = GetComponent<AICharacterAction>();
        _AIVehicleLand = GetComponent<AICharacterVehicleLand>();
        _AICombatLand = GetComponent<AICharacterCombatLand>();
        _Blackboard = GetComponent<Blackboard>();
        _HealthBase = GetComponent<HealthBase>();
    }
}
