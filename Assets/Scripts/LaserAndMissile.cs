using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class LaserAndMissile : Projectile
{
    [SerializeField]
    private AudioClip explosion;

    private new BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        CheckBunkerCollision(other);
        base.OnTriggerEnter2D(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckBunkerCollision(other);
        Destroy(gameObject);
    }

    private void CheckBunkerCollision(Collider2D other)
    {
        Bunker bunker = other.gameObject.GetComponent<Bunker>();
        if (bunker != null && bunker.CheckCollision(collider, transform.position))
        {
            GameManager.Instance.PlaySfx(explosion);
        }
    }
}
