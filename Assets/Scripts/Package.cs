using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Package : MonoBehaviour
{
    [SerializeField] private GameObject iconCanvas;
    [SerializeField] private GameObject boxIcon;
    [SerializeField] private float iconMargin;
    private GameObject m_BoxIcon;
    
    public void Start()
    {
        m_BoxIcon = Instantiate(boxIcon, iconCanvas.transform, false) as GameObject;
        m_BoxIcon.GetComponent<Image>().color = UnityEngine.Random.ColorHSV(0f, 1f, 0.8f, 1.0f, 0.5f, 1.0f);
    }
    
    public void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        var canvasTransform = iconCanvas.transform as RectTransform;
        print(screenPoint);

        var width = canvasTransform.rect.width;

        if (screenPoint.z < 0)
        {
            screenPoint.x = Mathf.Sign(screenPoint.x) * Mathf.NegativeInfinity;
        }

        m_BoxIcon.transform.position = new Vector3(
            Mathf.Clamp(screenPoint.x, width * iconMargin, width * (1f - iconMargin)),
            canvasTransform.rect.height * 0.9f
        );
        
        
        
    }

    public void OnDestroy()
    {
      //  Destroy(m_BoxIcon);
    }

    [SerializeField] public Vector3 anchorPoint;

    public Vector3 Destination;
    public MeshRenderer mainMesh;

}
