
using UnityEngine;
using System.Collections;

public class HitWaveBehaviour : EnhancedMonoBehaviour
{
	#region Private members

	#endregion

	#region Public members

	#endregion

	#region Properties

	public float MaxScale
	{
		get;
		set;
	}

	#endregion

	#region Events

	public delegate void WaveFinished();
	public event WaveFinished WaveFinishedEvent; 

	#endregion

	#region MonoBehaviour calls

	protected override void OnEnable() 
	{
		MaxScale = 1.5f;
		StartCoroutine (ScaleCoroutine ());
	}

	#endregion

	#region Private methods

	private IEnumerator ScaleCoroutine()
	{		
		while (TransformRef.localScale.x < MaxScale)
		{
			TransformRef.localScale += Constants.Vector3.one * 0.1f;
			yield return new WaitForSeconds (0.01f);
		}

		if(WaveFinishedEvent != null)
			WaveFinishedEvent();

		TransformRef.localScale = Constants.Vector3.one;
	}

	#endregion

	#region Public methods

	#endregion

}
