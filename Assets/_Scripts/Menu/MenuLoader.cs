using UnityEngine;
using UnityEngine.AddressableAssets;

public class MenuLoader : MonoBehaviour
{
    private string gameScenePath = "Assets/Scenes/Game.unity";
    public void LoadGameAsync()
    {
        Addressables.LoadSceneAsync(gameScenePath);
    }
}
