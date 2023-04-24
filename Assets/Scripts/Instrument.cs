using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _sourceClip;
    [SerializeField] private float _transpose = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource.clip = _sourceClip;
    }

    public void PlayNote(Note note)
    {
        _audioSource.pitch =  Mathf.Pow(2, ((int) note + _transpose) / 12.0f);
        _audioSource.Play();
    }

    public void StopNote()
    {
        _audioSource.Stop();
    }
}
