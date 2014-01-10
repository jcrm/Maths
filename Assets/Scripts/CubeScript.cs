using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {
	public float distance = 50f;
	public float force;
	private Vector3 centreOfMass;
	private Vector3 angularVelocity;
	private Vector3 pointOfForce;
	private Vector3 dirOfForce;
	private Vector3[] points = new Vector3[8];
	public float counter;
	public float countUp = 0.0f; 
	bool count = true;
	bool changeAng = false;
	int sscounter = 0;
	string path = "ScreenshotAuto.png";
	bool pressed = false;
	// Use this for initialization
	void Start () {
		points[0] = collider.bounds.min;
		points[1] = collider.bounds.max;
		points[2] = new Vector3(points[0].x, points[0].y, points[1].z);
		points[3] = new Vector3(points[0].x, points[1].y, points[0].z);
		points[4] = new Vector3(points[1].x, points[0].y, points[0].z);
		points[5] = new Vector3(points[0].x, points[1].y, points[1].z);
		points[6] = new Vector3(points[1].x, points[0].y, points[1].z);
		points[7] = new Vector3(points[1].x, points[1].y, points[0].z);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) && pressed == false){
			count = false;;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, distance)){
				pressed = true;
				Vector3 rayForce = (force* ray.direction);
				rigidbody.AddForceAtPosition(rayForce, hit.point, ForceMode.Impulse);
				dirOfForce = ray.direction;
				pointOfForce = hit.point;
				takeScreenShot();
			}
		}
		angularVelocity = rigidbody.angularVelocity;
		if (changeAng == false) {
			if(rigidbody.angularVelocity.x != 0 || rigidbody.angularVelocity.y != 0 ||rigidbody.angularVelocity.z != 0 ){
				angularVelocity = rigidbody.angularVelocity;
				changeAng = true;
				takeScreenShot();
			}
		}
		if (counter <= 0.0f && count == false) {
			centreOfMass = collider.bounds.center;
			count = true;
			rigidbody.velocity = Vector3.zero;
			rigidbody.isKinematic = true;
			calcCorners();
			takeScreenShot();
		}
		if (count == false) {
			counter -= Time.deltaTime;
			countUp += Time.deltaTime;
		}
	}
	void OnGUI () {
		int tWidth = 300;
		int tHeight = 20;
		int sWidth = Screen.width;
		int sHeight = Screen.height;
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.black;
		style.fontSize = 20;
		string temp ="Centre of Mass" + "(" + centreOfMass.x.ToString("0.00") + ", " + centreOfMass.y.ToString("0.00") + ", " + centreOfMass.z.ToString("0.00") + ")";
		GUI.Label(new Rect (0, 0, sWidth, tHeight), temp, style);
		temp ="Angular Velocity" + "(" + angularVelocity.x.ToString("0.00") + ", " + angularVelocity.y.ToString("0.00") + ", " + angularVelocity.z.ToString("0.00") + ")";
		GUI.Label(new Rect (sWidth-tWidth, 0, tWidth, tHeight), temp, style);
		temp = "Hitpoint" + "(" + pointOfForce.x.ToString("0.00") + ", " + pointOfForce.y.ToString("0.00") + ", " + pointOfForce.z.ToString("0.00") + ")";
		GUI.Label(new Rect (0, sHeight-tHeight, tWidth, tHeight), temp, style);
		temp = "DirForce" + "(" + dirOfForce.x.ToString("0.00") + ", " + dirOfForce.y.ToString("0.00") + ", " + dirOfForce.z.ToString("0.00") + ")";
		GUI.Label(new Rect (sWidth-tWidth, sHeight-tHeight, tWidth, tHeight), temp, style);
		temp = "Time: " + countUp.ToString() + "\tForce: " + force.ToString() + "\n Mass: " + rigidbody.mass.ToString();
		GUI.Label(new Rect (sWidth-tWidth, (sHeight/2)-tHeight, tWidth, tHeight), temp, style);
		temp = printCorners();
		GUI.Label(new Rect (0, sHeight/4, sWidth, (sHeight/4)*3), temp, style);
	}
	void takeScreenShot(){
		sscounter++;
		path = "ScreenshotAuto"+ sscounter.ToString() + ".png";
		Application.CaptureScreenshot(path);
	}
	string printCorners(){
		string temp = "";
		for (int i = 0; i < 8; i++) {
			temp+= "(" + points[i].x.ToString("0.00") + ", " + points[i].y.ToString("0.00") + ", " + points[i].z.ToString("0.00") + ")\n";
		}
		return temp;
	}
	void calcCorners(){
		points[0] = collider.bounds.min;
		points[1] = collider.bounds.max;
		points[2] = new Vector3(points[0].x, points[0].y, points[1].z);
		points[3] = new Vector3(points[0].x, points[1].y, points[0].z);
		points[4] = new Vector3(points[1].x, points[0].y, points[0].z);
		points[5] = new Vector3(points[0].x, points[1].y, points[1].z);
		points[6] = new Vector3(points[1].x, points[0].y, points[1].z);
		points[7] = new Vector3(points[1].x, points[1].y, points[0].z);
	}
}