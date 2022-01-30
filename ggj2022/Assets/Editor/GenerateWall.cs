 using UnityEngine;
 using UnityEditor;
 
 public class GenerateWall : EditorWindow
 {     
     [MenuItem("Window/Custom/Generate low wall")]
     public static void ShowWindow()
     {
         GetWindow<GenerateWall>("Generate low walls");
     }
 
     private void OnGUI()
     {
         if (GUILayout.Button("Create Wall"))
         {
             CreateWall();
         }
     }
 
     private void CreateWall()
     {              
        GameObject prefab = Selection.activeObject as GameObject;
        if (!prefab) {
            EditorUtility.DisplayDialog("Select Prefab", "You must select a prefab first!", "OK");
            return;
        }

         for(int i =0; i < 1; i ++) {
            for(int j=0;j< 5;j++){
                for(int k=0; k < 10;k++){
                    Vector3 pos = new Vector3(k * 1f,j * 1f,i * 1f);
                    //print(pos);
                    Transform wall = (Transform) Instantiate(prefab.transform,pos,Quaternion.identity);
                    wall.name = "WallCube";
                    
                }
            }
         }
     }
 }