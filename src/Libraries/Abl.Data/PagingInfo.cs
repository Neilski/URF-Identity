using System;
using System.Linq;


namespace Abl.Data
{
    /// <summary>
    /// Use a ViewModel helper to provide support for paging datagrids
    /// 
    /// Fluent support added to AddSortExpression() methods -  21st August 2015
    /// </summary>
    public class PagingInfo : IPagingInfo
    {

        public const int MaxSortSpecifications = 3;
        public static readonly int[] PageSizes = new int[] {5, 10, 20, 50, 100};
        public const int DefaultPageSize = 20;



        #region Fields
        private int m_pageNo = 1;
        private int m_TotalItems;
        private int m_PageSize = DefaultPageSize;
        #endregion Fields



        #region Properties
        public int PageNo
        {
            get { return m_pageNo; }
            set { m_pageNo = (value < 1) ? 1 : value; }
        }

        public int TotalPages
            => (int) Math.Ceiling((decimal) TotalItems/PageSize);

        public bool IsFirstPage => PageNo <= 1;
        public bool IsLastPage => PageNo >= TotalPages;
        public string SortMetaData { get; set; } = string.Empty;


        public int TotalItems
        {
            get { return m_TotalItems; }
            set { m_TotalItems = (value < 0) ? 0 : value; }
        }


        public int PageSize
        {
            get { return m_PageSize; }
            set
            {
                int pageSize = value;
                if (!PageSizes.Contains(value))
                {
                    pageSize = PageSizes.FirstOrDefault(size => size > value);
                    if (pageSize < 1)
                    {
                        pageSize = PageSizes.Last();
                    }
                }

                m_PageSize = pageSize;
            }
        }
        #endregion Properties



        #region CTORs
        public PagingInfo()
        {
        }


        public PagingInfo(string sortTitle, string sortExpression)
            : this(sortTitle, sortExpression, SortDirection.Ascending)
        {
        }


        public PagingInfo(
            string sortTitle,
            string sortExpression,
            SortDirection sortDirection)
        {
            AddSortExpression(sortTitle, sortExpression, sortDirection);
        }
        #endregion CTORs



        #region GetNearestSize() Method
        public static int GetNearestPageSize(int targetSize)
        {
            for (int i = 0; i < PageSizes.Length; i++)
            {
                int size = PageSizes[i];
                if (size > targetSize)
                {
                    if (i == 0)
                    {
                        return size;
                    }
                    int prevSize = PageSizes[i - 1];
                    return ((targetSize - prevSize) < (size - targetSize))
                        ? prevSize
                        : size;
                }
            }

            return PageSizes.Last();
        }
        #endregion GetNearestSize() Method



        #region Sorting Methods
        public PagingInfo AddSortExpression(string metaData)
        {
            SortExpression sortExpression = SortExpression.DeSerialize(metaData);
            AddSortExpression(sortExpression);
            return this;
        }


        public PagingInfo AddSortExpression(
            string title,
            string sortExpression,
            SortDirection direction = SortDirection.Ascending)
        {
            SortExpression exp = new SortExpression(
                title, sortExpression, direction);
            AddSortExpression(exp);
            return this;
        }


        public PagingInfo AddSortExpression(SortExpression sortExpression)
        {
            SortMetaData = AddSortExpression(
                SortMetaData, sortExpression);
            return this;
        }


        public void ClearSortExpressions()
        {
            SortMetaData = string.Empty;
        }

        public SortExpressionCollection GetSortExpressions()
        {
            return GetSortExpressions(SortMetaData);
        }


        public string GetSortDescription()
        {
            return GetSortDescription(SortMetaData);
        }
        #endregion Methods



        #region Static Method Helpers
        public static string AddSortExpression(
            string sortMetaData,
            SortExpression sortExpression)
        {
            // De-serialize the SortExpressionCollection meta data
            SortExpressionCollection collection =
                GetSortExpressions(sortMetaData);

            int index =
                collection.FindIndex(
                    s => s.Expression == sortExpression.Expression);

            if (index == 0)
            {
                collection[0].ToggleDirection();
            }
            else
            {
                if (index > 0)
                {
                    collection.RemoveAt(index);
                }

                collection.Insert(0, sortExpression);

                if (collection.Count > MaxSortSpecifications)
                {
                    collection.RemoveRange(MaxSortSpecifications, 1);
                }
            }

            // Re-serialize the SortExpressionCollection back into meta data
            return collection.Serialize();
        }


        public static SortExpressionCollection GetSortExpressions(
            string sortMetaData)
        {
            return SortExpressionCollection.DeSerialize(sortMetaData);
        }


        public static string GetSortDescription(string sortMetaData)
        {
            SortExpressionCollection collection =
                GetSortExpressions(sortMetaData);
            return collection.ToString();
        }
        #endregion Static Method Helpers
    }
}