using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float speed;

    [SerializeField]
    protected AudioClip boom;

    private new BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();
        if (bunker == null || bunker.CheckCollision(collider, transform.position))
        {
            GameManager.Instance.PlaySfx(boom);
            Destroy(gameObject);
        }
    }
}
