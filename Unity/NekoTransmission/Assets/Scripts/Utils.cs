using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	public static Bounds WorldToScreenBounds(Bounds bounds) {
		Bounds screenBounds = new Bounds ();
		screenBounds.SetMinMax (
			Camera.main.WorldToScreenPoint (new Vector2(bounds.min.x, bounds.min.y)), 
			Camera.main.WorldToScreenPoint (new Vector2(bounds.max.x, bounds.max.y)));
		screenBounds.extents = new Vector3(screenBounds.extents.x, screenBounds.extents.y, Mathf.Infinity);
		return screenBounds;
	}

	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f) {
		GameObject myLine = new GameObject("line");
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Sprites/Default"));
		lr.SetColors(color, color);
		lr.SetWidth(0.1f, 0.1f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}

}
