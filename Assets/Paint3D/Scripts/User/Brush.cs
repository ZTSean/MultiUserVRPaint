using UnityEngine;
using System.Collections.Generic;

public abstract class Brush : ScriptableObject, IBrush
{
	/// <summary>
	/// accesor for Brush type of current stroke
	/// </summary>
	public abstract BrushType BrushName { get; }

	/// <summary>
	/// Current Stroke
	/// </summary>
	private Stroke mStroke;

	public Stroke Stroke {
		get { return mStroke; }
		set {
			mStroke = value;
			Refresh ();
		}
	}

	//===================================================
	/// <summary>
	/// Add Vertex v to current brush
	/// </summary>
	public abstract void AddVertex (Vertex v);

	/// <summary>
	/// Mutator of options
	/// </summary>
	public abstract void SetOptions (Dictionary<string, object> newOptions);

	/// <summary>
	/// Accessor of options
	/// </summary>
	public abstract Dictionary<string, object> GetOptions ();

	//===================================================
	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public abstract void Initialize (Stroke s, Dictionary<string, object> newOptions);

	/// <summary>
	/// Refreshes the Brush
	/// </summary>
	public abstract void Refresh ();

	/// <summary>
	/// Called every Update
	/// </summary>
	public abstract void Draw ();

	/// <summary>
	/// Called when removing brush from stroke
	/// </summary>
	public abstract void Dispose ();

	public abstract void UpdateVertex (int index, Vector3 pos);

}
