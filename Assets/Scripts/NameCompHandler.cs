﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameCompHandler : MonoBehaviour
{
    public TextMeshProUGUI nameText,nameText2;
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = TemplateData.CompanyName.ToUpper() + " COMPANY";
        if (nameText2 != null)
        {
            nameText2.text = TemplateData.CompanyName.ToUpper() + " COMPANY";
        }
    }

    
}
