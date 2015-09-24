namespace Abl.Data
{
    /// <summary>
    /// Fluent support added to AddSortExpression() methods -  21st August 2015
    /// </summary>
    public interface IPagingInfo
    {

        int PageNo { get; }
        int PageSize { get; set; }
        int TotalItems { get; set; }
        int TotalPages { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
        string SortMetaData { get; set; }
        PagingInfo AddSortExpression(string metaData);


        PagingInfo AddSortExpression(
            string title,
            string sortExpression,
            SortDirection direction);


        PagingInfo AddSortExpression(SortExpression sortExpression);
        void ClearSortExpressions();
        SortExpressionCollection GetSortExpressions();
        string GetSortDescription();

    }
}