using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KOTF.Core.Gameplay.Equipment
{
    public class Weapon : ScriptableObject, IEquipment
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
