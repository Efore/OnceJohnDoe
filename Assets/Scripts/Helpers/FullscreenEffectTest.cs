using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FullscreenEffectTest : MonoBehaviour {

	public Material shaderToTest = null;
	public Transform pointInScreen = null;


	void Update()
	{
		if (shaderToTest != null && pointInScreen != null) {
			float xScreenPos = Camera.main.WorldToScreenPoint(pointInScreen.position).x / Camera.main.pixelWidth;
			shaderToTest.SetFloat ("_XPosOrigin", xScreenPos);
		}
	}

	// Update is called once per frame
	void OnRenderImage(RenderTexture srcTexture, RenderTexture destTexture)
	{			
		if(shaderToTest != null)
			Graphics.Blit (srcTexture, destTexture, shaderToTest);
	}
}
