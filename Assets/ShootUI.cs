using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootUI : MonoBehaviour
{
    [SerializeField] private Image fillBar;
    [SerializeField] private Gradient fillColor;

    public void SetShootPower(float t)
    {
        t = Mathf.Clamp(t, 0, 1);
        fillBar.color = fillColor.Evaluate(t);
        fillBar.fillAmount = t;
    }
}
