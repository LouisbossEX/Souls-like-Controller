using System;
using UnityEngine;

[Serializable]
public class ClampedFloatValue : ClampedValue<float>
{
    public ClampedFloatValue(float value, float min, float max) : base(value, min, max)
    {
    }

    protected override float Clamp(float value, float min, float max) => Mathf.Clamp(value, min, max);
    protected override float InverseLerp(float value, float min, float max) => Mathf.InverseLerp(min, max, value);
    protected override float Lerp(float lerpValue, float min, float max) => Mathf.Lerp(min, max, lerpValue);
}
