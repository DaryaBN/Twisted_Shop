using UnityEngine;
using TMPro; // �������� ��� ������ ������

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance; // �������� ��� ������� �� ������ ��������

    public Transform player;  // ����� (�������� ���� ������ ������ � Inspector)
    public Rigidbody playerRigidbody;  // Rigidbody ������ (�������� ����, ���� ���� ������; ����� ������ ������)
    public Vector3 startPosition;  // ������� ������ (����� � Inspector)
    public Quaternion startRotation;  // ������� ������ (����� � Inspector)

    //public TextMesh levelTextMesh;  // ������ �� TextMesh �� ������� (�������� ���� ������ � TextMesh)
    public TextMeshPro levelTextMesh;

    // ������� ��� ������������ (������ ���� ������� �������� � Inspector)
    public GameObject[] shelves;  // ������ ����� ��� ���������/�����������
    public GameObject[] npcs;     // ������ NPC
    public GameObject[] items;    // ������ ���������

    public int currentLevel = 0;  // ��������� ������� 0 (������ ��������� ��� ������� �� ������ ��������)
    public int maxLevels = 10;    // �������� ������� (�����������)

    // ������ ���������� ��� �������� ������� (������ �� ���� ���������)
    public bool itemFound = false;  // ������ ���� true, ����� ������� �� ��������� �������

    void Awake()
    {
        // ��������� ���������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ����� ������� (���� �����)
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        GenerateLevel();  // ��������� ������ (��� 0 � ������ �� ������)
    }

    // ����� ��� �������� �� ��������� ������� (������� �� ��������, ���� ������� ���������)
    public void AdvanceLevel()
    {
        if (currentLevel < maxLevels)
        {
            currentLevel++;
            TeleportPlayerToStart();  // �������� ������
            GenerateLevel();          // ������������ ��� ������ ������ (��� 1+)
            UpdateUI();
            Debug.Log("������� �� ������� " + currentLevel);
        }
        else
        {
            Debug.Log("������������ ������� ���������!");  // ��� ������� ����
        }
    }

    // ����� ��� �������� ������ (������� ��� �������: �� ������ �������, ����� NPC � �.�.)
    public void RestartLevel()
    {
        TeleportPlayerToStart();  // �������� ��� ��������� ������
        GenerateLevel();          // ������������� (��� 0 � ������, ��� 1+ � ������)
        UpdateUI();
        Debug.Log("������� ������ " + currentLevel);
    }

    // ������������ ������ � ������ (���������� �� AdvanceLevel ��� RestartLevel)
    public void TeleportPlayerToStart()
    {
        // �������� CharacterController � ������
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            // �������� ��������� ����������
            controller.enabled = false;

            // ������������� ������� � �������
            player.position = startPosition;
            player.rotation = startRotation;

            // ���������� ���������� ��������� ����������� (velocity � �.�.), ����� �������� �����
            // (�� ������ �����, �� �������)
            // controller.velocity = Vector3.zero;  // ���� ���� velocity

            // �������� ���������� �������
            controller.enabled = true;

            Debug.Log("������������ �������: ������� " + player.position + ", ����� " + startPosition);
        }
        else
        {
            // ���� ��� �����������, ���������� ������ ��������� (��� �����)
            player.position = startPosition;
            player.rotation = startRotation;
            Debug.Log("������������ ��� �����������: ������� " + player.position);
        }
    }

    // ����� �����: ������������ ������ �� ������������ ������� (��� ������������ ��������)
    public void TeleportPlayerToPosition(Vector3 position)
    {
        // �������� CharacterController � ������
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            // �������� ��������� ����������
            controller.enabled = false;

            // ������������� ������� (������� �������, ���� �� ����� ������; ����� �������� Quaternion, ���� ����������)
            player.position = position;

            // ���������� ���������� ��������� ����������� (velocity � �.�.), ����� �������� �����
            // (�� ������ �����, �� �������)

            // �������� ���������� �������
            controller.enabled = true;

            Debug.Log("������������ �� ������� " + position + " �������");
        }
        else
        {
            // ���� ��� �����������, ���������� ������ ��������� (��� �����)
            player.position = position;
            Debug.Log("������������ ��� ����������� �� ������� " + position);
        }

        // ������� � ����� (currentLevel, itemFound) �� �������� � ��� "������������" �������
    }

    // ��������� ������: ������������ �������� (������ ��� ������� 1+)
    private void GenerateLevel()
    {
        // ��� ������ 0 � ������� ��������� (������ ������ �� ��������)
        if (currentLevel == 0)
        {
            return;  // ����� ��� ������������
        }

        // ��� ������� 1+: �������� ���������/����������� ����� (50% ����)
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

        // ���������� ��� NPC � ��������� (������ ���� ������)
        if (npcs != null)
        {
            foreach (var npc in npcs)
            {
                if (npc != null)
                {
                    npc.SetActive(Random.value > 0.5f);
                    // ����� �������� ��������� �������: npc.transform.position += new Vector3(Random.Range(-5, 5), 0, 0);
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

        // ����� �������� �������� ��� ������ ������
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