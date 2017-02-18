using UnityEngine;
using System.Collections;

public class StageElement : EnhancedMonoBehaviour
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

	protected override void Awake ()
	{
		base.Awake ();
		TransformRef.position = StageManager.Singleton.Get3DPosition (TransformRef.position);
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}
