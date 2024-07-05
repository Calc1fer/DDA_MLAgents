using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    
  private void OnTriggerEnter(Collider other) 
  {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player_Controller>().respawn = true;
        }  
  }

}
