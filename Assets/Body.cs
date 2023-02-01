using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Body
{
    
    //The makeup of each individual robot:
    public List<Part> part;
    public List<Connection> connection;

    //Constructors
    public Body(List<Part> part, List<Connection> connection){
        this.connection = connection;
        this.part = part;
    }


    //container methods
    public void addConnection(Connection con){
        connection.Add(con);
    }

    public void addPart(Part par){
        part.Add(par);
    }

    public void removeConnection(Connection con){
        connection.Remove(con);
    }

    public void removePart(Part par){
        part.Remove(par);
    }


    // //print statement to display attributes of the robot
    // public void displayAttributes(){
        
    //     Debug.Log("The robot contains the following part:");
    //     string partes ="";
        
    //     foreach (Part p in this.part)  
    //     {
    //         partes = partes + p.id + "\n";
    //     }
    //     Debug.Log(partes);
        
    //     Debug.Log("The robot contains connection that link the following src/dest part pairs:");
    //     string cons= "";
        
    //     foreach (Connection c in this.connection)
    //     {
    //         cons = cons + c.displayLink() + "\n";
    //     }
    //     Debug.Log(cons);
    // }
}