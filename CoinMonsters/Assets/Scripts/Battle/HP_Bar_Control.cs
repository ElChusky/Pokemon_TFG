﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar_Control : MonoBehaviour
{
    public bool isPlayerBar;
    private Color green = new Color(24f / 255, 192f / 255, 32f / 255);
    private Color yellow = new Color(255f / 255, 222f / 255, 30f / 255);
    private Color red = new Color(255f / 255, 24f / 255, 26f / 255);

    private void Update()
    { 
        if (transform.localScale.x <= 0.25f)
        { 
            GetComponent<Image>().color = red;
        } 
        else if (transform.localScale.x <= 0.5f)
        {
            GetComponent<Image>().color = yellow;
        } else
        {
            GetComponent<Image>().color = green;
        }
    }

    public void SetHp(float hpNormalized)
    {
        transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmoothly(float newHP)
    {
        float currentHP = transform.localScale.x;

        float changeAmt = currentHP - newHP;

        if(changeAmt > 0)
        {
            while (currentHP - newHP > Mathf.Epsilon)
            {
                currentHP -= changeAmt * Time.deltaTime;
                transform.localScale = new Vector3(currentHP, 1f);
                yield return null;
            }
        } else
        {
            while (currentHP - newHP < Mathf.Epsilon)
            {
                currentHP -= changeAmt * Time.deltaTime;
                transform.localScale = new Vector3(currentHP, 1f);
                yield return null;
            }
        }
        
        transform.localScale = new Vector3(newHP, 1f);
    }
}
