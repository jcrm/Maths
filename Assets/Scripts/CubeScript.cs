using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {
	public float distance = 50f;
	public float force = 10f;
	private Vector3 dir2;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)){
			Debug.Log("Mouse click");
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			Debug.Log("Ray check");
			if (Physics.Raycast (ray, out hit, distance)){
				Vector3 rayForce = (force* ray.direction.normalized);
				rigidbody.AddForceAtPosition(rayForce, hit.point, ForceMode.Impulse);
			}
		}
	}
	void OnGUI () {
		string temp = "(" + dir2.x.ToString("0.00") + ", " + dir2.y.ToString("0.00") + ", " + dir2.z.ToString("0.00") + ")";
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.black;
		style.fontSize = 24;
		GUI.Label(new Rect (600, 300, 100, 20), temp, style);
	}
}
