using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;

    public System.Action destroyed;

    void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision();
    }

    private void HandleCollision()
    {
        NotifyDestroyed();
        DestroyProjectile();
    }

    private void NotifyDestroyed()
    {
        destroyed?.Invoke();
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}