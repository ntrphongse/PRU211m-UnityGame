using UnityEngine;
using UnityEngine.SceneManagement;
 
public class SaveAndRestorePosition : MonoBehaviour
{
    void Start() 
    {
        transform.position = SavedPositionManager.savedPositions;
    }
}