using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Part
{
    //attributes
    public string id;
    public string type;
    public bool root;
    public int orientation;

    //constructor
    public Part(string id, string type, bool root, int orientation){
        this.id = id;
        this.type = type;
        this.root = root;
        this.orientation = orientation;
    }

    public static Part CreatePartFromJson(string json){
        return JsonUtility.FromJson<Part>(json);
    }    
   
}
