namespace Emulator.Models
{
    internal class CustomItem
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public CustomItem() { }

        public CustomItem(int id, string name)
        {
            Name = name;
            Id = id;
        }
    }
}