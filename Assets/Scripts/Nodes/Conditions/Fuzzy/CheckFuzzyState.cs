using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Fuzzy")]
[TaskDescription("Evalúa si el valor difuso está dentro del rango especificado para activar la secuencia.")]
public class CheckFuzzyState : Conditional
{
    public float minValue;
    public float maxValue;

    private AICharacterActionLandUnicorn _actionUnicorn;

    public override void OnAwake()
    {
        _actionUnicorn = GetComponent<AICharacterActionLandUnicorn>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_actionUnicorn == null) return TaskStatus.Failure;

        // Accedemos al valor calculado por el motor difuso
        float currentTendency = _actionUnicorn.GetTendenciaActual();

        if (currentTendency >= minValue && currentTendency <= maxValue)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}