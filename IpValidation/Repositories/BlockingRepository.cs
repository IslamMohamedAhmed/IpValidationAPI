using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using IpValidation.Astracctions;
using IpValidation.Connections;
using IpValidation.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IpValidation.Repositories
{
    public class BlockingRepository : IBlockingRepository
    {
        private ConcurrentDictionary<string, int> BlockingDictionary = new();
        public ConcurrentDictionary<string, string> ResultPair = new ConcurrentDictionary<string, string> ();
        public readonly ConcurrentDictionary<string, string> countries = new ConcurrentDictionary<string, string>
        {
            ["US"] = "United States",
            ["CA"] = "Canada",
            ["GB"] = "United Kingdom",
            ["FR"] = "France",
            ["DE"] = "Germany",
            ["IN"] = "India",
            ["CN"] = "China",
            ["JP"] = "Japan",
            ["BR"] = "Brazil",
            ["AU"] = "Australia",
            ["IT"] = "Italy",
            ["RU"] = "Russia",
            ["ZA"] = "South Africa",
            ["MX"] = "Mexico",
            ["KR"] = "South Korea",
            ["ES"] = "Spain"
            // reference countries to search from 
        };

    

      
        public int Add(string code)
        {
            if (Regex.IsMatch(code, @"^[A-Za-z]{2,3}$"))
            {
                bool res1 = BlockingDictionary.ContainsKey(code);
                BlockingDictionary.GetOrAdd(code, BlockingDictionary.Count + 1 );
                if (!res1)
                {
                    return 1;
                    
                }
                else
                {
                    return 2;
                
                }
            }
            return 3;
           
            
        }

        public int Delete(string code)
        {

            if (Regex.IsMatch(code, @"^[A-Za-z]{2,3}$")) {
                var res = BlockingDictionary.ContainsKey(code);
                BlockingDictionary.TryRemove(code, out _);
                if (res)
                {
                    return 1;


                }
                else
                {
                    return 2; 
                }
            }
            return 3;

      

        }

        public PagiatedResult GetAll(int page, int pageSize, string search)
        {
           
                   var blockedList = countries.Values.AsQueryable();
          


            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                blockedList = blockedList.Where(c => c.Contains(search, StringComparison.OrdinalIgnoreCase) || c.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Apply pagination
            var totalItems = blockedList.Count();
            var TotalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
            var paginatedList = blockedList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagiatedResult(page,pageSize,totalItems,TotalPages,paginatedList);

           
        }

     
    }
}
