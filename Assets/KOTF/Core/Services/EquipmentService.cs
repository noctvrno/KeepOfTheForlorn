using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Gameplay.Equipment;
using KOTF.Utils.Path;
using KOTF.Utils.StringConstants;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KOTF.Core.Services
{
    public class EquipmentService : IService
    {
        // The key is a string for now as we are lacking any other identifiers.
        // In the future, the key should be a Guid.
        private Dictionary<string, IEquipment> _nameToEquipments = new();

        public void Init<T>()
            where T : Object, IEquipment
        {
            // The reflection usage here is temporary and it's only here to mimic database querying.
            HashSet<string> weaponPrefabNamesConstFields = typeof(WeaponPrefabNames)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly).Select(x => x.GetRawConstantValue().ToString())
                .ToHashSet();

            // Weapon Prefabs path hardcoded for now, it should be decided based on the type in the future.
            foreach (var weaponPrefab in Resources.LoadAll<T>(PathUtils.WEAPON_PREFABS))
            {
                if (!weaponPrefabNamesConstFields.Contains(weaponPrefab.name))
                {
                    Debug.LogWarning($"Could not find the prefab {weaponPrefab.name} defined in ${nameof(WeaponPrefabNames)}.");
                    continue;
                }

                _nameToEquipments.Add(weaponPrefab.name, weaponPrefab);
            }
        }

        /// <summary>
        /// Attaches an Object based on the provided <paramref name="key"/> to the <paramref name="host"/>.
        /// </summary>
        public void AttachEquipmentTo<T>(string key, GameObject host)
            where T : Object, IEquipment
        {
            if (string.IsNullOrEmpty(key) || host == null || !_nameToEquipments.TryGetValue(key, out IEquipment attachment))
                return;

            var attachmentGameObject = Object.Instantiate(attachment as T, host.transform) as Object;
            if (attachmentGameObject == null)
            {
                Debug.LogError($"Could not attach {WeaponPrefabNames.LONGSWORD} to {host.name}");
                return;
            }

            // Casting to Weapon here is temporary as it is not sustainable.
            if (attachmentGameObject is not Weapon weapon || !host.TryGetComponent(out CharacterBase hostCharacter))
                return;

            hostCharacter.WieldedWeapon = weapon;
            weapon.Owner = hostCharacter;
            weapon.name = key;
        }
    }
}
