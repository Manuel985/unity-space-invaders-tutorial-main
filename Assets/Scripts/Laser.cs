using UnityEngine;

public class Laser : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        CheckBoundaryCollision(other);
        base.OnTriggerEnter2D(other);
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        CheckBoundaryCollision(other);
        base.OnTriggerStay2D(other);
    }

    private void CheckBoundaryCollision(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
            GameManager.Instance.PlayerMissInvaders();
    }
}
