using UnityEngine;

// [REMOVA A LINHA "using CnControls;"]

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class pombo : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 10f;
    public Joystick joystick;

    [Header("Sons")]
    public AudioClip somDeDano;

    [Header("Tiro")]
    public GameObject projetilPrefab;
    public Transform pontoDeTiro;
    public float taxaDeTiro = 0.2f;

    private float moveInput = 0;
    private float proximoTiro = 0f;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private Vector3 escalaInicial;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        if (joystick != null && joystick.Horizontal != 0)
        {
            moveInput = joystick.Horizontal;
        }
        else
        {
            moveInput = Input.GetAxis("Horizontal");
        }


        if (Time.time > proximoTiro)
        {
            proximoTiro = Time.time + taxaDeTiro;
            Atirar();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0f)
        {
            animator.SetInteger("Move", 1);
        }
        else
        {
            animator.SetInteger("Move", 0);
        }

        if (moveInput > 0.01f)
        {
            transform.localScale = escalaInicial;
        }
        else if (moveInput < -0.01f)
        {
            transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);
        }
    }

    void Atirar()
    {
        Instantiate(projetilPrefab, pontoDeTiro.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool ehBola = collision.gameObject.layer == LayerMask.NameToLayer("Bolas");
        bool ehObstaculo = collision.gameObject.layer == LayerMask.NameToLayer("Obstaculos");

        if (ehBola || ehObstaculo)
        {
            if (somDeDano != null)
            {
                audioSource.PlayOneShot(somDeDano);
            }
            this.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Invoke("IniciarGameOver", 0.1f);
        }
    }

    void IniciarGameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGameOver();
        }
    }
}