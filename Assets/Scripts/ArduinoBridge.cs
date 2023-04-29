using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.tvOS;

public class ArduinoBridge
{
    SerialPort sp = new SerialPort("/dev/tty.usbmodem144101", 9600);
    private int[] _output;

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
        _output = new int[8];
    }
    
    public int[] Update()
    {
        if (sp.IsOpen){
            try
            {
                string[] splitInput = sp.ReadLine().Split(",");
                //Debug.Log(input);
                for (int i = 0; i < splitInput.Length; i++)
                {
                    _output[i] = Int32.Parse(splitInput[i]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        return _output;
    }

    public void SendChordData(int[] input)
    {
        if (!_serialAvailable)
        {
            return;
        }
        string s = String.Empty;
        for (int i = 0; i < input.Length; i++)
        {
            s += input[i];
        }
        sp.WriteLine(s);
    }
    
}