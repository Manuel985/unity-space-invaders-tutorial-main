using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    [SerializeField]
    private Sprite[] animationSprites;

    [SerializeField]
    private AudioClip explosion;

    internal int score;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    private const float animationTime = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = animationSprites[0];
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    private void AnimateSprite()
    {
        animationFrame = (animationFrame + 1) % animationSprites.Length;
        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser")
            || other.gameObject.layer == LayerMask.NameToLayer("LaserBeam"))
        {
            GameManager.Instance.PlaySfx(explosion);
            GameManager.Instance.OnInvaderKilled(this);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            GameManager.Instance.OnBoundaryReached();
        }
    }
}
