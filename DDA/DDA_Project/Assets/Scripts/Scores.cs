using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scores : MonoBehaviour
{
    [SerializeField] private Player_Controller player;
    [SerializeField] private MoveToGoalAgent agent_ref;
    [SerializeField] private ParameterManager params_ref;
    private float player_score = 0f;
    private float agent_score = 0f;
    [SerializeField] TMP_Text score_text;
    [SerializeField] TMP_Text agent_score_text;
    [SerializeField] TMP_Text ready_set_go;
    [SerializeField] TMP_Text paused_text;
    [SerializeField] TMP_Text player_wins;
    [SerializeField] TMP_Text agent_wins;
    private int countdown = 3;
    private float duration = 3f;
    private float countdown_timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        countdown_timer = duration;
        paused_text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.getPaused())
        {
            paused_text.text = "Paused";
            paused_text.enabled = true;
            score_text.enabled = false;
            agent_score_text.enabled = false;
        }
        else
        {
            paused_text.enabled = false;
            score_text.enabled = true;
            agent_score_text.enabled = true;

            //If the counter is below zero then display the scores
            //Display the agent and player scores
            score_text.text = "Player Score: " + player.getScore().ToString("0");
            agent_score_text.text = "Agent Score: " + agent_ref.getScore().ToString("0");

            //Update the wins for each side
            player_wins.text = "Player Wins: " + params_ref.getPlayerWins();
            agent_wins.text = "Agent Wins: " + params_ref.getAgentWins();

        }

        if(!GameManager.getMove() && !GameManager.getPaused())
        {
            ready_set_go.enabled = true;

            //Countdown functionality for the race to commence!! :)
            if(countdown_timer > 0)
            {
                countdown_timer-= Time.deltaTime;

                int new_count_val = Mathf.CeilToInt(countdown_timer);

                if(new_count_val != countdown)
                {
                    countdown = new_count_val;
                    
                    if(countdown > 0)
                    {
                        //Set the message to countdown value
                        ready_set_go.text = countdown.ToString("0");
                        
                    }
                    else
                    {
                        ready_set_go.text = "Go!!!";

                        GameManager.setMove(true);

                        Invoke("DestroyObject", 1.5f);
                    }
                }
            }
        }
    }

    private void DestroyObject()
    {
        ready_set_go.enabled = false;
        countdown_timer = duration; //Reset back to true so the countdown starts everytime at a new round
    }
}
