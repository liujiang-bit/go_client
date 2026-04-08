//
// Author:  Johance
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BangLayoutStyleEditor : Attribute
{
    public Type CompmentType { get; set; }
    public string Name { get; set; }
    public BangLayoutStyleEditor(Type compmentType, string name)
    {
        Name = name;
        CompmentType = compmentType;
    }
};