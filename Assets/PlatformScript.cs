using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public int type;
    public GameObject platformOn;
    public GameObject platformOff;
    public static bool KeyboardInstrument;
    public static bool DrumsInstrument;
    public static bool StringsInstrument;
    private void Start() {
        if(type == 0){
            KeyboardInstrument = true;
            DrumsInstrument = true;
            StringsInstrument = true;
        }
    }

    private void FixedUpdate() {
        switch (type){
            case 0:
                //Controller Type
                
                break;
            case 3:
                //Keyboard Type
                if (KeyboardInstrument){
                    platformOn.SetActive(true);
                    platformOff.SetActive(false);
                }else{
                    platformOn.SetActive(false);
                    platformOff.SetActive(true);
                }
                break;
            case 1:
                //Drums Type
                if (DrumsInstrument){
                    platformOn.SetActive(true);
                    platformOff.SetActive(false);
                }else{
                    platformOn.SetActive(false);
                    platformOff.SetActive(true);
                }
                break;
            case 2:
                //Strings Type
                if (StringsInstrument){
                    platformOn.SetActive(true);
                    platformOff.SetActive(false);
                }else{
                    platformOn.SetActive(false);
                    platformOff.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void enableKeyboard(){
        KeyboardInstrument = true;
    }
    public void disableKeyboard(){
        KeyboardInstrument = false;
    }
    public void enableDrums(){
        DrumsInstrument = true;
    }
    public void disableDums(){
        DrumsInstrument = false;
    }
    public void enableStrings(){
        StringsInstrument = true;
    }
    public void disableStrings(){
        StringsInstrument = false;
    }
}
