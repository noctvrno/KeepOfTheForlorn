using System;
using System.Collections;
using KOTF.Core.Gameplay.Attribute;
using UnityEngine;

namespace KOTF.Core.Services
{
    public enum AttributeUpdateType
    {
        Enhance,
        Diminish
    }

    public class AttributeUpdaterService : IService
    {
        private readonly CoroutineService _coroutineService = ServiceProvider.GetInstance().Get<CoroutineService>();

        public void Enhance(AnalogAttributeModifier analogAttributeModifier)
        {
            _coroutineService.Start(AnalogUpdate(analogAttributeModifier, AttributeUpdateType.Enhance));
        }

        public void Enhance(DiscreteAttributeModifier discreteAttributeModifier)
        {
            DiscreteUpdate(discreteAttributeModifier, AttributeUpdateType.Enhance);
        }

        public void Diminish(AnalogAttributeModifier analogAttributeModifier)
        {
            _coroutineService.Start(AnalogUpdate(analogAttributeModifier, AttributeUpdateType.Diminish));
        }

        public void Diminish(DiscreteAttributeModifier discreteAttributeModifier)
        {
            DiscreteUpdate(discreteAttributeModifier, AttributeUpdateType.Diminish);
        }

        private static IEnumerator AnalogUpdate(AnalogAttributeModifier analogAttributeModifier,
            AttributeUpdateType attributeUpdateType)
        {
            while (GetAnalogUpdateCondition(analogAttributeModifier, attributeUpdateType))
            {
                yield return new WaitForSeconds(analogAttributeModifier.Rate);
                DiscreteUpdate(analogAttributeModifier, attributeUpdateType);
            }
        }

        private static bool GetAnalogUpdateCondition(AnalogAttributeModifier analogAttributeModifier, AttributeUpdateType attributeUpdateType)
        {
            return attributeUpdateType switch
            {
                AttributeUpdateType.Enhance => analogAttributeModifier.Attribute.Value <
                                               analogAttributeModifier.Threshold,
                AttributeUpdateType.Diminish => analogAttributeModifier.Attribute.Value >
                                                analogAttributeModifier.Threshold,
                _ => throw new ArgumentException(
                    $"{attributeUpdateType} not part of enum {nameof(AttributeUpdateType)}")
            };
        }


        private static void DiscreteUpdate(IAttributeModifier attributeModifier,
            in AttributeUpdateType attributeUpdateType)
        {
            attributeModifier.Attribute.Value = attributeUpdateType switch
            {
                AttributeUpdateType.Enhance => attributeModifier.Attribute.Value + attributeModifier.Value,
                AttributeUpdateType.Diminish => attributeModifier.Attribute.Value - attributeModifier.Value,
                _ => throw new ArgumentException(
                    $"{attributeUpdateType} not part of enum {nameof(AttributeUpdateType)}")
            };
        }
    }
}
