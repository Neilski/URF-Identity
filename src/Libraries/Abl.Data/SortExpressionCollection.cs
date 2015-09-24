using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace Abl.Data
{
    [Serializable]
    [TypeConverter(typeof (SortExpressionCollectionConverter))]
    public class SortExpressionCollection : List<SortExpression>
    {
        #region Constructors
        public SortExpressionCollection()
        {
        }


        public SortExpressionCollection(
            string title,
            string sortExpression,
            SortDirection direction = SortDirection.Ascending)
            : this(new SortExpression(title, sortExpression, direction))
        {
        }


        public SortExpressionCollection(SortExpression sortBy)
            : this()
        {
            this.Add(sortBy);
        }
        #endregion Constructors



        #region Methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (SortExpression sortExpression in this)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(sortExpression);
            }

            return sb.ToString();
        }
        #endregion Methods



        #region Serialzation Helper Methods
        public string Serialize()
        {
            return Serialize(this);
        }


        public static string Serialize(SortExpressionCollection collection)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof (SortExpressionCollection));
            return (string) converter.ConvertTo(collection, typeof (string));
        }


        public static SortExpressionCollection DeSerialize(string data)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof (SortExpressionCollection));
            return (SortExpressionCollection) converter.ConvertFrom(data);
        }
        #endregion Serialzation Helper Methods
    }


    /// <summary>
    /// SortExpressionCollectionConverter
    /// </summary>
    public class SortExpressionCollectionConverter : TypeConverter
    {

        public const char SortExpressionDelimiter = ';';


        public override bool CanConvertFrom(
            ITypeDescriptorContext context,
            Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }


        public override bool CanConvertTo(
            ITypeDescriptorContext context,
            Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }


        public override object ConvertFrom(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value)
        {
            if (value == null)
            {
                return new SortExpressionCollection();
            }

            string data = value as string;
            if (data == null) return base.ConvertFrom(context, culture, value);

            if (data.Length < 1)
            {
                return new SortExpressionCollection();
            }

            string[] elements =
                data.Split(new char[] {SortExpressionDelimiter});

            if (elements.Length < 1)
            {
                return new SortExpressionCollection();
            }


            SortExpressionCollection collection =
                new SortExpressionCollection();
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof (SortExpression));

            foreach (string element in elements)
            {
                SortExpression sortExpression =
                    converter.ConvertFrom(element) as SortExpression;
                if (sortExpression != null)
                {
                    collection.Add(sortExpression);
                }
            }

            return collection;
        }


        public override object ConvertTo(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value,
            Type destinationType)
        {
            if ((value != null) && (!(value is SortExpressionCollection)))
            {
                string msg = String.Format(
                    "Unable to convert type '{0}'!", value.GetType());
                throw new Exception(msg);
            }

            SortExpressionCollection collection =
                value as SortExpressionCollection;

            if (destinationType == typeof (string))
            {
                if (value == null)
                {
                    return String.Empty;
                }


                StringBuilder sb = new StringBuilder();
                TypeConverter converter =
                    TypeDescriptor.GetConverter(typeof (SortExpression));

                foreach (SortExpression sortExpression in collection)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(SortExpressionDelimiter);
                    }
                    string s =
                        (string)
                            converter.ConvertTo(sortExpression, typeof (string));
                    sb.Append(s);
                }

                return sb.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}