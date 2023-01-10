using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInputBehaviour : MonoBehaviour
{
    public int audioSampleRate = 44100;
    public string microphone;
    public FFTWindow fftWindow;
    public static float currentMax = 0.0f;

    private List<string> options = new List<string>();
    private AudioSource audioSource;
    private float[] spectrum = new float[2048];
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        foreach(string device in Microphone.devices){
            if(microphone != null){
                microphone = device;
                break;
            }
        }
        UpdateMicrophone();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        currentMax = spectrum[0];

        for (int y = 0; y < spectrum.Length; y++){
            if(spectrum[y] >= currentMax){
                currentMax = spectrum[y];
            }
        }    
    }

    void UpdateMicrophone(){
        audioSource.Stop();
        audioSource.clip = Microphone.Start(microphone, true, 1, audioSampleRate);
        audioSource.loop = true;

        if(Microphone.IsRecording(microphone)){
            while(!(Microphone.GetPosition(microphone) > 0)){}
            audioSource.Play();
        }
        else{
            Debug.Log("Something went wrong");
        }
    }
}
