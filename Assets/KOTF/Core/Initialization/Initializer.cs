﻿using KOTF.Core.Gameplay.Equipment;
using KOTF.Core.Input;
using KOTF.Core.Services;

namespace KOTF.Core.Initialization
{
    public class Initializer
    {
        public void Initialize()
        {
            InputFactory.Create();
            var serviceProvider = ServiceProvider.GetInstance();
            serviceProvider.RegisterService<EquipmentService>();
            serviceProvider.RegisterService<CharacterColliderService>();
            serviceProvider.RegisterService<CoroutineService>();
            serviceProvider.RegisterService<AttributeUpdaterService>();

            serviceProvider.Get<EquipmentService>().Init<Weapon>();
        }
    }
}
