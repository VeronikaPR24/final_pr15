using final_pr15.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_pr15.Service
{
    public class DBService
    {
        private ShopContext _context;
        public ShopContext Context => _context;
        private static DBService _instance;
        public static DBService Instance => _instance ??= new DBService();

        private DBService()
        {
            _context = new ShopContext();
        }
    }
}