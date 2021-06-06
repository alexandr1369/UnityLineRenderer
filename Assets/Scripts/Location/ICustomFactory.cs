using UnityEngine;

public interface ICustomFactory<T>
{
    GridData Create(T data, Transform parent);
}
