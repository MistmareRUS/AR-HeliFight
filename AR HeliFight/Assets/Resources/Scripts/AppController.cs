using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class AppController
{
    public static bool isGame = false;
    public static int currentPlayer = 0;
    public static string PlayerA = string.Empty;
    public static string PlayerB = string.Empty;
    public static string PlayerC = string.Empty;
    public static string PlayerD = string.Empty;
    public static int PlayerCount = 0;

    //принимает информацию по игроку и по методам
    static public string MessageDecodingReliable(byte[] data)
    {
        string text = Encoding.Default.GetString(data);
        return text;
    }
    //принимает информацию по движению
    static public string MessageDecodingNonReliable(byte[] data)
    {
        string text = Encoding.Default.GetString(data);
        return text;
        
    }

    //передает инфу в начале боя и по методам
    static public byte[] MessageCodingReliable(string text)
    {
        byte[] code = Encoding.Default.GetBytes(text);
        return code;
    }
    //передает инфу по движению 
    static public byte[] MessageCodingNonReliable(string text)
    {
        byte[] code = Encoding.Default.GetBytes(text);
        return code;
    }
}


