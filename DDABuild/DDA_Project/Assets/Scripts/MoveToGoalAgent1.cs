using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

//Inherit from Agents
public class MoveToGoalAgent1 : Agent
{

    // [SerializeField] private GameObject Goal;
    private float distance_to_target;
    private float prev_distance = 0f;
    private float smallest_distance;
    private bool started = false;

    private float epsilon = 0.2f;
    private System.Random random = new System.Random();

    [SerializeField] private Transform target_transform;
    [SerializeField] private Material win_material;
    [SerializeField] private Material lose_material;
    [SerializeField] private MeshRenderer floor_meshrenderer;

    private float start_score = 0f;
    
    public override void OnEpisodeBegin()
    {
        //Can reset parameters back to starting state
        started = false;
        start_score = 0f;
        transform.localPosition = new Vector3(86.3390121f,77.6307983f,-44.0005836f);
    }

    //How the agent observes its environment
    //What information does the agent need to do what it needs to learn
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target_transform.localPosition);

        //Be aware of the score
        sensor.AddObservation(start_score);

        //Add the distance to target
        sensor.AddObservation(distance_to_target);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float move_x;
        float move_z;

        //Epsilon Greedy Method
        if(random.NextDouble() < epsilon)
        {
            move_x = (float)random.NextDouble() * 2f - 1f;
            move_z = (float)random.NextDouble() * 2f - 1f;
        }
        else
        {
            move_x = actions.ContinuousActions[0];
            move_z = actions.ContinuousActions[1];
        }

        // float move_x = actions.ContinuousActions[0];
        // float move_z = actions.ContinuousActions[1];

        float speed = 5f;
        transform.localPosition += new Vector3 (move_x, 0f, move_z) * Time.deltaTime * speed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuous_actions = actionsOut.ContinuousActions;
        continuous_actions[0] = Input.GetAxisRaw("Horizontal");
        continuous_actions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other) 
    {
        //Calculate the score in relation to the distance to the goal
        if(other.gameObject.tag == "Goal")
        {
            //There is Add reward and SetReward functions
            start_score += 10f; 
            SetReward(start_score);
            
            floor_meshrenderer.material = win_material;
            //Doesn't end the game but resets the states so we can train again
            Debug.Log("Win");
            EndEpisode();
        }

        //Decrement score from agent if they keep touching the wall
        if(other.gameObject.tag == "Wall")
        {
            //start_score -= 1f;
            SetReward(-1f);
            floor_meshrenderer.material = lose_material;

            distance_to_target = Vector3.Distance(transform.localPosition, target_transform.localPosition);
            
            Debug.Log("Lost");

            EndEpisode();

            // if(start_score <= 0)
            // {
            //     Debug.Log("Lose");
            //     EndEpisode();
            // }

            // Debug.Log(start_score);
        }   
    }

    private void OnCollisionStay(Collision other) 
    {
        //Reward the agent for playing but this will incentivise them to stay away from the wall to maximise the score
        if(other.gameObject.tag == "Ground")
        {   
            distance_to_target = Vector3.Distance(transform.localPosition, target_transform.localPosition);
            
            //Initialise the smallest distance to the target to the current distance. Only reward the agent if they get closer to the goal once!
            //If they move away from the goal again then do not reward them.
            if(!started)
            {
                smallest_distance = distance_to_target;
                started = true;
            }
 
            if(distance_to_target < smallest_distance)
            {
                smallest_distance = distance_to_target;
                
                //We want to reward the agent for getting closer to the goal
                if(distance_to_target < prev_distance)
                {
                    start_score += 1f;
                    SetReward(start_score);
                    Debug.Log("Moving closer to goal!");
                }
                // else if(distance_to_target > prev_distance)
                // {
                //     start_score -= 1f;
                //     SetReward(start_score);
                //     Debug.Log("Moving away from the goal!");
                // }
            }

            if(distance_to_target > prev_distance)
            {
                start_score -= 0.1f;
                SetReward(start_score);
                Debug.Log("Moving away from the goal!");
            }

            
            prev_distance = distance_to_target;

            //Check
            // if(start_score <= 0)
            // {
            //     Debug.Log("Lose");
            //     EndEpisode();
            // }

        }   
    }
}
