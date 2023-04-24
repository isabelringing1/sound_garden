using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Note
{
    C = 0,
    D = 2,
    E = 4,
    F = 5,
    G = 7,
    A = 9,
    B = 11
}
public class SceneController : MonoBehaviour
{
    [SerializeField] private FlowerRow _TopRow;
    [SerializeField] private FlowerRow[] _InstrumentRows;

    private FlowerRow _selectedFlowerRow;
    
    void Start()
    {
        _selectedFlowerRow = _TopRow;
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle between Rows
        if (Input.GetKeyDown("a"))
        {
            _selectedFlowerRow = _TopRow;
        }
        if (Input.GetKeyDown("s"))
        {
            _selectedFlowerRow = _InstrumentRows[0];
        }
        if (Input.GetKeyDown("d"))
        {
            _selectedFlowerRow = _InstrumentRows[1];
        }
        if (Input.GetKeyDown("f"))
        {
            _selectedFlowerRow = _InstrumentRows[2];
        }
        
        // listen for keyboard input
        if (Input.GetKeyDown("1"))
        {
            StartNote(Note.C, _selectedFlowerRow);
        }
        else if (Input.GetKeyUp("1"))
        {
            EndNote(Note.C, _selectedFlowerRow);
        }
        
        if (Input.GetKeyDown("2"))
        {
            StartNote(Note.D, _selectedFlowerRow);
        }
        else if (Input.GetKeyUp("2"))
        {
            EndNote(Note.D, _selectedFlowerRow);
        }
        
        if (Input.GetKeyDown("3"))
        {
            StartNote(Note.E, _selectedFlowerRow);
        }
        else if (Input.GetKeyUp("3"))
        {
            EndNote(Note.E, _selectedFlowerRow);
        }
        
        if (Input.GetKeyDown("4"))
        {
            StartNote(Note.F, _selectedFlowerRow);
        }
        else if (Input.GetKeyUp("4"))
        {
            EndNote(Note.F, _selectedFlowerRow);
        }
        
        if (Input.GetKeyDown("5"))
        {
            StartNote(Note.G, _selectedFlowerRow);
        }
        else if (Input.GetKeyUp("5"))
        {
            EndNote(Note.G, _selectedFlowerRow);
        }
        
        if (Input.GetKeyDown("6"))
        {
            StartNote(Note.A, _selectedFlowerRow);
        }
        else if (Input.GetKeyUp("6"))
        {
            EndNote(Note.A, _selectedFlowerRow);
        }
        
        if (Input.GetKeyDown("7"))
        {
            StartNote(Note.B, _selectedFlowerRow);
        }
        else if (Input.GetKeyUp("7"))
        {
            EndNote(Note.B, _selectedFlowerRow);
        }
    }

    void StartNote(Note note, FlowerRow flowerRow)
    {
        flowerRow.StartNote(note);
    }
    
    void EndNote(Note note, FlowerRow flowerRow)
    {
        flowerRow.EndNote(note);
    }
}
