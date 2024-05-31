using System.Collections;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [SerializeField]
    private Invader[] prefabs;

    [SerializeField]
    private AnimationCurve speed;

    [SerializeField]
    private AudioClip shooting;

    [SerializeField]
    private LayerMask playerAndBunkerLayer;

    [SerializeField]
    private Projectile missilePrefab;

    private Vector3 direction = Vector3.right;
    private Vector3 initialPosition;
    private const int rows = 5;
    private const int columns = 11;
    private const int totalCount = rows * columns;
    private const float missileSpawnRate = 1f;
    private const float offset = 1f;
    private Vector3 leftEdge;
    private Vector3 rightEdge;
    private Vector3 bottomEdge;

    private void Awake()
    {
        initialPosition = transform.position;
        CreateInvaderGrid();
    }

    private void CreateInvaderGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);
            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);
            for (int j = 0; j < columns; j++)
            {
                Invader invader = Instantiate(prefabs[i], transform);
                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        CameraBound();
        InvokeRepeating(nameof(Shoot), missileSpawnRate, missileSpawnRate);
        InvokeRepeating(nameof(HealInvaders), 0f, 5f);
    }

    private void CameraBound()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        bottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));
    }

    private void Shoot()
    {
        int amountAlive = GetAliveCount();
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            float raycastDistance = invader.position.y - bottomEdge.y;
            RaycastHit2D hit = Physics2D.Raycast(invader.position, Vector2.down, raycastDistance, playerAndBunkerLayer);
            if (hit.collider != null && Random.value < (1f / amountAlive))
            {
                GameManager.Instance.PlaySfx(shooting);
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void HealInvaders()
    {
        int amountAlive = GetAliveCount();
        if (amountAlive < 5)
        {
            return;
        }

        Transform[] activeInvaders = new Transform[amountAlive];
        int index = 0;
        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeInHierarchy)
            {
                activeInvaders[index++] = invader;
            }
        }

        int randomIndex = Random.Range(0, amountAlive);
        Transform randomInvader = activeInvaders[randomIndex];

        SpriteRenderer spriteRenderer = randomInvader.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
        }

        StartCoroutine(ActivateAdjacentInvaders(randomInvader, 5f));
    }

    private IEnumerator ActivateAdjacentInvaders(Transform invader, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (invader.gameObject.activeInHierarchy)
        {
            Vector3 invaderPosition = invader.localPosition;
            Vector3[] adjacentOffsets = new Vector3[]
            {
            new Vector3(2f, 0f, 0f),   // Destra
            new Vector3(-2f, 0f, 0f),  // Sinistra
            new Vector3(0f, -2f, 0f)   // Sotto
            };

            foreach (Vector3 offset in adjacentOffsets)
            {
                Vector3 adjacentPosition = invaderPosition + offset;
                foreach (Transform potentialInvader in transform)
                {
                    if (potentialInvader.localPosition == adjacentPosition && !potentialInvader.gameObject.activeSelf)
                    {
                        potentialInvader.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

        SpriteRenderer spriteRenderer = invader.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = (float)amountKilled / (float)totalCount;
        float speed = this.speed.Evaluate(percentKilled);
        transform.position += speed * Time.deltaTime * direction;
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if ((direction == Vector3.right && invader.position.x >= (rightEdge.x - offset))
                || (direction == Vector3.left && invader.position.x <= (leftEdge.x + offset)))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        direction = new Vector3(-direction.x, 0f, 0f);
        Vector3 position = transform.position;
        position.y -= offset;
        transform.position = position;
    }

    internal void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;
        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    internal int GetAliveCount()
    {
        int count = 0;
        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}
