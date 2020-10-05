using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class Utils : MonoBehaviour
    {
        public static string PrefabName(GameObject gameObject) => UnityEditor.PrefabUtility.FindPrefabRoot(gameObject).name;

    }
}