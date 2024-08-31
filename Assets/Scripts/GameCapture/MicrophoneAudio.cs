using UnityEngine;

public class MicrophoneAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private string micName;
    private AudioClip micClip;
    private const int sampleRate = 44100;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0]; // 使用するマイクの名前を取得
            micClip = Microphone.Start(micName, true, 10, sampleRate);
            audioSource.clip = micClip;
            audioSource.loop = true;

            // マイクの準備が整うまで待機
            while (Microphone.GetPosition(micName) <= 0) { }

            audioSource.Play();
        }
        else
        {
            Debug.LogError("No microphone detected.");
        }
    }

    void OnApplicationQuit()
    {
        Microphone.End(micName);
    }
}
