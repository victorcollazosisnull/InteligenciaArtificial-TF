using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCamera : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;

    [Header("Mouse")]
    public float mouseSensitivity = 0.15f;

    private float pitch;
    private float yaw;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 rot = transform.eulerAngles;
        pitch = rot.x;
        yaw = rot.y;
    }

    void Update()
    {
        // Rotación
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Movimiento
        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) move += transform.forward;
        if (Keyboard.current.sKey.isPressed) move -= transform.forward;
        if (Keyboard.current.dKey.isPressed) move += transform.right;
        if (Keyboard.current.aKey.isPressed) move -= transform.right;
        if (Keyboard.current.spaceKey.isPressed) move += Vector3.up;
        if (Keyboard.current.leftCtrlKey.isPressed) move -= Vector3.up;

        float speed = Keyboard.current.leftShiftKey.isPressed ? sprintSpeed : moveSpeed;

        transform.position += move.normalized * speed * Time.deltaTime;

        // Liberar/Bloquear cursor
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
}