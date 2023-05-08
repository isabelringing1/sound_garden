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
    
    [SerializeField] private int _segmentLength = 4;
    
    [SerializeField] private AudioClip[] _chords;
    [SerializeField] private AudioSource _chordSrc;

    private Record[] _records;
    private FlowerRow[] _instrumentRows;

    private float _beatTime;
    private int _numInstruments;
    private int _beatsPerMeasure;
    private int _measuresPerLoop;
    private float _segmentStartTime;
    private int _beatNumber;
    private int _measureNumber;
    
    private bool _isRecording;
    private bool _isPlaying;
    private List<Note> _notes;

    private ArduinoBridge _bridge;
    
    private int[][] _chordMapping =
    {
        //       c  d  e  f  g  a  b  c
        new [] { 1, 0, 1, 0, 1, 0, 0, 1 },
        new [] { 1, 0, 1, 0, 0, 1, 0, 1 },
        new [] { 1, 0, 0, 1, 0, 1, 0, 1 },
        new [] { 0, 1, 0, 0, 1, 0, 1, 0 },
    };

    public void Initialize(FlowerRow[] instrumentRows, ArduinoBridge bridge, List<Note> notes)
    {
        _instrumentRows = instrumentRows;
        _bridge = bridge;
        _notes = notes;
        
        _metronomeSource.clip = _metronome;
        _beatTime = 60.0f / _BPM;
        _numInstruments = instrumentRows.Length - 1; // subtract one row for the chords
        _beatsPerMeasure = 4;
        _measuresPerLoop = 4;
        _records = new Record[4];
        
        _recordingDebug.SetActive(false);
        _playbackDebug.SetActive(false);
        //_bridge.SendChordData(_chordMapping);
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
            if (_beatNumber % _beatsPerMeasure == 1)
            {
                _measureNumber++;
                _measureCt.text = _measureNumber.ToString();

                // number of measures in the total round of one loop iteration for each instrument
                int measuresPerRound = _measuresPerLoop * _numInstruments;

                if (_measureNumber % measuresPerRound == 1)
                {
                    Debug.LogWarning("Start of round");
                    SelectedInstrumentIndex = 0;
                    //start recording instrument 0, play all other instruments
                    StartRecordingPart(SelectedInstrumentIndex);
                    StartPlayback(SelectedInstrumentIndex);
                }
                else if (_measureNumber % _measuresPerLoop == 1)
                {
                    SelectedInstrumentIndex = (SelectedInstrumentIndex + 1) % _numInstruments;
                    //start recording instrument 
                    StartRecordingPart(SelectedInstrumentIndex);
                    StartPlayback(SelectedInstrumentIndex);
                }

                if (_measureNumber % 4 == 1)
                {
                    PlayChord(0);
                }
                else if (_measureNumber % 4 == 2)
                {
                    PlayChord(1);
                }
                else if (_measureNumber % 4 == 3)
                {
                    PlayChord(2);
                }
                else
                {
                    PlayChord(3);
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

    private void StartPlayback(int recordingInstrumentNumber)
    {
        _isPlaying = true;
        for (int i = 0; i < _numInstruments; i++)
        {
            //Don't play currently recording instrument
            if(i == recordingInstrumentNumber) {
                continue;
            }
            Record record = _records[i];
            if(record == null) continue;
            FlowerRow row = _instrumentRows[i];
            for (int j = 0; j < record.Notes.Count; j++)
            {
                //Debug.LogWarning("Instrument " + i + " queuing " + record.Notes[j] + " at time " + record.Timings[j] + " for duration " + record.Durations[j]);
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

    void PlayChord(int index)
    {
        _chordSrc.Stop();
        _chordSrc.clip = _chords[index];
        _chordSrc.Play();
        int[] chordMap = _chordMapping[index];
        for (int i = 0; i < chordMap.Length; i++)
        {
            _instrumentRows[4].CloseFlower(_notes[i]); 
            
            if (chordMap[i] == 1)
            {
                _instrumentRows[4].OpenFlower(_notes[i]); 
            }
        }
        _bridge.UpdateChordData(chordMap);
    }
    
    private void StopMetronome()
    {
        _metronomeOn = false; 
    }
}
