using System;
using System.ComponentModel;


namespace Abl.Data
{
    [Serializable]
    [TypeConverter(typeof (SortExpressionConverter))]
    public class SortExpression
    {
        #region Properties
        public string Title { get; set; }
        public string Expression { get; set; }
        public SortDirection Direction { get; set; }
        #endregion Properties



        #region Constructors
        public SortExpression()
        {
            Title = "";
            Expression = "";
            Direction = SortDirection.Ascending;
        }


        public SortExpression(
            string title,
            string sortExpression,
            SortDirection direction = SortDirection.Ascending)
            : this()
        {
            this.Title = title;
            this.Expression = sortExpression;
            this.Direction = direction;
        }
        #endregion Constructors



        #region Methods
        public void ToggleDirection()
        {
            Direction = (Direction == SortDirection.Descending)
                ? SortDirection.Ascending
                : SortDirection.Descending;
        }


        public override string ToString()
        {
            return $"{Title} ({Direction})";
        }
        #endregion Methods



        #region Serialzation Helper Methods
        public string Serialize()
        {
            return Serialize(this);
        }


        public static string Serialize(SortExpression sortExpression)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof (SortExpression));
            return (string) converter.ConvertTo(sortExpression, typeof (string));
        }


        public static SortExpression DeSerialize(string data)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof (SortExpression));
            return (SortExpression) converter.ConvertFrom(data);
        }
        #endregion Serialzation Helper Methods
    }


    /// <summary>
    /// SortExpressionConverter
    /// </summary>
    public class SortExpressionConverter : TypeConverter
    {

        public const char SortExpressionFieldDelimiter = ',';


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
                return new SortExpression();
            }

            string data = value as string;
            if (data != null)
            {
                if (data.Length < 1)
                {
                    return new SortExpression();
                }

                string[] elements =
                    data.Split(new char[] {SortExpressionFieldDelimiter});
                if (elements.Length != 3)
                {
                    throw new Exception("Invalid data format!");
                }

                string title = elements[0];
                string expression = elements[1];
                SortDirection direction =
                    (SortDirection)
                        Enum.Parse(typeof (SortDirection), elements[2], true);

                SortExpression sortExpression = new SortExpression(
                    title, expression, direction);

                return sortExpression;
            }

            return base.ConvertFrom(context, culture, value);
        }


        public override object ConvertTo(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value,
            Type destinationType)
        {
            if ((value != null) && (!(value is SortExpression)))
            {
                string msg = $"Unable to convert type '{value.GetType()}'!";
                throw new Exception(msg);
            }

            if (destinationType == typeof (string))
            {
                if (value == null)
                {
                    return String.Empty;
                }

                SortExpression sort = value as SortExpression;
                string data = String.Format(
                    "{1}{0}{2}{0}{3}",
                    SortExpressionFieldDelimiter, sort.Title, sort.Expression,
                    sort.Direction);

                return data;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}