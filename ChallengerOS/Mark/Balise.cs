using System.Collections.Generic;
using UnityEngine;

namespace ChallengerOS.Objects
{
    public class Balise
    {
        public static List<Balise> balise = new List<Balise>();

        public GameObject _balise;
        public GameObject background;

        public static Sprite garlicSprite;

        public static Sprite backgroundSprite;
       

        public Balise(Vector2 p)
        {
            _balise = new GameObject("BaitAreaSpawn") { layer = 11 };
            background = new GameObject("BaitArea") { layer = 11 };
            background.transform.SetParent(_balise.transform);
            Vector3 position = new Vector3(p.x, p.y, p.y / 1000 + 0.001f); // just behind player
            _balise.transform.position = position;
            background.transform.localPosition = new Vector3(0, 0, -1f); // before player

            var garlicRenderer = _balise.AddComponent<SpriteRenderer>();
            garlicRenderer.sprite = ChallengerMod.Unity.empty;
            var backgroundRenderer = background.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = ChallengerMod.Unity.empty;


            _balise.SetActive(true);
            balise.Add(this);
        }

        public static void clear()
        {
            balise = new List<Balise>();
        }

        public static void UpdateAll()
        {
            foreach (Balise B in balise)
            {
                if (B != null)
                    B.Update();
            }
        }

        public void Update()
        {
            if (background != null)
                background.transform.Rotate(Vector3.forward * 6 * Time.fixedDeltaTime);
        }
    }
}