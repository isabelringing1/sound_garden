using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.tvOS;

public class ArduinoBridge : MonoBehaviour
{
    SerialPort sp = new SerialPort("/dev/tty.usbmodem14401", 9600);
    private int[] _output;
    private string _savedChord;

    private bool _serialAvailable = true;
    
    public void Initialize()
    {
        try
        {
            sp.Open();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Serial Not Available!");
            _serialAvailable = false;
            return;
        }
        sp.ReadTimeout = 100; // In my case, 100 was a good amount to allow quite smooth transition. 
        sp.WriteTimeout = 100;
        _output = new int[8];
    }
    
    public int[] ReadArduinoInput()
    {
        if (!_serialAvailable)
        {
            return null;
        }
        
        StartCoroutine(AsynchronousReadFromArduino
            (   ReadInputData,     // Callback
                () => {}, // Error callback
                1f                          // Timeout (milliseconds)
            )
        );
        return _output;
    }

    public void ReadInputData(string input)
    {
        string[] splitInput = input.Split(",");
        for (int i = 0; i < splitInput.Length; i++)
        {
            _output[i] = Int32.Parse(splitInput[i]);
        }
    }
    
    public void UpdateChordData(int[] input)
    {
        string s = String.Empty;
        for (int i = 0; i < input.Length; i++)
        {
            s += input[i];
        }
        sp.WriteLine(s);
    }
    
    private IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity) {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);
        string dataString = null;
        do {
            try {
                dataString = sp.ReadLine();
            }
            catch (TimeoutException) {
                dataString = null;
            }
            if (dataString != null)
            {
                callback(dataString);
                yield break; // Terminates the Coroutine
            }
            yield return null; // Wait for next frame
            nowTime = DateTime.Now;
            diff = nowTime - initialTime;
        } while (diff.Milliseconds < timeout);

        if (fail != null)
        {
            fail();
        }
        yield return null;
    }
    
}