using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InfoPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Title;

    [SerializeField]
    private TextMeshProUGUI Container;


    public void setText(string titleText, string containerText)
    {
        if(titleText != null)
        {
            //Title = titleText;
        }

        if(containerText != null) 
        {
            //Container = containerText;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
