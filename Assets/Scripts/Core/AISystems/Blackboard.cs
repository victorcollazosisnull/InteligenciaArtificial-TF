// Blackboard.cs
// Pizarron de datos compartidos del NPC.
// Los nodos del BehaviorTree leen y escriben aqui
// sin necesitar referencias directas entre si.
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    [Header("Objetivos actuales")]
    public Transform enemyTarget;            // Enemigo detectado
    public Transform allyTarget;

    public Transform currentTarget;           // enemigo o presa en seguimiento

    public Vector3   lastKnownPosition;       // ultima posicion conocida del objetivo
    public bool      hasLastKnownPosition;

    [Header("Comida")]
    public Transform foodTarget;              // comida mas cercana encontrada

    [Header("Estados")]
    public bool      isHungry;               // true cuando el hambre supera el umbral
    public float     hunger = 0f;            // nivel de hambre actual (0-100)
    public bool      isDead;                 // sincronizado con HealthBase.IsDead

    [Header("Trampa (Cazador)")]
    public Vector3   trapPosition;           // posicion donde se coloco la trampa
    public bool      trapPlaced;             // true si hay una trampa activa
}
