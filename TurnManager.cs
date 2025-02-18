using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    private int currentPlayerTeam = 0; 
    public List<GameObject> team1Players;
    public List<GameObject> team2Players;
    private bool turnShouldChange = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        UpdateHalos(); 
    }

    void Update()
    {
        if (turnShouldChange && AllPlayersStopped())
        {
            SwitchTurn();
            turnShouldChange = false;
        }
    }

    bool AllPlayersStopped()
    {
        List<GameObject> allPlayers = new List<GameObject>();
        allPlayers.AddRange(team1Players);
        allPlayers.AddRange(team2Players);

        foreach (GameObject player in allPlayers)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb.velocity.magnitude > 0.1f)
            {
                return false;
            }
        }
        return true;
    }

    public void MarkTurnChange()
    {
        turnShouldChange = true; 
        ToggleHalos(false); 
    }

    public void SwitchTurn()
    {
        currentPlayerTeam = (currentPlayerTeam == 0) ? 1 : 0;
        Debug.Log("Şimdi " + (currentPlayerTeam == 0 ? "Player 1 Takımı" : "Player 2 Takımı") + " oynuyor.");
        UpdateHalos();
    }

    public void SetStartingTeam(int startingTeam)
    {
        currentPlayerTeam = startingTeam; 
        Debug.Log("Başlangıçta kim oynuyorsa her golde değişti: " + (currentPlayerTeam == 0 ? "Player 1" : "Player 2"));
        UpdateHalos();
    }

    public bool IsPlayerTurn(GameObject player)
    {
        return (currentPlayerTeam == 0 && team1Players.Contains(player)) ||
               (currentPlayerTeam == 1 && team2Players.Contains(player));
    }

    private void UpdateHalos()
    {
        ToggleHalos(false);

        List<GameObject> currentTeam = (currentPlayerTeam == 0) ? team1Players : team2Players;
        foreach (GameObject player in currentTeam)
        {
            Transform halo = player.transform.Find("Halo");
            if (halo != null)
            {
                halo.gameObject.SetActive(true);
            }
        }
    }

    private void ToggleHalos(bool isActive)
    {
        List<GameObject> allPlayers = new List<GameObject>();
        allPlayers.AddRange(team1Players);
        allPlayers.AddRange(team2Players);

        foreach (GameObject player in allPlayers)
        {
            Transform halo = player.transform.Find("Halo");
            if (halo != null)
            {
                halo.gameObject.SetActive(isActive);
            }
        }
    }
}