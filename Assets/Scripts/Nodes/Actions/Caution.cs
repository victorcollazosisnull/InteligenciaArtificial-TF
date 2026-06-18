using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Move")]
public class Caution : ActionNode
{
    public SharedVector3 targetPosition;

    public float cautiousSpeed = 1.5f;

    private AICharacterVehicleLandUnicorn _unicorn;

    public override void OnAwake()
    {
        base.OnAwake();
        _unicorn = GetComponent<AICharacterVehicleLandUnicorn>();
    }

    public override void OnStart()
    {
        if (_unicorn != null)
        {
            // Llama a la función que creamos en el script del Unicornio
            _unicorn.MoveCautiously(targetPosition.Value, cautiousSpeed);
        }
    }

    public override TaskStatus OnUpdate()
    {
        // Si no encuentra el componente, la tarea falla para no romper el árbol
        if (_unicorn == null)
            return TaskStatus.Failure;

        // Comprueba si ya llegó a su destino usando la función heredada
        if (_unicorn.HasReachedDestination())
        {
            // Opcional: restaurar la velocidad normal al terminar la acción de cautela
            _unicorn.ResetSpeed();
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}