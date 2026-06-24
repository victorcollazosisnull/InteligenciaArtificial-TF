// Componente que marca un objeto como comida en el escenario.
// FindFood lo busca por OverlapSphere y EatFood lo consume.
using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Comida")]
    public float nutritionValue = 50f;  
    public string foodType      = "Generico";
}
