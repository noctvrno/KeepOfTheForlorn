using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Character;

namespace KOTF.Core.Gameplay.Equipment
{
    public interface IEquipment
    {
        string Name { get; }
        string Description { get; }
        CharacterBase Owner { get; set; }
    }
}
