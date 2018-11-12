using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBehaviorScript : MonoBehaviour {

   private Animator cartAnim;

	// Use this for initialization
	void Start () {
      /* Import the Animator component of this GameObject (Cart) */
		cartAnim = GetComponent(typeof(Animator)) as Animator;
	}
	
   void OnCollisionStay(Collision otherObj) 
   {
      /* Destroy gameobject if it is released on the Cart */
      if (Input.GetMouseButtonUp(0) && otherObj.gameObject.CompareTag("ListItem")) 
      {
         /* Trigger Animation */
         cartAnim.SetTrigger("NewElementDrop");
      }
   }
}
