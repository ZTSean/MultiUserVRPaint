﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// manager renderer part of stroke
public class LineBrush : Brush
{
	// Total number of vertex in current stroke
	private int count;
	// Current Stroke
	private Stroke stroke;
	// Brush component to draw the line
	private LineRenderer lr;

	private BrushType brushType = BrushType.LineBrush;

	// Options
	private float startWidth;
	private float endWidth;
	private Color startColor;
	private Color endColor;

	/// <summary>
	/// Override Accessor of BrushName
	/// </summary>
	public override BrushType BrushName {
		get {
			return brushType;
		}
	}

	//===================================================
	/// <summary>
	/// Add Vertex v to current brush
	/// </summary>
	public override void AddVertex (Vertex v)
	{
		lr.SetPosition (count, v.position);
	}

	/// <summary>
	/// Mutator of options
	/// </summary>
	public override void SetOptions (Dictionary<string, object> newOptions)
	{
		foreach (var item in newOptions) {
			Debug.Log (item.Key + " " + item.Value.ToString ());
		}


		object startW, endW, startC, endC;
		if (newOptions.TryGetValue ("StartWidth", out startW)) {
			// TODO: try catch invalid value exception
			if (startW is float) {
				startWidth = (float)startW;
			}
		}

		if (newOptions.TryGetValue ("EndWidth", out endW)) {
			// TODO: try catch invalid value exception
			if (startW is float) {
				startWidth = (float)startW;
			}
		}

		if (newOptions.TryGetValue ("StartColor", out startC)) {
			// TODO: try catch invalid value exception
			if (startC is Color) {
				startColor = (Color)startC;
			}
		}

		if (newOptions.TryGetValue ("EndColor", out endC)) {
			// TODO: try catch invalid value exception
			if (endColor is Color) {
				endColor = (Color)endC;
			}
		}
	}


	/// <summary>
	/// Accessor of options
	/// </summary>
	public override Dictionary<string, object> GetOptions ()
	{
		Dictionary<string, object> opt = new Dictionary<string, object> ();
		opt.Add ("StartWidth", startWidth);
		opt.Add ("EndWidth", endWidth);
		opt.Add ("StartColor", Color.blue);
		opt.Add ("EndColor", Color.blue);
		return opt;
	}

	//===================================================
	/// <summary>
	/// Refreshes the Brush(update information from its parent stroke such as count, linerenderer)
	/// </summary>
	public override void Refresh ()
	{
		count = stroke.vertices.Count;
	}

	/// <summary>
	/// Called every Update
	/// </summary>
	public override void Draw ()
	{
		//TODO: this function do not need to be called, because add vertex to linerender will be automaticly drawn
	}

	/// <summary>
	/// Called when removing brush from stroke
	/// </summary>
	public override void Dispose ()
	{
		UnityEngine.Object.Destroy (lr);
	}
}
