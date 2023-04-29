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
    
    public void Initialize()
    {
        sp.Open();
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
    
}