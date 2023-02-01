using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
public class main : MonoBehaviour
{
    // Attributes
    int swarmSize;
    int numPositions;
    int numRobots;
    float[] terrainVars;
    
    public Root root;
    public Swarm robot;
    public List<GameObject> prefabs;

    private GameObject highlight;
    private int highlightType;
    public static bool MoveMenu = false;
    
    public GameObject RobotMoveMenu;

    public static bool orientFixedBricks = false;

    /// <summary>
    ///  Unity's start method. Start is called before the first frame update
    /// </summary>
    void Start()
    {
        Physics.autoSyncTransforms = true;

        string jason = System.IO.File.ReadAllText(StartMenu.json);
        createRobots(jason);

        terrainVars = readConfig(StartMenu.config);
        setTerrainPlane();
        
        Vector3[] positions = readPos(StartMenu.pos);
        List<Swarm> rbt = root.swarm;

        int SuccessfulRender = 1;

        int situation = CheckCase(swarmSize, numRobots, numPositions);
        switch(situation) {
            case 0: // render each given robot at given position (enough robots and positions, limit by swarmsize)\
                // input 
                Debug.Log("case0");
                for (int r=0; r<swarmSize; r++) {
                    // set the position
                    rbt[r].setPosition(positions[r]);
                    // check if position is valid
                    if (rbt[r].CheckPosition(terrainVars)) {
                        // change to output prompt to screen
                        Debug.Log("Robot is out of bounds");
                    } else {
                        SuccessfulRender = RenderRobot(rbt[r], true);
                    }
                }
                break;
            case 1: // render same robots in different positions limited by swarmSize (not enough robots, make more. limit by swarmsize)
                Debug.Log("case1");
                
                for (int r=0; r<swarmSize; r++) {
                    int i = r % rbt.Count;
                    rbt[i].setPosition(positions[r]);
                    // check if position is valid
                    if (rbt[i].CheckPosition(terrainVars)) {
                        // change to output prompt to screen
                        Debug.Log("Robot is out of bounds");
                    } else {
                        SuccessfulRender = RenderRobot(rbt[i], true);
                    }
                }
                break;
            case 2: // enough robots - limited by numPositions - changed to auto position ( maybe add setting ? )
                Debug.Log("case2");
                for (int p=0; p<numPositions; p++) {
                    // set the position
                    rbt[p].setPosition(positions[p]);
                    // check if position is valid
                    if (rbt[p].CheckPosition(terrainVars)) {
                        // change to output prompt to screen
                        Debug.Log("Robot is out of bounds");
                    } else {
                        SuccessfulRender = RenderRobot(rbt[p], true);
                    }
                }
                break;
            case 3:
                // render same robots in different positions limited by numPositions
                Debug.Log("case3");
                for (int p=0; p<numPositions; p++) {
                    int i = p % rbt.Count;
                    rbt[i].setPosition(positions[p]);
                    // check if position is valid
                    if (rbt[i].CheckPosition(terrainVars)) {
                        // change to output prompt to screen
                        Debug.Log("Robot is out of bounds");
                    } else {
                        SuccessfulRender = RenderRobot(rbt[i], true);
                    }
                }
                break;
        }
    }

    /// <summary>
    ///  Compare swarmsize, number of positinos specified and number of robots defined and determines which pat to take in main
    /// </summary>
    /// <returns>An integer value that tells the program which case to operate under</returns>
    int CheckCase(int SwarmSize, int numRobots, int numPositions) {

        if (SwarmSize <= numPositions) {
            if (swarmSize <= numRobots) {
                // normal case, just go based on swarm size DONE & WORKING
                return 0;
            } else if(swarmSize > numRobots ) {
                // not enough robots, create multiple of same robots DONE & WORKING
                return 1;
            } else { return -1; }
        } else if(swarmSize > numPositions) {
            if (numPositions <= numRobots) {
                // render only as many as positions allow
                return 2;
            } else if (numPositions > numRobots) {
                // render only as many as positions, but copy to make enough robots ( same as case 1 kinda )
                return 3;
            } else { return -1; }
        } else { return -1; }
    }

    /// <summary>
    ///  Unity's update method. It is called once per frame. Essentially, a clocked system.
    /// </summary>
    void Update() {
        if (Input.GetMouseButtonDown(0)){
            HighlightPart();
            // if (GetHighlightRobot() != null) {
            //     MoveMenuShow();
            // }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && MoveMenu)  {
            MoveMenuHide();
        } 

        if (Input.GetMouseButtonDown(1)){
            int success = 1;
            Debug.Log(GetHighlightRobot());
            if (GetHighlightRobot() != null){
                GetHighlightRobot().AddPart(1, 0, highlight.name, 2);
                GameObject.Destroy(highlight.transform.root.gameObject);
                success = RenderRobot(GetHighlightRobot(), false);
            }
        }
    } 

