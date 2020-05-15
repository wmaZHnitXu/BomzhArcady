using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class leftBar : MonoBehaviour
{
    public string[] lines = new string[8];
    [SerializeField] private Text bar;
    public static leftBar me;
    private int filledLines;

    private void Start()
    {
        filledLines = 0;
        me = this;
        StartCoroutine(LineUp());
        AddLine("Бомж инициализирован");
    }

    public static void AddLine(string line)
    {
        if (me.filledLines < me.lines.Length)
        {
            me.lines[me.filledLines] = "> " + line;
            me.filledLines++;
        }
        else
        {
            for (int i = 0; i < me.lines.Length-1; i++)
            {
                me.lines[i] = me.lines[i + 1];
            }
            me.lines[me.filledLines - 1] = "> " + line;
        }
        me.LoadBar();
    }

    private IEnumerator LineUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if (filledLines > 0)
            {
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    lines[i] = lines[i + 1];
                }

                filledLines--;
                lines[filledLines] = string.Empty;
            }
            LoadBar();
        }
    }

    private void LoadBar()
    {
        bar.text = string.Empty;
        foreach (var str in lines)
        {
            bar.text += str + "\r\n";
        }
    }
}
