using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class DeclencheurCinematique : MonoBehaviour
{
    [Header("Références")]
    public VideoPlayer lecteurVideo;
    public RawImage ecranDAffichage;
    public GameObject interfaceJeu;

    private bool aEteJoue = false;
    private bool cinematiqueEnCours = false;

    void Start()
    {
        lecteurVideo.loopPointReached += FinDeLaVideo;
    }

    void Update()
    {
        // Si la vidéo tourne et que le joueur appuie sur Espace
        if (cinematiqueEnCours && Input.GetKeyDown(KeyCode.Space))
        {
            PasserCinematique();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !aEteJoue)
        {
            aEteJoue = true;
            LancerCinematique();
        }
    }

    void LancerCinematique()
    {
        cinematiqueEnCours = true;
        if(interfaceJeu != null) interfaceJeu.SetActive(false);
        
        // Si tu utilises toujours la RawImage
        if(ecranDAffichage != null) ecranDAffichage.enabled = true;

        lecteurVideo.Play();
    }

    void PasserCinematique()
    {
        // On force l'arrêt de la vidéo, ce qui appellera automatiquement FinDeLaVideo
        lecteurVideo.Stop();
        FinDeLaVideo(lecteurVideo);
    }

    void FinDeLaVideo(VideoPlayer vp)
    {
        cinematiqueEnCours = false;
        
        if(ecranDAffichage != null) ecranDAffichage.enabled = false;
        if(interfaceJeu != null) interfaceJeu.SetActive(true);
        
        Debug.Log("Cinématique terminée ou passée.");
    }
}