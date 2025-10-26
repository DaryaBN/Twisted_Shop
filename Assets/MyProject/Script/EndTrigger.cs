using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger. Current level: " + LevelManager.Instance.currentLevel + ", itemFound: " + LevelManager.Instance.itemFound);  // ������� ��� ��������

            if (LevelManager.Instance != null)
            {
                if (LevelManager.Instance.currentLevel == 0 || LevelManager.Instance.itemFound)  // ������� 0: ������ Advance; ����� �������� itemFound
                {
                    Debug.Log("Advancing level!");  // �������
                    LevelManager.Instance.AdvanceLevel();
                }
                else
                {
                    Debug.Log("Restarting level!");  // �������
                    LevelManager.Instance.RestartLevel();
                }
            }
        }
    }
}