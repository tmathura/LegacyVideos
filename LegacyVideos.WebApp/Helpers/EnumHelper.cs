using LegacyVideos.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel;

namespace LegacyVideos.WebApp.Helpers
{
    public static class EnumHelper
    {
        public static SelectList EnumMovieTypeToList()
        {
            var list = new ArrayList();
            var enumValues = Enum.GetValues(typeof(MovieType));

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetEnumDescription((MovieType)value)));
            }

            var selectList = new SelectList(list, "Key", "Value");

            return selectList;
        }

        private static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}