using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class handScript : MonoBehaviour {
	
	private Hand hand;
    public GameObject fireBall;
    private GameObject fire;
	private bool wandFired = false;
	public GameObject spawn;
	private float previous;
	
	void Start (){
        hand = gameObject.GetComponent<Hand>();
    }
	/*private SteamVR_Controller.Device Controller{
        get{
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }*/
	
	void Update (){
		if(Input.GetKeyDown("escape")) {
			Application.Quit();
		}
	} 
	
	public bool getPinch(){
		return SteamVR_Actions._default.GrabPinch.GetStateDown(hand.handType);
    }
	public bool getPinchDown(){
        return SteamVR_Actions._default.GrabPinch.GetStateDown(hand.handType);
    }

    public bool getPinchUp(){
        return SteamVR_Actions._default.GrabPinch.GetStateUp(hand.handType);
    }
	public Vector3 getControllerPosition(){
        SteamVR_Action_Pose[] poseActions = SteamVR_Actions._default.poseActions;
        if (poseActions.Length > 0){
            return poseActions[0].GetLocalPosition(hand.handType);
        }
        return new Vector3(0, 0, 0);
    }
	/*[SteamVR_DefaultAction("Squeeze")]

	public SteamVR_Action_Single squeezeAction;
	public SteamVR_Action_Vector2 touchPadAction;*/
	
    void Awake() {
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate() {
		//Debug.Log("Left Trigger value:" + SteamVR_Actions._default.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand).ToString());
		//Debug.Log("Right Trigger value:" + SteamVR_Actions._default.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand).ToString());
		//if(m_BooleanAction[SteamVR_Input_sources.LeftHand].stateDown)
		/*if(SteamVR_Actions._default.GrabGrip.GetStateDown()){
			Debug.Log("oi");
		}*/
        //device = SteamVR_Controller.Input((int)trackedObj.index);

        if (getPinchDown()) {
			if (!wandFired){
				fire = Instantiate(fireBall, spawn.transform.position, transform.rotation) as GameObject;
				fire.transform.SetParent(this.gameObject.transform);
			}
			wandFired = true;
        }

        if (getPinch()) {
            if(fire.transform.localScale.x < 0.25f)
                fire.transform.localScale += new Vector3(0.002f, 0.002f, 0.002f);
        }

        if (getPinchUp()) {
            fire.transform.SetParent(null);
            Rigidbody rb = fire.GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * 8f, ForceMode.Impulse);
			//Debug.Log("Velocity: "+rb.velocity);
            //rb.velocity = SteamVR_Actions._default.poseActions.GetVelocity().velocity;
            //rb.angularVelocity = device.angularVelocity;
            rb.useGravity = true;
			wandFired = false;
        }
    }
}
