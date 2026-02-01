using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use : MonoBehaviour
{
    [SerializeField] private SoundManager target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            target.OnSecurityGuardWake();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            target.OnDiscoveredUnconsciousSecurityGuard();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            target.OnStrike();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            target.OnDisguise();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            target.OnDiscovery();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            target.OnSteal();
        }
    }
}