    /// <summary>
    ///  Reading the JSON file and creating objects of all respective classes that make up the robots in our swarm
    /// </summary>
    void createRobots(string jason) {
        if (jason.IndexOf("swarm") == -1) {
            // Single Robot
            robot = JsonUtility.FromJson<Swarm>(jason);
            numRobots = 1;
            List<Swarm> swrm = new List<Swarm>();
            swrm.Add(robot);
            root = new Root(swrm);
        }
        else {
            // Multiple Robots
            root = JsonUtility.FromJson<Root>(jason);
            numRobots = root.swarm.Count;

            // Set position of each robot
            // int i =0;
            // foreach (Swarm rob in root.swarm) {
            //     rob.setPosition(positions[i]);
            //     i++;
            // }
        }
    }

    /// <summary>
    ///  Mehtod to initiate our floor as a metal grid material
    /// </summary>
    void setTerrainPlane() {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        UnityEngine.Object.Destroy(plane.GetComponent<Collider>());
        plane.transform.localScale = new Vector3( terrainVars[0]/5f, 1f, terrainVars[1]/5f);
        plane.GetComponent<MeshRenderer>().material = (Material)Resources.Load("MetalGridFloor");
    }

    /// <summary>
    ///  Reading the configuration txt file and storing the length, width and height of the environment
    /// </summary>
    /// <returns>A float array with the configurations of the 3D space</returns>
    float[] readConfig(string filename) {
        string[] lines = File.ReadAllLines(filename);
        float length = float.Parse(lines[0]);
        float width = float.Parse(lines[1]);
        float SS = float.Parse(lines[2]);
        float[] config = new float[] {length,width,SS};
        swarmSize = (int)config[2];
        return config;
    }

    /// <summary>
    ///  Reading the positions txt file and storing the x, y and z position of each robot. Number of lines in the file
    ///  corresponds to the number of robopts in the swarm
    /// </summary>
    /// <returns>A 2D int array where each element is an array containing a specific robot's position</returns>
    Vector3[] readPos(string filename) {
        
        string[] lines = File.ReadAllLines(filename);
        numPositions = lines.Length;
        Vector3[] positions = new Vector3[numPositions];

        //each entry in positions contains a coordinate in the form: x y z
        for(int i = 0; i < numPositions; i++){
            positions[i] = StringToVector(lines[i]);
        }
        return positions;
    }

