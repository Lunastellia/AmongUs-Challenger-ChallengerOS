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
        public static System.Collections.Generic.List<BarghestVentObject> AllBarghestVentes = new System.Collections.Generic.List<BarghestVentObject>();
        public static int BarghestVentLimit = 5;

        private GameObject _GameObject;
        public Vent _Vent;
        private SpriteRenderer _SpriteRenderer;

        public BarghestVentObject(Vector2 p)
        {
            _GameObject = new GameObject("BarghestVent") { layer = 11 };
            //gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(p.x, p.y, p.y / 1000f + 0.01f);
            position += (Vector3)PlayerControl.LocalPlayer.Collider.offset; 
            _GameObject.transform.position = position;
            _SpriteRenderer = _GameObject.AddComponent<SpriteRenderer>();
            _SpriteRenderer.sprite = ventmapIco;


            // Create the vent
            var referenceVent = UnityEngine.Object.FindObjectOfType<Vent>();
            _Vent = UnityEngine.Object.Instantiate<Vent>(referenceVent);
            //vent.gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            _Vent.transform.position = _GameObject.transform.position;
            _Vent.Left = null;
            _Vent.Right = null;
            _Vent.Center = null;
            _Vent.EnterVentAnim = null;
            _Vent.ExitVentAnim = null;
            _Vent.Offset = new Vector3(0f, 0.25f, 0f);
            _Vent.GetComponent<PowerTools.SpriteAnim>()?.Stop();
            _Vent.Id = ShipStatus.Instance.AllVents.Select(x => x.Id).Max() + 1; // Make sure we have a unique id
            var ventRenderer = _Vent.GetComponent<SpriteRenderer>();
            ventRenderer.sprite = ventmapIco;  
            _Vent.myRend = ventRenderer;
            var allVentsList = ShipStatus.Instance.AllVents.ToList();
            allVentsList.Add(_Vent);
            ShipStatus.Instance.AllVents = allVentsList.ToArray();
            _Vent.gameObject.SetActive(false);
            _Vent.name = "BarghestVentVent_" + _Vent.Id;

            // Only render the box for the Trickster
            var playerIsBarghest = PlayerControl.LocalPlayer == Barghest.Role;
            _GameObject.SetActive(true);

            AllBarghestVentes.Add(this);
            _GameObject.SetActive(true);
            _Vent.gameObject.SetActive(true);
            MergeVent();
        }

       

        private static void MergeVent()
        {
            for (var i = 0; i < AllBarghestVentes.Count - 1; i++)
            {
                var a = AllBarghestVentes[i];
                var b = AllBarghestVentes[i + 1];
                a._Vent.Right = b._Vent;
                b._Vent.Left = a._Vent;
            }
            // Connect first with last
            AllBarghestVentes.First()._Vent.Left = AllBarghestVentes.Last()._Vent;
            AllBarghestVentes.Last()._Vent.Right = AllBarghestVentes.First()._Vent;
        }

    }

}