using UnityEngine;
using System.Collections;

public class  CameraBorder : EnhancedMonoBehaviour
{
	#region Private members
	#endregion

	#region Public members
	#endregion

	#region Properties
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	void OnTriggerEnter(Collider other)
	{
		other.gameObject.SendMessage("CameraLimitReached",this,SendMessageOptions.DontRequireReceiver);
	}
	#endregion

	#region Private methods
	#endregion

	#region Public methods
	#endregion



}
