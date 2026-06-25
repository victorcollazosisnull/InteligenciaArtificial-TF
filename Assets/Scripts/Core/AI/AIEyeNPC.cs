// AIEyeNPC.cs
// Sensor de vision del NPC.
// Almacena referencias al enemigo y aliado mas cercano detectado.
// Los nodos del BehaviorTree leen Enemy y Allied para tomar decisiones.
// La logica de deteccion (cono de vision, raycast) se implementa
// en una subclase o en un componente separado que escriba aqui.
using UnityEngine;

public class AIEyeNPC : MonoBehaviour
{
    public Transform Enemy;    // enemigo detectado actualmente
    public Transform Allied;   // aliado detectado actualmente
}
