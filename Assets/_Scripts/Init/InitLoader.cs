using UnityEngine;
using UnityEngine.SceneManagement;

public class InitLoader : MonoBehaviour
{
   private const int MAIN_MENU_BUILD_INDEX = 1;

    void Start()
    {
        Invoke(nameof(LoadMainMenu), 0.1f); // load main menu after player data initialized   
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_BUILD_INDEX);
    }
}
