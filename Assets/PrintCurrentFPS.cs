using UnityEngine;

public class PrintCurrentFPS : MonoBehaviour
{
    // private float[] _deltaTimes = new float [600];
    // private int _deltaTimeIndex = 0;
        
    // // Update is called once per frame
    // void Update()
    // {
    //     _deltaTimes[_deltaTimeIndex] = Time.deltaTime;
    //     _deltaTimeIndex = (_deltaTimeIndex + 1) % _deltaTimes.Length;

    //     if (_deltaTimeIndex == 0)
    //     {
    //         logFps();
    //     }
    // }

    // void logFps()
    // {
    //     int samplingSeconds = 1;
    //     GetFps(samplingSeconds, out int totalFrames, out float averageTime, out float maxTime);
    //     Debug.Log( $"samplingSeconds: {samplingSeconds}, fps: {totalFrames}, avg: {averageTime * 1000}ms, max: {maxTime * 1000}ms");
    // }

    // void GetFps(int samplingSeconds, out int totalFrames, out float averageTime, out float maxTime)
    // {
    //     float accumulatedTime = 0;
    //     int frames = samplingSeconds * 60;
    //     maxTime = 0;
    //     totalFrames = 0;
        
        // for (int i = 0; i < frames; i++)
    //     {
    //         int index = _deltaTimeIndex - i - 1;
    //         if (index < 0)
    //             index += _deltaTimes.Length;
    //         float time = _deltaTimes[index];

    //         if (accumulatedTime < samplingSeconds)
    //             totalFrames++;

    //         if (time > maxTime)
    //             maxTime = time;

    //         accumulatedTime += time;
    //     }

    //     averageTime = accumulatedTime / frames;
    // }
}