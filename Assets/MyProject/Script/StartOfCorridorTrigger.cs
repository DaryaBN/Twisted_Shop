using UnityEngine;

public class StartOfCorridorTrigger : MonoBehaviour
{
    // ����� ������������: ���������� ��� � ���������� �� ��������� ���������� ����� �� �������� (��������, �� 5-10 ������ ������)
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ���������, ��� teleportTarget ���������� � LevelManager �����
            if (teleportTarget != null && LevelManager.Instance != null)
            {
                // ��������������� ������ � ���� ����� LevelManager (��� ��������� ������/������)
                LevelManager.Instance.TeleportPlayerToPosition(teleportTarget.position);
                Debug.Log("Player teleported forward in the endless corridor.");
            }
            else
            {
                Debug.LogError("TeleportTarget or LevelManager not set!");
            }
        }
    }
}