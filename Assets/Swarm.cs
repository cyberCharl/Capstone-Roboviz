using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Swarm
{
    // Atributes and Proporties
    public int id;
    public Body body;
    private Vector3 position = new Vector3();
    public List<GameObject> robotObjects;

    // get and set methods
    public void setPosition(Vector3 position) {
        this.position = position;
    }
    public Vector3 getPosition() {
        return position;
    }

    public void setObjects(List<GameObject> robotObjects) {
        this.robotObjects = robotObjects;
    }
    public List<GameObject> getObjects() {
        return robotObjects;
    }


    // check the position 
    public bool CheckPosition(float[] terrainInfo) {
        // dis is wrong please fix 
        return (float)position.x >= terrainInfo[0]/2 ||  (float)position.x <= -terrainInfo[0]/2 || (float)position.z >= terrainInfo[1]/2 || (float)position.z <= -terrainInfo[1]/2;
    }

    // puts robot at position
    public void MoveToPosition() {
        robotObjects[0].transform.position = position;
    }
    
    // Load parts
    public List<GameObject> LoadParts() {
        List<GameObject> prefabs = new List<GameObject>();
        
        foreach (Part par in body.part){
            switch (par.type){
                case "CoreComponent":
                    if (par.root == true) {
                        GameObject coreRoot = (GameObject)Resources.Load("Core_FDM");
                        coreRoot.transform.position = getPosition();
                        prefabs.Add(coreRoot);
                        break;
                    }
                    else {
                    prefabs.Add((GameObject)Resources.Load("Core_FDM"));
                    break;
                    }
                case "ActiveHinge":
                    prefabs.Add((GameObject)Resources.Load("FullActiveHinge"));
                    break;
                case "PassiveHinge":
                    prefabs.Add((GameObject)Resources.Load("FullPassiveHinge"));
                    break;
                case "FixedBrick":
                    prefabs.Add((GameObject)Resources.Load("Core_FDM"));
                    break;
            }
        }

        return prefabs;
    }

    // position objects with respect to each other and link them together
    public void linkObjects() {
        List<Connection> connections = body.connection;
        List<Part> parts = body.part;

        foreach (Connection con in connections){
            for (int k = 0; k < parts.Count; k++){
                if (con.src == parts[k].id){
                    for (int j = 0; j < parts.Count; j++){
                        if (con.dest == parts[j].id){
                            robotObjects[j].transform.SetParent(robotObjects[k].transform);
                            move(robotObjects[j], con.srcSlot, parts[j].orientation);
                            break;
                        }
                    }
                    break;
                }
            }
        }
    }

    // change object orientations
    public void OrientObjects() {
        List<Part> parts = body.part;
        for (int i = 1; i < parts.Count - 1; i++){
            if ((parts[i].type != "FixedBrick") || (main.orientFixedBricks == true)){
                List<Transform> children = new List<Transform>();
                Transform par = robotObjects[i].transform.parent;
                GameObject obj = robotObjects[i];
                Vector3 centre = obj.GetComponent<Collider>().bounds.center;
                if (parts[i].type == "FixedBrick"){
                    for (int k = 0; k < obj.transform.childCount; k++){
                        children.Add(obj.transform.GetChild(k));
                    }
                    obj.transform.DetachChildren();
                } else {
                    for (int k = 2; k < obj.transform.childCount; k++){
                        children.Add(obj.transform.GetChild(k));
                    }
                    for (int k = 0; k < children.Count; k++){
                        children[k].SetParent(null);
                    }
                }
                //obj.transform.rotation = Quaternion.Euler(0, par.rotation.eulerAngles.y, 0);
                Vector3 orVec = par.gameObject.GetComponent<Collider>().bounds.center - centre;
                orVec = new Vector3(Mathf.Round(orVec.x * 10f) / 10f, Mathf.Round(orVec.y * 10f) / 10f, Mathf.Round(orVec.z * 10f) / 10f);
                obj.transform.RotateAround(centre, orVec, parts[i].orientation * 90);
                for (int k = 0; k < children.Count; k++){
                    children[k].SetParent(obj.transform);
                }
            }
        }
    }

    // Assign a colour from resources to each part
    public void ColourObjects() {
        List<Part> parts = body.part;
        for (int i = 0; i < parts.Count; i++){
            switch(parts[i].type){
            case "CoreComponent":
                robotObjects[i].GetComponent<MeshRenderer>().material = (Material)Resources.Load("CoreMaterial");
                break;
            case "ActiveHinge":
                robotObjects[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("ActiveHingeMaterial");
                robotObjects[i].transform.GetChild(1).GetComponent<MeshRenderer>().material = (Material)Resources.Load("ActiveHingeMaterial");
                break;
            case "PassiveHinge":
                robotObjects[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("PassiveHingeMaterial");
                robotObjects[i].transform.GetChild(1).GetComponent<MeshRenderer>().material = (Material)Resources.Load("PassiveHingeMaterial");
                break;
            case "FixedBrick":
                robotObjects[i].GetComponent<MeshRenderer>().material = (Material)Resources.Load("BrickMaterial");
                break;
            }
        }
    }

    /// <summary>
    ///  Moving the destination object in the right direction relative
    /// to the source object.
    /// </summary>
    /// <returns>A Vector3 that represents the change from source position to destination position</returns>
    void move(GameObject obj, int src, int orientation){
        switch(src){
            case 0:
                obj.transform.localPosition = Vector3.Scale(Vector3.right, new Vector3(0.4f, 0.4f, 0.4f));
                obj.transform.rotation = Quaternion.Euler(0, obj.transform.parent.rotation.eulerAngles.y + 180, 0) * obj.transform.rotation;
                break;
            case 1:
                obj.transform.localPosition = Vector3.Scale(Vector3.left, new Vector3(0.4f, 0.4f, 0.4f));
                obj.transform.rotation = Quaternion.Euler(0, obj.transform.parent.rotation.eulerAngles.y, 0) * obj.transform.rotation;
                break;
            case 2:
                obj.transform.localPosition = Vector3.Scale(Vector3.forward, new Vector3(0.4f, 0.4f, 0.4f));
                obj.transform.rotation = Quaternion.Euler(0, obj.transform.parent.rotation.eulerAngles.y + 90, 0) * obj.transform.rotation;
                break;
            case 3:
                obj.transform.localPosition = Vector3.Scale(Vector3.back, new Vector3(0.4f, 0.4f, 0.4f));
                obj.transform.rotation = Quaternion.Euler(0, obj.transform.parent.rotation.eulerAngles.y - 90, 0) * obj.transform.rotation;
                break;
        }
    }

    public void AddPart(int type, int orientation, String src, int srcSlot){
        String partType = "";
        switch(type){
            case 1: partType = "ActiveHinge"; break;
            case 2: partType = "PassiveHinge"; break;
            case 3: partType = "FixedBrick"; break;
        }
        body.addPart(new Part(src + srcSlot.ToString(), partType, false, orientation));
        body.addConnection(new Connection(src, src + srcSlot.ToString(), srcSlot, 0));
    }
}
