using UnityEngine;

public class StartOfCorridorTrigger : MonoBehaviour
{
    // “очка телепортации: установите это в инспекторе на небольшое рассто€ние вперЄд по коридору (например, на 5-10 метров дальше)
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ”бедитесь, что teleportTarget установлен и LevelManager готов
            if (teleportTarget != null && LevelManager.Instance != null)
            {
                // “елепортировать игрока к цели через LevelManager (без изменений уровн€/флагов)
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