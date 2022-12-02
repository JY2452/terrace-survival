using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public void Setup(string message)
    {
        gameObject.SetActive(true);
        this.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
