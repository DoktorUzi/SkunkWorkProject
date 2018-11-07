using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

   /* Object Movement and Interaction */
   public float TractionFactorFree     = 0.01F;    // Traction factor during free movement (toward list position)
   public float TractionFactorCursor   = 0.03F;    // Traction factor during mouse hold (toward cursor position)
   public float DampingFactor          = 0.15F;    // Damping factor to avoid unwanted oscillations  
   public float ScrollSpeed         = 0.02F;    // Vertical speed of the list
   public float TreshX              = 2.5F;     // Horizontal dimension of the GameObject PickUp rectangle
   public float TreshY              = 0.8F;     // Vertical dimension of the GameObject PickUp rectangle
   
   /* This Element Value Parameters */
   public float   FullPrice         = 60.0F;
   public float   SalePrice         = 40.0F;
   public int     ID                = 1;
      
   public Camera mainSceneCamera;   
   private Vector3 TargetPosition; // To store initial value of target position and future evolution
   private Vector3 Momentum = new Vector3();   // To emulate 'inertia' to the gameobject
   
   private enum ObjectState {FREE, HOLD} // State machine to manage input from user / free movement of gameobject
   private ObjectState state = ObjectState.FREE; // Initialized state to 'free'

	// Use this for initialization
	void Start () 
   {
      /* Set initial target position as the spawn point */
      TargetPosition = transform.position;
      /* Store Main Camera as Local Variable */
      mainSceneCamera = (GameObject.FindWithTag("MainCamera")).GetComponent(typeof(Camera)) as Camera;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
   {  
      /* Update target position */
		TargetPosition = TargetPosition + new Vector3(0,ScrollSpeed,0);
      
      /* Input mouse position is mapped to the 3D World (only work for orthogonal camera) */
      /* NOTE: Camera Component should be tagged "MainCamera" to avoid missing pointer    */
      Vector3 CursorPosition = mainSceneCamera.ScreenToWorldPoint(Input.mousePosition);
      CursorPosition.z = 0.0F; // 2D subspace where game happen is [Z=0]
      
      /* If abs(deltaX) is less than TreshX and abs(deltaY) is less than TreshY => HOLD, else FREE */
      if (Input.GetMouseButtonDown(0))
      {
         if (CursorPosition[0]<(transform.position[0]+TreshX) && CursorPosition[0]>(transform.position[0]-TreshX) &&
             CursorPosition[1]<(transform.position[1]+TreshY) && CursorPosition[1]>(transform.position[1]-TreshY) )
             {
                state = ObjectState.HOLD;
             }
      }
      if (Input.GetMouseButtonUp(0))
      {
         state = ObjectState.FREE;
      }
      
      /* Update position of the gameObject */
      switch (state) 
      {
         case ObjectState.FREE:
            /* NOTE: To simulate presence of inertia, the equation of motion is solved: m x''+ c x' = F, where F is
               a force trusting in the direction of the target position. Differentiating: m dV + c V = k*Error, therefore
               dividing by m: dV = k1*Error - k2 V and V(k+1) = (1-k2) V(k) + k1 * Error. The overall transition can be described by tuning k1 and k2 parameters. */
         
            Momentum = (1 - DampingFactor) * Momentum + TractionFactorFree * (TargetPosition-transform.position);
            /* GameObject should move toward TargetPosition */
            transform.position = transform.position + Momentum;
            break;
            
         case ObjectState.HOLD:
            /* GameObject should follow the user input */
            Momentum = (1 - DampingFactor) * Momentum + TractionFactorCursor * (CursorPosition-transform.position);
            transform.position = transform.position + Momentum;
            break;
      }
	}
   
   /* OnCollisionEnter is called whenever this object collider encounter another object collider */
   // Collision events are only sent if one of the colliders also has a non-kinematic rigidbody attached.
   void OnCollisionStay(Collision otherObj) 
   {
      /* Destroy gameobject if it is released on the Cart */
      if (Input.GetMouseButtonUp(0) && otherObj.gameObject.name == "CartSample") 
      {         
         /* Call GameController method to add gameobject to cart list (avoiding ripetitions)*/
         if (!GameController.objList.Contains(ID))
         {
            GameController.objList.Add(ID);
         }

         /* TODO: Smooth disappear into the cart */
         
         /* Destroy this gameObject from scene */
         Destroy(gameObject);
      }
   }
}
