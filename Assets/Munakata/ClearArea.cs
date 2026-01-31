using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearArea : MonoBehaviour
{
    [SerializeField]
    Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fade.IsActive && fade.IsFinished)
        {
            SceneManager.LoadScene("ResultScene");
        }
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
                Debug.Log("クリア！！");
                GameManager.Instance.ResultType = ResultType.Clear;
                fade.StartFade();
            }
            else
            {
                Debug.Log("盗むもん盗んでから来やがれ！！");
            }
        }
    }
}
