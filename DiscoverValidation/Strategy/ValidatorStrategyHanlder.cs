using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Strategy.Interface;
using DiscoverValidation.Strategy.Strategies;
using FluentValidation.Results;

namespace DiscoverValidation.Strategy
{
    public class ValidatorStrategyHanlder<T>
    {
        private readonly Dictionary<Func<ValidationResult, bool>, IValidatableStrategy> _strategiesCreateDataDictionary;

        public ValidatorStrategyHanlder()
        {
            _strategiesCreateDataDictionary = new Dictionary<Func<ValidationResult, bool>, IValidatableStrategy>
            {
                {vRes => vRes == null, new CreateNotValidatableDataStrategy()},
                {vRes => vRes?.IsValid == true, new CreateValidDataStrategy()},
                {vRes => vRes?.IsValid == false, new CreateInvalidDataStrategy()}
            };
        }

        public void UpdateValidationResuls(DiscoverValidatorContext context, T element)
        {
            ValidationResult validationResult = null;

            if (context.AllValidatorsDictionary.ContainsKey(element.GetType()))
            {
                validationResult = GetValidator(context, element).ValidateEntity(element);
            }

            var validatableStrategy = _strategiesCreateDataDictionary.Single(p => p.Key.Invoke(validationResult)).Value;
            validatableStrategy.UpdateValidationResuls(context, element, validationResult);
        }

        private static IDiscoverValidator GetValidator<TElement>(DiscoverValidatorContext context, TElement element)
        {
            if (context.ValidatorsInstancesDictionary.ContainsKey(element.GetType()))
            {
                return context.ValidatorsInstancesDictionary[element.GetType()];
            }

            var validatorType = context.AllValidatorsDictionary[element.GetType()];

            var validator = (IDiscoverValidator) Activator.CreateInstance(validatorType);
            RegisterValidatorInstance(context, element.GetType(), validator);

            return validator;
        }

        private static void RegisterValidatorInstance(DiscoverValidatorContext context, Type elementType, IDiscoverValidator validator)
        {
            context.ValidatorsInstancesDictionary.Add(elementType, validator);
        }
    }
}