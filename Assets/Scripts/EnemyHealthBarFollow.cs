using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class EnemyHealthBarFollow : MonoBehaviour
    {
        [Header("Health Bar")]
        public Canvas HealthBarCanvasPrefab;
        public Vector2 HealthBarOffset;
        public GameObject HealthBarCanvasInstance;
        public HealthBar HealthBar;

        private void Start()
        {
            if (HealthBarCanvasPrefab != null)
            {
                HealthBarCanvasInstance = Instantiate(HealthBarCanvasPrefab.gameObject, new Vector2(transform.position.x + HealthBarOffset.x, transform.position.y + HealthBarOffset.y), Quaternion.identity);
                HealthBar = HealthBarCanvasInstance.transform.Find("HUD/HealthBar").GetComponent<HealthBar>();
            }
        }

        private void Update()
        {
            HealthBarCanvasInstance.transform.position = new Vector2(transform.position.x + HealthBarOffset.x, transform.position.y + HealthBarOffset.y);
        }
    }
}