using LegacyVideos.Domain.Enums;
using LegacyVideos.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel;

namespace LegacyVideos.WebApp.Models
{
    public class AddMovieModel
    {
        public Movie Movie { get; set; }

        public SelectList EnumMovieTypeToList()
        {
            var list = new ArrayList();
            var enumValues = Enum.GetValues(typeof(MovieType));

            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<Enum, string>(value, GetEnumDescription((MovieType)value)));
            }

            var enumMovieTypeToList = new SelectList(list, "Key", "Value");

            return enumMovieTypeToList;
        }

        public static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
