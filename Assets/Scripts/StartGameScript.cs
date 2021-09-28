using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public GameObject canvasStart;
    public GameObject canvasGame;
    public GameObject canvasEnd;
    public GameObject continueButton;
    public GameObject player;
    // Start is called before the first frame update
    public void Switch()
    {
        canvasStart.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;
        canvasGame.SetActive(true);
    }

    public void GameOver()
    {
        Application.Quit();
    }

    public void Continue()
    {
        continueButton.SetActive(false);
        canvasEnd.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
        Time.timeScale = 1;
    }
}
