using UnityEngine;
using System.Collections.Generic;
public class LocationFactory : ICustomFactory<LocationData>
{
    private const string _locationPath = "Textures/Location/";
    private const float _locationPartScale = 5.12f;

    private List<GameObject> _locationParts;

    /// <summary>
    /// Create and spawn location grid.
    /// </summary>
    public GridData Create(LocationData locationData, Transform parent)
    {
        _locationParts = new List<GameObject>();

        Vector3 lpoLocalScale = new Vector3(_locationPartScale, _locationPartScale);
        for (int i = 0; i < locationData.List.Count; i++)
        {
            // create location part
            LocationPart locationPart = locationData.List[i];
            GameObject locationPartObject = new GameObject();
            locationPartObject.transform.parent = parent;
            locationPartObject.name = "Location Part (" + locationPart.Id + ")";

            // set scale
            locationPartObject.transform.localScale = lpoLocalScale;

            // set position from [0; 0]
            Vector3 lpoPosition = new Vector3(
                locationPart.X * lpoLocalScale.x + Mathf.Pow(lpoLocalScale.x, 2) / 2,
                locationPart.Y * lpoLocalScale.y - Mathf.Pow(lpoLocalScale.y, 2) / 2,
                0);
            locationPartObject.transform.position = lpoPosition;

            // create sprite renderer
            string spritePath = _locationPath + locationPart.Id;
            SpriteRenderer lpSpriteRenderer = locationPartObject.AddComponent<SpriteRenderer>();
            lpSpriteRenderer.sprite = GameManager.instance.GetLocationPartSprite(spritePath);

            // create box collider
            locationPartObject.AddComponent<BoxCollider2D>();

            // save location part
            _locationParts.Add(locationPartObject);
        }

        #region Outside grid elements resizing

        // get last grid rows and columns
        LocationPart lastLP = locationData.List[locationData.List.Count - 1];
        Vector2 partData = new Vector2(-1, -1);
        foreach (string part in lastLP.Id.Split('_'))
        {
            int number;
            if (int.TryParse(part, out number))
            {
                if (partData.y == -1)
                    partData.y = number;
                else
                    partData.x = number;
            }
        }

        // resize right side (last column)
        float width = 0, height = 0;
        for (int i = 0; i <= (int)partData.y; i++)
        {
            int listIndex = (int)partData.x * (i < 0 ? 1 : i + 1) + i;
            LocationPart currentLocationPart = locationData.List[listIndex];
            LocationPart previousLocationPart = locationData.List[listIndex - 1];

            Vector3 previouslpPosition = _locationParts[listIndex - 1].transform.position;
            float halfOfNewLpWidth = currentLocationPart.Width / 2 * lpoLocalScale.x;
            float newLpXPosition = previouslpPosition.x + previousLocationPart.Width / 2 * lpoLocalScale.x + halfOfNewLpWidth;
            _locationParts[listIndex].transform.position =
                new Vector3(newLpXPosition, previouslpPosition.y, previouslpPosition.z);

            if (i == 0)
                width = newLpXPosition + halfOfNewLpWidth;
        }

        // resize bottom side (last row)
        for (int i = 0; i <= (int)partData.x; i++)
        {
            // TODO: logic
            if(i == 0)
            {
                // ** for demo we'll just return right answer
                LocationPart lastLocationPart = locationData.List[locationData.List.Count - 1];
                height = Mathf.Abs(_locationParts[_locationParts.Count - 1].transform.position.y
                    - lastLocationPart.Height / 2 * lpoLocalScale.y);
                break;
            }
        }

        #endregion

        return new GridData(0, 0, width, height);
    }
}
