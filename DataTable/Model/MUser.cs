using System;
using System.Collections.Generic;

namespace DataTable.Model
{
    public class MUser
    {
        public Guid UID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string UserIP { get; set; }
        public string PostalCode { get; set; }
    }
    public class DTable
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool AutoWidth { get; set; }
    }
    public class DataTablesRequest
    {
        public int Draw { get; set; }

        public List<Column> Columns { get; set; }

        public List<Order> Order { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public Search Search { get; set; }
    }

    public class Column
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public Search Search { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }

        public string Dir { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }

        public bool IsRegex { get; set; }
    }

}
