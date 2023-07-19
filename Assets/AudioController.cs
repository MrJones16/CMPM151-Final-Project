using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public PlatformScript platformScript;
    private int[][] instrumentSequences;

    public PlatformGenerator platformGenerator;

    private Hv_thePatch_AudioLib pd;

    private float[] keyboardSequence;
    private float[] stringsSequence;
    private float[] drumSequence;

    private int lastScale = 0;
    public bool keyboardDisabled = false;
    public bool drumsdisabled = false;
    public bool stringsDisabled = false;

    public void CalledStart(int[][] instrumentSequences)
    {
        // Get reference to the heavy AudioLib script
        pd = GetComponent<Hv_thePatch_AudioLib>();

        // Get generated instrument sequences

        // Create float arrays for each instrument
        keyboardSequence = new float[instrumentSequences[0].Length];
        stringsSequence = new float[instrumentSequences[0].Length];
        drumSequence = new float[instrumentSequences[0].Length];
        for (int color = 0; color < instrumentSequences.Length; ++color){
            Debug.Log(color);
            for (int pos = 0; pos < instrumentSequences[color].Length; pos++){
                switch (color) {
                    case 0:
                        drumSequence[pos] = (float)instrumentSequences[color][pos];
                        break;
                    case 1:
                        stringsSequence[pos] = (float)instrumentSequences[color][pos];
                        break;
                    case 2:
                        keyboardSequence[pos] = (float)instrumentSequences[color][pos];
                        break;
                    default:
                        Debug.Log("Error");
                        break;
                }
            }
        }
        for (int i = 0; i < instrumentSequences[0].Length; i++){
            Debug.Log("keyboard "+i+"'s note is:  " + drumSequence[i]);
        }
        // Write the sequences to the Pd patch tables
        SetSequences();

        // Set up the callback for receiving messages from the Pd patch
        pd.RegisterSendHook();
        pd.FloatReceivedCallback += OnNotePlayed;
    }

    private void OnNotePlayed(Hv_thePatch_AudioLib.FloatMessage message)
    {
        // Show the receiver name and value
        // The event is currently just sending a bang (1)
        Debug.Log(message.receiverName + ": " + message.value);

        switch (message.receiverName)
        {
            case "keyboardPlatform":
                // Make the next keyboard platform appear
                if (!keyboardDisabled)
                platformScript.enableKeyboard();
                break;
            case "stringsPlatform":
                // Make the next strings platform appear
                if (!stringsDisabled)
                platformScript.enableStrings();
                break;
            case "drumPlatform":
                // Make the next drum platform appear
                if (!drumsdisabled)
                platformScript.enableDrums();
                break;
        }
    }

    public void SetSequences()
    {
        // These float arrays must have the same length as the table being written to (12)
        pd.FillTableWithFloatBuffer("keyboardSequence", keyboardSequence);
        pd.FillTableWithFloatBuffer("stringsSequence", stringsSequence);
        pd.FillTableWithFloatBuffer("drumSequence", drumSequence);
    }

    public void ChangeScale()
    {
        int newScale = 0;
        switch (lastScale)
        {
            case -3:
                switch (Random.Range(0, 4))
                {
                    case 0:
                        newScale = -2;
                        break;
                    case 1:
                        newScale = -1;
                        break;
                    case 2:
                        newScale = 0;
                        break;
                    case 3:
                        newScale = 2;
                        break;
                    default:
                        newScale = 0;
                        break;
                }
                break;
            case -2:
                switch (Random.Range(0, 5))
                {
                    case 0:
                        newScale = -3;
                        break;
                    case 1:
                        newScale = 0;
                        break;
                    case 2:
                        newScale = 1;
                        break;
                    case 3:
                        newScale = 2;
                        break;
                    case 4:
                        newScale = 3;
                        break;
                    default:
                        newScale = 0;
                        break;
                }
                break;
            case -1:
                switch (Random.Range(0, 6))
                {
                    case 0:
                        newScale = -3;
                        break;
                    case 1:
                        newScale = -2;
                        break;
                    case 2:
                        newScale = 0;
                        break;
                    case 3:
                        newScale = 1;
                        break;
                    case 4:
                        newScale = 2;
                        break;
                    case 5:
                        newScale = 3;
                        break;
                    default:
                        newScale = 0;
                        break;
                }
                break;
            case 0:
                switch (Random.Range(0, 6))
                {
                    case 0:
                        newScale = -3;
                        break;
                    case 1:
                        newScale = -2;
                        break;
                    case 2:
                        newScale = -1;
                        break;
                    case 3:
                        newScale = 1;
                        break;
                    case 4:
                        newScale = 2;
                        break;
                    case 5:
                        newScale = 3;
                        break;
                    default:
                        newScale = -2;
                        break;
                }
                break;
            case 1:
                switch (Random.Range(0, 4))
                {
                    case 0:
                        newScale = -3;
                        break;
                    case 1:
                        newScale = -2;
                        break;
                    case 2:
                        newScale = -1;
                        break;
                    case 3:
                        newScale = 3;
                        break;
                    default:
                        newScale = -1;
                        break;
                }
                break;
            case 2:
                switch (Random.Range(0, 6))
                {
                    case 0:
                        newScale = -3;
                        break;
                    case 1:
                        newScale = -2;
                        break;
                    case 2:
                        newScale = -1;
                        break;
                    case 3:
                        newScale = 0;
                        break;
                    case 4:
                        newScale = 1;
                        break;
                    case 5:
                        newScale = 3;
                        break;
                    default:
                        newScale = -3;
                        break;
                }
                break;
            case 3:
                switch (Random.Range(0, 6))
                {
                    case 0:
                        newScale = -3;
                        break;
                    case 1:
                        newScale = -2;
                        break;
                    case 2:
                        newScale = -1;
                        break;
                    case 3:
                        newScale = 0;
                        break;
                    case 4:
                        newScale = 1;
                        break;
                    case 5:
                        newScale = 2;
                        break;
                    default:
                        newScale = 1;
                        break;
                }
                break;
            default:
                newScale = 0;
                break;
        }

        // Send as a param to Pd
        pd.SetFloatParameter(Hv_thePatch_AudioLib.Parameter.Changescale, newScale);

        // store new scale number to avoid repeating it
        lastScale = newScale;
    }
    public void ResetSequence(){
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Resetsequence);
    }

    public void StartMetro()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Startmetro);
    }

    public void StopMetro()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Stopmetro);
    }

    public void MuteKeyboard()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Mutekeyboard);
    }

    public void UnmuteKeyboard()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Unmutekeyboard);
    }

    public void MuteStrings()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Mutestrings);
    }

    public void UnmuteStrings()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Unmutestrings);
    }

    public void MuteDrum()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Mutedrum);
    }

    public void UnmuteDrum()
    {
        pd.SendEvent(Hv_thePatch_AudioLib.Event.Unmutedrum);
    }
}
