﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Project.WebUI.Helpers
{

    /// <summary>
    /// Usage:  add .ToDictionary() to an object
    /// </summary>
    public static class ObjectToDictionaryHelper
    {

        public static IDictionary<string, object> ToDictionary(this object source)
        {

            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }

    }

}