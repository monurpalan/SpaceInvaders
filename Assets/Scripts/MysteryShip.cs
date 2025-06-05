using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    [Header("Mystery Ship Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private AudioClip deathSound;

    private Vector3 direction = Vector3.right;

    void Update()
    {
        Move();
        CheckScreenBounds();
    }

    private void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void CheckScreenBounds()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

        // Ekranın sağından çıkarsa sola dön
        if (transform.position.x > rightEdge.x)
        {
            direction = Vector3.left;
        }
        // Ekranın solundan çıkarsa sağa dön
        else if (transform.position.x < leftEdge.x)
        {
            direction = Vector3.right;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        PlayDeathSound();
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position); // Ölüm sesini çal
        }
    }
}