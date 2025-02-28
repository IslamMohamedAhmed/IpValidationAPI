using System.Collections.Concurrent;

namespace IpValidation.Shared
{
    public class PagiatedResult
    {
        public PagiatedResult(int page, int pageResult, int allItems, int allPages, List<string> countriesCodes)
        {
            Page = page;
            PageResult = pageResult;
            AllItems = allItems;
            AllPages = allPages;
            CountriesCodes = countriesCodes;

        }

        public int Page { get; set; }
        public int PageResult { get; set; }
        public int AllItems { get; set; }
        public int AllPages { get; set; }
        public List<string> CountriesCodes { get; set; }



    }
}
