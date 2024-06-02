using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ArtilleryObject : MonoBehaviour
{
    [SerializeField]
    protected Vector3 direction;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected AudioClip boom;

    protected new BoxCollider2D collider;

    protected virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
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

    protected virtual void CheckCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();
        if (bunker == null || bunker.CheckCollision(collider, transform.position))
        {
            GameManager.Instance.PlaySfx(boom);
            Destroy(gameObject);
        }
    }
}