using UnityEngine;
using System.IO;
public class HandleTextFile : MonoBehaviour
{
    public string file;
    public void WriteFile(string[] lines)
    {
        string path = Application.persistentDataPath + file;
        //Write some text to the path file
        StreamWriter writer = new StreamWriter(path, true);
        if (lines != null)
        {
            foreach (string line in lines) {
                writer.WriteLine(line);
            }
        }
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    public string[] ReadFile()
    {
        string path = Application.persistentDataPath + file;
        //Read the text from directly from the path file
        StreamReader reader = new StreamReader(path);
        string[] lines = new string[100];
        int i = 0;
        while (!reader.EndOfStream)
        {
            lines[i] = reader.ReadLine();
            Debug.Log(lines[i]);
            i++;
        }
        reader.Close();

        return lines;
    }

    public string ReadValueOf(string key)
    {
        string path = Application.persistentDataPath + file;
        //Read the text from directly from the path file
        StreamReader reader = new StreamReader(path);
        string[] lines = new string[100];
        int i = 0;
        while (!reader.EndOfStream)
        {
            lines[i] = reader.ReadLine();
            Debug.Log(lines[i]);
            i++;
        }
        reader.Close();
        foreach (string line in lines)
        {
            string[] pair = line.Split("=");
            if (pair[0].Trim() == key)
            {
                return pair[1].Trim();
            }
        }
        return "";
    }
}