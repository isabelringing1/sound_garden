using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerRow : MonoBehaviour
{
	[SerializeField] private Flower[] _flowers;
	[SerializeField] private Instrument _instrument;


	private Dictionary<Note, Flower> _flowerDict;
	
	// Start is called before the first frame update
	void Start()
	{
		_flowerDict = new Dictionary<Note, Flower>();
		int index = 0;
		foreach (Note note in Enum.GetValues(typeof(Note)))
		{
			_flowerDict.Add(note, _flowers[index]);
			_flowers[index].Initialize(note, _instrument);
			index++;
		}
	}

	public void StartNote(Note note)
	{
		_flowerDict[note].Open();
	}

	public void EndNote(Note note)
	{
		_flowerDict[note].Close();
	}

	public void PlayNote(Note note, float duration)
	{
		
	}
}