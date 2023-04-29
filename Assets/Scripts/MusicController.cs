using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record
{
    public List<Note> Notes;
    public List<float> Timings;
    public List<float> Durations;
}

public class MusicController : MonoBehaviour
{
    public int SelectedInstrumentIndex;

    [SerializeField] private AudioSource _metronomeSource;
    [SerializeField] private AudioClip _metronome;
    
    [SerializeField] private GameObject _recordingDebug;
    [SerializeField] private GameObject _playbackDebug;

    [SerializeField] private Text _measureCt;
    [SerializeField] private Text _beatCt;
    
    [SerializeField] private float _BPM = 70.0f;
    [SerializeField] private bool _metronomeOn = true;

    [SerializeField] private GameObject[] _indicators;

    private Record[] _records;
    private FlowerRow[] _instrumentRows;

    private float _beatTime;
    private float _segmentStartTime;
    private int _beatNumber;
    private int _measureNumber;
    
    private bool _isRecording;
    private bool _isPlaying;

    public void Initialize(FlowerRow[] instrumentRows)
    {
        _instrumentRows = instrumentRows;
        
        _metronomeSource.clip = _metronome;
        _beatTime = 60.0f / _BPM;
        _records = new Record[3];
        
        _recordingDebug.SetActive(false);
        _playbackDebug.SetActive(false);
    }

    public void StartTime()
    {
        StartCoroutine(KeepTime());
    }

    private IEnumerator KeepTime()
    {
        //count in
        for (int i = 0; i < 4; i++)
        {
            _metronomeSource.Play();
            yield return new WaitForSeconds(_beatTime);
        }
        
        while (true)
        {
            _beatNumber++;
            _beatCt.text = (_beatNumber % 4 + 1).ToString();
            if (_beatNumber % 4 == 1)
            {
                _measureNumber++;
                _measureCt.text = _measureNumber.ToString();
                
                if (_measureNumber % 32 == 1)
                {
                    //TODO: Figure out what's going on in this part
                    SelectedInstrumentIndex = 3;
                    _isPlaying = false;
                    _isRecording = false;
                    _recordingDebug.SetActive(true);
                    _playbackDebug.SetActive(false);
                    SetIndicator(3);
                }
                else if (_measureNumber % 32 == 5)
                {
                    Debug.LogWarning("Instrument 1");
                    SelectedInstrumentIndex = 0;
                    //start recording instrument 0, no instruments yet
                    StartRecordingPart(SelectedInstrumentIndex);
                }
                else if (_measureNumber % 32 == 9)
                {
                    Debug.LogWarning("Instrument 2");
                    SelectedInstrumentIndex++;
                    //start recording instrument 1, playing instrument 0
                    StartRecordingPart(SelectedInstrumentIndex);
                    StartPlayback(SelectedInstrumentIndex);
                }
                else if (_measureNumber % 32 == 13)
                {
                    SelectedInstrumentIndex++;
                    //start recording instrument 2, playing two other instruments
                    StartRecordingPart(SelectedInstrumentIndex);
                    StartPlayback(SelectedInstrumentIndex);
                }
                // all other times - play what we've recorded
                else if (_measureNumber % 4 == 1)
                {
                    SetIndicator(-1);
                    StartPlayback(_records.Length);
                    _recordingDebug.SetActive(false);
                    _playbackDebug.SetActive(true);

                }
            }

            if (_metronomeOn)
            {
                _metronomeSource.Play();
            }
            yield return new WaitForSeconds(_beatTime);
        }
    }
    
    private void StartRecordingPart(int instrumentIndex)
    {                    
        Debug.LogWarning("Starting recording instrument " + instrumentIndex);
        _segmentStartTime = Time.time;
        _isRecording = true;
        _isPlaying = false;
        _recordingDebug.SetActive(true);
        _playbackDebug.SetActive(false);
        SetIndicator(instrumentIndex);
        
        _records[instrumentIndex] = new Record
        {
            Notes = new List<Note>(100),
            Timings = new List<float>(100),
            Durations = new List<float>(100),
        };
    }

    public void ProcessInputNote(int instrumentIndex, Note note, float startTime, float duration)
    {
        if (!_isRecording || instrumentIndex != SelectedInstrumentIndex || SelectedInstrumentIndex > 2)
        {
            return;
        }

        float adjustedStartTime = startTime - _segmentStartTime;
        _records[instrumentIndex].Notes.Add(note);
        _records[instrumentIndex].Timings.Add(adjustedStartTime);
        _records[instrumentIndex].Durations.Add(duration);
    }

    private void StartPlayback(int numInstruments)
    {
        _isPlaying = true;
        for (int i = 0; i < numInstruments; i++)
        {
            Record record = _records[i];
            FlowerRow row = _instrumentRows[i];
            for (int j = 0; j < record.Notes.Count; j++)
            {
                Debug.LogWarning("For instrument " + i + " queuing note " + record.Notes[j] + "at time " + record.Timings[j] + "for duration " + record.Durations[j]);
                StartCoroutine(row.QueueNote(record.Notes[j], record.Timings[j], record.Durations[j]));
            }
        }
    }

    private void SetIndicator(int index)
    {
        for (int i = 0; i < _indicators.Length; i++)
        {
            _indicators[i].SetActive(i == index);
        }
    }

    private void StopMetronome()
    {
        _metronomeOn = false; 
    }
}
