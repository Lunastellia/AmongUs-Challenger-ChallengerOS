using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ChallengerMod.Unity;
using static ChallengerMod.Roles;

namespace ChallengerOS.Object
{
    public class BarghestVentObject
    {
        private static List<BarghestVentObject> _allBarghestVentes = new List<BarghestVentObject>();
        private static int _barghestVentLimit = 5;
        public GameObject GameObject { get; private set; }
        public Vent Vent { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }

        public BarghestVentObject(Vector2 p)
        {
            GameObject = new GameObject("BarghestVent") { layer = 11 };
            Vector3 position = new Vector3(p.x, p.y, p.y / 1000f + 0.01f);
            position += (Vector3)PlayerControl.LocalPlayer.Collider.offset;
            GameObject.transform.position = position;
            spriteRenderer = GameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = ventmapIco;

            var referenceVent = UnityEngine.Object.FindObjectOfType<Vent>();
            Vent = UnityEngine.Object.Instantiate<Vent>(referenceVent);
            Vent.transform.position = GameObject.transform.position;
            Vent.Left = null;
            Vent.Right = null;
            Vent.Center = null;
            Vent.EnterVentAnim = null;
            Vent.ExitVentAnim = null;
            Vent.Offset = new Vector3(0f, 0.25f, 0f);
            Vent.GetComponent<PowerTools.SpriteAnim>()?.Stop();
            Vent.Id = ShipStatus.Instance.AllVents.Select(x => x.Id).Max() + 1; 
            var ventRenderer = Vent.GetComponent<SpriteRenderer>();
            ventRenderer.sprite = ventmapIco;
            Vent.myRend = ventRenderer;
            var allVentsList = ShipStatus.Instance.AllVents.ToList();
            allVentsList.Add(Vent);
            ShipStatus.Instance.AllVents = allVentsList.ToArray();
            Vent.gameObject.SetActive(false);
            Vent.name = "BarghestVentVent_" + Vent.Id;

            var playerIsBarghest = PlayerControl.LocalPlayer == Barghest.Role;
            GameObject.SetActive(true);

            _allBarghestVentes.Add(this);
            GameObject.SetActive(true);
            Vent.gameObject.SetActive(true);
            MergeVent();
        }
        private static void MergeVent()
        {
            for (var i = 0; i < _allBarghestVentes.Count - 1; i++)
            {
                var a = _allBarghestVentes[i];
                var b = _allBarghestVentes[i + 1];
                a.Vent.Right = b.Vent;
                b.Vent.Left = a.Vent;
            }

            _allBarghestVentes.First().Vent.Left = _allBarghestVentes.Last().Vent;
            _allBarghestVentes.Last().Vent.Right = _allBarghestVentes.First().Vent;
        }

        public static int BarghestVentLimit
        {
            get { return _barghestVentLimit; }
            set { _barghestVentLimit = value; }
        }

        public static List<BarghestVentObject> AllBarghestVentes
        {
            get { return _allBarghestVentes; }
        }
    }
}
