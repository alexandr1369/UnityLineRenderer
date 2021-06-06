using UnityEngine;

public class LocationLoader : MonoBehaviour
{
    public GridData GridData { get; set; }

    private TextAsset _jsonLocationData;
    private LocationFactory _locationFactory;
    private LocationData _locationData;

    private void Start()
    {
        _locationFactory = new LocationFactory();
        _jsonLocationData = GameManager.instance.GetJsonLocationData();
        _locationData = JsonUtility.FromJson<LocationData>(_jsonLocationData.text);

        GridData = _locationFactory.Create(_locationData, transform);
    }
}
