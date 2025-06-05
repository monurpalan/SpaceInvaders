using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    [Header("Invader Settings")]
    [SerializeField] private Invader[] invaderPrefab;
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 11;

    [Header("Missile Settings")]
    [SerializeField] private Projectile missilePrefab;
    [SerializeField] private float missileAttackRate = 1f;

    [Header("Movement Settings")]
    [SerializeField] private AnimationCurve speed;
    private Vector3 direction = Vector2.right;

    public int amountkilled { get; private set; }
    public int totalInvaders => rows * columns;
    public float percentKilled => (float)amountkilled / totalInvaders;
    public int amountAlive => totalInvaders - amountkilled;

    private void Awake()
    {
        InitializeInvaders();
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileAttackRate, missileAttackRate);
    }

    private void Update()
    {
        MoveInvaders();
        CheckScreenEdges();
    }

    private void InitializeInvaders()
    {
        for (int row = 0; row < rows; row++)
        {
            // Genişlik ve yükseklik, invader'ların düzenli bir gridde yerleştirilmesi için hesaplanıyor
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);
            // Grid'i merkeze hizalamak için başlangıç noktası (sol alt köşe) hesaplanıyor
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + row * 2f, 0);

            for (int column = 0; column < columns; column++)
            {
                Invader invader = Instantiate(invaderPrefab[row], transform);
                invader.killed += InvaderKilled;

                Vector3 position = rowPosition;
                position.x += column * 2f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void MoveInvaders()
    {
        // Hız, AnimationCurve üzerinden öldürülen invader oranına (percentKilled) bağlı olarak dinamik olarak belirleniyor
        float currentSpeed = speed.Evaluate(percentKilled);
        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    private void CheckScreenEdges()
    {
        // Ekranın sol ve sağ kenarları, kamera görünümünden dünya koordinatlarına çevriliyor
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) continue;

            if (direction == Vector3.right && invader.position.x >= rightEdge.x - 1f)
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= leftEdge.x + 1f)
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        direction.x *= -1f; // Yönü ters çevir
        Vector3 position = transform.position;
        position.y -= 1f; // Bir satır aşağı in
        transform.position = position;
    }

    private void MissileAttack()
    {
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) continue;

            // Rastgele bir invader'ın füze atma şansı, hayatta kalan invader sayısına bağlı olarak hesaplanıyor
            if (Random.value < (1.0f / amountAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                Debug.Log("Missile Fired!");
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        amountkilled++;
        if (amountkilled >= totalInvaders)
        {
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}