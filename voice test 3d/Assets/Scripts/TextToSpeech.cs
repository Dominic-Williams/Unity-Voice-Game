using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    private async void Start()
    {
        var credentials = new BasicAWSCredentials("AKIASFIXCZYLDHCWU4EA", "ki552IkzrdY0ihRgNPw5JW3/9engZGxm9HeCqLpa");
        var client = new AmazonPollyClient(credentials, RegionEndpoint.EUWest1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = "Testing amazon polly, in unity!",
            Engine = Engine.Neural,
            VoiceId = VoiceId.Emma,
            OutputFormat = OutputFormat.Mp3
        };

        var response = await client.SynthesizeSpeechAsync(request);

        WriteIntoFile(response.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
        {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            //audioSource.Play();
        }
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            { 
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}
