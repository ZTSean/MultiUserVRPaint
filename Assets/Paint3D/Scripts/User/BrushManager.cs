using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

public class BrushManager
{
	// available brushes for current user
	private static List<BrushType> availableBrushes;

	private static Dictionary<BrushType, GameObject> availableBrushPrefabs;

	// accessor of availableBrushes
	public static List<BrushType> AvailableBrushes {
		get {
			if (availableBrushes == null) {
				availableBrushes = new List<BrushType> ();
				foreach (BrushType b in Enum.GetValues(typeof(BrushType))) {
					availableBrushes.Add (b);

				}
			}

			return availableBrushes;
		}
	}

	// accessor of availableBrushPrefabs
	public static Dictionary<BrushType, GameObject> AvailableBrushPrefabs {
		get {
			if (availableBrushPrefabs == null) {
				availableBrushPrefabs = new Dictionary<BrushType, GameObject> ();
				List<GameObject> prefabs = new List<GameObject> ();

				// !!! the path of all available brushes prefab
				string[] path = new string[]{ "Assets/Paint3D/Prefabs/Strokes" };
				string[] guids = AssetDatabase.FindAssets ("Stroke", path);


				foreach (string guid in guids) {
					string tmp = AssetDatabase.GUIDToAssetPath (guid);
					Debug.Log (tmp);
					prefabs.Add (AssetDatabase.LoadAssetAtPath (tmp, typeof(GameObject)) as GameObject);
				}


				Debug.Log ("loop through prefab:" + prefabs.Count);
				// console write names of all prefabs
				foreach (var item in prefabs) {
					Debug.Log (item.name);
				}

				// following code prerequisite: prefab name is constant to name in enum BrushType
				foreach (BrushType type in Enum.GetValues(typeof(BrushType))) {
					foreach (var item in prefabs) {
						// !!! play with file name: need to be optimazed
						if ((type.ToString () + "Stroke") == item.name) {
							AvailableBrushPrefabs.Add (type, (GameObject)item);
							break;
						}
					}
				}

			}

			return availableBrushPrefabs;
		}
	}

	/// <summary>
	/// Creates the brush.
	/// </summary>
	/// <returns>The new brush.</returns>
	/// <param name="stroke">Stroke.</param>
	/// <param name="name">Name.</param>
	/// <param name="options">Options.</param>
	public static IBrush CreateBrush (Stroke stroke, BrushType type, Dictionary<string, object> newOptions)
	{
		// if unable to instantiate, return LineBrush with default options by default
		// prerequisite: enum BrushType is constant with Brush class name
		IBrush o = (ScriptableObject.CreateInstance (type.ToString ()) as IBrush) ?? ScriptableObject.CreateInstance<LineBrush> ();
		o.Initialize (stroke, newOptions);
		return o;
	}

	/// <summary>
	/// Gets the default options.
	/// </summary>
	/// <returns>The default options.</returns>
	/// <param name="brushName">Brush name.</param>
	public static Dictionary<string, object> GetDefaultOptions (BrushType type)
	{
		// prerequisite: enum BrushType is constant with Brush class name
		string name = type.ToString ();
		Debug.Log ("Brush type: " + name);

		// create temporary instance of brush object
		IBrush o = (ScriptableObject.CreateInstance (name) as IBrush);

		// TODO: try catch "could not create instance exception"
		if (o == null) {
			return null;
		}

		// TODO: release temporary object o

		return o.GetOptions ();
	}

	public static GameObject getPrefab (BrushType type)
	{
		GameObject tmp;
		AvailableBrushPrefabs.TryGetValue (type, out tmp);
		return tmp;
	}
}
