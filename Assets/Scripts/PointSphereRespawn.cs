using UnityEngine;

public class PointSphereRespawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameControllerScript gameController;
    public Collider airplane;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        gameController.points += 100;
        if (other.Equals(airplane))
        {
            gameController.spheres += 1;
            Destroy(transform.gameObject);
        }
    }
}
