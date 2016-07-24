using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallPhysics : MonoBehaviour {

	public GameObject left;
	public GameObject right;
	public GameObject ball;
	public GameObject textLabel;
	public bool gameEnded = false;
	public GameObject rBut;

	private float speed;
	private float topB;
	private float bottomB;
	private float leftB;
	private float rightB;
	private Color bgCol;
	private Vector3 startPos;
	private Rigidbody rb;
	private Camera cam;

	// Use this for initialization
	void Start () {
		ball.transform.localScale = new Vector3 (left.transform.localScale.y / 3, left.transform.localScale.y / 3, left.transform.localScale.y / 3);
		startPos = new Vector3 ((left.transform.position.x + right.transform.position.x) / 2, left.transform.position.y, left.transform.position.z);
		ball.transform.position = startPos;
		rb = ball.GetComponent<Rigidbody> ();
		rb.AddForce (-900, 0, 0);
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		bgCol = cam.backgroundColor;
		rBut.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		Text t = textLabel.GetComponent<Text>();

		Color trans = t.color;
		trans.a = 255;

		Color orig = t.color;

		t.color = Color.Lerp (orig, trans, Mathf.PingPong(Time.time, 1));

		if (!gameEnded) {

			//set boundries.
			topB = new Vector3 (Screen.width, Screen.height, cam.nearClipPlane).y;
			bottomB = new Vector3 (0, 0, cam.nearClipPlane).y + cam.WorldToScreenPoint (ball.transform.localScale).y / 4;
			leftB = cam.WorldToScreenPoint (left.transform.position).x + cam.WorldToScreenPoint (ball.transform.localScale).x / 8;
			rightB = cam.WorldToScreenPoint (right.transform.position).x - cam.WorldToScreenPoint (ball.transform.localScale).x / 8;

			speed = 2;

			//set where the paddles are relative to the screen
			left.transform.position = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth / 10, cam.WorldToScreenPoint (left.transform.position).y, cam.WorldToScreenPoint (left.transform.position).z));
			right.transform.position = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth - (cam.pixelWidth / 10), cam.WorldToScreenPoint (right.transform.position).y, cam.WorldToScreenPoint (right.transform.position).z));

			//set where the ball bounces off of
			//bottom
			if (cam.WorldToScreenPoint (ball.transform.position).y <= bottomB) {
				rb.AddForce (new Vector3 (0, 300, 0));
			}
			//top
			if (cam.WorldToScreenPoint (ball.transform.position).y >= topB) {
				rb.AddForce (new Vector3 (0, -300, 0));
			}
			//left paddle
			if (cam.WorldToScreenPoint (ball.transform.position).x <= leftB && isInY (left)) {
				rb.AddForce (new Vector3 (900, 0, 0));
			}
			//right paddle
			if (cam.WorldToScreenPoint (ball.transform.position).x >= rightB && isInY (right)) {
				rb.AddForce (new Vector3 (-900, 0, 0));
			}

			//set where the game ends
			//left side
			if (cam.WorldToScreenPoint (ball.transform.position).x < cam.WorldToScreenPoint (left.transform.position).x - cam.WorldToScreenPoint (ball.transform.localScale).x / 8) {
				gameOver ();
			}
			//right side
			if (cam.WorldToScreenPoint (ball.transform.position).x > cam.WorldToScreenPoint (right.transform.position).x + cam.WorldToScreenPoint (ball.transform.localScale).x / 8) {
				gameOver ();
			}
		}
	}

	public bool getGameEnded(){
		return gameEnded;
	}

	void gameOver(){
		gameEnded = true;

		rBut.SetActive (true);

		StartCoroutine (fadeText());

		StartCoroutine (fadeToRed());
	}

	IEnumerator fadeText(){
		yield return new WaitForSeconds(0);
		textLabel.SetActive (true);
	}

	IEnumerator fadeToRed(){
		yield return new WaitForSeconds (0);
		Color mRed = new Color (.906f, .4f, .3f, 1);
		cam.backgroundColor = mRed;
	}

	public void resetGame(){

		/*
		 * make button go away
		 * change BG to blue
		 * reset paddel and ball pos
		 * make gameended to false
		*/

		rBut.SetActive (false);
		cam.backgroundColor = bgCol;

		//set where the paddles are relative to the screen
		left.transform.position = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth / 10, cam.WorldToScreenPoint (left.transform.position).y, cam.WorldToScreenPoint (left.transform.position).z));
		right.transform.position = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth - (cam.pixelWidth / 10), cam.WorldToScreenPoint (right.transform.position).y, cam.WorldToScreenPoint (right.transform.position).z));

		ball.transform.position = new Vector3 ((left.transform.position.x + right.transform.position.x) / 2, left.transform.position.y, left.transform.position.z);
	

		gameEnded = false;
	}

	bool isInY(GameObject t){
		float tBot = t.transform.position.y - (t.transform.localScale.y / 2);
		float tTop = t.transform.position.y + (t.transform.localScale.y / 2);

		if (ball.transform.position.y >= tBot && ball.transform.position.y <= tTop){
			return true;
		}else{
			return false;
		}

	}

}
