using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class BallBalanceAgent : Agent
{
    //Variables
    [SerializeField] private GameObject ball;
    private Rigidbody ball_rb;
    private EnvironmentParameters default_params;

    //Function overrides
    //Similar to Start but called slightly earlier.
    public override void Initialize()
    {
        Time.timeScale = 5f;
        //Initialise variables
        ball_rb = ball.GetComponent<Rigidbody>();
        default_params = Academy.Instance.EnvironmentParameters;

        //Reset here to be safe
        ResetScene();

        //base.Initialize();
    }

    //Where we send observations to the Academy.
    public override void CollectObservations(VectorSensor sensor)
    {
        //Telling the Academy to observe the values here.
        //We need the agent to be aware of the ball velocity, rotation and position as well as the cube rotation on the X and Z axes.
        sensor.AddObservation(ball_rb.velocity);
        sensor.AddObservation(ball.transform.position);
        sensor.AddObservation(transform.rotation.z);
        sensor.AddObservation(transform.rotation.x);

        //base.CollectObservations(sensor);
    }

    //Where we get and action from the Academy.
    public override void OnActionReceived(ActionBuffers actions)
    {
        ActionSegment<float> angles = actions.ContinuousActions;
        var z_angle = 2f * Mathf.Clamp(angles[0], -1f, 1f);
        var x_angle = 2f * Mathf.Clamp(angles[1], -1f, 1f);

        //Prevent the agent rotating the cube past 25 degrees on either axis.
        if((gameObject.transform.rotation.z < 0.25f && z_angle > 0f)||
        (gameObject.transform.rotation.z > -0.25f && z_angle < 0f))
        {
            Debug.Log(gameObject.transform.rotation);
            gameObject.transform.Rotate(new Vector3(0,0,1), z_angle);
        }

        if((gameObject.transform.rotation.x < 0.25f && x_angle > 0f)||
        (gameObject.transform.rotation.x > -0.25f && x_angle < 0f))
        {
            Debug.Log(gameObject.transform.rotation);
            gameObject.transform.Rotate(new Vector3(1,0,0), x_angle);
        }

        //Check if the ball has fallen off the cube
        if((ball.transform.position.y - gameObject.transform.position.y) <  -2f ||
        Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
        Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f)
        {
            SetReward(-3f);
            EndEpisode();
        }
        else
        {
            SetReward(0.5f);
        }

        //base.OnActionReceived(actions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuous_actions = actionsOut.ContinuousActions;
        continuous_actions[0] = -Input.GetAxis("Horizontal");
        continuous_actions[1] = Input.GetAxis("Vertical");

        //base.Heuristic(actionsOut);

    }

    //Called whenever a new Episode begins
    public override void OnEpisodeBegin()
    {
        gameObject.transform.rotation = new Quaternion(0f,0f,0f,0f);
        gameObject.transform.Rotate(new Vector3(1,0,0), Random.Range(-10f, 10f));
        gameObject.transform.Rotate(new Vector3(0,0,1), Random.Range(-10f,10f));
        
        ball_rb.velocity = new Vector3(0f,0f,0f);
        ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f))
        + gameObject.transform.position;

        ResetScene();

        //base.OnEpisodeBegin();
    }

    //Function for resetting the scene
    private void ResetScene()
    {
        //Sets te mass and scale of the ball to its default size. It gets the value from the Academy's environment params.
        ball_rb.mass = default_params.GetWithDefault("mass", 1f);
        var scale = default_params.GetWithDefault("scale", 1f);
        ball.transform.localScale = new Vector3(scale, scale, scale);
    }
}
