﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

/**
 * This class is nescessary since Unity does 
 * not support serialization of nested lists.
 * The class implements custom [int, int] operator to support easy access. 
 **/

[System.Serializable]
public class EdgeMatrix : ISerializationCallbackReceiver {

	[SerializeField]
	public Dictionary<string,Edge> edges;

	public List<string> keys = new List<string>();
	public List<Edge> values = new List<Edge>();

	
	public EdgeMatrix(){
		edges = new Dictionary<string,Edge> ();
	}

	/*
	 * The [int,int] operator accesses an edge from x to y if there is such an edge
	 * or returns null if no such edge can be retrieved.
	 * @throw throws argumentexception if trying to add an edge that already exists
	 * */
	public Edge this[int x, int y]{
		get{
			string key = x + "," + y;
			Edge val;
			if(edges.TryGetValue(key, out val)) return val;
			else return null;
			
		}
		set{
			string key = x + "," + y;

			if((Edge) value == null) {
				edges.Remove(key);
				Debug.Log ("Removed " + key + " from edges");
			}
			if(edges.ContainsKey(key)){
				throw new ArgumentException("Trying to add duplicate key");
			}
			else{
				edges.Add(key, (Edge) value);
			}
		}

	}

	public void Remove(int x, int y){
		string key = x + "," + y;
		edges.Remove (key);
	}

	public void OnBeforeSerialize()
	{
		if (!Application.isPlaying) {
			keys.Clear ();
			values.Clear ();
			foreach (var kvp in edges) {
				keys.Add (kvp.Key);
				values.Add (kvp.Value);
			}
		}

	}
	public void OnAfterDeserialize()
	{	
		Debug.Log ("Deserialized edges");
		edges = new Dictionary<string, Edge>();
		for (int i=0; i!= Math.Min(keys.Count,values.Count); ++i)
			edges.Add(keys[i],values[i]);
	}




	public Edge[] GetList(){
		Edge[] edgelist = new Edge[edges.Values.Count];
		edges.Values.CopyTo (edgelist, 0);
		Debug.Log ("Get list of edges with size " + edgelist.Length);
		return edgelist;
	}
	




}
