using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Interfaces {
    interface IBoss {
        void Dying();

        void OnArrival();

        void OnCollisionEnter2D();

        void OnDeath();

        void StartEncounter();
    }
}
