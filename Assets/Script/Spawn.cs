using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spherePrefab;

    // Fixed spawn position as per your request
    private Vector3 spawnPosition = new Vector3(-1.58780003f, 0.97299999f, 8.91629982f);

    // Method to spawn a sphere with a specific tag
    public void SpawnSphere(string tag)
    {
        GameObject newSphere;

        if (spherePrefab != null)
        {
            newSphere = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newSphere.transform.position = spawnPosition;
        }

        // Assign the tag to the new sphere
        newSphere.tag = tag;

        // Get color from the tag as hex code
        string hexColor = GetHexColorFromTag(tag);

        // Convert hex to Color
        Color sphereColor;
        if (TryParseHexColor(hexColor, out sphereColor))
        {
            Renderer sphereRenderer = newSphere.GetComponent<Renderer>();
            if (sphereRenderer != null)
            {
                sphereRenderer.material.color = sphereColor;
            }
        }
        else
        {
            Debug.LogWarning("Invalid hex color code: " + hexColor);
        }
    }

    // Map tags to hex color codes
    private string GetHexColorFromTag(string tag)
    {
        switch (tag)
        {
            case "ClAtom":
                return "#36BD37";
            default:
                return "#FFFFFF";
        }
    }

    // Converts hex string to Color, returns true if successful
    private bool TryParseHexColor(string hex, out Color color)
    {
        color = Color.white;

        if (string.IsNullOrEmpty(hex))
            return false;

        // Remove '#' if present
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length == 6 || hex.Length == 8)
        {
            uint hexVal;
            if (uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hexVal))
            {
                if (hex.Length == 6)
                {
                    float r = ((hexVal >> 16) & 0xFF) / 255f;
                    float g = ((hexVal >> 8) & 0xFF) / 255f;
                    float b = (hexVal & 0xFF) / 255f;
                    color = new Color(r, g, b, 1f);
                    return true;
                }
                else if (hex.Length == 8)
                {
                    float a = ((hexVal >> 24) & 0xFF) / 255f;
                    float r = ((hexVal >> 16) & 0xFF) / 255f;
                    float g = ((hexVal >> 8) & 0xFF) / 255f;
                    float b = (hexVal & 0xFF) / 255f;
                    color = new Color(r, g, b, a);
                    return true;
                }
            }
        }

        return false;
    }
}