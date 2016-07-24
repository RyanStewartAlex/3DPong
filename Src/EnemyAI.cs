using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	BallPhysics bp;

	public GameObject enemy;
	public GameObject ball;

	private float speed = 0;
	private bool selSpeed = true;
	private const float RANGE = 5.5f;
	private const float DEAD_ZONE = .1f;
	private float minHeight;
	private float maxHeight;
	private Camera cam;

	void moveEnemy(){
		if (ball.transform.position.y >= enemy.transform.position.y && !isLevel() && cam.WorldToScreenPoint(enemy.transform.position).y < maxHeight){
			enemy.transform.position = new Vector3 (enemy.transform.position.x, enemy.transform.position.y + speed, enemy.transform.position.z);
		}else if(ball.transform.position.y < enemy.transform.position.y && !isLevel() && cam.WorldToScreenPoint(enemy.transform.position).y > minHeight){
			enemy.transform.position = new Vector3 (enemy.transform.position.x, enemy.transform.position.y - speed, enemy.transform.position.z);
		}
	}

	bool isLevel(){
		if (Mathf.Abs(ball.transform.position.y - enemy.transform.position.y) < DEAD_ZONE) {
			return true;
		}else{
			return false;
		}
	}

	void Start() {
		bp = GetComponent<BallPhysics> ();
		cam = Camera.main.GetComponent<Camera> ();
	}

	void Update() {

		if (!bp.getGameEnded()) {

			maxHeight = Screen.height - (cam.WorldToScreenPoint (enemy.transform.localScale).y / 4) - 50;
			minHeight = 0 + (cam.WorldToScreenPoint (enemy.transform.localScale).y / 4) + 50;

			if (selSpeed) {
				selSpeed = false;
				speed = Random.Range (0.1f, .5f);
			}

			//if in X range.
			if (Mathf.Abs (enemy.transform.position.x - ball.transform.position.x) <= RANGE) {
				selSpeed = false;
				moveEnemy ();
			} else {
				selSpeed = true;
			}
		}

	}

}
