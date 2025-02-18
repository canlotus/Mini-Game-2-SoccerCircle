using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public int scoringTeam; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger tetiklendi: " + collision.gameObject.name);

        if (collision.CompareTag("Ball")) 
        {
            Debug.Log("GOL! " + (scoringTeam == 1 ? "Player 2" : "Player 1") + " puan kazandı!");
            GoalManager.instance.GoalScored(scoringTeam);
        }
    }
}