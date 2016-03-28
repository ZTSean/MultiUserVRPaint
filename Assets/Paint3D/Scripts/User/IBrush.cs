using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BrushType
{
	LineBrush
	//,TubeBrush,
	//BubbleBrush
}

public interface IBrush
{
	Stroke Stroke { get; set; }

	BrushType BrushName { get; }

	//===================================================
	/// <summary>
	/// Add Vertex v to current brush
	/// </summary>
	void AddVertex (Vertex v);

	/// <summary>
	/// Mutator of options
	/// </summary>
	void SetOptions (Dictionary<string, object> newOptions);

	/// <summary>
	/// Accessor of options
	/// </summary>
	Dictionary<string, object> GetOptions ();


	//===================================================
	/// <summary>
	/// Initialize this instance.
	/// </summary>
	void Initialize (Stroke s, Dictionary<string, object> newOptions);

	/// <summary>
	/// Refreshes the Brush
	/// </summary>
	void Refresh ();

	/// <summary>
	/// Called every Update
	/// </summary>
	void Draw ();

	/// <summary>
	/// Called when removing brush from stroke
	/// </summary>
	void Dispose ();
}