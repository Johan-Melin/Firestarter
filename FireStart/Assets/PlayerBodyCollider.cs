using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;
 
public class PlayerBodyCollider : MonoBehaviour {
	private int life = 3;
	private bool takeDamage = false;
	public Image damageImage;
	private bool cantTakeDamage = false;
	
	void Start (){
		//SteamVR_Render.instance.pauseGameWhenDashboardIsVisible = false;
		colorAlpha(0f);
	}
	
	void Update () {
		Vector3 pos = transform.position;
		pos.x = Camera.main.transform.position.x;
		pos.z = Camera.main.transform.position.z;
		transform.position = pos;
	}
	
    /*void OnTriggerEnter(Collider other) {
		if (other.tag == "SpearCollider"){
			GameObject goblin = other.transform.parent.parent.gameObject;
			if (goblin.GetComponent<GoblinScript>().isAttacking);
				showDamage();
		}
	}*/
	
	public void showDamage(){
		if (cantTakeDamage)
			return;
		cantTakeDamage = true;
		if(--life <=0){
			gameOver();
			return;
		}
		colorAlpha(0.8f);
		Invoke("resetDamage", 0.2f);
	}
	
	void resetDamage(){
		cantTakeDamage = false;
		colorAlpha(0f);
	}
	
	void gameOver(){
		colorAlpha(0.95f);
		Invoke("resetScene", 3);
	}
	
	void resetScene(){
		colorAlpha(0f);
		SceneManager.LoadScene("main");
	}
	
	void colorAlpha(float a){
		Color tempColor = damageImage.color;
        tempColor.a = a;
        damageImage.color = tempColor;		
	}
	
	/*void Fade (float start, float end) { //define Fade parmeters
		if (damageImage.color.a == start){
			for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/0.5f)) { //for the length of time
				damageImage.color = Mathf.Lerp(start, end, i); //lerp the value of the transparency from the start value to the end value in equal increments
				yield;
				//damageImage.color.a = end; // ensure the fade is completely finished (because lerp doesn't always end on an exact value)
			} //end for
		} //end if
	} //end Fade


	public IEnumerator FlashWhenHit (){
		Debug.Log("hit");
		Fade (0, 0.8f);
		yield return new WaitForSeconds(0.01f);
		Fade (0.8f, 0);
    }*/
}
