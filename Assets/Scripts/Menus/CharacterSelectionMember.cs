using UnityEngine;
using System.Collections;

public class CharacterSelectionMember : MonoBehaviour
{
	#region Private members

	[SerializeField]
	private GameObject _stats = null;
	[SerializeField]
	private GameObject _selected = null;
	[SerializeField]
	private GameObject _unselected = null;

	#endregion

	#region Public members

	public GameObject inGamePrefab = null;

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	public void SetSelected(bool selected)
	{
		_stats.SetActive (selected);
		_selected.SetActive (selected);
		_unselected.SetActive (!selected);
	}

	#endregion

}
