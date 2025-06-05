using UnityEngine;

public class Invader : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private float animationTime = 1f;

    [Header("Death Settings")]
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private AudioClip deathSound;

    public System.Action killed;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private int animationFrame;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartAnimation();
    }

    private void StartAnimation()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    private void StopAnimation()
    {
        CancelInvoke(nameof(AnimateSprite));
    }

    private void AnimateSprite()
    {
        animationFrame = (animationFrame + 1) % animationSprites.Length;
        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            killed?.Invoke();
            StartCoroutine(HandleDeath());
        }
    }

    private System.Collections.IEnumerator HandleDeath()
    {
        StopAnimation();
        spriteRenderer.sprite = deathSprite;

        PlayDeathSound();

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            Debug.Log("Ölüm sesi çalınıyor");
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("Death sound or AudioSource is missing!");
        }
    }
}