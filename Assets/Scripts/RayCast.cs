using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {
	public float distance = 50f;
	public LineRenderer line;
	public Material lineMaterial;
	private Vector3 loc;
	private Vector3 dir1;
	private Vector3 dir2;
	// Use this for initialization
	void Start () {
		loc = new Vector3 (0, 0, 0);
		line = GetComponent<LineRenderer>();
		line.SetVertexCount(2);
		line.renderer.material = lineMaterial;
		line.SetWidth(0.1f, 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, distance)){
			if(hit.collider.CompareTag("Objects")){
				//draw invisible ray cast/vector	
				loc = hit.point;
				dir1 = hit.point - ray.origin;
				dir2 = ray.direction;
				line.enabled = true;
				line.SetPosition(0, transform.position);
				line.SetPosition(1, hit.point + hit.normal);
				if (Input.GetMouseButton (0)){

				}
			}
		}
	}
	void OnGUI () {
		string temp = "(" + loc.x.ToString("0.00") + ", " + loc.y.ToString("0.00") + ", " + loc.z.ToString("0.00") + ")";
		string temp1 = "(" + dir1.x.ToString("0.00") + ", " + dir1.y.ToString("0.00") + ", " + dir1.z.ToString("0.00") + ")";
		string temp2 = "(" + dir2.x.ToString("0.00") + ", " + dir2.y.ToString("0.00") + ", " + dir2.z.ToString("0.00") + ")";
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.black;
		style.fontSize = 24;
		GUI.Label(new Rect (10, 10, 100, 20), temp, style);
		GUI.Label(new Rect (600, 10, 100, 20), temp1, style);
		GUI.Label(new Rect (10, 600, 100, 20), temp2, style);
	}
}