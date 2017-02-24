﻿using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverValidation.GenericValidator;
using DiscoverValidation.Model.Context;
using DiscoverValidation.Model.Interface;
using DiscoverValidation.Model.ValidationResults;
using DiscoverValidation.Strategy;
using FluentValidation.Results;

namespace DiscoverValidation.Helpers
{
    public static class CreateInstanceFactory
    {
        internal static IData<T> CreateDataCasted<T>(Type typeOfData, T element, IList<ValidationFailure> failures = null)
        {
            return (IData<T>)CreateData(typeOfData, element, failures);
        }

        internal static object CreateData(Type typeOfData, object element, IList<ValidationFailure> failures = null)
        {
            Type[] typeArgs = { element.GetType() };
            var makeme = typeOfData.MakeGenericType(typeArgs);

            if (failures == null)
            {
                return Activator.CreateInstance(makeme, element);
            }
            var ctorParams = new[]
            {
                element,
                failures
            };
            return Activator.CreateInstance(makeme, ctorParams);
        }

        internal static DiscoverValidatorContext CreateDiscoverValidationContext()
        {
            var context = new DiscoverValidatorContext
            {
                AllValidatorsDictionary = AssembliesHelper.LoadValidators(),
                ValidatorsInstancesDictionary = new Dictionary<Type, IDiscoverValidator>()
            };
            return context;
        }

        internal static DiscoverValidatorContext CreateDiscoverValidationResults(DiscoverValidatorContext context)
        {
            context.DiscoverValidationResults = new DiscoverValidationResults()
            {
                ValidatableEntityTypes = context.AllValidatorsDictionary.Keys.ToList()
            };

            return context;
        }

        internal static ValidatorStrategyHanlder<T> CreateValidatorStrategyHandler<T>()
        {
            return new ValidatorStrategyHanlder<T>(); ;
        }

    }
}
