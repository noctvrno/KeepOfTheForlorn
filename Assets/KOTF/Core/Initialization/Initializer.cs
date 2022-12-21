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

            serviceProvider.Get<EquipmentService>().Load();
        }
    }
}
