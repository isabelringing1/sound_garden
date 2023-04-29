using System;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    [SerializeField] private AudioClip _sourceClip;
    [SerializeField] private float _transpose = 0f;
    [SerializeField] private AudioSource[] _audioSources;
    
    private int _audioSrcIndex = 0;
    private Dictionary<Note, AudioSource> _sourceDict;

    public void Initialize()
    {
        foreach (AudioSource src in _audioSources)
        {
            src.clip = _sourceClip;
        }

        int i = 0;
        _sourceDict = new Dictionary<Note, AudioSource>();
        foreach (Note note in Enum.GetValues(typeof(Note)))
        {
            _sourceDict.Add(note, _audioSources[i]);
            i++;
        }
    }

    public void PlayNote(Note note)
    {
        _sourceDict[note].pitch =  Mathf.Pow(2, ((int) note + _transpose) / 12.0f);
        _sourceDict[note].Play();
    }

    public void StopNote(Note note)
    {
        _sourceDict[note].Stop();
    }
}
