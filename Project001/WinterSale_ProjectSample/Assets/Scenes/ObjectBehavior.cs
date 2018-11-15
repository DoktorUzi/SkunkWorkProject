using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

   /* Object Movement and Interaction */
   public float TractionFactorFree     = 0.01F;    // Traction factor during free movement (toward list position)
   public float TractionFactorCursor   = 0.01F;    // Traction factor during mouse hold (toward cursor position)
   public float DampingFactor          = 0.15F;    // Damping factor to avoid unwanted oscillations  
   public float ScrollSpeed         = 0.02F;    // Vertical speed of the list
   public float TreshX              = 2.5F;     // Horizontal dimension of the GameObject PickUp rectangle
   public float TreshY              = 0.8F;     // Vertical dimension of the GameObject PickUp rectangle
   public float dissolveAnimationInSeconds = 2.0F; // Duration of the main scene in seconds


   /* This Element Value Parameters */
   public float   FullPrice         = 60.0F;
   public float   SalePrice         = 40.0F;
   public int     ID                = 1;

   private float dissolveInitialTime; // Variable to store initial time for Item Dissolve
   private Camera mainSceneCamera;   
   private Animator itemAnim;
   private Vector3 TargetPosition; // To store initial value of target position and future evolution
   private Vector3 Momentum = new Vector3();   // To emulate 'inertia' to the gameobject
   
   private enum ObjectState {FREE, HOLD, DISSOLVING} // State machine to manage input from user / free movement of gameobject
   private ObjectState state;

	// Use this for initialization
	void Start () 
   {
      /* Set initial target position as the spawn point */
      TargetPosition = transform.position;
      /* Initialize state to FREE */
      state = ObjectState.FREE;
      /* Store Main Camera as Local Variable */
      mainSceneCamera = (GameObject.FindWithTag("MainCamera")).GetComponent(typeof(Camera)) as Camera;
      /* Import the Animator component of this GameObject (Cart) */
		itemAnim = GetComponent(typeof(Animator)) as Animator;
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
      
      /* Object is set to FREE movement when it is released and not dissolving */
      if (state != ObjectState.DISSOLVING && Input.GetMouseButtonUp(0))
      {
         itemAnim.SetTrigger("IsFreeToMove");
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
            Momentum = (1 - DampingFactor) * Momentum + TractionFactorCursor * (CursorPosition-transform.position);
            /* GameObject should follow the user input */
            transform.position = transform.position + Momentum;
            break;

         case ObjectState.DISSOLVING:
            Momentum = (1 - DampingFactor) * Momentum + TractionFactorCursor * (TargetPosition-transform.position);
            /* GameObject should drift to the cart */
            transform.position = transform.position + Momentum;
            
            if (Time.time >= dissolveInitialTime + dissolveAnimationInSeconds)
            {
               Destroy(gameObject);
            }
            break;

      }
	}

   /* Function used to switch state of this object to 'HOLD'. Workaround to solve the multiple grab unwanted behavior */
   public void GrabItem()
   {
      itemAnim.SetTrigger("IsOnHold");
      state = ObjectState.HOLD;
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

         /* Disappear into the cart */
         dissolveInitialTime = Time.time; // Store initial time of Dissolve Animation
         gameObject.GetComponent<Collider>().enabled = false;
         /* Set the TargetPosition of the movement equal to the Cart */
         TargetPosition = otherObj.gameObject.transform.position;
         itemAnim.SetTrigger("IsFreeToMove");
         itemAnim.SetTrigger("DissolveTrigger");
         state = ObjectState.DISSOLVING;
      }
   }
}
