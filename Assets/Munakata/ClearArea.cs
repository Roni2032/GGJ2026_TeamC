using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.transform.gameObject;
        Debug.Log("何か入った！！！");
        if(obj.tag == "Player")
        {
            Debug.Log("てめえぷれいやーだな！！！");
            var pickup = obj.GetComponent<PlayerCtrl>();
            if(pickup != null)
            {
                if (pickup.IsGemPickup)
                {
                    GameManager.Instance.GameClear();
                    return;
                }
            }
            Debug.Log("盗むもん盗んでから来やがれ！！");
        }
    }
}
