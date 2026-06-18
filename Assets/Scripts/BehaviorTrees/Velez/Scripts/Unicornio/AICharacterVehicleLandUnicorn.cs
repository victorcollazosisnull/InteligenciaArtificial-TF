using UnityEngine;
using UnityEngine.AI;

public class AICharacterVehicleLandUnicorn : AICharacterActionLand
{
    private float _defaultSpeed;

    // Aprovechamos LoadComponent para guardar la velocidad original del NavMeshAgent
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (agent != null)
        {
            _defaultSpeed = agent.speed;
        }
    }

    // ── Función: Huir (Flee) ────────────────────────────────

    public bool Flee(Transform enemy, float fleeDistance, float sampleRadius)
    {
        if (enemy == null) return false;

        // Calculamos la dirección opuesta al enemigo
        Vector3 dir = (transform.position - enemy.position).normalized;
        Vector3 desiredPosition = transform.position + dir * fleeDistance;

        NavMeshHit hit;

        // Buscamos un punto válido en el NavMesh cerca de la posición deseada
        if (NavMesh.SamplePosition(desiredPosition, out hit, sampleRadius, NavMesh.AllAreas))
        {
            ResetSpeed(); // Nos aseguramos de correr a velocidad normal al huir
            MoveTo(hit.position);
            return true; // Se encontró un punto de huida exitosamente
        }

        return false; // No se encontró un punto válido para huir
    }

    // ── Función: Cautela (Move Cautiously) ──────────────────

    public void MoveCautiously(Vector3 destination, float cautiousSpeed = 1.5f)
    {
        if (agent != null)
        {
            agent.speed = cautiousSpeed; // Reducimos la velocidad
            MoveTo(destination);
        }
    }

    // Restablece la velocidad del agente a su valor por defecto
    public void ResetSpeed()
    {
        if (agent != null)
        {
            agent.speed = _defaultSpeed;
        }
    }
}