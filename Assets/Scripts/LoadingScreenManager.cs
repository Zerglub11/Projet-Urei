using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private float minimumLoadingTime = 3f; // Durée minimale en secondes
    [SerializeField] private string gameSceneName = "GameScene"; // Nom de ta scène de jeu

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        float elapsedTime = 0f;

        // Lance le chargement en arrière-plan SANS activer la scène automatiquement
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
        asyncLoad.allowSceneActivation = false;

        // Attend que les deux conditions soient remplies :
        // 1. Le temps minimum est écoulé
        // 2. La scène est prête (progress >= 0.9f)
        while (elapsedTime < minimumLoadingTime || asyncLoad.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Active la scène une fois les conditions remplies
        asyncLoad.allowSceneActivation = true;
    }
}