using Emulator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace Emulator.Utils
{
    public static class Extensions
    {
        private static Random r;

        static Extensions()
        {
            r = new Random();
        }

        public static int GetRandon(int MaxValue)
        {
            return r.Next(MaxValue);
        }

        public static string ToHex(this ushort n)
        {
            return n.ToString("X4");
        }

        /// <summary>
        /// Sets the given <c>Enum</c> as the control's data source (adds the text "Selecione" in the 0 index)
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="control">Control to receive the Enum as data source</param>
        public static void SetEnumSource<T>(this ListControl control, bool defineInitialValue = false, int selectedValue = 0, string placeholder = "SELECIONE", int placeholderValue = 0, bool capitalizeText = false, bool ommitSelector = false)
        {
            try
            {
                var dataSource = new List<CustomItem>();

                if (!ommitSelector)
                {
                    dataSource.Add(new CustomItem(placeholderValue, capitalizeText ? placeholder.ToUpper() : placeholder));
                }

                foreach (T eItem in Enum.GetValues(typeof(T)))
                {
                    var newItem = new CustomItem();

                    string description = GetDescription(eItem);

                    newItem.Name = capitalizeText ? description.ToUpper() : description;
                    newItem.Id = (int)Convert.ChangeType(eItem, TypeCode.Int32);

                    dataSource.Add(newItem);
                }

                control.ValueMember = "Id";
                control.DisplayMember = "Name";
                control.DataSource = dataSource;

                if (!defineInitialValue)
                {
                    control.SelectedIndex = -1;
                }
                else
                {
                    control.SelectedValue = selectedValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the text from the Description <c>Attribute</c> associated with the given value
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>Description associated with the given value</returns>
        public static string GetDescription(Enum value)
        {
            try
            {
                return GetDescription((object)value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the text from the Description <c>Attribute</c> associated with the given value
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>Description associated with the given value</returns>
        public static string GetDescription(object value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return (attributes != null && attributes.Length > 0)
                    ? attributes[0].Description
                    : value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
