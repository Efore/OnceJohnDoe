using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
	#region Private members
	private static ObjectPoolManager _instance = null;

	private Dictionary<string, List<GameObject>> _pooledObjects = new Dictionary<string, List<GameObject>>();

	#endregion

	#region Public members
	#endregion

	#region Properties
	public static ObjectPoolManager Singleton
	{
		get { return _instance; }
	}
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	void Awake()
	{
		_instance = this;

	}
	#endregion

	#region Private methods
	#endregion

	#region Public methods
	public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
	{		
		GameObject instance = null;
		string name = original.name.Replace("(Clone)","");
		if(_pooledObjects.ContainsKey(name))
		{
			bool unactiveFound = false;

			for(int i = 0; i < _pooledObjects[name].Count && !unactiveFound; ++i)
			{
				GameObject pooledObject = _pooledObjects[name][i];
				if(!pooledObject.activeInHierarchy && pooledObject.transform.parent == parent)
				{
					pooledObject.transform.position = position;
					pooledObject.transform.rotation = rotation;
					if(parent != null)
						pooledObject.transform.parent = parent;
					pooledObject.SetActive(true);
					instance = pooledObject;
					unactiveFound = true;
				}
			}

			if(!unactiveFound)
			{
				instance = GameObject.Instantiate(original,position,rotation) as GameObject;
				if(parent != null)
					instance.transform.parent = parent;
				instance.SetActive (true);
				_pooledObjects[name].Add(instance);
				instance.name = name + "@pooled@" + _pooledObjects[name].Count;
			}
		}
		else
		{			
			_pooledObjects.Add(name,new List<GameObject>());
			instance = GameObject.Instantiate(original,position,rotation) as GameObject;
			if(parent != null)
				instance.transform.parent = parent;
			_pooledObjects[name].Add(instance);
			instance.name = name + "@pooled@" + _pooledObjects[name].Count;
		}
		return instance;
	}

	public GameObject Instantiate(GameObject original, Transform transformData, Transform parent = null)
	{
		return this.Instantiate(original,transformData.position, transformData.rotation, parent);
	}
	#endregion



}
