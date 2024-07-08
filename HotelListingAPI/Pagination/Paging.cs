namespace HotelListingAPI.Pagination
{
    public class Paging
    {
        const int maxPageSize = 50;//max page size is 50. I set it by default i.e total records
        public int PageNumber { get; set;} = 1;//page number start from 1
        private int _pageSize = 10;//numbers of record in one page is set to 10


        public int PageSize
        { get
            { 
                return _pageSize; 
            } 
          set 
            { 
               _pageSize = (value > maxPageSize) ? maxPageSize : value; }//to get d page records, if d value requested is more than max Page size wc is 50, it will return d max page record(i.e 50) else gives d value
            }
    }
}
