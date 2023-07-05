using KOTF.Core.Gameplay.Equipment;
using KOTF.Core.Input;
using KOTF.Core.Services;

namespace KOTF.Core.Initialization
{
    public class Initializer
    {
        public void Initialize()
        {
            var serviceProvider = ServiceProvider.GetInstance();
            serviceProvider.RegisterService<EquipmentService>();
            serviceProvider.RegisterService<CharacterColliderService>();
            serviceProvider.RegisterService<CoroutineService>();
            serviceProvider.RegisterService<AttributeUpdaterService>();
            serviceProvider.RegisterService<InputService>();

            serviceProvider.Get<EquipmentService>().Init<Weapon>();
        }
    }
}
