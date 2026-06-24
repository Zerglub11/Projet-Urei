using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string[] loadingScreenNames = { "LoadingScreen1", "LoadingScreen2" };

    public void PlayGame()
    {
        int randomIndex = Random.Range(0, loadingScreenNames.Length);
        SceneManager.LoadSceneAsync(loadingScreenNames[randomIndex]);
    }
}