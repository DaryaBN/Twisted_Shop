using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger. Current level: " + LevelManager.Instance.currentLevel + ", itemFound: " + LevelManager.Instance.itemFound);  // Отладка для проверки

            if (LevelManager.Instance != null)
            {
                if (LevelManager.Instance.currentLevel == 0 || LevelManager.Instance.itemFound)  // Уровень 0: всегда Advance; иначе проверка itemFound
                {
                    Debug.Log("Advancing level!");  // Отладка
                    LevelManager.Instance.AdvanceLevel();
                }
                else
                {
                    Debug.Log("Restarting level!");  // Отладка
                    LevelManager.Instance.RestartLevel();
                }
            }
        }
    }
}