using UnityEngine;
using System.Collections;

public class GoblinScript : MonoBehaviour {
    
    Animator anim;
    private int health = 2;
    public GameObject goblinNeck;
    private bool dead = false;
	public GameObject spawnParticle;
    private bool shrink = false;
	private float distanceBeforeAttack = 2f;
	public bool isAttacking = false;
	
	//private Collider[] rigColliders;
	//private Rigidbody[] rigRigidbodies;
	//private Rigidbody mainRb;

	void Awake(){
		/*rigColliders = GetComponentsInChildren<Collider>();
        rigRigidbodies = GetComponentsInChildren<Rigidbody>();
		mainRb = GetComponent<Rigidbody>();
        foreach (Rigidbody rb in rigRigidbodies){
            rb.isKinematic = true;
        }
		mainRb.isKinematic = false;*/
	}
	
	void Start () {
        anim = GetComponent<Animator>();
        transform.LookAt(Camera.main.transform);
		
		OptionalCloathing(0);
		OptionalCloathing(1);
		OptionalCloathing(4);
		OptionalCloathing(5);
		OptionalCloathing(6);
		transform.localScale = new Vector3(0.01F, 0.01F,0.01F);
   }

    void Update() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") ||
			anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")){
			if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f &&
			anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f){
				isAttacking = true;
				GameObject bC = GameObject.Find("BodyCollider");
				bC.GetComponent<PlayerBodyCollider>().showDamage();
				//Debug.Log("hit");
			}
		}
		else
            isAttacking = false;			
		if (!dead && transform.localScale.x < 1.0f)
			transform.localScale += new Vector3(0.01F, 0.01F,0.01F);
		if (dead && shrink){
			Vector3 pos = transform.position;
			pos.y -= 0.002f;
			transform.position = pos;
		}
		if(dead)
			return;
		Vector3 targetPos = Camera.main.gameObject.transform.position;
        float dist = Vector3.Distance(targetPos, transform.position);
        if (dist > distanceBeforeAttack) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
                anim.SetBool("advance", false);
            else {
                anim.SetBool("advance", true);
                AdvanceOnPlayer();
            }
        }else {
            anim.SetBool("advance", false);
            if ((anim.GetNextAnimatorStateInfo(0).IsName("Idle"))) {
                string attackState = string.Format("attack{0}", Random.Range((int)1, (int)3));
                anim.SetTrigger(attackState);
            }
        }
    }
	
	/*void OnDrawGizmosSelected()
    {
    Vector3 targetPos = Camera.main.gameObject.transform.position;
    // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }*/
	
	void OptionalCloathing(int i){
		if(Random.Range(0f, 1f) > 0.5f)
			transform.GetChild(i).gameObject.SetActive(false);		
	}

    void AdvanceOnPlayer() {
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
			return;
        float step = 1f * Time.deltaTime;
        Vector3 dir = Camera.main.transform.position;
        dir.y = 0f;
        dir.Normalize();
        transform.position = Vector3.MoveTowards(transform.position, dir, step);
    }

    //Look at player
    void LateUpdate () {
        if (health <= 0)
            return;
		Vector3 lookVector = Camera.main.transform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
        goblinNeck.transform.LookAt(Camera.main.transform);
        goblinNeck.transform.rotation *= Quaternion.Euler(0, -75, -110);
    }

    void OnTriggerEnter(Collider other) {
        if (dead)
            return;
        if (other.tag == "fireball" && transform.parent == null) {
            if (--health <= 0) {
				GameObject score = GameObject.Find("ScoreDisplay");
				score.GetComponent<Score>().score++;
				int newScore = score.GetComponent<Score>().score;
				score.GetComponent<TextMesh>().text = "Score: "+newScore;
				//Rigidbody otherRb = other.GetComponent<Rigidbody>();
				//toggleRagdoll(otherRb.velocity);
                anim.SetTrigger("killed");
				dead = true;
				Invoke("Shrink", 2.0f);
				Object.Destroy(gameObject, 5.0f);
				Destroy(other.gameObject);
            }else
                anim.SetTrigger("takingDamage");
        }
    }
	
	/*void toggleRagdoll(Vector3 force){
		GetComponent<Animator>().enabled = false;
		foreach (Collider col in rigColliders){
			col.enabled = true;
		}
		foreach (Rigidbody rb in rigRigidbodies){
			rb.isKinematic = false;
			if(rb.name == "goblinPelvis"){
				rb.AddForce(force * 0.5f, ForceMode.Impulse);
			}
		}
	}*/

	void Shrink(){
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.detectCollisions = false;
		rb.useGravity = false;
		shrink = true;
	}
}
