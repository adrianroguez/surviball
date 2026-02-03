using UnityEngine;

public class GameCamera : MonoBehaviour
{
    private Vector3 cameraTarget;
    private Transform target;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogError("¡No encuentro al Player! Asegúrate de ponerle el Tag 'Player' en el inspector.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            cameraTarget = new Vector3(target.position.x, transform.position.y, target.position.z);

            transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.deltaTime * 8);
        }
    }
}