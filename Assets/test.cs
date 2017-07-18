using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class test : MonoBehaviour {

	public Material _mat;

	void OnRenderImage(RenderTexture srcTexture, RenderTexture destTexture)
	{	
		Graphics.Blit (srcTexture, destTexture, _mat);
	}
}
