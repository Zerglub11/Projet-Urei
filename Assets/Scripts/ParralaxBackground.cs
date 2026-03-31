using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform; // Glisse la Main Camera ici
    public float parallaxEffect;     // Valeur entre 0 et 1 (ex: 0.5f)
    
    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // On calcule de combien la caméra a bougé
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        // On déplace le fond d'une fraction de ce mouvement
        // Plus parallaxEffect est petit, plus le fond bouge lentement
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, deltaMovement.y * parallaxEffect, 0);
        
        lastCameraPosition = cameraTransform.position;
    }
}