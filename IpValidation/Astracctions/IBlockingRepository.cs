﻿using System.Collections.Concurrent;
using IpValidation.Shared;

namespace IpValidation.Astracctions
{
    public interface IBlockingRepository
    {
        
        public PagiatedResult GetAll(int page, int pageSize, string search); 
    
        public int Add(string code); 
        public int Delete(string code);
        
 
    }
}
