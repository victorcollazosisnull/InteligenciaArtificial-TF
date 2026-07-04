using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCamera : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;

    [Header("Mouse")]
    public float mouseSensitivity = 0.15f;

    [Header("Límites de Escenario")]
    [Tooltip("Distancia máxima permitida en X, Y, Z desde la posición inicial.")]
    public Vector3 limitesMaximos = new Vector3(50f, 30f, 50f);

    private float pitch;
    private float yaw;
    private Vector3 posicionInicial; // Guarda el punto de origen

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 rot = transform.eulerAngles;
        pitch = rot.x;
        yaw = rot.y;

        // Guardamos la posición donde inicia la cámara en la escena
        posicionInicial = transform.position;
    }

    void Update()
    {
        // 1. ROTACIÓN
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // 2. MOVIMIENTO
        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) move += transform.forward;
        if (Keyboard.current.sKey.isPressed) move -= transform.forward;
        if (Keyboard.current.dKey.isPressed) move += transform.right;
        if (Keyboard.current.aKey.isPressed) move -= transform.right;
        if (Keyboard.current.spaceKey.isPressed) move += Vector3.up;
        if (Keyboard.current.leftCtrlKey.isPressed) move -= Vector3.up;

        float speed = Keyboard.current.leftShiftKey.isPressed ? sprintSpeed : moveSpeed;

        // Calculamos la siguiente posición deseada
        Vector3 siguientePosicion = transform.position + (move.normalized * speed * Time.deltaTime);

        // 3. CONTROL DE LÍMITES (El "Freno" o Teletransporte)
        // Calculamos la distancia actual respecto al origen
        float distanciaX = Mathf.Abs(siguientePosicion.x - posicionInicial.x);
        float distanciaY = Mathf.Abs(siguientePosicion.y - posicionInicial.y);
        float distanciaZ = Mathf.Abs(siguientePosicion.z - posicionInicial.z);

        // Si supera el límite en cualquier eje, la devolvemos a la posición original
        if (distanciaX > limitesMaximos.x || distanciaY > limitesMaximos.y || distanciaZ > limitesMaximos.z)
        {
            transform.position = posicionInicial;
            Debug.LogWarning("¡Cámara devuelta al origen por alejarse demasiado del escenario!");
        }
        else
        {
            // Si está dentro de los límites, se mueve normalmente
            transform.position = siguientePosicion;
        }

        // 4. INPUT DE MOUSE Y ESCAPE
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Dibuja una caja visual en el editor de Unity para que veas los límites reales
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 centro = Application.isPlaying ? posicionInicial : transform.position;
        Gizmos.DrawWireCube(centro, limitesMaximos * 2f);
    }
}
