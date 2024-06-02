using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class LaserBeam : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collider2D other)
    {
        GameManager.Instance.PlaySfx(boom);
        if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
            Destroy(gameObject);
    }
}
