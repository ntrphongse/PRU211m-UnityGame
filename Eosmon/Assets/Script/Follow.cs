using UnityEngine;

public class Follow : MonoBehaviour
{

    public Transform player;

    // Update is called once per frame
    void Update()
    {
        if (player != null && transform != null)
        {
            transform.position = player.transform.position + new Vector3(0, 1, -5);
        }
    }
}