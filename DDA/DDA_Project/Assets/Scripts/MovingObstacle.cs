using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    //Script that will move an obstacle back and forward
    private float speed;
    private Vector3 dir = Vector3.left;

    [SerializeField] private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(5f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
        //transform.position += dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Wall")
        {
            dir = -dir;
        }    
        
        if(other.gameObject.tag == "Obstacle")
        {
            dir = -dir;
        }
    }
}
