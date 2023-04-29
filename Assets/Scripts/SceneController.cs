using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Note
{
    C1 = 0,
    D = 2,
    E = 4,
    F = 5,
    G = 7,
    A = 9,
    B = 11,
    C2 = 12,
}
public class SceneController : MonoBehaviour
{
    [SerializeField] private MusicController _MusicController;
    [SerializeField] private FlowerRow[] _InstrumentRows;

    private ArduinoBridge _arduinoBridge;
    private float[,] _timingsMatrix;
    private bool[,] _flowerStates;
    private List<Note> _notes;
    
    void Start()
    {
        _arduinoBridge = new ArduinoBridge();
        _arduinoBridge.Initialize();
        _timingsMatrix = new float[3, 13];
        _flowerStates = new bool[3, 8];
        _notes = new List<Note>();
        foreach (Note note in Enum.GetValues(typeof(Note)))
        {
            _notes.Add(note);
        }
        _MusicController.Initialize(_InstrumentRows, _arduinoBridge, _notes);
    }

    // Update is called once per frame
    void Update()
    {
        int[] outputs = _arduinoBridge.Update();
        if (outputs == null)
        {
            return;
        }
        for (int i = 0; i < outputs.Length; i++)
        {
            if (outputs[i] != 0 && !_flowerStates[0, i])
            {
                ProcessInputNoteStart(_notes[i]);
                _flowerStates[0, i] = true;
            }
            else if (outputs[i] == 0 && _flowerStates[0, i])
            {
                ProcessInputNoteEnd(_notes[i]);
                _flowerStates[0, i] = false;
            }
        }
        
        // Toggle between Rows
        if (Input.GetKeyDown("a"))
        {
            _MusicController.SelectedInstrumentIndex = 3;
        }
        if (Input.GetKeyDown("s"))
        {
            _MusicController.SelectedInstrumentIndex = 0;
        }
        if (Input.GetKeyDown("d"))
        {
            _MusicController.SelectedInstrumentIndex = 1;
        }
        if (Input.GetKeyDown("f"))
        {
            _MusicController.SelectedInstrumentIndex = 2;
        }
        
        // listen for keyboard input
        if (Input.GetKeyDown("1"))
        {
            ProcessInputNoteStart(Note.C1);
        }
        else if (Input.GetKeyUp("1"))
        {
            ProcessInputNoteEnd(Note.C1);
        }
        
        if (Input.GetKeyDown("2"))
        {
            ProcessInputNoteStart(Note.D);
        }
        else if (Input.GetKeyUp("2"))
        {
            ProcessInputNoteEnd(Note.D);
        }
        
        if (Input.GetKeyDown("3"))
        {
            ProcessInputNoteStart(Note.E);
        }
        else if (Input.GetKeyUp("3"))
        {
            ProcessInputNoteEnd(Note.E);
        }
        
        if (Input.GetKeyDown("4"))
        {
            ProcessInputNoteStart(Note.F);
        }
        else if (Input.GetKeyUp("4"))
        {
            ProcessInputNoteEnd(Note.F);
        }
        
        if (Input.GetKeyDown("5"))
        {
            ProcessInputNoteStart(Note.G);
        }
        else if (Input.GetKeyUp("5"))
        {
            ProcessInputNoteEnd(Note.G);
        }
        
        if (Input.GetKeyDown("6"))
        {
            ProcessInputNoteStart(Note.A);
        }
        else if (Input.GetKeyUp("6"))
        {
            ProcessInputNoteEnd(Note.A);
        }
        
        if (Input.GetKeyDown("7"))
        {
            ProcessInputNoteStart(Note.B);
        }
        else if (Input.GetKeyUp("7"))
        {
            ProcessInputNoteEnd(Note.B);
        }
        
        if (Input.GetKeyDown("8"))
        {
            ProcessInputNoteStart(Note.C2);
        }
        else if (Input.GetKeyUp("8"))
        {
            ProcessInputNoteEnd(Note.C2);
        }
    }

    void ProcessInputNoteStart(Note note)
    {
        _InstrumentRows[_MusicController.SelectedInstrumentIndex].StartNote(note);
        if (_MusicController.SelectedInstrumentIndex < 3)
        {
            _timingsMatrix[_MusicController.SelectedInstrumentIndex, (int) note] = Time.time;
        }
    }

    void ProcessInputNoteEnd(Note note)
    {
        _InstrumentRows[_MusicController.SelectedInstrumentIndex].EndNote(note);
        if (_MusicController.SelectedInstrumentIndex == 3 || _timingsMatrix[_MusicController.SelectedInstrumentIndex, (int) note] == 0)
        {
            return;
        }

        float startTime = _timingsMatrix[_MusicController.SelectedInstrumentIndex, (int) note];
        float duration = Time.time - startTime;
        _MusicController.ProcessInputNote(_MusicController.SelectedInstrumentIndex, note, startTime, duration);

        _timingsMatrix[_MusicController.SelectedInstrumentIndex, (int) note] = 0;
    }
}
