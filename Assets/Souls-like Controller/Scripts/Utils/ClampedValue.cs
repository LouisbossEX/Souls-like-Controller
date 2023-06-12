using UnityEngine;

public abstract class ClampedValue<T>
{
    private T value;
    private T min;
    private T max;
    private float lerpValue;

    public T Value
    {
        get => value;
        set
        {
            this.value = Clamp(value, min, max);
            lerpValue = InverseLerp(this.value, min, max);
        }
    }

    public float LerpValue
    {
        get => lerpValue;
        set
        {
            value = Mathf.Clamp01(value);
            this.value = Lerp(value, min, max);
            lerpValue = value;
        }
    }

    protected ClampedValue(T value, T min, T max)
    {
        this.min = min;
        this.max = max;
        Value = value;
    }

    protected abstract T Clamp(T value, T min, T max);
    protected abstract T Lerp(float lerpValue, T min, T max);
    protected abstract float InverseLerp(T value, T min, T max);

}