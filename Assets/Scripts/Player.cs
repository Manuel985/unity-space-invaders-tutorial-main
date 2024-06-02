using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private Projectile laserPrefab;

    [SerializeField]
    private Projectile laserBeamPrefab;

    [SerializeField]
    private AudioClip shooting;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float coolDownTime;

    private float minX, maxX;
    private const float offset = 0.85f;
    private float shootTimer;
    internal bool hasLaserPowerUp = false;

    private void Start()
    {
        CameraBounds();
    }

    private void CameraBounds()
    {
        Camera mainCamera = Camera.main;
        float halfWidth = mainCamera.aspect * mainCamera.orthographicSize;
        minX = -halfWidth + transform.localScale.x / offset;
        maxX = halfWidth - transform.localScale.x / offset;
    }

    private void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = Vector3.right * horizontalInput * speed * Time.deltaTime;
        Vector3 desiredPosition = transform.position + movement;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        transform.position = desiredPosition;
    }

    private void Shoot()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > coolDownTime && (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            shootTimer = 0f;
            Projectile projectile;
            if (hasLaserPowerUp)
            {
                projectile = laserBeamPrefab;
                hasLaserPowerUp = false;
            }
            else
            {
                projectile = laserPrefab;
            }
            GameManager.Instance.PlaySfx(shooting);
            Instantiate(projectile, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }
}
