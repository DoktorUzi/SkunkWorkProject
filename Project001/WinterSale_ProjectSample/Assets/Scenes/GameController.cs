using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

   /********************/
   /* Public Variables */
   /********************/
   
   /* Prefab elements for List of games */
   public float mainSceneDurationSeconds= 15.0F; // Duration of the main scene in seconds
	public GameObject Prototype1;   // Input prefab gameobject 1
	public GameObject Prototype2;   // Input prefab gameobject 2
	public GameObject Prototype3;   // Input prefab gameobject 3
	public GameObject Prototype4;   // Input prefab gameobject 4
	public GameObject Prototype5;   // Input prefab gameobject 5
	public GameObject Prototype6;   // Input prefab gameobject 6
	public GameObject Prototype7;   // Input prefab gameobject 7
	public GameObject Prototype8;   // Input prefab gameobject 8
	public GameObject Prototype9;   // Input prefab gameobject 9
	public GameObject Prototype10;  // Input prefab gameobject 10
   
   /* Cameras of the scene */
   public Camera mainSceneCamera;
   public Camera menuCamera;
   public Camera gameOverCamera;
   
   public const int TOT_OBJECT_NUM      = 10; // Number of available elements for the list
   public const int LIST_ELEMENTS_NUM   = 10; // Number of elements in the list
   
   /* Variable containing the list of gathered objects IDs. Global, updated from ObjectBehavior */
   public static List<int> objList = new List<int>();
   
   /********************/
   /* Local variables **/
   /********************/
   
   private List<GameObject> GamePrefab = new List<GameObject>();
   private float initialTime; // Variable to store initial time of the principal scene
   private int i; // Loop counter
   
   private enum en_MachineState {IDLE, OPENING, MAINMENU, INTRO, SCENE, OUTRO, ENDGAME};
   private en_MachineState MachineState = en_MachineState.IDLE; // Initialized state to 'IDLE'

   
   /********************/
   /* Public functions */
   /********************/
   
	// Use this for initialization
	void Start () {
      /* Set initial state of the Cameras */
      
      mainSceneCamera = (GameObject.FindWithTag("MainCamera")).GetComponent(typeof(Camera)) as Camera;
      menuCamera = (GameObject.FindWithTag("MenuCamera")).GetComponent(typeof(Camera)) as Camera;
      gameOverCamera = (GameObject.FindWithTag("GameOverCamera")).GetComponent(typeof(Camera)) as Camera;

      mainSceneCamera.enabled =  false;
      menuCamera.enabled =       true;
      gameOverCamera.enabled =   false;
      
      /* Load prefab in the array */
      GamePrefab.Add(Prototype1);
      GamePrefab.Add(Prototype2);
      GamePrefab.Add(Prototype3);
      GamePrefab.Add(Prototype4);
      GamePrefab.Add(Prototype5);
      GamePrefab.Add(Prototype6);
      GamePrefab.Add(Prototype7);
      GamePrefab.Add(Prototype8);
      GamePrefab.Add(Prototype9);
      GamePrefab.Add(Prototype10);

      /* TODO: Algorithm to select and sort elements of listObjects */
      /*------------------------------------------------------------*/
      /**************************************************************/
      
	}

	
	// Update is called once per frame
	void Update () {
      
      /* Videogame controller State Machine */
      switch (MachineState)
      {
         case en_MachineState.IDLE:
            /* Idle state. Once loading of all components is terminated, the machine goes into Opening scene state */
            
            MachineState = en_MachineState.OPENING;
            break;
            
         case en_MachineState.OPENING:
            /* Opening state. Use this to show cinematic scene at the opening. Then go to Main Menu scene */
            
            MachineState = en_MachineState.MAINMENU;
            break;
            
         case en_MachineState.MAINMENU:
            /* Wait for the player to start by pushing the New Game Button and go into the Intro state */ 
            /* Set the main menu camera active */
            mainSceneCamera.enabled =  false;
            menuCamera.enabled =       true;
            gameOverCamera.enabled =   false;
            
            if (Input.GetMouseButtonDown(0))
            {
               /* Input mouse position is mapped to the 3D World (only work for orthogonal camera) */
               /* NOTE: Camera Component should be tagged "MainCamera" to avoid missing pointer    */
               Vector3 CursorPosition = menuCamera.ScreenToWorldPoint(Input.mousePosition);
               CursorPosition.z = 0.0F; // 2D subspace, where game happen, is [Z=0]

               /* Transform the click of the mouse in a Ray */
               Ray ray = menuCamera.ScreenPointToRay(Input.mousePosition);
               RaycastHit hit;

               /* Detect a collision of the Ray with a collider and that the collider gameObject is 
                  tagged as StartButton*/
               if(Physics.Raycast(ray,out hit) && hit.transform.tag == "StartButton")
               {
                  /* Instantiate LIST_ELEMENTS_NUM Prefabs */
                  i = 0;
                  foreach (GameObject GamePrefab in GamePrefab) 
                  {
                     Instantiate<GameObject>(GamePrefab, new Vector3(0,(5.0F-1.55F * i),0), Quaternion.identity);
                     i++;
                  }
                  
                  MachineState = en_MachineState.INTRO;
               }
            }

            break;
            
         case en_MachineState.INTRO:
            /* Play a cinematic introduction to the main scene of the game */
            
            MachineState = en_MachineState.SCENE;
            initialTime = Time.time;
            break;
            
         case en_MachineState.SCENE:
            /* Main scene of the game. At the end, switch to Cinematic Outro state */
            mainSceneCamera.enabled =  true;
            menuCamera.enabled =       false;
            
            if (Input.GetMouseButtonDown(0))
            {
               /* Input mouse position is mapped to the 3D World (only work for orthogonal camera) */
               /* NOTE: Camera Component should be tagged "MainCamera" to avoid missing pointer    */
               Vector3 CursorPosition = menuCamera.ScreenToWorldPoint(Input.mousePosition);
               CursorPosition.z = 0.0F; // 2D subspace, where game happen, is [Z=0]

               /* Transform the click of the mouse in a Ray */
               Ray ray = mainSceneCamera.ScreenPointToRay(Input.mousePosition);
               RaycastHit hit;

               if(Physics.Raycast(ray,out hit) && hit.transform.tag == "ListItem")
               {
                  /* Call method on the ListItem script to set the state on HOLD */
                  hit.transform.gameObject.GetComponent<ObjectBehavior>().GrabItem();
               }
            }

            /* Go to end scene when the time is finished */
            if (Time.time - initialTime > mainSceneDurationSeconds)
            {
               MachineState = en_MachineState.OUTRO;
            }
            break;      
            
         case en_MachineState.OUTRO:
            /* Play a cinematic outro if available. Then switch to Endgame state */            
            mainSceneCamera.enabled =  false;
            menuCamera.enabled =       false;
            gameOverCamera.enabled =   true;
            
            MachineState = en_MachineState.ENDGAME;
            break;   
            
         case en_MachineState.ENDGAME:
            /* Display player score and unlocked future challenges. Then switch to Main Menu */
            
            // MachineState = en_MachineState.MAINMENU;
            break;   
            
      }
	}
}
