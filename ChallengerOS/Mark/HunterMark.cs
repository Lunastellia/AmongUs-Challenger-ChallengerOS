using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static ChallengerMod.Challenger;
using static ChallengerMod.Roles;
using static ChallengerMod.Unity;


namespace ChallengerOS
{
    public class HunterFootprint
    {
        private static List<HunterFootprint> Marks = new List<HunterFootprint>();
        private static Sprite _Sprite;
        private Color _Color;
        private GameObject _GameObject;
        private SpriteRenderer _SpriteRenderer;
        private PlayerControl _Player;
        private bool GreyC;

        public static Sprite getHunterFootprintSprite()
        {
            if (_Sprite) return _Sprite;
            _Sprite = HPrint;
            return _Sprite;
        }

        public HunterFootprint(float MarkDuration, bool GreyColor, PlayerControl player)
        {
            this._Player = player;
            this.GreyC = GreyColor;
            if (GreyColor)
                this._Color = Palette.PlayerColors[11];
            else
                this._Color = Palette.PlayerColors[(int)player.CurrentOutfit.ColorId];

            _GameObject = new GameObject("HunterFootprint");
            Vector3 _POS = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            _GameObject.transform.position = _POS;
            _GameObject.transform.localPosition = _POS;
            _GameObject.transform.SetParent(player.transform.parent);
            _GameObject.layer = 5;
            

            _GameObject.transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));


            _SpriteRenderer = _GameObject.AddComponent<SpriteRenderer>();
            _SpriteRenderer.sprite = getHunterFootprintSprite();
            _SpriteRenderer.color = _Color;

            _GameObject.SetActive(true);
            Marks.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(MarkDuration, new Action<float>((p) => {
                Color c = _Color;
                

                if (_SpriteRenderer) _SpriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

                if (p == 1f && _GameObject != null)
                {
                    UnityEngine.Object.Destroy(_GameObject);
                    Marks.Remove(this);
                }
            })));
        }
    }
}