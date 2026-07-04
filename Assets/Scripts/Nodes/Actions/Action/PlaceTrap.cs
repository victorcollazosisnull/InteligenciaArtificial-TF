// PlaceTrap.cs
// El Cazador coloca una trampa en su posicion actual.
// Solo funciona en el objeto que tenga AICharacterActionLandCazador.
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Attack")]
public class PlaceTrap : ActionNode
{
    private AICharacterActionLandCazador _cazador;

    public override void OnAwake()
    {
        base.OnAwake();
        _cazador = GetComponent<AICharacterActionLandCazador>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_cazador == null) return TaskStatus.Failure;
        _cazador.PlaceTrap();
        return TaskStatus.Success;
    }
}
