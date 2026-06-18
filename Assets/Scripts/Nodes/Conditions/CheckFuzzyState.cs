using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Fuzzy")]
[TaskDescription("Evalúa si el valor difuso está dentro del rango especificado para activar la secuencia.")]
public class CheckFuzzyState : Conditional
{
    public float minValue;
    public float maxValue;

    private FuzzyInferenceController _fuzzyController;

    public override void OnAwake()
    {
        _fuzzyController = GetComponent<FuzzyInferenceController>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_fuzzyController == null) return TaskStatus.Failure;

        // Accedemos al valor calculado por el motor difuso
        float currentTendency = _fuzzyController.GetTendenciaActual();

        if (currentTendency >= minValue && currentTendency <= maxValue)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}