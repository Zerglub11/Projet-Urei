using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 400f;   // Vitesse de marche
    public float sprintSpeed = 600f; // Vitesse de course
    public float jumpForce = 15f;
    [Range(0, .3f)] public float m_MovementSmoothing = .05f; // Lissage du mouvement

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Ground Detection")]
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public bool isGrounded;

    // Variables internes
    private Vector3 velocity = Vector3.zero;
    private bool isJumping;
    private float horizontalInput;
    private bool isSprinting;

    void Update()
    {
        // 1. On récupère TOUS les inputs dans Update (plus réactif)
        horizontalInput = Input.GetAxisRaw("Horizontal"); // GetAxisRaw est souvent mieux pour les jeux 2D (arrêt plus net)

        // On vérifie si le joueur maintient Shift
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Détection du saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        // 2. Physique et Mouvement
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position);

        // Calcul de la vitesse cible
        float targetSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // ATTENTION : On ne multiplie PAS par Time.fixedDeltaTime ici pour la vélocité directe.
        // On envoie la vitesse voulue (Direction * Vitesse)
        float move = horizontalInput * targetSpeed;

        MovePlayer(move);

        // 3. Gestion du Flip et de l'Animation
        // On utilise la vitesse réelle du Rigidbody pour l'animation
        float actualSpeed = Mathf.Abs(rb.linearVelocity.x);

        Flip(rb.linearVelocity.x);
        // On définit une valeur simple pour l'Animator
        float stateValue = 0f;

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // Si on bouge, on est au moins à 1 (Marche)
            // Si on sprint, on passe à 2 (Course)
            stateValue = isSprinting ? 2f : 1f;
        }

        animator.SetFloat("Speed", stateValue);
    }

    void MovePlayer(float _targetVelocityX)
    {
        // On calcule la vélocité cible (on garde le Y actuel pour la gravité)
        Vector3 targetVelocity = new Vector2(_targetVelocityX * Time.fixedDeltaTime * 10f, rb.linearVelocity.y);

        // Note : Si tu trouves ton perso trop lent sans le Time.fixedDeltaTime précédent,
        // augmente simplement tes variables moveSpeed/sprintSpeed dans l'inspecteur.
        // C'est plus propre que de multiplier par le temps dans la formule de vélocité.

        // Application du SmoothDamp
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, m_MovementSmoothing);

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); // ForceMode2D.Impulse est mieux pour un saut instantané
            isJumping = false;
        }
    }

    void Flip(float _velocity)
    {
        // On utilise une petite marge (0.1f) pour éviter que ça clignote à l'arrêt
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
}