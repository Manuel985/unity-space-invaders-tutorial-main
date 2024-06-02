using UnityEngine;

public class Projectile : ArtilleryObject
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        CheckEnemyMiss(other);
        base.OnTriggerEnter2D(other); // Call the base class method
    }

    protected void CheckEnemyMiss(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boundary") ||
            other.gameObject.layer == LayerMask.NameToLayer("Bunker"))
        {
            GameManager.Instance.OnProjectileMissEnemy();
        }
    }
}