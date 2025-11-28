using Microsoft.Data.SqlClient;
using System.Data;

namespace BlazorWebAppTestDMS.Data
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<DataModel.Group>> GetDataAsync()
        {
            List<DataModel.Group> data = new List<DataModel.Group>();
            List<DataModel.GroupItemFlattend> flattendDatas = new List<DataModel.GroupItemFlattend>();
            DataTable groups = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT " +
                    "g.Id AS GroupId ,g.Name AS GroupName, g.Weight AS GroupWeight," +
                    "gi.Id AS GroupItemId, gi.Name AS GroupItemName, gi.Weight AS GroupItemWeight," +
                    "i.Id AS ItemId, i.Name AS ItemName, i.Weight AS ItemWeight " +
                    "FROM Groups g JOIN GroupItems gi ON g.Id = gi.GroupId JOIN Items i ON gi.Id = i.GroupItemId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(groups);
                    }
                }
            }

            data = groups.AsEnumerable()
                .GroupBy(row => new { Id = row.Field<int>("GroupId"), Name = row.Field<string>("GroupName"), Weight = row.Field<decimal>("GroupWeight") })
                .Select(group => new DataModel.Group
                {
                    Id = group.Key.Id,
                    Name = group.Key.Name,
                    Weight = group.Key.Weight,
                    GroupItems = group.GroupBy(newRow => new { Id = newRow.Field<int>("GroupItemId"), Name = newRow.Field<string>("GroupItemName"), Weight = newRow.Field<decimal>("GroupItemWeight") })
                    .Select(groupItemRow => new DataModel.GroupItem
                    {
                        Id = groupItemRow.Key.Id,
                        Name = groupItemRow.Key.Name,
                        Weight = groupItemRow.Key.Weight,
                        Items = groupItemRow.Select(itemRow => new DataModel.Item
                        {
                            Id = itemRow.Field<int>("ItemId"),
                            Name = itemRow.Field<string>("ItemName"),
                            Weight = itemRow.Field<decimal>("ItemWeight")
                        }).ToList()
                    }).ToList()
                }).ToList();

            return data;
        }

        public async Task<List<DataModel.GroupItemFlattend>> GetDataFlattendAsync()
        {
            List<DataModel.GroupItemFlattend> flattendDatas = new List<DataModel.GroupItemFlattend>();
            DataTable groups = new DataTable();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT " +
                    "g.Id AS GroupId ,g.Name AS GroupName, g.Weight AS GroupWeight," +
                    "gi.Id AS GroupItemId, gi.Name AS GroupItemName, gi.Weight AS GroupItemWeight," +
                    "i.Id AS ItemId, i.Name AS ItemName, i.Weight AS ItemWeight " +
                    "FROM Groups g JOIN GroupItems gi ON g.Id = gi.GroupId JOIN Items i ON gi.Id = i.GroupItemId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(groups);
                    }
                }
            }

            
            flattendDatas = groups.AsEnumerable().GroupBy(newRow => new { GroupId = newRow.Field<int>("GroupId"), GroupName = newRow.Field<string>("GroupName"), GroupWeight = newRow.Field<decimal>("GroupWeight"), GroupItemId = newRow.Field<int>("GroupItemId"), GroupItemName = newRow.Field<string>("GroupItemName"), GroupItemWeight = newRow.Field<decimal>("GroupItemWeight") })
                    .Select(groupItemRow => new DataModel.GroupItemFlattend
                    {
                        GroupId = groupItemRow.Key.GroupId,
                        GroupName = groupItemRow.Key.GroupName, 
                        GroupWeight = groupItemRow.Key.GroupWeight,
                        GroupItemId = groupItemRow.Key.GroupItemId,
                        GroupItemName = groupItemRow.Key.GroupItemName,
                        GroupItemWeight = groupItemRow.Key.GroupItemWeight,
                        Items = groupItemRow.Select(itemRow => new DataModel.Item
                        {
                            Id = itemRow.Field<int>("ItemId"),
                            Name = itemRow.Field<string>("ItemName"),
                            Weight = itemRow.Field<decimal>("ItemWeight")
                        }).ToList()
                    }).ToList();

            return flattendDatas;
        }
    }
}
