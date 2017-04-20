using ESPL.KP.Entities;
using ESPL.KP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESPL.KP.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               { "Id", new PropertyMappingValue(new List<string>() { "Id" } ) },
               { "Genre", new PropertyMappingValue(new List<string>() { "Genre" } )},
               { "Age", new PropertyMappingValue(new List<string>() { "DateOfBirth" } , true) },
               { "Name", new PropertyMappingValue(new List<string>() { "FirstName", "LastName" }) }
           };

        private Dictionary<string, PropertyMappingValue> _departmentPropertyMapping =
           new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
           {
               { "DepartmentID", new PropertyMappingValue(new List<string>() { "DepartmentID" } ) },
               { "DepartmentName", new PropertyMappingValue(new List<string>() { "DepartmentName" } )},
               { "DepartmentDespcription", new PropertyMappingValue(new List<string>() { "DepartmentDespcription" } )}
           };

        private Dictionary<string, PropertyMappingValue> _occurrenctTypePropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
               { "OBTypeID", new PropertyMappingValue(new List<string>() { "OBTypeID" } ) },
               { "OBTypeName", new PropertyMappingValue(new List<string>() { "OBTypeName" } )},
        };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
            propertyMappings.Add(new PropertyMapping<DepartmentDto, MstDepartment>(_departmentPropertyMapping));
            propertyMappings.Add(new PropertyMapping<OccurrenceTypeDto, MstOccurrenceType>(_occurrenctTypePropertyMapping));
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
