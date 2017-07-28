using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamerGuy : MonoBehaviour 
{
	public float scrollSpeed;
	public float topBarrier;
	public float botBarrier;
	public float leftBarrier;
	public float rightBarrier;
	public float maxZoom;
	public float minZoom;
	public float zoomSize=5;


	void Update()
	{
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) 
		{
			zoomSize -= 1;
			if (zoomSize < maxZoom) 
			{
				zoomSize = maxZoom;
			}

		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) 
		{
			zoomSize += 1;

			if (zoomSize > minZoom) 
			{
				zoomSize = minZoom;
			}
		}


		GetComponent<Camera> ().orthographicSize = zoomSize;


		if (Input.mousePosition.y >= Screen.height * topBarrier)
		{
			transform.Translate (Vector3.up * Time.deltaTime * scrollSpeed, Space.World);
		}
		if (Input.mousePosition.y <= Screen.height * botBarrier)
		{
			transform.Translate (Vector3.down * Time.deltaTime * scrollSpeed, Space.World);
		}
		if (Input.mousePosition.x >= Screen.width * rightBarrier)
		{
			transform.Translate (Vector3.right * Time.deltaTime * scrollSpeed, Space.World);
			transform.Translate (Vector3.forward  * Time.deltaTime * scrollSpeed/2, Space.World);

		}
		if (Input.mousePosition.x <= Screen.width * leftBarrier)
		{
			transform.Translate (Vector3.left * Time.deltaTime * scrollSpeed, Space.World);
			transform.Translate (Vector3.back * Time.deltaTime * scrollSpeed/2, Space.World);

		}
	}
}