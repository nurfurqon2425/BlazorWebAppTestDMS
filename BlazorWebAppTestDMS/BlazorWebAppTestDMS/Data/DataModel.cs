namespace BlazorWebAppTestDMS.Data
{
    public class DataModel
    {
        public class Group
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Weight { get; set; }
            public decimal sum { get; set; }
            public List<GroupItem> GroupItems { get; set; } = new();
        }

        public class GroupItem
        { 
            public int Id { get; set; } 
            public string Name { get; set; }
            public decimal Weight { get; set; }
            public int selectedItem { get; set; }
            public int weightSum { get; set; }
            public decimal weightProportion { get; set; }
            public List<Item> Items { get; set; } = new();
        }

        public class GroupItemFlattend
        {
            public int GroupId { get; set; }
            public string GroupName { get; set; }
            public decimal GroupWeight { get; set; }
            public int GroupItemId { get; set; }
            public string GroupItemName { get; set; }
            public decimal GroupItemWeight { get; set; }
            public int selectedItem { get; set; }
            public List<Item> Items { get; set; } = new();
        }

        public class Item
        { 
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Weight { get; set; }
        }
    }
}
