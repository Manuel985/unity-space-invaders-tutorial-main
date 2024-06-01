using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    internal static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text livesText;

    [SerializeField]
    private AudioSource sfx;

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;
    private int score;
    private int lives;
    internal int Score => score;
    internal int Lives => lives;
    internal void PlaySfx(AudioClip clip) => sfx.PlayOneShot(clip);

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        gameOverUI.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);
        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }
        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = this.lives.ToString();
    }

    internal void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);
        player.gameObject.SetActive(false);
        if (lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    internal void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);
        SetScore(score + invader.score);
        if (invaders.GetAliveCount() == 0)
        {
            NewRound();
        }
    }

    internal void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        player.hasLaserPowerUp = true;
        SetScore(score + mysteryShip.score);
    }

    internal void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayerKilled(player);
        }
    }
}
