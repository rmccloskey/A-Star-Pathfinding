using UnityEngine;
using System.Collections;

public class ScreenshotTool : MonoBehaviour {

	void LateUpdate()
	{
		if(Input.GetKeyUp(KeyCode.F12))
		{
			Debug.Log( "Capturing Screenshot..." );
			string fileName = "Screenshot_" + Random.Range( 0, 9999 ) + ".png";
			Application.CaptureScreenshot( fileName, 4 );
		}
	}
}
