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

        /// <summary>
        /// This method will enhance the provided <paramref name="analogAttributeModifier"/> over time.
        /// </summary>
        /// <param name="analogAttributeModifier"/>
        public void Enhance(AnalogAttributeModifier analogAttributeModifier)
        {
            _coroutineService.Start(AnalogUpdate(analogAttributeModifier, AttributeUpdateType.Enhance));
        }

        /// <summary>
        /// This method will enhance the provided <paramref name="analogAttributeEnhancer"/> over time and stop <paramref name="analogAttributeDiminisherToStop"/>.
        /// </summary>
        /// <param name="analogAttributeEnhancer"/>
        /// <param name="analogAttributeDiminisherToStop"/>
        public void Enhance(AnalogAttributeModifier analogAttributeEnhancer, AnalogAttributeModifier analogAttributeDiminisherToStop)
        {
            _coroutineService.Stop(analogAttributeDiminisherToStop.Guid);
            _coroutineService.Start(analogAttributeEnhancer.Guid, AnalogUpdate(analogAttributeEnhancer, AttributeUpdateType.Enhance));
        }

        /// <summary>
        /// This method will enhance the provided <paramref name="discreteAttributeModifier"/> instantly.
        /// </summary>
        /// <param name="discreteAttributeModifier"/>
        public void Enhance(DiscreteAttributeModifier discreteAttributeModifier)
        {
            DiscreteUpdate(discreteAttributeModifier, AttributeUpdateType.Enhance);
        }

        /// <summary>
        /// This method will diminish the provided <paramref name="analogAttributeModifier"/> over time.
        /// </summary>
        /// <param name="analogAttributeModifier"/>
        public void Diminish(AnalogAttributeModifier analogAttributeModifier)
        {
            _coroutineService.Start(AnalogUpdate(analogAttributeModifier, AttributeUpdateType.Diminish));
        }

        /// <summary>
        /// This method will diminish the provided <paramref name="analogAttributeDiminisher"/> over time and stop <paramref name="analogAttributeEnhancerToStop"/>.
        /// </summary>
        /// <param name="analogAttributeDiminisher"/>
        /// <param name="analogAttributeEnhancerToStop"/>
        public void Diminish(AnalogAttributeModifier analogAttributeDiminisher, AnalogAttributeModifier analogAttributeEnhancerToStop)
        {
            _coroutineService.Stop(analogAttributeEnhancerToStop.Guid);
            _coroutineService.Start(AnalogUpdate(analogAttributeDiminisher, AttributeUpdateType.Diminish));
        }

        /// <summary>
        /// This method will diminish the provided <paramref name="discreteAttributeModifier"/> instantly.
        /// </summary>
        /// <param name="discreteAttributeModifier"/>
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
