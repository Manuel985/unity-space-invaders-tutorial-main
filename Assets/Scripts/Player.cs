using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private Projectile laserPrefab;

    [SerializeField]
    private GameObject laserBeamPrefab;

    private const float speed = 5f;
    private float minX, maxX;
    private const float offset = 0.85f;
    private float shootTimer;
    private const float coolDownTime = 0.5f;
    private bool hasLaserPowerUp = false;

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
            if (hasLaserPowerUp)
            {
                Instantiate(laserBeamPrefab, transform.position, Quaternion.identity);
                hasLaserPowerUp = false;
            }
            else
            {
                Instantiate(laserPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameManager.Instance.OnPlayerKilled(this);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("PowerUp"))
        {
            hasLaserPowerUp = true;
        }
    }
}
