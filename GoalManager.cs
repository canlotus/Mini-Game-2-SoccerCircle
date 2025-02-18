using UnityEngine;
using TMPro;
using System.Collections;

public class GoalManager : MonoBehaviour
{
    public static GoalManager instance;

    public int player1Score = 0;
    public int player2Score = 0;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI goalText; 
    private float initialBallDrag; 
    public Transform[] team1StartPositions;
    public Transform[] team2StartPositions;
    public ParticleSystem goalEffectLeft; 
    public ParticleSystem goalEffectRight; 
    public GameObject ball;

    private int firstStartingTeam = 0; 
    private bool goalScored = false; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        UpdateScoreUI();
        goalText.gameObject.SetActive(false); 

        initialBallDrag = ball.GetComponent<Rigidbody2D>().drag;
    }

    public void GoalScored(int scoringTeam)
    {
        if (goalScored) return;
        goalScored = true;

        if (scoringTeam == 1)
        {
            player2Score++;
            goalText.text = "GOAL! BLUE!";
            if (goalEffectLeft != null) goalEffectLeft.Play();
        }
        else
        {
            player1Score++;
            goalText.text = "GOAL! RED";
            if (goalEffectRight != null) goalEffectRight.Play(); 
        }

        goalText.gameObject.SetActive(true);
        UpdateScoreUI();
        ball.GetComponent<Rigidbody2D>().drag = 3f;

        if (player1Score >= 3 || player2Score >= 3)
        {
            GameOverManager.instance.ShowGameOverPanel(player1Score >= 3 ? "RED WINS!" : "BLUE WINS!");
        }

        StartCoroutine(ResetPositions());
    }

    private void StopAllPlayers()
    {
        foreach (GameObject player in TurnManager.instance.team1Players)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        foreach (GameObject player in TurnManager.instance.team2Players)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void UpdateScoreUI()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    private IEnumerator ResetPositions()
    {
        yield return new WaitForSeconds(1.5f);

        goalText.gameObject.SetActive(false);

        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();


        ballRb.drag = initialBallDrag;
        ballRb.velocity = Vector2.zero;
        ballRb.angularVelocity = 0f;
        ballRb.Sleep();
        yield return new WaitForSeconds(0.1f);
        ballRb.WakeUp();

        ball.transform.position = Vector3.zero;

        for (int i = 0; i < TurnManager.instance.team1Players.Count; i++)
        {
            TurnManager.instance.team1Players[i].transform.position = team1StartPositions[i].position;
            TurnManager.instance.team1Players[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        for (int i = 0; i < TurnManager.instance.team2Players.Count; i++)
        {
            TurnManager.instance.team2Players[i].transform.position = team2StartPositions[i].position;
            TurnManager.instance.team2Players[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        firstStartingTeam = (firstStartingTeam == 0) ? 1 : 0;
        TurnManager.instance.SetStartingTeam(firstStartingTeam);

        goalScored = false;
        Debug.Log("Gol sonrası sıfırlandı, goalScored: " + goalScored);
    }
}