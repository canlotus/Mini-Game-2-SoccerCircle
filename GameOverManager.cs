using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;


    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;


    public GameObject pausePanel;


    public GameObject startPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        gameOverPanel.SetActive(false); 
        pausePanel.SetActive(false); 
        startPanel.SetActive(true); 

 
        Time.timeScale = 0f;
    }

    public void ShowGameOverPanel(string winner)
    {
        gameOverPanel.SetActive(true);
        winnerText.text = winner;
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void OpenPauseMenu()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ClosePauseMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}