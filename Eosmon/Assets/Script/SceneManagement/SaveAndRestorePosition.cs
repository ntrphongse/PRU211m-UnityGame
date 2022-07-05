using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndRestorePosition : MonoBehaviour
{
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.name.Contains("Room"))
        {
            if (SavedPositionManager.savedPositions != new Vector2(-0.25f, -0.5f))
            {
                transform.position = SavedPositionManager.savedPositions;
            }
        }

    }
}