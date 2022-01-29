using UnityEngine;

public class Screenshot : MonoBehaviour
{
	// Use this for initialization
	void Start () {
                ScreenCapture.CaptureScreenshot("Screenshot.png");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
