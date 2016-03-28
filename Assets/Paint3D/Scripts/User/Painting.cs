using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Painting : MonoBehaviour
{
	/// <summary>
	/// List of all strokes created, including the current
	/// </summary>
	public List<Stroke> strokes;

	/// <summary>
	/// current stroke index in the list
	/// </summary>
	public int curStrokeIndex;

	/// <summary>
	/// The curr
	/// </summary>
	public Stroke CurStroke { get; set; }

	/// <summary>
	/// brush type of current selected brush
	/// </summary>
	public BrushType SelectedBrush { get; set; }

	public Dictionary<BrushType, Dictionary<string, object>> options;

	/// <summary>
	/// Starts the new stroke.
	/// </summary>
	public void startNewStroke (GameObject painting, GameObject brushCursor)
	{

		if (CurStroke == null) {
			// ################# using prefab to create stroke game object #################
			/*
			// Make sure current stroke is empty and Create a new stroke according to selected brush type
			System.Console.WriteLine ("strokes length", strokes.Count + 1);
			GameObject newStroke = GameObject.Instantiate (BrushManager.getPrefab (SelectedBrush));
			*/
			// #############################################################################
			string name = "Stroke_" + strokes.Count.ToString ();
			GameObject newStroke = new GameObject (name);

			// Add stroke component according to SelectedBrush
			newStroke.AddComponent <Stroke> ();

			// Make new stroke be a child of painting object
			newStroke.transform.parent = painting.transform;

			// Set the new stroke position,rotation according to brushCursor position,rotation
			newStroke.transform.position = brushCursor.transform.position;
			newStroke.transform.rotation = brushCursor.transform.rotation;

			// Scale new stroke size according whole project scale
			// TODO: whole project scale needed to be redesigned
			newStroke.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);


			// get newStroke's stroke object from script component
			CurStroke = newStroke.GetComponent<Stroke> ();
			if (CurStroke is Stroke) {
				Debug.Log ("Found Current strokeaaa");
			} else {
				Debug.Log ("Not found Strokeaaa");
			}

			// Add newStroke to stroke list 
			strokes.Add (CurStroke);
			curStrokeIndex = strokes.Count - 1;
			if (CurStroke is Stroke) {
				Debug.Log ("Found Current strokebbba");
			} else {
				Debug.Log ("Not found Strokebbb");
			}

			if (options == null) {
				Debug.Log ("opt null");
			} else {
				Debug.Log ("opt");
			}

			// Create brush scriptableobject related to current stroke
			CurStroke.Brush = BrushManager.CreateBrush (CurStroke, SelectedBrush, null);

			// because in above line, brush of curStroke haven't been created, so pass null, to CreateBrush
			// Set options after CurStroke.Brush been created
			CurStroke.Brush.SetOptions (options [CurStroke.Brush.BrushName]);

		}
		// TODO: if the CurStroke is not empty
	}

	/// <summary>
	/// Adds the vertex to current stroke's vertices.
	/// </summary>
	/// <param name="v">V.</param>
	public void AddVertex (Vertex v)
	{
		// TODO: try catch exception that if v == null or could not add v to CurStroke 
		if (CurStroke != null) {
			CurStroke.AddVertex (v);	
		}
	}

	public void EndStroke ()
	{
		CurStroke = null;
	}

	//===================================================
	void Start ()
	{
		// Allocate space for strokes list
		strokes = new List<Stroke> ();
		// Allocaate space for options dictionary
		options = new Dictionary<BrushType, Dictionary<string, object>> ();

		//Debug.Log (BrushManager.AvailableBrushes.Count);

		// Load all default options for all available brushes 
		foreach (var type in BrushManager.AvailableBrushes) {
			Dictionary<string, object> defaultOpt = BrushManager.GetDefaultOptions (type);
			// TODO: catch exception
			Dictionary<string, object> opt = new Dictionary<string, object> ();
			// loop through default options for specific brush type and add to temp opt
			foreach (var item in defaultOpt) {
				opt.Add (item.Key, item.Value);

			}

			options.Add (type, opt);
		}

		Debug.Log (options.Count);
		foreach (var type in options) {
			Debug.Log (type.Key);
			foreach (var item in type.Value) {
				Debug.Log (item.Key + " " + item.Value.ToString ());
			}
		}

		// Initialize selected brush with line brush
		SelectedBrush = BrushType.LineBrush;

		// Initialize curStroke with empty
		CurStroke = null;
		Debug.Log ("CurStroke initialize: " + CurStroke);
		curStrokeIndex = 0;
		Debug.Log ("CurStrokeIndex initialize: " + curStrokeIndex);
	}
}
