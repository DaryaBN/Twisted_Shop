using UnityEngine;
using UnityEngine.InputSystem;  // ��������� ��� ������ Input System

public class PlayerMovement : MonoBehaviour
{
    // ������ �� CharacterController
    private CharacterController controller;

    // ���������� ��� ��������
    public float walkSpeed = 5f;          // �������� ������
    public float runSpeed = 10f;          // �������� ���� (���� ������ Shift)
    public float gravity = -9.81f;        // ����������
    public float jumpHeight = 2f;         // ������ ������
    public float mouseSensitivity = 100f; // ���������������� ����

    // ������ �������� � ��������
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;

    // ������ �� ������ (������������, ��� ��� �������� � Player)
    private Transform playerCamera;

    // ����� Input System: ������ �� PlayerControls
    private InputSystem_Actions controls;

    // ���������� ��� ������������� ���� ������ (pitch) � ����� �������� ������� � EulerAngles
    private float cameraPitch = 0f;

    void Awake()
    {
        // �������������� ����������
        controller = GetComponent<CharacterController>();
        playerCamera = transform.GetChild(0);  // ������������, ��� ������ - ������ �������� ������

        // ������ � �������� PlayerControls
        controls = new InputSystem_Actions();
        controls.Enable();
    }

    void Update()
    {
        // ���������, �� ����� �� �����
        isGrounded = controller.isGrounded;

        // ���� �� ����� � �������� ���� - ����������
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // ��������� ��������, ����� ������� �� �����
        }

        // ������ ���� �� ������ Input System
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();  // WASD: x - ����/�����, y - �����/�����
        Vector2 lookInput = controls.Player.Look.ReadValue<Vector2>();  // ����: x - ������� �����/������, y - �����/����

        // �������� �����/������ � �����/�����
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // ���������� ��������: ���, ���� ������������ ���� > 0.5, ������� ����� (��� ������ ��������� Action ��� Run)
        currentSpeed = (moveInput.magnitude > 0.5f) ? runSpeed : walkSpeed;  // ������� ������ ��� ����

        // ��������� ��������
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // ������: ���� ������ Jump ������ � �� �����
        if (controls.Player.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // ������� ��� ������
        }

        // ��������� ����������
        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);

        // ������� ������ (����)
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // ������� ������ �� ����������� (����/�����)
        transform.Rotate(Vector3.up * mouseX);

        // ������� ������ �� ��������� (�����/����) � ��� �����������, ����� �������� �� �������
        cameraPitch -= mouseY;  // ����� ��� ����������� �������� (����� � �����). ���� ������ �������������, ����� �����.
        // ������ Clamp � ������ ������ �������!
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void OnDisable()
    {
        // ��������� Input System ��� ���������� �������
        controls.Disable();
    }
}