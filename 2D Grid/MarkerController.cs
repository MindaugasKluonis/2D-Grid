using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour
{
    public Marker markerPrefab;
    public Sprite markerLineSprite;
    [Range(0f, 1f)]
    public float markerOpacity;

    private Pool<Marker> _markerPool;
    private List<Marker> _activeMarkers;
    private const int PRELOAD_COUNT = 10;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);

    private void Awake()
    {
        _markerPool = new Pool<Marker>(markerPrefab, PRELOAD_COUNT);
        _activeMarkers = new List<Marker>(PRELOAD_COUNT);
    }

    public void ShowMarkerAt(Vector3 position, Color color)
    {
        Marker current = _markerPool.Spawn(position + offset, Quaternion.identity, transform);
        current.SetSpriteAndOpacity(markerLineSprite, markerOpacity);
        current.SetColor(color);

        _activeMarkers.Add(current);
    }

    public void DisableMarkers()
    {
        Marker currentMarker;

        for (int i = _activeMarkers.Count - 1; i >= 0; i--)
        {
            currentMarker = _activeMarkers[i];
            _markerPool.Despawn(currentMarker);
            _activeMarkers.Remove(currentMarker);
        }
    }
}
