using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransformRecorder
{
    public List<Vector2> positionRecords { get; set; }
    public List<Quaternion> rotationRecords { get; set; }
    bool isRecording {  get; set; }
    bool isReplaying{  get; set; }
    int currentReplayingFrame { get; set; }
    void StartRecording();
    void RecordTransform();
    void ReplayPath();
}
