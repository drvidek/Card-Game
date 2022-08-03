using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelMap : MonoBehaviour
{
    public Texture2D mapImage;
    [System.Serializable]
    public struct Mappings
    {
        public GameObject spawnObj;
        public Color spawnColour;
    }
    public Mappings[] mappedElement;
    private Color pixelColour;

    void GenerateObject(int x, int y)
    {
        //read the pixel colour
        pixelColour = mapImage.GetPixel(x, y);
        if (pixelColour.a == 0)
        {
            //if no colour, do nothing
            Debug.Log("Skipped empty pixel");
            return;
        }

        //check each mapped element for matches
        foreach (Mappings map in mappedElement)
        {
            Debug.Log("Checking match: " + pixelColour.ToString() + " against " + map.spawnColour.ToString());
            //if the currently checked pixel matches one of our mapped elements
            if (map.spawnColour.Equals(pixelColour))
            {
                Debug.Log("Colour Match Found");
                //create a vector from the pixel position
                Vector2 pos = new Vector2(x, y);
                //instantiate the corresponding object
                GameObject obj = Instantiate(map.spawnObj, pos, Quaternion.identity, transform);
            }
        }
    }

    void GenerateLevel()
    {
        //scan whole texture and get pixel positions
        for (int x = 0; x < mapImage.width; x++)
        {
            for (int y = 0; y < mapImage.height; y++)
            {
                //generate an object based on the pixel found
                GenerateObject(x, y);
            }
        }
    }

    private void Start()
    {
        GenerateLevel();
    }
}
