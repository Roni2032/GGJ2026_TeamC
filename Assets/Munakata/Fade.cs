using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField,Header("画像")]
    Image image;

    [SerializeField, Header("1秒間で進む割合")]
    float speed;

    [SerializeField, Header("色")]
    Color fadeColor;

    [SerializeField, Header("フェードアウト？")]
    bool isOut;

    bool isActive = false;
    bool isFinished = false;

    public bool IsFinished { get { return isFinished; } }
    public bool IsActive { get { return isActive; } }

    Color currentColor;
    public void StartFade()
    {
        currentColor = fadeColor;
        currentColor.a = isOut ? 0.0f : 1.0f;
        image.color = currentColor;
        image.gameObject.SetActive(true);

        isFinished = false;
        isActive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        float fadeSpeed = isOut ? speed : -speed;

        currentColor.a += speed * Time.deltaTime;
        if (isOut && currentColor.a > 1.0f)
        {
            currentColor.a = Mathf.Max(0.0f, currentColor.a);
            isFinished = true;
        }
        if(!isOut && currentColor.a < 0.0f)
        {
            currentColor.a = Mathf.Min(1.0f, currentColor.a);
            isFinished = true;
        }
        image.color = currentColor;
    }
}
