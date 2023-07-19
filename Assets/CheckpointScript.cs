using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public GameObject PlatformGeneratorPrefab;
    private bool alreadyEntered = false;
    private void OnTriggerEnter(Collider other) {
        if(other.name == "Player" && !alreadyEntered){
            alreadyEntered = true;
            Debug.Log("Entered Checkpoint");
            //create the new platform generator
            GameObject platformgen = Instantiate(PlatformGeneratorPrefab, transform.position + new Vector3(0,0,8.5f), transform.rotation);
            //platformgen.GetComponent<PlatformGenerator>().ManualStart();
            AudioController audioController = GameObject.Find("GenerativeAudio").GetComponent<AudioController>();
            audioController.StopMetro();
            other.GetComponent<PlayerController>().reachedCheckpoint = true;
            audioController.ResetSequence();
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.startedMusic = false;
            pc.CheckpointPosition = this.transform.position + new Vector3(0,1,0);
            
            
        }
    }
}
