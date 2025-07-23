using System;
using System.Collections;
using UnityEngine;
using TMPro;
public class DataTime : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text displayTime;
    public string GetCurrentTime()
    {
        return DateTime.Now.ToString(("HH : mm"));
    }
    IEnumerator Start()
    {
        while (true)
        {
            displayTime.text = GetCurrentTime();
            yield return new WaitForSeconds(1f);
        }
    }
}
