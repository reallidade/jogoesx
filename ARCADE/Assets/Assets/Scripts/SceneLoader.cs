using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para carregar cenas!

public class SceneLoader : MonoBehaviour
{
    public void LoadGameOverScene()
    {
        // Carrega a cena que adicionamos ao Build Settings
        SceneManager.LoadScene("GAMEOVER");
    }
}
