using UnityEngine;
using UnityEngine.InputSystem;  // Добавлено для нового Input System

public class PlayerMovement : MonoBehaviour
{
    // Ссылка на CharacterController
    private CharacterController controller;

    // Переменные для движения
    public float walkSpeed = 5f;          // Скорость ходьбы
    public float runSpeed = 10f;          // Скорость бега (если зажать Shift)
    public float gravity = -9.81f;        // Гравитация
    public float jumpHeight = 2f;         // Высота прыжка
    public float mouseSensitivity = 100f; // Чувствительность мыши

    // Вектор движения и скорость
    private Vector3 velocity;
    private bool isGrounded;
    private float currentSpeed;

    // Ссылка на камеру (предполагаем, что она дочерняя к Player)
    private Transform playerCamera;

    // Новый Input System: ссылка на PlayerControls
    private InputSystem_Actions controls;

    // Переменная для вертикального угла камеры (pitch) — чтобы избежать проблем с EulerAngles
    private float cameraPitch = 0f;

    void Awake()
    {
        // Инициализируем контроллер
        controller = GetComponent<CharacterController>();
        playerCamera = transform.GetChild(0);  // Предполагаем, что камера - первый дочерний объект

        // Создаём и включаем PlayerControls
        controls = new InputSystem_Actions();
        controls.Enable();
    }

    void Update()
    {
        // Проверяем, на земле ли игрок
        isGrounded = controller.isGrounded;

        // Если на земле и скорость вниз - сбрасываем
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Маленькое значение, чтобы держать на земле
        }

        // Читаем ввод от нового Input System
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();  // WASD: x - лево/право, y - вперёд/назад
        Vector2 lookInput = controls.Player.Look.ReadValue<Vector2>();  // Мышь: x - поворот влево/вправо, y - вверх/вниз

        // Движение влево/вправо и вперёд/назад
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Определяем скорость: бег, если вертикальный ввод > 0.5, считаем бегом (или добавь отдельный Action для Run)
        currentSpeed = (moveInput.magnitude > 0.5f) ? runSpeed : walkSpeed;  // Простая логика для бега

        // Применяем движение
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Прыжок: если кнопка Jump нажата и на земле
        if (controls.Player.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);  // Формула для прыжка
        }

        // Применяем гравитацию
        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);

        // Поворот камеры (мышь)
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Поворот игрока по горизонтали (лево/право)
        transform.Rotate(Vector3.up * mouseX);

        // Поворот камеры по вертикали (вверх/вниз) — без ограничений, чтобы смотреть на потолок
        cameraPitch -= mouseY;  // Минус для стандартной инверсии (вверх — вверх). Если хочешь инвертировать, убери минус.
        // Убрали Clamp — теперь полный поворот!
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void OnDisable()
    {
        // Отключаем Input System при выключении скрипта
        controls.Disable();
    }
}