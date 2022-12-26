using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        private Dictionary<string, Object> _prefabNameToObjects = new();

        public void Load()
        {
            LoadWeapons();
        }

        private void LoadWeapons()
        {
            // The reflection usage here is temporary and it's only here to mimic database querying.
            HashSet<string> weaponPrefabNamesConstFields = typeof(WeaponPrefabNames)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly).Select(x => x.GetRawConstantValue().ToString())
                .ToHashSet();

            foreach (var weaponPrefab in Resources.LoadAll(Path.Combine(PathUtils.WEAPON_PREFABS)))
            {
                if (!weaponPrefabNamesConstFields.Contains(weaponPrefab.name))
                {
                    Debug.LogWarning($"Could not find the prefab {weaponPrefab.name} defined in ${nameof(WeaponPrefabNames)}.");
                    continue;
                }

                _prefabNameToObjects.Add(weaponPrefab.name, weaponPrefab);
            }
        }

        /// <summary>
        /// Attaches an Object based on the provided <paramref name="key"/> to the <paramref name="host"/>.
        /// </summary>
        public void AttachEquipmentTo(string key, GameObject host)
        {
            if (string.IsNullOrEmpty(key) || host == null || !_prefabNameToObjects.TryGetValue(key, out Object attachment))
                return;

            var attachmentGameObject = Object.Instantiate(attachment, host.transform) as GameObject;
            if (attachmentGameObject == null || attachmentGameObject.transform.parent != host.transform)
            {
                Debug.LogError($"Could not attach {WeaponPrefabNames.LONGSWORD} to {host.name}");
                return;
            }

            attachmentGameObject.name = key;
        }
    }
}
