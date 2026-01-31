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
            //ここで盗んだものを持っているか判別
            if (true)
            {
                GameManager.Instance.GameClear();
            }
            else
            {
                Debug.Log("盗むもん盗んでから来やがれ！！");
            }
        }
    }
}
