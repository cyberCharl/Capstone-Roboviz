// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class RobotInteract : MonoBehaviour
// {
//     // Start is called before the first frame update
//     // public GameObject RobotMoveMenu;
//     // public TMP_InputField newXcoord;
//     // public TMP_InputField newZcoord;
//     // public string X;
//     // public string Z;


//     void Update() {
//         if (Input.GetMouseButtonDown(0)){
//             // HighlightPart();
//             MoveMenuShow();
//             // interaction.MoveMenuShow();
//         }
//         if (Input.GetKeyDown(KeyCode.Tab))  {
//             MoveMenuHide();
//         } 
//     }

//     public void MoveMenuHide() {
//         RobotMoveMenu.SetActive(false);
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = false;
//         PauseMenu.PauseState = false;
//     }

//     public void MoveMenuShow() {
//         RobotMoveMenu.SetActive(true);
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//         PauseMenu.PauseState = true;
//     }
// }
