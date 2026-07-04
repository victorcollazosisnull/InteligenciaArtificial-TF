using UnityEngine;
using UnityEngine.AI; // Obligatorio para detectar el NavMesh
using System.Collections.Generic;

public class FoodSpawner : MonoBehaviour
{
    [Header("Configuración de Comida")]
    [Tooltip("El prefab de la comida que se va a spawnear.")]
    public GameObject foodPrefab;

    [Tooltip("El agente asignado en el script Food (Orco, Humano, etc.).")]
    public TypeAgent targetAgentForArea = TypeAgent.Orco;

    [Header("Límites del Spawner")]
    [Tooltip("Cantidad máxima de comida permitida en esta área a la vez.")]
    public int maxFoodCount = 10;

    [Tooltip("Radio del área circular desde la posición de este objeto donde puede aparecer comida.")]
    public float spawnRadius = 20f;

    [Header("Tiempos de Spawn")]
    [Tooltip("Tiempo en segundos entre cada intento de aparición de comida.")]
    public float spawnInterval = 5f;

    [Header("Evitar Amontonamiento")]
    public float distanciaMinimaEntreComida = 2f;
    // NUEVO: Esto le dice a la esfera qué capa debe buscar ignorando el suelo
    public LayerMask capaComida;

    // Lista interna para rastrear qué comidas siguen activas en el mundo
    private List<GameObject> activeFoodList = new List<GameObject>();
    private float timer;

    void Start()
    {
        // Forzamos un primer intento de spawn inmediato al arrancar el juego
        TrySpawnFood();
    }

    void Update()
    {
        // Temporizador simple e independiente para no spawnear a cada rato
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawnFood();
        }
    }

    void TrySpawnFood()
    {
        activeFoodList.RemoveAll(item => item == null);
        if (activeFoodList.Count >= maxFoodCount) return;

        Vector3 randomPosition = GetRandomNavMeshPosition();

        if (randomPosition != Vector3.zero)
        {
            Vector3 posicionConAltura = new Vector3(randomPosition.x, 0.5f, randomPosition.z);

            // MODIFICADO: Ahora CheckSphere solo chocará si toca elementos en la "capaComida"
            if (Physics.CheckSphere(posicionConAltura, distanciaMinimaEntreComida, capaComida))
            {
                return; // Hay otra comida cerca, cancelamos
            }

            GameObject newFood = Instantiate(foodPrefab, posicionConAltura, Quaternion.identity);

            Food foodComponent = newFood.GetComponent<Food>();
            if (foodComponent != null)
            {
                foodComponent.targetAgent = targetAgentForArea;
            }

            activeFoodList.Add(newFood);
        }
    }


    Vector3 GetRandomNavMeshPosition()
    {
        // Generamos un punto aleatorio dentro de una esfera usando el radio configurado
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position; // Centrado en la posición de este Spawner

        NavMeshHit hit;
        // Buscamos el punto más cercano en el NavMesh dentro del rango del radio.
        // El '1' o NavMesh.AllAreas indica que busque en cualquier capa caminable del NavMesh
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position; // Retorna la coordenada exacta pegada al suelo del NavMesh
        }

        return Vector3.zero; // Si falló en encontrar NavMesh, retorna un vector nulo
    }

    // Dibuja un círculo azul en la pestaña Scene para que veas el área real del Spawner
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        // Dibuja una línea de cables esférica para previsualizar el rango
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
