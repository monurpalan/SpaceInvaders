using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private Projectile projectilePrefab;

    private bool laserActive = false;
    private float minX;
    private float maxX;

    void Start()
    {
        CalculateScreenBounds();
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Move(Vector3.left);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            Move(Vector3.right);
        }

        ClampPosition();
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;
    }

    private void CalculateScreenBounds()
    {
        Vector3 screenBoundsLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenBoundsRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        minX = screenBoundsLeft.x;
        maxX = screenBoundsRight.x;
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!laserActive)
        {
            laserActive = true;
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // Lazer yok olduğunda OnLaserDestroyed çağrılacak, böylece tekrar ateş edilebilir
            projectile.destroyed += OnLaserDestroyed;
        }
    }

    private void OnLaserDestroyed()
    {
        laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsCollisionWithInvaderOrMissile(collision))
        {
            HandlePlayerHit();
        }
    }

    private bool IsCollisionWithInvaderOrMissile(Collider2D collision)
    {
        return collision.gameObject.layer == LayerMask.NameToLayer("Invaders") ||
               collision.gameObject.layer == LayerMask.NameToLayer("Missile");
    }

    private void HandlePlayerHit()
    {
        Debug.Log("Player hit by invader or missile!");
        ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}