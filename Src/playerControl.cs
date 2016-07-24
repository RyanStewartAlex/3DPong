using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

	public GameObject plr;

	private float maxHeight;
	private float minHeight;
	private float speed = .4f;
	private Camera cam;
	private BallPhysics bp;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		bp = GetComponent<BallPhysics> ();
	}

	// Update is called once per frame
	void Update () {
		if (!bp.getGameEnded ()) {
			maxHeight = Screen.height - (cam.WorldToScreenPoint (plr.transform.localScale).y / 4) - 50;
			minHeight = 0 + (cam.WorldToScreenPoint (plr.transform.localScale).y / 4) + 50;

			//controlling
			if (Input.GetKey (KeyCode.W) && cam.WorldToScreenPoint (plr.transform.position).y < maxHeight) {
				plr.transform.position += new Vector3 (0, speed, 0);
			}
			if (Input.GetKey (KeyCode.S) && cam.WorldToScreenPoint (plr.transform.position).y > minHeight) {
				plr.transform.position -= new Vector3 (0, speed, 0);
			}
		}
	}
}
