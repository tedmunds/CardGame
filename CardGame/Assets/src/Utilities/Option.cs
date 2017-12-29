using System;
using System.Collections;
using System.Collections.Generic;

public struct Option<T> : IEnumerable<T>
{
    public T Value { get; private set; }
    private bool valid;
        
    public Option(T value) : this()
    {
        Value = value;
        valid = true;
    }

    public Option(T value, bool valid) : this()
    {
        Value = value;
        this.valid = valid;
    }

    public static implicit operator Option<T>(T value)
    {
        return new Option<T>(value);
    }

    public static implicit operator T(Option<T> option)
    {
        return option.Value;
    }
        

    public static Option<T> None = new Option<T>(default(T), false);
        
    public IEnumerator<T> GetEnumerator()
    {
        if(IsValid)
        {
            yield return Value;
        }
    }

    public void Invalidate()
    {
        Value = default(T);
        valid = false;
    }

    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); } 
    public bool IsValid { get { return valid; } }

    public override string ToString() { return valid? Value.ToString() : "None"; }
}

