﻿using System;
using ValidationAttribute.GenericValidator;
using ValidationAttribute.Exceptions;

namespace ValidationAttribute.CustomAttribute
{
    public class MyValidationAttribute : Attribute
    {
        public IAttributeValidator Validator { get; set; }

        public MyValidationAttribute(Type validator)
        {
            if (!typeof(IAttributeValidator).IsAssignableFrom(validator))
                throw new ValidationAttributeException(validator.Name);

            Validator = (IAttributeValidator) Activator.CreateInstance(validator);
        }
    }
}
