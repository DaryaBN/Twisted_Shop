using UnityEngine;
using TMPro; // Добавьте эту строку вверху

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; // Синглтон для доступа из других скриптов

    public Transform player;  // Игрок (перетащи сюда объект игрока в Inspector)
    public Rigidbody playerRigidbody;  // Rigidbody игрока (перетащи сюда, если есть физика; иначе оставь пустым)
    public Vector3 startPosition;  // Позиция начала (задай в Inspector)
    public Quaternion startRotation;  // Поворот начала (задай в Inspector)

    //public TextMesh levelTextMesh;  // Ссылка на TextMesh на плакате (перетащи сюда объект с TextMesh)
    public TextMeshPro levelTextMesh;

    // Примеры для рандомизации (добавь свои массивы объектов в Inspector)
    public GameObject[] shelves;  // Массив полок для активации/деактивации
    public GameObject[] npcs;     // Массив NPC
    public GameObject[] items;    // Массив предметов

    public int currentLevel = 0;  // Начальный уровень 0 (теперь публичный для доступа из других скриптов)
    public int maxLevels = 10;    // Максимум уровней (опционально)

    // Пример переменной для проверки условий (замени на свой инвентарь)
    public bool itemFound = false;  // Должен быть true, чтобы перейти на следующий уровень

    void Awake()
    {
        // Настройка синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать между сценами (если нужно)
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        GenerateLevel();  // Генерация уровня (для 0 — ничего не меняет)
    }

    // Метод для перехода на следующий уровень (вызывай из триггера, если условия выполнены)
    public void AdvanceLevel()
    {
        if (currentLevel < maxLevels)
        {
            currentLevel++;
            TeleportPlayerToStart();  // Телепорт игрока
            GenerateLevel();          // Рандомизация для нового уровня (для 1+)
            UpdateUI();
            Debug.Log("Переход на уровень " + currentLevel);
        }
        else
        {
            Debug.Log("Максимальный уровень достигнут!");  // Или заверши игру
        }
    }

    // Метод для рестарта уровня (вызывай при неудаче: не найден предмет, атака NPC и т.д.)
    public void RestartLevel()
    {
        TeleportPlayerToStart();  // Телепорт без изменения уровня
        GenerateLevel();          // Перегенерация (для 0 — ничего, для 1+ — рандом)
        UpdateUI();
        Debug.Log("Рестарт уровня " + currentLevel);
    }

    // Телепортация игрока в начало (вызывается из AdvanceLevel или RestartLevel)
    public void TeleportPlayerToStart()
    {
        // Получаем CharacterController с игрока
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            // Временно отключаем контроллер
            controller.enabled = false;

            // Устанавливаем позицию и ротацию
            player.position = startPosition;
            player.rotation = startRotation;

            // Сбрасываем внутренние параметры контроллера (velocity и т.д.), чтобы избежать багов
            // (Не всегда нужно, но полезно)
            // controller.velocity = Vector3.zero;  // Если есть velocity

            // Включаем контроллер обратно
            controller.enabled = true;

            Debug.Log("Телепортация успешна: позиция " + player.position + ", старт " + startPosition);
        }
        else
        {
            // Если нет контроллера, используем прямую установку (для теста)
            player.position = startPosition;
            player.rotation = startRotation;
            Debug.Log("Телепортация без контроллера: позиция " + player.position);
        }
    }

    // НОВЫЙ МЕТОД: Телепортация игрока на произвольную позицию (для бесконечного коридора)
    public void TeleportPlayerToPosition(Vector3 position)
    {
        // Получаем CharacterController с игрока
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            // Временно отключаем контроллер
            controller.enabled = false;

            // Устанавливаем позицию (ротация остаётся, если не нужно менять; можно добавить Quaternion, если пригодится)
            player.position = position;

            // Сбрасываем внутренние параметры контроллера (velocity и т.д.), чтобы избежать багов
            // (Не всегда нужно, но полезно)

            // Включаем контроллер обратно
            controller.enabled = true;

            Debug.Log("Телепортация на позицию " + position + " успешна");
        }
        else
        {
            // Если нет контроллера, используем прямую установку (для теста)
            player.position = position;
            Debug.Log("Телепортация без контроллера на позицию " + position);
        }

        // Уровень и флаги (currentLevel, itemFound) не меняются — для "бесконечного" эффекта
    }

    // Генерация уровня: рандомизация объектов (только для уровней 1+)
    private void GenerateLevel()
    {
        // Для уровня 0 — никаких изменений (просто проход по коридору)
        if (currentLevel == 0)
        {
            return;  // Выход без рандомизации
        }

        // Для уровней 1+: рандомно активируй/деактивируй полки (50% шанс)
        if (shelves != null)
        {
            foreach (var shelf in shelves)
            {
                if (shelf != null)
                {
                    shelf.SetActive(Random.value > 0.5f);
                }
            }
        }

        // Аналогично для NPC и предметов (добавь свою логику)
        if (npcs != null)
        {
            foreach (var npc in npcs)
            {
                if (npc != null)
                {
                    npc.SetActive(Random.value > 0.5f);
                    // Можно добавить изменение позиции: npc.transform.position += new Vector3(Random.Range(-5, 5), 0, 0);
                }
            }
        }

        if (items != null)
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    item.SetActive(Random.value > 0.5f);
                }
            }
        }

        // Сброс проверки предмета для нового уровня
        itemFound = false;
    }

    private void UpdateUI()
    {
        if (levelTextMesh != null)
        {
            levelTextMesh.text = currentLevel.ToString();
        }
    }
}