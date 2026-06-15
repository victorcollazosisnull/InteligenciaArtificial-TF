using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("BosqueEncantado/Fuzzy")]
[TaskDescription("Evalúa si el valor difuso está dentro del rango especificado para activar la secuencia.")]
public class CheckFuzzyState : Conditional
{
    public SharedFloat tendenciaAccion;

    public float minValue;

    public float maxValue;

    public override TaskStatus OnUpdate()
    {
        // Verificamos si el valor difuso actual cae dentro del rango de este nodo
        if (tendenciaAccion.Value >= minValue && tendenciaAccion.Value <= maxValue)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}