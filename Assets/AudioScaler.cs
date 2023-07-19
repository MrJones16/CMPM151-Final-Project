using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AudioScaler : MonoBehaviour
{
	public static float setback = 4f;
    public static float multiplier = 8f;

	public Volume volumeObject;
	private VolumeProfile profile;
	private Bloom bloom;

    // Use this for initialization
	void Start () {
		profile = volumeObject.sharedProfile;
		profile.TryGet(out bloom);
	}
	
	// Update is called once per frame
	void Update () {

		// this rotates each cube.
		//transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);

		// animate the cube size based on sample data.
		int numPartitions = 1;
		float[] aveMag = new float[numPartitions];
		float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; //NOTE: we only display half the spectral data because the max displayable frequency is Nyquist (at half the num of bins)

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions)
            {
				aveMag[(int)partitionIndx] += AudioPeer.spectrumData [i] / (512/numPartitions);
			}
			else
            {
				partitionIndx++;
				i--;
			}
		}

        //scale and bound the average magnitude.
        for (int i = 0; i < numPartitions; i++)
        {
            aveMag[i] = (float)0.5 + aveMag[i] * 100;
            if (aveMag[i] > 100)
            {
                aveMag[i] = 100;
            }
        }
		bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, (aveMag[0] * multiplier) - setback, 8*Time.deltaTime);

	}
}
