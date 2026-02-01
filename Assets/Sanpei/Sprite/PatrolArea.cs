using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Œx”õ”ÍˆÍ‚ÌŠÇ—
public class PatrolArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // “–‚½‚Á‚Ä‚¢‚½‚Ì‚ªPlayer‚È‚ç‚»‚ê‚É‘Î‚µ‚Ä‚ ‚È‚½‚Í‚±‚ÌƒGƒŠƒA‚É‹‚Ü‚·‚æ‚Æ“`‚¦‚é
        if (other.gameObject.TryGetComponent(out PlayerCtrl playerCtrl))
        {
            playerCtrl.CurrentArea = this;
        }

    }
}
