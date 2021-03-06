using System;
using System.Collections.Generic;
using DiscoverValidation.Helpers;
using DiscoverValidation.Model.Data;
using DiscoverValidation.Model.Data.Interface;

namespace DiscoverValidation.Model.ValidationResults
{
    public class DiscoverValidationResults
    {
        public IList<object> AllDataList { get; set; }
        public IList<object> ValidDataList { get; set; }
        public IList<object> InvalidDataList { get; set; }
        public IList<object> NotValidatableDataList { get; set; }
        public IList<object> NotValidatedDataList { get; set; }

        public DiscoverValidationResults()
        {
            AllDataList = new List<object>();
            ValidDataList = new List<object>();
            InvalidDataList = new List<object>();
            NotValidatableDataList = new List<object>();
            NotValidatedDataList = new List<object>();
            ValidatableEntityTypes = new List<Type>();
            NotValidatableEntityTypes = new List<Type>();
            NotValidatedEntityTypes = new List<Type>();
        }

        public IList<Type> ValidatableEntityTypes { get; set; }
        public IList<Type> NotValidatableEntityTypes { get; set; }
        public IList<Type> NotValidatedEntityTypes { get; set; }

        /// <summary>
        /// Get all the results data of a type
        /// </summary>
        /// <typeparam name="T">Type of the results data</typeparam>
        /// <returns>Returns a list of IData of the specified type</returns>
        public IList<IData<T>> GetDataOfType<T>()
        {
            var result = new List<IData<T>>();
            foreach (var data in AllDataList)
            {
                if (!(data is IData<T>)) continue;

                var castedData = data as IData<T>;

                var entity = castedData.Entity;
                result.Add(CreateInstanceFactory.CreateDataCasted(typeof(ValidData<>), entity));
            }
            return result;
        }

        /// <summary>
        /// Get the valid data of a type
        /// </summary>
        /// <typeparam name="T">Type of the results data</typeparam>
        /// <returns>Returns a list of IData of the specified type</returns>
        public IList<IData<T>> GetValidDataOfType<T>()
        {
            return GetDataOfType<T>(typeof(ValidData<>), ValidDataList);
        }

        /// <summary>
        /// Get the invalid data of a type
        /// </summary>
        /// <typeparam name="T">Type of the results data</typeparam>
        /// <returns>Returns a list of IData of the specified type</returns>
        public IList<IData<T>> GetInvalidDataOfType<T>()
        {
            return GetDataOfType<T>(typeof(InvalidData<>), InvalidDataList);
        }

        /// <summary>
        /// Get the not validatable data of a type
        /// </summary>
        /// <typeparam name="T">Type of the results data</typeparam>
        /// <returns>Returns a list of IData of the specified type</returns>
        public IList<IData<T>> GetNotValidatableDataOfType<T>()
        {
            return GetDataOfType<T>(typeof(NotValidatableData<>), NotValidatableDataList);
        }

        public IList<IData<T>> GetNotValidatedDataOfType<T>()
        {
            return GetDataOfType<T>(typeof(NotValidatedData<>), NotValidatedDataList);
        }

        private static IList<IData<T>> GetDataOfType<T>(Type dataType, IEnumerable<object> dataList)
        {
            var result = new List<IData<T>>();
            foreach (var data in dataList)
            {
                var type = data.GetType();
                if (type.GetGenericTypeDefinition() != dataType) continue;

                var castedData = data as IData<T>;

                if (castedData == null) continue;
                var entity = castedData.Entity;

                var failures = (data as InvalidData<T>)?.ValidationFailures;

                result.Add(CreateInstanceFactory.CreateDataCasted(dataType, entity, failures));
            }
            return result;
        }
    }
}