using Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts {
    class MrBrickworm : Boss {
        

        public override void Dying() {
            throw new NotImplementedException();
        }

        public override SoundSystem.PlayListID GetPlayListID() {
            return SoundSystem.PlayListID.Boss;
        }

        public override void OnArrival() {
            print("MrBrickworm: OnArrival: test OK");
        }

        public override void OnCollisionEnter2D() {
            throw new NotImplementedException();
        }

        public override void OnDeath() {
            throw new NotImplementedException();
        }

        public override void StartEncounter() {
            OnArrival();
        }
    }
}
