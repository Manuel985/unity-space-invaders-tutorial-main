using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MysteryShip : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private GameObject powerUpPrefab;

    [SerializeField]
    private AudioClip explosion;

    private const float cycleTime = 30f;
    internal int score = 300;
    private Vector2 leftDestination;
    private Vector2 rightDestination;
    private const float offset = 1f;
    private int direction = -1;
    private bool spawned;

    private void Start()
    {
        CameraBounds();
        Despawn();
    }

    private void CameraBounds()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        leftDestination = new Vector2(leftEdge.x - offset, transform.position.y);
        rightDestination = new Vector2(rightEdge.x + offset, transform.position.y);
    }

    private void Update()
    {
        if (!spawned)
        {
            return;
        }
        if (direction == 1)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x >= rightDestination.x)
        {
            Despawn();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x <= leftDestination.x)
        {
            Despawn();
        }
    }

    private void Spawn()
    {
        direction *= -1;
        if (direction == 1)
        {
            transform.position = leftDestination;
        }
        else
        {
            transform.position = rightDestination;
        }
        spawned = true;
    }

    private void Despawn()
    {
        spawned = false;
        if (direction == 1)
        {
            transform.position = rightDestination;
        }
        else
        {
            transform.position = leftDestination;
        }
        Invoke(nameof(Spawn), cycleTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")
            || other.gameObject.layer == LayerMask.NameToLayer("LaserBeam"))
        {
            GameManager.Instance.PlaySfx(explosion);
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            Despawn();
            GameManager.Instance.OnMysteryShipKilled(this);
        }
    }
}
