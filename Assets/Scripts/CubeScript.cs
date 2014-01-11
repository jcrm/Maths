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
	private int screenshotAutoCount = 0;
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
				Vector3 forceVector = (force* ray.direction);
				//Add force to force at a particular location on object. Use Impulse so its an insantenous force.
				rigidbody.AddForceAtPosition(forceVector, hit.point, ForceMode.Impulse);
				//Collect direction of force and the position the force hits.
				dirOfForce = ray.direction;
				pointOfForce = hit.point;
				centreOfMass = transform.TransformPoint(rigidbody.centerOfMass);
				//Output a screenshot.
				takeScreenShot();
			}
		}
		//Check to see if the angular velocity has changed
		if (hasAngVeloChanged == false) {
			if(rigidbody.angularVelocity.x != 0 || rigidbody.angularVelocity.y != 0 ||rigidbody.angularVelocity.z != 0 ){
				//Store the latest angular velocity vector
				angularVelocity = rigidbody.angularVelocity;
				//Set boolean to true
				hasAngVeloChanged = true;
				//Calculate the Corners
				calcCorners();
				//Take a screenshot
				takeScreenShot();
			}
		}
		//if the timer is couting check to see if need to turn it off
		if (isTimerOn == true) {
			//if the timer is larger than the max time turn of the timer
			if (countUp >= maxTimer) {
				//update the centr of mass varible to be displayed later
				centreOfMass = transform.TransformPoint(rigidbody.centerOfMass);
				//turn timer off
				isTimerOn = false;
				//stop the cube rotating
				rigidbody.velocity = Vector3.zero;
				rigidbody.isKinematic = true;
				//claculate the corners to be displayed on screen
				calcCorners();
				//take a screen shot
				takeScreenShot();
			}else{
				//count up if timer hasn't run out
				countUp += Time.deltaTime;
				float temp = countUp * 10;
				//take images every 0.1sec aprrox.
				if(temp >= screenshotAutoCount){
					calcCorners();
					takeScreenShot();
					screenshotAutoCount++;
				}
			}
		}
	}
	void OnGUI () {
		int tWidth = 300;
		int tHeight = 20;
		int sWidth = Screen.width;
		int sHeight = Screen.height;
		//create font size and colour to be used when outputting to screen
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.black;
		style.fontSize = 20;
		//create string of text for the centre of mass and fill it with relevant data
		string temp ="Centre of Mass" + "(" + centreOfMass.x.ToString("0.00") + ", " + centreOfMass.y.ToString("0.00") + ", " + centreOfMass.z.ToString("0.00") + ")";
		GUI.Label(new Rect (0, 0, sWidth, tHeight), temp, style);
		//update temp string with angular velocity data
		temp ="Angular Velocity" + "(" + angularVelocity.x.ToString("0.00") + ", " + angularVelocity.y.ToString("0.00") + ", " + angularVelocity.z.ToString("0.00") + ")";
		GUI.Label(new Rect (sWidth-tWidth-50, 0, tWidth+50, tHeight), temp, style);
		//update temp string with hit location data
		temp = "Hitpoint" + "(" + pointOfForce.x.ToString("0.00") + ", " + pointOfForce.y.ToString("0.00") + ", " + pointOfForce.z.ToString("0.00") + ")";
		GUI.Label(new Rect (0, sHeight-tHeight, tWidth, tHeight), temp, style);
		//update string with the force direction vector
		temp = "DirForce" + "(" + dirOfForce.x.ToString("0.00") + ", " + dirOfForce.y.ToString("0.00") + ", " + dirOfForce.z.ToString("0.00") + ")";
		GUI.Label(new Rect (sWidth-tWidth, sHeight-tHeight, tWidth, tHeight), temp, style);
		//update emp string with force, time, and mass variables
		temp = "Time: " + countUp.ToString() + "\tForce: " + force.ToString() + "\n Mass: " + rigidbody.mass.ToString();
		GUI.Label(new Rect (sWidth-tWidth, (sHeight/2)-tHeight, tWidth, tHeight), temp, style);
		//update temp string with data from the corners of the object
		temp = printCorners();
		GUI.Label(new Rect (0, sHeight/4, sWidth, (sHeight/4)*3), temp, style);
	}
	void takeScreenShot(){
		//automatically take screenshots and update the path name
		screenshotCounter++;
		filename = "ScreenshotAuto"+ screenshotCounter.ToString() + ".png";
		Application.CaptureScreenshot(filename);
	}
	string printCorners(){
		//print the location of the corners to the string
		string temp = "";
		for (int i = 0; i < 8; i++) {
			temp+= "(" + points[i].x.ToString("0.00") + ", " + points[i].y.ToString("0.00") + ", " + points[i].z.ToString("0.00") + ")\n";
		}
		return temp;
	}
	void calcCorners(){
		//work out where the corners of the obejct is.
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		for(int i = 0; i < 8; i++){
			points[i] =  transform.TransformPoint(vertices[i]);
		}
	}
}