    /// <summary>
    ///  Converting an input string into a Vector.
    /// </summary>
    /// <returns>A Vector object</returns>
    public Vector3 StringToVector(string sVector) {
        // Split the input
        string[] sArray = sVector.Split(' ');

        // store as vector
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[2]),
            float.Parse(sArray[1])); 
        // return vector
        return result;
    }

    /// <summary>
    ///  A method to render a robot object into the 3D environment
    /// </summary>
    int RenderRobot(Swarm robToRend, bool DisplayRobotCollisionMessage) {

        List<Part> parts = robToRend.body.part;
        List<GameObject> prefabs = robToRend.LoadParts();

        //Load objects
        List<GameObject> objects = new List<GameObject>();
        foreach (GameObject obj in prefabs){
            objects.Add((GameObject)Instantiate(obj));
        }

        //Name objects
        objects[0].name = robToRend.id.ToString();
        for (int i = 1; i < parts.Count; i++){
            objects[i].name = parts[i].id;
        }
        
        robToRend.setObjects(objects);

        //Position objects new method in main
        robToRend.linkObjects();

        //Orient objects new method in main
        robToRend.OrientObjects();

        //Colour Objects new method in main pass gameobject lists
        robToRend.ColourObjects();

        //Move robot to correct position
        robToRend.MoveToPosition();

        //Check collisions
        return CheckCollision(robToRend.getObjects(), DisplayRobotCollisionMessage);
    }

    /// <summary>
    ///  Checking for collisions with robots
    /// </summary>
    /// <returns>1, if there is a collision</returns>
    int CheckCollision(List<GameObject> objects, bool DisplayRobotCollisionMessage){
        foreach (GameObject obj in objects){
            obj.GetComponent<Collider>().enabled = false;
        }

        foreach (GameObject obj in objects){
            if (Physics.CheckBox(obj.GetComponent<Collider>().bounds.center, Vector3.Scale(obj.GetComponent<Collider>().bounds.size, new Vector3(0.5f,0.5f,0.5f)))){
                UnityEngine.Object.Destroy(objects[0]);
                if (DisplayRobotCollisionMessage){
                    Debug.Log("Collision between robots detected");
                    return 1;
                }
                break;
            }
        }

        foreach (GameObject obj in objects){
            obj.GetComponent<Collider>().enabled = true;
        }

        if (Physics.Raycast(new Vector3(terrainVars[0] / 2, 0.1f, terrainVars[1] / 2), Vector3.left)){
            UnityEngine.Object.Destroy(objects[0]);
            Debug.Log("Collision at boundary detected");
            return 1;
        } else if (Physics.Raycast(new Vector3(terrainVars[0] / 2, 0.1f, terrainVars[1] / 2), Vector3.back)){
            UnityEngine.Object.Destroy(objects[0]);
            Debug.Log("Collision at boundary detected");
            return 1;
        } else if (Physics.Raycast(new Vector3( -terrainVars[0] / 2, 0.1f, -terrainVars[1] / 2), Vector3.right)){
            UnityEngine.Object.Destroy(objects[0]);
            Debug.Log("Collision at boundary detected");
            return 1;
        } else if (Physics.Raycast(new Vector3( -terrainVars[0] / 2, 0.1f, -terrainVars[1] / 2), Vector3.forward)){
            UnityEngine.Object.Destroy(objects[0]);
            Debug.Log("Collision at boundary detected");
            return 1;
        }

        return 0;
    }


    /// <summary>
    ///  Changes the colour of a game object when clicked
    /// </summary>
    void HighlightPart(){
        if (highlight != null){
            switch(highlightType){
                case 0:
                    highlight.GetComponent<MeshRenderer>().material = (Material)Resources.Load("CoreMaterial");
                    break;
                case 1:
                    highlight.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("ActiveHingeMaterial");
                    highlight.transform.GetChild(1).GetComponent<MeshRenderer>().material = (Material)Resources.Load("ActiveHingeMaterial");
                    break;
                case 2:
                    highlight.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("PassiveHingeMaterial");
                    highlight.transform.GetChild(1).GetComponent<MeshRenderer>().material = (Material)Resources.Load("PassiveHingeMaterial");
                    break;
                case 3:
                    highlight.GetComponent<MeshRenderer>().material = (Material)Resources.Load("BrickMaterial");
                    break;
                }
        }

        RaycastHit hit;
        if (Physics.Raycast(GameObject.Find("Main Camera").transform.position, GameObject.Find("Main Camera").transform.forward, out hit, 20f)){
            GameObject hitObj = hit.collider.gameObject;
            highlight = hitObj;
            
            if (hitObj.GetComponent<MeshRenderer>().sharedMaterial == (Material)Resources.Load("CoreMaterial")){ highlightType = 0;}
            else if (hitObj.GetComponent<MeshRenderer>().sharedMaterial == (Material)Resources.Load("BrickMaterial")){ highlightType = 3;}
            else if (hitObj.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial == (Material)Resources.Load("ActiveHingeMaterial")){ highlightType = 1;}
            else if (hitObj.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial == (Material)Resources.Load("PassiveHingeMaterial")){ highlightType = 2;}

            if (highlightType == 0 || highlightType == 3){
                hitObj.GetComponent<MeshRenderer>().material = (Material)Resources.Load("HighLightMat");
            } else {
                hitObj.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("HighLightMat");
                hitObj.transform.GetChild(1).GetComponent<MeshRenderer>().material = (Material)Resources.Load("HighLightMat");
            }
        }
    }

    /// <summary>
    ///  Gewt method for highlighting
    /// </summary>
    /// <returns>The selected robot</returns>
    Swarm GetHighlightRobot(){
        GameObject rootObj = highlight.transform.root.gameObject;
        for (int i = 0; i < root.swarm.Count; i++){
            if (rootObj.name == root.swarm[i].robotObjects[0].name){
                return root.swarm[i];
            }
        }
        return null;
    }

    void AutoPosition(Swarm Robot){
        for (int i = (int)Mathf.Floor(terrainVars[0] - (terrainVars[0] / 2)); i < (int)Mathf.Floor(terrainVars[0] / 2); i++){
            for (int k = (int)Mathf.Floor(terrainVars[1] - (terrainVars[1] / 2)); k < (int)Mathf.Floor(terrainVars[1] / 2); k++){
                if (RenderRobot(Robot, false) == 0){
                    break;
                }
            }
        }
    }

    public void MoveMenuHide() {
        RobotMoveMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        MoveMenu = false;
    }

    public void MoveMenuShow() {
        RobotMoveMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MoveMenu = true;
    }
}