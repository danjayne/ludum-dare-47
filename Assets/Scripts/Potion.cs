using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Must;

namespace Assets.Scripts
{
    public class Potion : MonoBehaviour
    {
        public int HealthEffector;
        public SoundEffectEnum DrinkSoundEffect;
        public AnimationCurve myCurve;

        private bool _HasBeenUsed;
        private Vector3 _OriginalPosition;

        private void Start()
        {
            _OriginalPosition = transform.position;
        }

        private void Update()
        {
            var curveTime = (Time.time % myCurve.length);

            if (Time.time == 0)
                curveTime = 0f;

            //transform.position = new Vector3(transform.position.x, myCurve.Evaluate(curveTime) + _OriginalPosition.y, transform.position.z);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_HasBeenUsed && collision.gameObject.tag.Equals("Player"))
                DrinkPotion();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_HasBeenUsed && collision.gameObject.tag.Equals("Player"))
                DrinkPotion();
        }

        private void DrinkPotion()
        {
            _HasBeenUsed = true;

            if (DrinkSoundEffect != SoundEffectEnum.None)
                AudioManager.Instance.PlaySoundEffect(DrinkSoundEffect, 2f);

            PlayerHealth.Instance.TakeDamage(-HealthEffector);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}