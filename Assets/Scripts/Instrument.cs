using System;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    [SerializeField] private AudioClip _sourceClip;
    [SerializeField] private AudioClip[] _octaveClips;
    
    [SerializeField] private float _transpose = 0f;
    [SerializeField] private AudioSource[] _audioSources;
    
    private int _audioSrcIndex = 0;
    private Dictionary<Note, AudioSource> _sourceDict;

    public void Initialize()
    {
        if (_sourceClip != null)
        {
            foreach (AudioSource src in _audioSources)
            {
                src.clip = _sourceClip;
            }
        }
        else if (_octaveClips.Length > 0)
        {
            for (int j = 0; j < _octaveClips.Length; j++)
            {
                _audioSources[j].clip = _octaveClips[j];
            }
        }
        
        _sourceDict = new Dictionary<Note, AudioSource>();
        int i = 0;
        foreach (Note note in Enum.GetValues(typeof(Note)))
        {
            _sourceDict.Add(note, _audioSources[i]);
            i++;
        }
    }

    public void PlayNote(Note note)
    {
        if (_sourceClip != null)
        {
            _sourceDict[note].pitch =  Mathf.Pow(2, ((int) note + _transpose) / 12.0f);
        }
        _sourceDict[note].Play();
    }

    public void StopNote(Note note)
    {
        _sourceDict[note].Stop();
    }
}
