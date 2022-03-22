using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (sceneIndex)
            {
                case 1:
                    break;
                case 2:
                    SavedPositionManager.savedPositions = new Vector2(
                         collision.transform.position.x,
                         collision.transform.position.y - 1f
                         );
                    break;
            }
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
