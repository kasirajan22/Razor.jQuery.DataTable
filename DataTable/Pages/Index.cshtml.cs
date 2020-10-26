using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTable.Interface;
using DataTable.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;

namespace DataTable.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUser dal;

        public IndexModel(ILogger<IndexModel> logger,IUser dal)
        {
            _logger = logger;
            this.dal = dal;
        }
        [BindProperty]
        public List<MUser> user { get; set; }
        public JsonResult DTablesJson { get; set; }
        public List<DTable> DTables { get; set; }

        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }
        public void OnGet()
        {
            DTables = new List<DTable>() {
                new DTable { Data = "firstName", Name = "First Name", AutoWidth = true },
                new DTable { Data = "lastName", Name = "Last Name", AutoWidth = true },
                new DTable { Data = "email", Name = "Email", AutoWidth = true },
                new DTable { Data = "gender", Name = "Gender", AutoWidth = true },
                new DTable { Data = "userIP", Name = "IP", AutoWidth = true },
                new DTable { Data = "postalCode", Name = "Postal Code", AutoWidth = true },
                //new DTable { Data = "", Name = "Action", AutoWidth = true }
            };
            DTablesJson = new JsonResult(new { DTables });
        }
        public JsonResult OnPost()
        {
            try
            {
                string json = dal.GetUserData();
                JArray ary = JArray.Parse(json);
                List<MUser> dt = ary.ToObject<List<MUser>>();

                var recordsTotal = dt.Count();

                var customersQuery = dt.AsQueryable();

                var searchText = DataTablesRequest.Search.Value?.ToUpper();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    customersQuery = customersQuery.Where(s =>
                        s.FirstName.ToUpper().Contains(searchText) ||
                        s.LastName.ToUpper().Contains(searchText) ||
                        s.Email.ToUpper().Contains(searchText) ||
                        s.Gender.ToUpper().Contains(searchText) ||
                        s.PostalCode.ToUpper().Contains(searchText)
                    );
                }

                var recordsFiltered = customersQuery.Count();

                var sortColumnName = DataTablesRequest.Columns.ElementAt(DataTablesRequest.Order.ElementAt(0).Column).Data;
                var sortDirection = DataTablesRequest.Order.ElementAt(0).Dir.ToLower();                
                customersQuery = customersQuery.OrderBy($"{sortColumnName} {sortDirection}");

                var skip = DataTablesRequest.Start;
                var take = DataTablesRequest.Length;
                var data = customersQuery
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return new JsonResult(new
                {
                    Draw = DataTablesRequest.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = data
                });

            }
            catch (Exception ex)
            {
                return new JsonResult(null);
            }
        }
    }
}
