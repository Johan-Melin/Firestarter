using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    Rigidbody rb;
	bool wildFire = false;
	bool wildGrow = true;
	float force = 5.0f;
	
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if(rb.useGravity == true) {
            GetComponent<ConstantForce>().force = new Vector3(0, force, 0);
        }
		if (wildFire){
			Vector3 scale = transform.localScale;
			if (scale.x >= .5f && wildGrow)
				wildGrow = false;
			if (wildGrow){
				scale.x +=0.005f;
				scale.y +=0.005f;
				scale.z +=0.005f;
			}else{
				scale.x -=0.002f;
				scale.y -=0.002f;
				scale.z -=0.002f;				
			}
			transform.localScale = scale;
			if (scale.z <= 0)
				Destroy(this);
		}
    }

    void OnTriggerEnter(Collider other) {
         if (other.tag == "grass" && transform.parent == null) 
			 startWildFire();
    }
	
	void startWildFire(){
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
		wildFire = true;
	}
}
