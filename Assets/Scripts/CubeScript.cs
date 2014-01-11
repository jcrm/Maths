using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {
	public float force;
	private float distance = 50f;
	private Vector3 centreOfMass;
	private Vector3 angularVelocity;
	private Vector3 pointOfForce;
	private Vector3 dirOfForce;
	private Vector3[] points = new Vector3[8];
	private float countUp = 0.0f; 
	private float maxTimer = 0.5f;
	private bool isTimerOn = false;
	private bool hasAngVeloChanged = false;
	private int screenshotCounter = 0;
	private string filename = "ScreenshotAuto.png";
	private bool isPressed = false;
	// Use this for initialization
	void Start () {
		calcCorners ();
	}

	// Update is called once per frame
	void Update () {
		//Check to see if the mouse has been clicked and it the first time.
		if (Input.GetMouseButton (0) && isPressed == false){
			//Cast a Ray forwards and check to see if it hits the object.
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			//If the ray hits and object initalise the variables.
			if (Physics.Raycast (ray, out hit, distance)){
				//Start the timer and disable pressing the mouse button again.
				isTimerOn = true;
				isPressed = true;
				//Calculate the force * direction of force.
				Vector3 rayForce = (force* ray.direction);
				//Add force to force at a particular location on object. Use Impulse so its an insantenous force.
				rigidbody.AddForceAtPosition(rayForce, hit.point, ForceMode.Impulse);
				//Collect direction of force and the position the force hits.
				dirOfForce = ray.direction;
				pointOfForce = hit.point;
				centreOfMass = collider.bounds.center;
				//Output a screenshot.
				takeScreenShot();
			}
		}
		//Store the latest angular velocity vector
		angularVelocity = rigidbody.angularVelocity;
		//Check to see if the angular velocity has changed
		if (hasAngVeloChanged == false) {
			if(rigidbody.angularVelocity.x != 0 || rigidbody.angularVelocity.y != 0 ||rigidbody.angularVelocity.z != 0 ){
				//Store the latest angular velocity vector
				angularVelocity = rigidbody.angularVelocity;
				hasAngVeloChanged = true;
				calcCorners();
				takeScreenShot();
			}
		}
		if (isTimerOn == true) {
			if (countUp >= maxTimer) {
				centreOfMass = collider.bounds.center;
				isTimerOn = false;
				rigidbody.velocity = Vector3.zero;
				rigidbody.isKinematic = true;
				calcCorners();
				takeScreenShot();
			}else{
				countUp += Time.deltaTime;
			}
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
		screenshotCounter++;
		filename = "ScreenshotAuto"+ screenshotCounter.ToString() + ".png";
		Application.CaptureScreenshot(filename);
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