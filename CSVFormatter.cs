﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Text;

namespace Mtama
{
    public class CSVFormatter : OutputFormatter
    {
        private readonly CsvFormatterOptions _options;

        public string ContentType { get; private set; }

        public CSVFormatter(CsvFormatterOptions csvFormatterOptions)
        {
            ContentType = "text/csv";
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
            _options = csvFormatterOptions ?? throw new ArgumentNullException(nameof(csvFormatterOptions));
        }

        protected override bool CanWriteType(Type type)
        {

            if (type == null)
                throw new ArgumentNullException("type");

            return IsTypeOfIEnumerable(type);
        }

        private bool IsTypeOfIEnumerable(Type type)
        {

            foreach (Type interfaceType in type.GetInterfaces())
            {

                if (interfaceType == typeof(IList))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the JsonProperty data annotation name
        /// </summary>
        /// <param name="pi">Property Info</param>
        /// <returns></returns>
        private string GetDisplayNameFromNewtonsoftJsonAnnotations(PropertyInfo pi)
        {
            if (pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.PropertyName is string value)
            {
                return value;
            }

            return pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            Type type = context.Object.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            var streamWriter = new StreamWriter(response.Body);//, Encoding.GetEncoding(_options.Encoding));


            if (_options.UseSingleLineHeaderInCsv)
            {
                //var values = _options.UseNewtonsoftJsonDataAnnotations
                //    ? itemType.GetProperties().Where(pi => !pi.GetCustomAttributes<JsonIgnoreAttribute>(false).Any())    // Only get the properties that do not define JsonIgnore
                //        .Select(GetDisplayNameFromNewtonsoftJsonAnnotations)
                //    : itemType.GetProperties().Select(pi => pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name);

                var values = itemType.GetProperties().Select(pi => pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name);
                await streamWriter.WriteLineAsync(string.Join(_options.CsvDelimiter, values));
            }


            foreach (var obj in (IEnumerable<object>)context.Object)
            {
                //var vals = _options.UseNewtonsoftJsonDataAnnotations
                //    ? obj.GetType().GetProperties()
                //        .Where(pi => !pi.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                //        .Select(pi => new
                //        {
                //            Value = pi.GetValue(obj, null)
                //        })
                //    : obj.GetType().GetProperties().Select(
                //        pi => new
                //        {
                //            Value = pi.GetValue(obj, null)
                //        });
                var vals = obj.GetType().GetProperties().Select(
                        pi => new
                        {
                            Value = pi.GetValue(obj, null)
                        });

                string valueLine = string.Empty;

                foreach (var val in vals)
                {
                    if (val.Value != null)
                    {

                        var _val = val.Value.ToString();

                        //Check if the value contains a comma and place it in quotes if so
                        if (_val.Contains(","))
                            _val = string.Concat("\"", _val, "\"");

                        //Replace any \r or \n special characters from a new line with a space
                        if (_val.Contains("\r"))
                            _val = _val.Replace("\r", " ");
                        if (_val.Contains("\n"))
                            _val = _val.Replace("\n", " ");

                        valueLine = string.Concat(valueLine, _val, _options.CsvDelimiter);

                    }
                    else
                    {
                        valueLine = string.Concat(valueLine, string.Empty, _options.CsvDelimiter);
                    }
                }

                await streamWriter.WriteLineAsync(valueLine.TrimEnd(_options.CsvDelimiter.ToCharArray()));
            }

            await streamWriter.FlushAsync();
        }
    }

    public class CsvFormatterOptions
    {
        public bool UseSingleLineHeaderInCsv { get; set; } = true;

        public string CsvDelimiter { get; set; } = ",";

        public Encoding Encoding { get; set; } = Encoding.Default;

        public bool IncludeExcelDelimiterHeader { get; set; } = false;
    }
}
