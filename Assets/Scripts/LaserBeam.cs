using UnityEngine;

public class LaserBeam : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            GameManager.Instance.PlaySfx(boom);
            Destroy(gameObject);
        }
    }
}
