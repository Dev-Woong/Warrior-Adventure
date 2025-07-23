using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TalkBoxEffect : MonoBehaviour
{
    public Image BoxArrow1;
    public Image BoxArrow2;
    public Image BoxArrow3;
    float TimeSet;


    // Update is called once per frame
    private void Update()
    {
        TimeSet += Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (TimeSet % 1.5f < 0.5f)
        {
            BoxArrow1.color = new Color32(255, 255, 255, 50);
            BoxArrow2.color = new Color32(255, 255, 255, 150);
            BoxArrow3.color = new Color32(255, 255, 255, 250);
        }
        else if (TimeSet % 1.5f>= 0.5f && TimeSet % 1.5f < 1f)
        {
            BoxArrow1.color = new Color32(255, 255, 255, 250);
            BoxArrow2.color = new Color32(255, 255, 255, 50);
            BoxArrow3.color = new Color32(255, 255, 255, 150);
        }
        else if (TimeSet % 1.5f >= 1f && TimeSet % 1.5f <= 1.5f)
        {
            BoxArrow1.color = new Color32(255,255,255,150);
            BoxArrow2.color = new Color32(255,255,255,250);
            BoxArrow3.color = new Color32(255,255,255,50);
        }
    }
}
