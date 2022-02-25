using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void RestartingGame(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
