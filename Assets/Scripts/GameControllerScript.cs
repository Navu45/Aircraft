using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public float fuel;
    public int points;
    public int spheres;
    public Rigidbody rb;
    public PlayerController playerController;
    public GameObject[] explosions;
    public Text displayParams;
    public GameObject canvasEnd;
    public GameObject sphere;
    public GameObject colliders;
    public GameObject restartButton;
    public bool IsGameOver;
    public bool isCrushed;
    public Text endText;
    public Text score;
    void Start()
    {
        fuel = 100f;
        points = 0;
        spheres = 0;

        for (int i = 0; i < 50; i++)
        {
            GameObject pointSphere = Instantiate(sphere);
            pointSphere.transform.position = rb.transform.position + new Vector3(Random.value * 1000 * (Random.value >= 0.5 ? 1 : -1), Random.value * 200 + 130, Random.value * 1000 * (Random.value >= 0.5 ? 1 : -1));
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isCrushed)
        {
            IsGameOver = true;
            playerController.isFuelOver = true;
            rb.isKinematic = true;
            foreach(GameObject explosion in explosions)
            {
                explosion.SetActive(true);
            }
            Time.timeScale = (Time.timeScale - 0.7f * Time.fixedDeltaTime) > 0 ? Time.timeScale - 0.7f * Time.fixedDeltaTime : 0;


            endText.text = "YOU LOSE.";
            score.text = "Score: " + points + '\n';
            score.text += "Collected spheres: " + spheres;

            restartButton.SetActive(true);
            canvasEnd.SetActive(true);            
        }
        else if (fuel == 0 && rb.velocity.magnitude < 0.5)
        {
            Time.timeScale = 0;
            IsGameOver = true;

            endText.text = "YOU WON!";
            score.text = "Score: " + points + '\n';
            score.text += "Collected spheres: " + spheres;
            restartButton.SetActive(true);
            canvasEnd.SetActive(true);

        }

        float gas = Input.GetKey(KeyCode.U) && playerController.isFlying ? 0.15f : 0;
        if (rb.velocity.magnitude > 1 && (Input.GetKey(KeyCode.U) || playerController.isFlying))
        {
            if (fuel > 0)
                fuel -= (0.05f + gas) * Time.fixedDeltaTime;
            else
            {
                playerController.isFuelOver = true;
                fuel = 0;
            }
        }
        displayParams.text = "Fuel: " + ((int)(fuel * 100) / 100) + "%\n";
        displayParams.text += "Velocity: " + ((int)(rb.velocity.magnitude * 100) / 100)  + "m/s\n";
        displayParams.text += "Points: " + points + "\n";
    }
}
