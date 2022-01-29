using UnityEngine;

public class Screenshot : MonoBehaviour
{
	void Start ()
	{
                ScreenCapture.CaptureScreenshot("Screenshot.png");
	}
}
