using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform FloorTilePrefab;
    public Transform FireTilePrefab;
    public Transform BackgroundPrefab;
    public Transform TreePrefab;
    public Transform SpikeTilePrefab;
    public Vector2 BottomLeftCoordinates;

    public const int numHorizontalTiles = 100;

    private List<PlatformComboEnum> platformCombos = new List<PlatformComboEnum>()
    {
        PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
,PlatformComboEnum.None
    };

    private List<PlatformEnum> platforms = new List<PlatformEnum>()
    {
        PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    ,PlatformEnum.None
    };

    private List<TreeTileEnum> treeTiles = new List<TreeTileEnum>()
    {
        TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.None
        ,TreeTileEnum.None
        ,TreeTileEnum.None
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.None
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
        ,TreeTileEnum.Tree
        ,TreeTileEnum.None
        ,TreeTileEnum.Tree
    };

    private List<FloorTileEnum> floorTiles = new List<FloorTileEnum>()
        {
           FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Fire
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Spike
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Spike
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Spike
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
            ,FloorTileEnum.Stone
        };

    private void Start()
    {
        var width = FloorTilePrefab.localScale.x;
        var height = FloorTilePrefab.localScale.y;

        for (var i = 0; i < floorTiles.Count; i++)
        {
            Instantiate(GetFloorPrefabFromData(i), new Vector2(BottomLeftCoordinates.x + (i * width), BottomLeftCoordinates.y), Quaternion.identity);
        }

        var widthBackground = BackgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        var numBackgrounds = widthBackground / width;

        for (var i = 0; i < 10; i++)
        {
            Instantiate(BackgroundPrefab, new Vector2(BottomLeftCoordinates.x + (i * widthBackground), -1.74f), Quaternion.identity);
        }

        for (var i = 0; i < treeTiles.Count; i++)
        {
            var prefab = GetTreePrefabFromData(i);

            if (prefab != null)
                Instantiate(prefab, new Vector2(BottomLeftCoordinates.x + (i * width * (floorTiles.Count / treeTiles.Count)), -4.023f), Quaternion.identity);
        }

        // lower platform

        for (var i = 0; i < platformCombos.Count; i++)
        {
            var prefab = GetTreePrefabFromData(i);

            if (prefab != null)
                Instantiate(prefab, new Vector2(BottomLeftCoordinates.x + (i * width), -4.023f), Quaternion.identity);
        }

        for (var i = 0; i < platformCombos.Count; i++)
        {
            var prefab = GetTreePrefabFromData(i);

            if (prefab != null)
                Instantiate(prefab, new Vector2(BottomLeftCoordinates.x + (i * width * (floorTiles.Count / treeTiles.Count)), -4.023f), Quaternion.identity);
        }


        // raised platform


    }

    private GameObject GetFloorPrefabFromData(int i)
    {
        var tileChoice = floorTiles[i];

        switch (tileChoice)
        {
            case FloorTileEnum.Stone:
                return FloorTilePrefab.gameObject;
            case FloorTileEnum.Fire:
                return FireTilePrefab.gameObject;
            case FloorTileEnum.Spike:
                return SpikeTilePrefab.gameObject;
            default:
                return FloorTilePrefab.gameObject;
        }
    }

    private GameObject GetTreePrefabFromData(int i)
    {
        if (treeTiles[i] == TreeTileEnum.None)
            return null;
        else
            return TreePrefab.gameObject;
    }
}

public enum FloorTileEnum
{
    None = 0,
    Stone = 1,
    Fire = 2,
    Spike = 3
}

public enum TreeTileEnum
{
    None = 0,
    Tree = 1
}

public enum PlatformComboEnum
{
    None,
    Frank,
    Spider,
    Cauldron,
    Ghost
}

public enum PlatformEnum
{
    None,
    Platform
}