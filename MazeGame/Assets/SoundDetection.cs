using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    public float soundThreshold = 0.1f;     // sound threshold to trigger monsters
    public float soundCooldown = 1f;        // time before next detection, to avoid monsters keep changing direction
    private AudioClip micInput;             // microphone input clip
    private float lastSoundTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.Log("No Microphone detected!");
        } else
        {
            micInput = Microphone.Start(null, true, 1, 44100);      // use default microphone to keep recording 1s audio clip
        }
    }

    // Update is called once per frame
    void Update()
    {
        // detect audio input
        if (micInput != null)
        {
            // extract aduio
            NativeArray<float> samples = new NativeArray<float>(micInput.samples * micInput.channels, Allocator.Temp);
            micInput.GetData(samples.ToArray(), 0);

            // calculate average sound level
            float averageLevel = 0f;
            foreach(float sample in samples)
            {
                averageLevel += sample;
            }
            averageLevel /= samples.Length;

            // check if the sound level exceeds the threshold
            if (averageLevel > soundThreshold && Time.time > lastSoundTime + soundCooldown)
            {
                lastSoundTime = Time.time;

                Debug.Log("Player noise deteced!");
                wakeMonsters();
            }
        }
    }

    void wakeMonsters()
    {
        // call sound system's function to wake monsters chasing players
        SoundSystem.Instance.OnPlayerMakesSound(transform);
    }
}
