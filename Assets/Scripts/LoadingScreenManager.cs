using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Scène")]
    [SerializeField] private string gameSceneName = "GameScene"; // ← Nom de ta scène de destination
    [SerializeField] private float minimumLoadingTime = 3f;

    [Header("UI")]
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private TextMeshProUGUI percentText;

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        float elapsedTime = 0f;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
        asyncLoad.allowSceneActivation = false;

        while (elapsedTime < minimumLoadingTime || asyncLoad.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;

            // Progression combinée : temps + chargement réel
            float timeProgress = elapsedTime / minimumLoadingTime;
            float loadProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float displayedProgress = Mathf.Min(timeProgress, loadProgress); // Le plus lent des deux
            displayedProgress = Mathf.Clamp01(displayedProgress);

            // Met à jour l'UI
            loadingBarFill.fillAmount = displayedProgress;
            percentText.text = Mathf.RoundToInt(displayedProgress * 100f) + "%";

            yield return null;
        }

        // S'assure que la barre est bien à 100% avant de switcher
        loadingBarFill.fillAmount = 1f;
        percentText.text = "100%";
        yield return new WaitForSeconds(0.2f);

        asyncLoad.allowSceneActivation = true;
    }
}