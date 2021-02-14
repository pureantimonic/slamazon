using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Image icon;
    public void SetColor(Color col)
    {
        icon.color = col;
    }
    [SerializeField] private Image timeFill;
    public void SetTimeRemaining(float t)
    {
        timeFill.fillAmount = t;
    }
    // Start is called before the first frame update
    public void OnFail()
    {
        anim.SetTrigger("Fail");
        Destroy(gameObject, 3);
    }

    public void OnComplete()
    {
        anim.SetTrigger("Complete");
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
