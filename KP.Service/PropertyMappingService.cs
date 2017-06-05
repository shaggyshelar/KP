using System;
using System.Collections.Generic;
using System.Linq;
using KP.Application.Departments;
using KP.Common.Services;
using KP.Service.Department;

namespace KP.Service
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        private Dictionary<string, PropertyMappingValue> _departmentPropertyMapping =
          new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
          {
               { "DepartmentID", new PropertyMappingValue(new List<string>() { "DepartmentID" } ) },
               { "DepartmentName", new PropertyMappingValue(new List<string>() { "DepartmentName" } )},
               { "DepartmentCode", new PropertyMappingValue(new List<string>() { "DepartmentCode" } )},
               { "DepartmentDespcription", new PropertyMappingValue(new List<string>() { "DepartmentDespcription" } )}
          };

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<DepartmentDto, KP.Domain.Department.Department>(_departmentPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
           <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;

        }

    }
}