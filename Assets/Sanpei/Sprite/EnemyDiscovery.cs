using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// “G‚Ì”­Œ©ˆ—
public class EnemyDiscovery : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("‚Ô‚Â‚©‚Á‚½");
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("‰ö“‚ğŒ©‚Â‚¯‚½I");
        }
        //Debug.Log("‚Ô‚Â‚©‚Á‚½");
    }
}
