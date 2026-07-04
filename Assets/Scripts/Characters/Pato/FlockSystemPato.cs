using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlockSystemPato : MonoBehaviour
{
    [Header("Flocking params")]
    public float neighborRadius = 3f;
    public float separationRadius = 2f;
    public float maxSpeed = 3.5f;

    [Range(0, 3)] public float separationWeight = 1.5f;
    [Range(0, 3)] public float alignmentWeight = 1.0f;
    [Range(0, 3)] public float cohesionWeight = 1.0f;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public bool HasNearbyPatos()
    {
        return GetNearbyPatos().Count > 0;
    }

    public Vector3 GetFlockMovePosition()
    {
        var nearby = GetNearbyPatos();

        if (nearby.Count == 0)
            return transform.position;

        Vector3 sep = CalculateSeparation(nearby);
        Vector3 ali = CalculateAlignment(nearby);
        Vector3 coh = CalculateCohesion(nearby);

        Vector3 desired = (sep * separationWeight) + (ali * alignmentWeight) + (coh * cohesionWeight);
        desired.y = 0f;

        if (desired.sqrMagnitude <= 0.01f)
            return transform.position;

        NavMeshHit hit;
        Vector3 samplePos = transform.position + desired.normalized * 1.5f;
        Vector3 finalPosition = transform.position;
        if (NavMesh.SamplePosition(samplePos, out hit, 2.5f, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private List<FlockSystemPato> GetNearbyPatos()
    {
        var result = new List<FlockSystemPato>();
        foreach (var pato in _allPatos)
        {
            if (pato == this || pato == null) continue;
            float dist = Vector3.Distance(transform.position, pato.transform.position);
            if (dist <= neighborRadius)
                result.Add(pato);
        }
        return result;
    }

    private Vector3 CalculateSeparation(List<FlockSystemPato> neighbors)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (var pato in neighbors)
        {
            float dist = Vector3.Distance(transform.position, pato.transform.position);
            if (dist <= separationRadius && dist > 0.01f)
            {
                Vector3 away = (transform.position - pato.transform.position).normalized;
                steer += away / dist;
                count++;
            }
        }

        if (count > 0)
            steer /= count;

        return steer.normalized;
    }

    private Vector3 CalculateAlignment(List<FlockSystemPato> neighbors)
    {
        Vector3 avgVelocity = Vector3.zero;
        int count = 0;

        foreach (var pato in neighbors)
        {
            NavMeshAgent otherAgent = pato.GetComponent<NavMeshAgent>();
            if (otherAgent != null && otherAgent.velocity.sqrMagnitude > 0.01f)
            {
                avgVelocity += otherAgent.velocity.normalized;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        avgVelocity /= count;
        return avgVelocity.normalized;
    }

    private Vector3 CalculateCohesion(List<FlockSystemPato> neighbors)
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        foreach (var pato in neighbors)
        {
            center += pato.transform.position;
            count++;
        }

        if (count == 0) return Vector3.zero;

        center /= count;
        Vector3 dir = center - transform.position;
        return dir.normalized;
    }

    private static List<FlockSystemPato> _allPatos = new List<FlockSystemPato>();

    private void OnEnable() { if (!_allPatos.Contains(this)) _allPatos.Add(this); }
    private void OnDisable() { _allPatos.Remove(this); }
}
