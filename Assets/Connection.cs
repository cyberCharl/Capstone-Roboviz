using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Connection
{
    //attributes
    public string src;
    public string dest;
    public int srcSlot;
    public int destSlot;
    public double weight;
    
    //constructor
    public Connection(string src, string dest, int srcSlot, int destSlot){
        this.src = src;
        this.dest = dest;
        this.srcSlot = srcSlot;
        this.destSlot = destSlot;
    }

    //Display src/dest pair
    public string displayLink(){
        return this.src +"--"+ this.dest;
    }

}
