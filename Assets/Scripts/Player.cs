using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    private const float speed = 5f;
    private Projectile laser;
    private Vector3 leftEdge;
    private Vector3 rightEdge;

    private void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    private void Update()
    {
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            position.x += speed * Time.deltaTime;
        }
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;
        if (laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }
}
