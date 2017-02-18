using UnityEngine;
using System.Collections;

public class EnhancedMonoBehaviour : MonoBehaviour {

	#region Private Members

	#endregion

	#region Properties

	public Transform TransformRef
	{
		get; set;
	}

	#endregion

	#region Private methods

	protected virtual void Awake()
	{
		TransformRef = transform;
	}

	protected virtual void OnEnable()
	{
		
	}

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	protected virtual void OnDisable()
	{

	}

	#endregion
}
